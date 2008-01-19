//    This file is part of Qaryan.
//
//    Qaryan is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Qaryan is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Qaryan.  If not, see <http://www.gnu.org/licenses/>.

/*
 * Created by SharpDevelop.
 * User: Moti Z
 * Date: 9/22/2007
 * Time: 6:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using MotiZilberman;
using Qaryan.Core;

namespace Qaryan.Core
{
    public delegate void DataPointEvent(double x,double y);
    public delegate void AccentCommandEvent(double t1, double t2, double a);
    public delegate void ProcDelegate();

	/// <summary>
	/// Generates pitch information using an instance of the Fujisaki model.
	/// </summary>
    /// <seealso cref="Phonetizer"/>
    /// <seealso cref="Qaryan.Synths.MBROLA.MBROLATranslator"/>
	public class FujisakiProcessor:LookaheadConsumerProducer<Phone,Phone>
	{
        public override string Name
        {
            get
            {
                return "Fujisaki";
            }
        }
		FujisakiModel Fujisaki=new FujisakiModel();

        /// <summary>
        /// Exposes the internal Fujisaki model object.
        /// </summary>
        public FujisakiModel Model
        {
            get
            {
                return Fujisaki;
            }
        }

        /// <summary>
        /// Raised when a pitch point has been computed.
        /// </summary>
        public event DataPointEvent PitchPointComputed;

        /// <summary>
        /// Raised when the phrase (long-term) component of a pitch point has been computed.
        /// </summary>
        public event DataPointEvent PhraseComponentComputed;

        /// <summary>
        /// Raised when the accent (short-term) component of a pitch point has been computed.
        /// </summary>
        public event DataPointEvent AccentComponentComputed;

        /// <summary>
        /// Raised when an accent command is sent to the underlying Fujisaki model.
        /// </summary>
        public event AccentCommandEvent AccentCommand;

        /// <summary>
        /// Raised when a phrase command is sent co the underlyling Fujisaki model.
        /// </summary>
        public event DataPointEvent PhraseCommand;

        /// <summary>
        /// To be deleted via refactoring.
        /// </summary>
        public event ProcDelegate NoMoreData;

        Queue<Phone> MidQueue = new Queue<Phone>();
		double UtteranceTime=0;
		double MidQueueDuration=0;

        /// <summary>
        /// Resets the underlying Fujisaki model and clears all phones pending F0 calculation.
        /// </summary>
        public void Reset()
        {
            MidQueue.Clear();
            Model.Reset();
        }

		protected override void BeforeConsumption()
		{
			Log(LogLevel.MajorInfo,"Started");
			base.BeforeConsumption();
			MidQueue=new Queue<Phone>();
			UtteranceTime=0;
			MidQueueDuration=0;
		}
	
		protected override void Consume(Queue<Phone> InQueue)
		{
            if (InQueue.Count == 0)
                return;
			Phone p=InQueue.Dequeue();

			
			if (p.Symbol=="_") {                
				Fujisaki.AddPhraseCommand((UtteranceTime+p.Duration)/1000,0.3);
                if (PhraseCommand!=null)
                    PhraseCommand(UtteranceTime + p.Duration, 0.3);
			}
			else if (p.Context.IsNucleus) {
                if (p.Context.IsAccented)
                {
                    double Aa = 0.5;
                    Fujisaki.AddAccentCommand((UtteranceTime - 200) / 1000, (UtteranceTime + 0) / 1000, Aa);
                    if (AccentCommand != null)
                        AccentCommand(UtteranceTime - 200, UtteranceTime, Aa);
                }

			}
			MidQueue.Enqueue(p);
            UtteranceTime += p.Duration;
            MidQueueDuration += p.Duration;
			
			if (MidQueueDuration>=210)
				ProcessMidQueue();
		}
		
		protected override void AfterConsumption()
		{
			
			ProcessMidQueue();
            if (NoMoreData != null) 
                NoMoreData();
			base.AfterConsumption();
            _DoneProducing();
			Log(LogLevel.MajorInfo,"Finished");
		}


		protected void ProcessMidQueue() {
			if (MidQueue.Count==0)
				return;
			double LocalTime=UtteranceTime-MidQueueDuration;
			
			while (MidQueue.Count>0) {
				Phone p=MidQueue.Dequeue();
				MidQueueDuration-=p.Duration;
				p.PitchCurve.Clear();
				for (double t=0;t<p.Duration;t+=p.Duration/8) {
					double f0=Fujisaki.F0((LocalTime+t)/1000);
					p.PitchCurve.Add(new PitchPoint(t,f0));
                    if (PitchPointComputed!=null)
                        PitchPointComputed(LocalTime + t, f0);
                    if (PhraseComponentComputed!=null)
                        PhraseComponentComputed(LocalTime + t, Fujisaki.F0PComponent((LocalTime + t) / 1000));
                    if (AccentComponentComputed!=null)
                        AccentComponentComputed(LocalTime + t, Fujisaki.F0AComponent((LocalTime + t) / 1000));
				}
				Emit(p);
				LocalTime+=p.Duration;
			}
		}
			
		public override void Run(Producer<Phone> producer)
		{
			base.Run(producer,1);
		}
	}
}
