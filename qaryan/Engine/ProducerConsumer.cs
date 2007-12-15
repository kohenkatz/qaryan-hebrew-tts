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
 * Date: 8/24/2007
 * Time: 2:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Threading;
using System.Collections.Generic;

namespace MotiZilberman
{
    public delegate void StringEventHandler(object sender, string value);

    public class QQueue<T> : Queue<T>
    {

    }
    public delegate void ConsumeDelegate<T>(Queue<T> queue);

    public class ItemEventArgs<T> : EventArgs
    {
        protected T item;

        public T Item
        {
            get
            {
                return item;
            }
        }

        public ItemEventArgs(T Item)
        {
            item = Item;
        }
    }

    public delegate void ProduceEventHandler<T>(Producer<T> sender, ItemEventArgs<T> e);
    public delegate void ConsumeEventHandler<T>(ThreadedConsumer<T> sender, ItemEventArgs<T> e);

    /// <summary>
    /// Designates a generic subprocess which produces a particular type of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to be produced.</typeparam>
    public interface Producer<T>
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsRunning
        {
            get;
        }

        Queue<T> OutQueue
        {
            get;
        }

        event ProduceEventHandler<T> ItemProduced;
        event EventHandler DoneProducing;
    }

    /// <summary>
    /// Implements a generic subprocess which consumes a particular type of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to be consumed.</typeparam>
    public abstract class ThreadedConsumer<T>
    {
        protected delegate bool Condition();

        protected abstract void Consume(Queue<T> InQueue);
        protected Thread thread;

        public Thread Thread
        {
            get
            {
                return thread;
            }
        }

        protected virtual void BeforeConsumption()
        {

        }

        protected virtual void AfterConsumption()
        {
            _DoneConsuming();
        }

        protected void Run(Producer<T> producer, Condition WaitCondition, Condition EndCondition/*, ConsumeDelegate<T> Consume*/)
        {
            thread = new Thread(delegate()
            {
                BeforeConsumption();
                while (Thread.CurrentThread.IsAlive && (producer.IsRunning || !EndCondition()))
                {

                    while ((producer.IsRunning) && WaitCondition())
                        Thread.Sleep(0);
                    if (Thread.CurrentThread.IsAlive)
                        Consume(producer.OutQueue);
                }
                AfterConsumption();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = this.GetType().Name;
            thread.Start();

        }

        public virtual void Join()
        {
            thread.Join();
        }

        public abstract void Run(Producer<T> producer);

        public event ConsumeEventHandler<T> ItemConsumed;
        public event EventHandler DoneConsuming;

        protected void _DoneConsuming()
        {
            if (DoneConsuming != null)
                DoneConsuming(this, new EventArgs());
        }

        protected void _ItemConsumed(T item)
        {
            if (ItemConsumed != null)
                ItemConsumed(this, new ItemEventArgs<T>(item));
        }
    }

    public abstract class LookaheadConsumer<T> : ThreadedConsumer<T>, ILogSource
    {
        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        protected void Run(Producer<T> producer, int windowSize)
        {
            base.Run(producer, delegate { return (producer.OutQueue.Count < windowSize); }, delegate { return (producer.OutQueue.Count <= 0); });
        }

        public override void Run(Producer<T> producer)
        {
            Run(producer, 1);
        }

        public event LogLineHandler LogLine;

        protected void Log(LogLevel visibility, string s)
        {
            if (LogLine != null)
                LogLine(this, s, visibility);
        }

        protected void Log(LogLevel visibility, object o)
        {
            if (LogLine != null)
                LogLine(this, o.ToString(), visibility);
        }

        protected void Log(LogLevel visibility, string s, params object[] objs)
        {
            Log(visibility, String.Format(s, objs));
        }

        protected void Log(string s)
        {
            Log(LogLevel.Debug, s);
        }

        protected void Log(object o)
        {
            Log(LogLevel.Debug, o.ToString());
        }


        protected internal void Log(string s, params object[] objs)
        {
            Log(String.Format(s, objs));
        }
    }

    public abstract class ConsumerProducer<T1, T2> : ThreadedConsumer<T1>, Producer<T2>, ILogSource
    {
        Queue<T2> producedQueue;
        bool isRunning;
        public event ProduceEventHandler<T2> ItemProduced;
        public event EventHandler DoneProducing;

        protected ConsumerProducer()
        {
            producedQueue = new Queue<T2>();
            isRunning = true;
        }

        protected override void BeforeConsumption()
        {
            isRunning = true;
            base.BeforeConsumption();
        }

        protected override void AfterConsumption()
        {
            base.AfterConsumption();
            isRunning = false;
        }

        protected void _DoneProducing()
        {
            if (DoneProducing != null)
                DoneProducing(this, new EventArgs());
        }

        public virtual bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public Queue<T2> OutQueue
        {
            get
            {
                return producedQueue;
            }
        }

        protected void Emit(T2 item)
        {
            if (ItemProduced != null)
                ItemProduced(this, new ItemEventArgs<T2>(item));
            OutQueue.Enqueue(item);
        }

        protected override void Consume(Queue<T1> InQueue)
        {
            //			base.Consume(queue);
        }

        public event LogLineHandler LogLine;

        protected void Log(LogLevel visibility, string s)
        {
            if (LogLine != null)
                LogLine(this, s, visibility);
        }

        protected void Log(LogLevel visibility, object o)
        {
            if (LogLine != null)
                LogLine(this, o.ToString(), visibility);
        }

        protected void Log(LogLevel visibility, string s, params object[] objs)
        {
            Log(visibility, String.Format(s, objs));
        }

        protected void Log(string s)
        {
            Log(LogLevel.Debug, s);
        }

        protected void Log(object o)
        {
            Log(LogLevel.Debug, o.ToString());
        }


        protected internal void Log(string s, params object[] objs)
        {
            Log(String.Format(s, objs));
        }

        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }

    public abstract class LookaheadConsumerProducer<T1, T2> : ConsumerProducer<T1, T2>
    {
        protected void Run(Producer<T1> producer, int windowSize)
        {
            base.Run(producer, delegate { return (producer.OutQueue.Count < windowSize); }, delegate { return (producer.OutQueue.Count <= 0); });
        }

        public override void Run(Producer<T1> producer)
        {
            Run(producer, 1);
        }
    }

    public delegate void ItemConsumedDelegate<T>(T item);

    public class DelegateConsumer<T> : LookaheadConsumer<T>
    {


        protected override void Consume(Queue<T> InQueue)
        {
            while (InQueue.Count > 0)
                _ItemConsumed(InQueue.Dequeue());
        }
    }

    public delegate void SimpleNotify();
}
