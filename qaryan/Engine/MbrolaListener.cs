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
using Qaryan.Core;

namespace Qaryan.Synths.MBROLA
{
    public class MBROLAListener
    {
        public event StringEventHandler TextUpdated;

        StringBuilder mbrolaText  = new StringBuilder();

        public MBROLAListener(ThreadedConsumer<MBROLAElement> listenTo)
        {
            listenTo.ItemConsumed += new ConsumeEventHandler<MBROLAElement>(listenTo_ItemConsumed);
            if (listenTo is MBROLASynthesizer)
                (listenTo as MBROLASynthesizer).Error += new StringEventHandler(MbrolaListener_Error);
        }

        void MbrolaListener_Error(object sender, string value)
        {
            mbrolaText.AppendLine(";MBROLA error at this location or earlier");
            mbrolaText.AppendLine(";"+value.Replace("\n","\n;"));
            if (TextUpdated != null)
                TextUpdated(this, MbrolaText);
        }

        void listenTo_ItemConsumed(ThreadedConsumer<MBROLAElement> sender, ItemEventArgs<MBROLAElement> e)
        {
            mbrolaText.Append(e.Item);
            if (TextUpdated != null)
                TextUpdated(this, MbrolaText);
        }

        public string MbrolaText
        {
            get
            {
                return mbrolaText.ToString();
            }
        }
    }
}
