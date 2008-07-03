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
 * Time: 2:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Text;

using MotiZilberman;

namespace Qaryan.Core
{
    /// <summary>
    /// Produces a stream of <see cref="char">char</see>s, usable in the chained producer/consumer model, from an ordinary <see cref="string">string</see>.
    /// </summary>
    /// <seealso cref="Tokenizer"/>
	public class StringCharProducer: Producer<char> {
		Queue<char> queue;
        public event ProduceEventHandler<char> ItemProduced;
        public event EventHandler DoneProducing;

        /// <summary>
        /// Creates a new StringCharProducer from a given string.
        /// </summary>
        /// <param name="s">A string.</param>
		public StringCharProducer(string s) {
			queue=new Queue<char>();
            foreach (char c in s)
            {
                lock(queue)
                    queue.Enqueue(c);
                if (ItemProduced != null)
                    ItemProduced(this, new ItemEventArgs<char>(c));
            }
            if (DoneProducing != null)
                DoneProducing(this, new EventArgs());
		}
		
		public bool IsRunning {
			get {
				return false;
			}
		}
		
		public Queue<char> OutQueue {
			get {
				return queue;
			}
		}
	}
}
