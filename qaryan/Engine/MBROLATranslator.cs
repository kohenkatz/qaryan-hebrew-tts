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

using System;
using System.IO;
using System.Collections.Generic;
using Qaryan.Core;
using MotiZilberman;

namespace Qaryan.Synths.MBROLA
{
	/// <summary>
	/// Translates <see cref="Qaryan.Core.Phone">Phone</see>s to <see cref="MBROLAElement">MBROLAElement</see>s according to the capabilities of a selected <see cref="MBROLAVoice">MBROLAVoice</see>.
	/// </summary>
	public class MBROLATranslator : LookaheadConsumerProducer<Phone,MBROLAElement> {
        public override string Name
        {
            get
            {
                return "Translator";
            }
        }

		MBROLAVoice voice;
		MBROLAElement curElement=null,last_e=null;

        /// <summary>
        /// Gets or sets the current voice.
        /// </summary>
		public MBROLAVoice Voice {
			get { return voice; }
			set { voice = value; }
		}
		
//		List<MBROLAElement> elements;

		protected override void BeforeConsumption()
		{
			Log(LogLevel.MajorInfo,"Started");
			base.BeforeConsumption();
			curElement=null;
			last_e=null;
		}
		
		protected override void AfterConsumption()
		{
			AddElement(null);
            Log(LogLevel.MajorInfo, "Finished");
            base.AfterConsumption();
            _DoneProducing();

		}
		
		void AddElement(MBROLAElement e) {
			if (curElement!=null) {
				MBROLAElement mbre=null;
				if (e!=null)
					mbre=voice.Unify(curElement,e);
				if (mbre!=null) {
                    string pair=String.Format("/{0}{1}/", curElement.Symbol, e.Symbol);
					if (mbre.Symbol=="") {
						curElement=e;
						e=null;
                        Log("{1} -> /{0}/", curElement.Symbol, pair);
					}
					else {
						curElement=mbre;
						e=null;
                        Log("{1} -> /{0}/", curElement.Symbol, pair);
					}
				}
				else {
					Emit(curElement);
					curElement=e;
				}
			}
			else
				curElement=e;
		}
		
		void InsertElement(MBROLAElement e) {
			MBROLAElement temp=curElement;
			curElement=e;
			AddElement(temp);
		}
		
		protected override void Consume(Queue<Phone> InQueue)
		{
			if (InQueue.Count==0)
				return;
//			elements=new List<MBROLAElement>();
			MBROLAElement mbre;
			Phone p=InQueue.Dequeue();
            _ItemConsumed(p);
			foreach (MBROLAElement e in MBROLAElement.CreateFragment(p,voice)) {
				AddElement(e);
				if (last_e!=null) {
					mbre=voice.InsertBuffer(last_e,e);
					if (mbre!=null)
						InsertElement(mbre);
				}
				last_e=e;
			}
		}
	}
	
}
