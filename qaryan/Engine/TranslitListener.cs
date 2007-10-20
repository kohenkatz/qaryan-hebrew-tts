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
using System.Collections.Generic;
using System.Text;
using MotiZilberman;

namespace Qaryan.Core
{
    

    public class TranslitListener
    {
        public event StringEventHandler TranslitUpdated;

        StringBuilder translit = new StringBuilder();

        public TranslitListener(Producer<SpeechElement> listenTo)
        {
            listenTo.ItemProduced += new ProduceEventHandler<SpeechElement>(listenTo_ItemProduced);
        }

        void listenTo_ItemProduced(Producer<SpeechElement> sender, ItemEventArgs<SpeechElement> e)
        {
            if (!(e.Item is Cantillation || e.Item is WordTag))
            {
                translit.Append(e.Item.Translit);
                if (TranslitUpdated != null)
                    TranslitUpdated(this, Translit);
            }
        }

        public string Translit
        {
            get
            {
                return translit.ToString();
            }
        }
    }
}
