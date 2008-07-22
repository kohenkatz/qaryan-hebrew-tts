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
    public class MbrolaTranslator : LookaheadConsumerProducer<Phone, MBROLAElement>
    {
        public override string Name
        {
            get
            {
                return "Translator";
            }
        }

        const int QueueSize = 3;
        List<MBROLAElement> MidQueue = new List<MBROLAElement>();

        Voice _Voice;
        public Voice Voice
        {
            get
            {
                return _Voice;
            }
            set
            {
                _Voice = value;
            }
        }

        MBROLAElement curElement = null, last_e = null;

        //		List<MBROLAElement> elements;

        protected override void BeforeConsumption()
        {
            MidQueue.Clear();
            Log(LogLevel.MajorInfo, "Started");
            base.BeforeConsumption();
            curElement = null;
            last_e = null;
        }

        void FlushMidQueue()
        {
            foreach (MBROLAElement e in MidQueue)
            {
                Emit(e);
                Log("Emit2 {0}", e.Symbol);
            }
            MidQueue.Clear();
        }

        protected override void AfterConsumption()
        {
            FlushMidQueue();
            Log(LogLevel.MajorInfo, "Finished");
            base.AfterConsumption();
            _DoneProducing();
        }

        void AddElement(MBROLAElement e)
        {
            MidQueue.Add(e);
            Log("Add {0}", e.Symbol);
            MBROLAElement temp;
            while (MidQueue.Count > QueueSize)
            {
                temp = MidQueue[0];
                MidQueue.Remove(temp);
                Emit(temp);
                Log("Emit1 {0}", temp.Symbol);
            }

            for (int i = MidQueue.Count - 2; i >= 0; i--)
            {
                int j = i + 1;
                MBROLAElement e1 = MidQueue[i], e2 = MidQueue[j];
                DiphoneRuleAction action = ((MbrolaVoiceNew)(Voice.BackendVoice)).MatchRules(e1, e2);
                if (action.Action == DiphoneAction.None)
                    continue;
                string op;
                switch (action.Op)
                {
                    case DiphoneOp.Symbol1:
                        op = e1.Symbol;
                        break;
                    case DiphoneOp.Symbol2:
                        op = e2.Symbol;
                        break;
                    case DiphoneOp.Value:
                    default:
                        op = action.Value;
                        break;
                }
                switch (action.Action)
                {
                    case DiphoneAction.Merge:
                        MidQueue.Remove(e1);
                        MidQueue.Remove(e2);
                        temp = MBROLAElement.CreateUnify(e1, e2, op);
                        MidQueue.Insert(i, temp);
                        Log("{0}-{1} => {2}",e1.Symbol,e2.Symbol,temp.Symbol);
                        break;
                    case DiphoneAction.Delete:
                        switch (action.Op)
                        {
                            case DiphoneOp.Symbol1:
                                MidQueue.Remove(e1);
                                Log("{0}-{1} => (del)-{1}",e1.Symbol,e2.Symbol);
                                break;
                            case DiphoneOp.Symbol2:
                                MidQueue.Remove(e2);
                                Log("{0}-{1} => {0}-(del)",e1.Symbol,e2.Symbol);
                                break;
                        }
                        break;
                    case DiphoneAction.Insert:
                        temp=MBROLAElement.CreateBuffer(e1, e2, op);
                        MidQueue.Insert(j, temp);
                        Log("{0}-{1} => {0}-{2}-{1}",e1.Symbol,e2.Symbol,temp.Symbol);
                        break;
                }
            }

            /*
            if (curElement != null)
            {
                MBROLAElement mbre = null;
                if (e != null)
                    mbre = voice.Unify(curElement, e);
                if (mbre != null)
                {
                    string pair = String.Format("/{0}{1}/", curElement.Symbol, e.Symbol);
                    if (mbre.Symbol == "")
                    {
                        curElement = e;
                        e = null;
                        Log("{1} -> /{0}/", curElement.Symbol, pair);
                    }
                    else
                    {
                        curElement = mbre;
                        e = null;
                        Log("{1} -> /{0}/", curElement.Symbol, pair);
                    }
                }
                else
                {
                    Emit(curElement);
                    curElement = e;
                }
            }
            else
                curElement = e;*/
        }

        void InsertElement(MBROLAElement e)
        {
            MBROLAElement temp = curElement;
            curElement = e;
            AddElement(temp);
        }

        protected override void Consume(Queue<Phone> InQueue)
        {
            if (InQueue.Count == 0)
                return;
            //			elements=new List<MBROLAElement>();
            Phone p = InQueue.Dequeue();
            _ItemConsumed(p);
            foreach (MBROLAElement e in ((MbrolaVoiceNew)Voice.BackendVoice).CreateFragment(p))
            {
                AddElement(e);
                /*if (last_e != null)
                {
                    mbre = voice.InsertBuffer(last_e, e);
                    if (mbre != null)
                        InsertElement(mbre);
                }*/
                last_e = e;
            }
        }
    }

}
