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
    public delegate void ConsumeEventHandler<T>(Consumer<T> sender, ItemEventArgs<T> e);

    /// <summary>
    /// Exposes a generic subprocess capable of producing objects of arbitrary type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to be produced.</typeparam>
    /// <seealso cref="ThreadedConsumer{T}"/>
    public interface Producer<T>
    {
        /// <summary>
        /// Enables a consumer to poll a producer's current state.
        /// <para>
        /// Returns <see langword="true">true</see> if the consumer should expect more <typeparamref name="T"/>s to appear on the queue or <see langword="false">false</see> if output has ceased.
        /// </para>
        /// </summary>
        bool IsRunning
        {
            get;
        }

        /// <summary>
        /// A first-in, first-out collection of the <typeparamref name="T"/>s.
        /// </summary>
        Queue<T> OutQueue
        {
            get;
        }

        /// <summary>
        /// Raised whenever an item of type <typeparamref name="T"/> has been produced.
        /// <para>This event faciliates monitoring and logging outside of the one-to-one producer/consumer model.</para>
        /// </summary>
        event ProduceEventHandler<T> ItemProduced;

        /// <summary>
        /// Raised whenever output from this producer has stopped.
        /// <para>Upon receiving this event, a consumer may enter a state in which it cannot consume any further items on this producer's queue until it is explicitly run again.</para>
        /// </summary>
        event EventHandler DoneProducing;
    }

    public interface Consumer<T>
    {
        void Run(Producer<T> producer);
        void Join();
        event ConsumeEventHandler<T> ItemConsumed;
        event EventHandler DoneConsuming;
    }

    /// <summary>
    /// Exposes a generic subprocess capable of consuming objects of arbitrary type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to be consumed.</typeparam>
    /// <seealso cref="Producer{T}"/>
    public abstract class ThreadedConsumer<T>: Consumer<T>
    {
        protected delegate bool Condition();

        protected abstract void Consume(Queue<T> InQueue);
        protected Thread thread;

        /// <summary>
        /// The <c>System.Threading.Thread</c> under which the consumer runs.
        /// </summary>
        public Thread Thread
        {
            get
            {
                return thread;
            }
        }

        /// <summary>
        /// Invoked in the consumer's thread after it is initialized and before consumption starts.
        /// </summary>
        /// <seealso cref="AfterConsumption"/>
        /// <seealso cref="Consume"/>
        /// <seealso cref="Run"/>
        protected virtual void BeforeConsumption()
        {

        }

        /// <summary>
        /// Invoked in the consumer's thread after consumption ends.
        /// </summary>
        /// <seealso cref="BeforeConsumption"/>
        /// <seealso cref="Consume"/>
        /// <seealso cref="Run"/>
        protected virtual void AfterConsumption()
        {
            _DoneConsuming();
        }

        /// <summary>
        /// Attaches the consumer to a specified producer and begins consumption of <typeparamref name="T"/>s subject to the externally-defined <paramref name="WaitCondition">wait condition</paramref> and <paramref name="EndCondition">end condition</paramref>.
        /// </summary>
        /// <param name="producer">A producer of <typeparamref name="T"/> objects.</param>
        /// <param name="WaitCondition">The consumption thread will yield to other threads as long as the producer is running and this delegate returns <see langword="true"/>.</param>
        /// <param name="EndCondition">The consumption thread will terminate (gracefully) when this delegte returns <see langword="true"/>.</param>
        /// <seealso cref="BeforeConsumption"/>
        /// <seealso cref="AfterConsumption"/>
        /// <seealso cref="Consume"/>
        protected void Run(Producer<T> producer, Condition WaitCondition, Condition EndCondition/*, ConsumeDelegate<T> Consume*/)
        {
            BeforeConsumption();
            thread = new Thread(delegate()
            {
                while (Thread.CurrentThread.IsAlive && (producer.IsRunning || !EndCondition()))
                {

                    while ((producer.IsRunning) && WaitCondition())
                        Thread.Sleep(0);
                    if (Thread.CurrentThread.IsAlive)
//                        lock (producer.OutQueue)
                        {
                            Consume(producer.OutQueue);
                        }
                }
                AfterConsumption();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Name = this.GetType().Name;
            thread.Start();

        }

        /// <summary>
        /// Causes the current thread to wait until the consumer's own thread has terminated, facilitating synchronous behavior.
        /// </summary>
        /// <seealso cref="Thread.Join"/>
        public virtual void Join()
        {
            thread.Join();
        }

        /// <summary>
        /// Attaches the consumer to a specified producer and begins consumption of <typeparamref name="T"/>s.
        /// </summary>
        public abstract void Run(Producer<T> producer);

        /// <summary>
        /// Raised whenever an item of type <typeparamref name="T"/> has been consumed.
        /// <para>This event faciliates monitoring and logging outside of the one-to-one producer/consumer model.</para>
        /// </summary>
        public event ConsumeEventHandler<T> ItemConsumed;

        /// <summary>
        /// Raised when consumption is over.
        /// <para>This event faciliates monitoring and logging outside of the one-to-one producer/consumer model.</para>
        /// </summary>
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

    /// <summary>
    /// Implements general logic for consumers which operate on a "lookahead window" of a certain minimal size.
    /// <para>This class implements common logging facilities for its subclasses.</para>
    /// </summary>
    /// <typeparam name="T">The type of objects to be consumed.</typeparam>
    /// <seealso cref="ThreadedConsumer{T}"/>
    /// <seealso cref="Producer{T}"/>
    public abstract class LookaheadConsumer<T> : ThreadedConsumer<T>, ILogSource
    {
        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Attaches the consumer to a specified producer and begins consumption of <typeparamref name="T"/>s, using a lookahead window of at least <paramref name="windowSize"/> elements.
        /// </summary>
        protected void Run(Producer<T> producer, int windowSize)
        {
            base.Run(producer, delegate { return (producer.OutQueue.Count < windowSize); }, delegate { return (producer.OutQueue.Count <= 0); });
        }

        /// <summary>
        /// Attaches the consumer to a specified producer and begins consumption of <typeparamref name="T"/>s, using a lookahead window of at least one element.
        /// </summary>
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

    public interface IConsumerProducer<T1, T2> : Consumer<T1>, Producer<T2>
    {
    }

    /// <summary>
    /// Encapsulates a dual producer/consumer process, operating on consumed <typeparam name="T1"/>s to produce a stream of <typeparam name="T2"/>s.
    /// <para>This class implements common logging facilities for its subclasses.</para>
    /// </summary>
    /// <typeparam name="T">The type of objects to be consumed.</typeparam>
    /// <seealso cref="ThreadedConsumer{T}"/>
    /// <seealso cref="Producer{T}"/>
    public abstract class ConsumerProducer<T1, T2> : ThreadedConsumer<T1>, IConsumerProducer<T1,T2>, ILogSource
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

        /// <summary>
        /// Adds an item to the output queue.
        /// </summary>
        /// <param name="item"></param>
        protected virtual void Emit(T2 item)
        {
            if (ItemProduced != null)
                ItemProduced(this, new ItemEventArgs<T2>(item));
//            lock (OutQueue)
            //{
                OutQueue.Enqueue(item);
            //}
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

    /// <summary>
    /// Encapsulates a dual producer/consumer process, operating on a "lookahead window" of <typeparam name="T1"/>s to produce a stream of <typeparam name="T2"/>s.
    /// </summary>
    /// <typeparam name="T">The type of objects to be consumed.</typeparam>
    /// <seealso cref="LookaheadConsumer{T}"/>
    /// <seealso cref="Producer{T}"/>
    public abstract class LookaheadConsumerProducer<T1, T2> : ConsumerProducer<T1, T2>
    {
        /// <summary>
        /// Attaches the consumer to a specified producer and begins consumption of <typeparamref name="T"/>s, using a lookahead window of at least <paramref name="windowSize"/> elements.
        /// </summary>
        protected void Run(Producer<T1> producer, int windowSize)
        {
            base.Run(producer, delegate { return (producer.OutQueue.Count < windowSize); }, delegate { return (producer.OutQueue.Count <= 0); });
        }

        /// <summary>
        /// Attaches the consumer to a specified producer and begins consumption of <typeparamref name="T"/>s, using a lookahead window of at least one element.
        /// </summary>
        public override void Run(Producer<T1> producer)
        {
            Run(producer, 1);
        }
    }

    /// <summary>
    /// A "dummy" <see cref="LookaheadConsumer">LookaheadConsumer</see>.
    /// <para>This class is primarily useful as a bridge between the producer/consumer chain and ordinary event-driver programming.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateConsumer<T> : LookaheadConsumer<T>
    {
        protected override void Consume(Queue<T> InQueue)
        {
            while (InQueue.Count > 0)
                _ItemConsumed(InQueue.Dequeue());
        }
    }

    /// <summary>
    /// Provides a blank, parameterless delegate type for simple notifications.
    /// </summary>
    public delegate void SimpleNotify();

    public abstract class Chain<Tin, Tout> : IConsumerProducer<Tin, Tout>
    {
        protected Consumer<Tin> ChainFirst;
        protected Producer<Tout> ChainLast;

        protected abstract void CreateChain(out Consumer<Tin> First, out Producer<Tout> Last);

        protected Chain()
        {
            CreateChain(out ChainFirst, out ChainLast);
            ChainFirst.ItemConsumed += new ConsumeEventHandler<Tin>(First_ItemConsumed);
            ChainFirst.DoneConsuming += new EventHandler(First_DoneConsuming);
            ChainLast.ItemProduced += new ProduceEventHandler<Tout>(Last_ItemProduced);
            ChainLast.DoneProducing += new EventHandler(Last_DoneProducing);
        }

        void Last_DoneProducing(object sender, EventArgs e)
        {
            if (DoneProducing != null)
                DoneProducing(this, e);
        }

        void Last_ItemProduced(Producer<Tout> sender, ItemEventArgs<Tout> e)
        {
            if (ItemProduced != null)
                ItemProduced(this, e);
        }

        void First_DoneConsuming(object sender, EventArgs e)
        {
            if (DoneConsuming != null)
                DoneConsuming(this, e);
        }

        void First_ItemConsumed(Consumer<Tin> sender, ItemEventArgs<Tin> e)
        {
            if (ItemConsumed != null)
                ItemConsumed(this, e);
        }

        #region Consumer<Tin> Members

        public abstract void Run(Producer<Tin> producer);

        public void Join()
        {
            while (ChainLast.IsRunning)
                System.Threading.Thread.Sleep(0);
        }

        public event ConsumeEventHandler<Tin> ItemConsumed;

        public event EventHandler DoneConsuming;

        #endregion

        #region Producer<Tout> Members

        public bool IsRunning
        {
            get { return ChainLast.IsRunning; }
        }

        public Queue<Tout> OutQueue
        {
            get { return ChainLast.OutQueue; }
        }

        public event ProduceEventHandler<Tout> ItemProduced;

        public event EventHandler DoneProducing;

        #endregion
    }
}
