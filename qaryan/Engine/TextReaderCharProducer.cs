using System;
using System.Collections.Generic;
using System.Text;
using MotiZilberman;
using System.IO;
using System.Threading;

namespace Qaryan.Core
{
    public class TextReaderCharProducer : Producer<char>
    {
        Queue<char> queue;
        public event ProduceEventHandler<char> ItemProduced;
        public event EventHandler DoneProducing;
        bool isRunning = false;

        public TextReaderCharProducer()
        {
            queue = new Queue<char>();

        }

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public Queue<char> OutQueue
        {
            get
            {
                return queue;
            }
        }

        public void Run(TextReader reader)
        {
            isRunning = true;
            Thread t = new Thread(delegate()
            {
                try
                {
                    while (Thread.CurrentThread.IsAlive)
                    {
                        char c;
                        try
                        {
                            c = (char)(reader.Read());
                        }
                        catch (OverflowException)
                        {
                            break;
                        }
                        OutQueue.Enqueue(c);
                        if (ItemProduced != null)
                            ItemProduced(this, new ItemEventArgs<char>(c));
                    }
                }
                catch (IOException)
                {
                }
                isRunning = false;
                if (DoneProducing != null)
                    DoneProducing(this, new EventArgs());

            });
            t.Start();
        }
    }
}
