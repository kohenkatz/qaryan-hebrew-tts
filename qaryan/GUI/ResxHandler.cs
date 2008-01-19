using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using global::ArtSko.Tools.IeApps;
using System.Runtime.InteropServices;

namespace Qaryan.GUI
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("7604962d-6710-47be-acd8-fe21aeb6ec7d")]
    public class ResxHandler : IeApp
    {
        protected override string Scheme
        {
            get
            {
                return "resx";
            }
        }

        protected override void ProcessRequest(ref int result, out byte[] data)
        {
            HeaderResult(result);
            string filename = RequestedUrl.Query.Remove(0, 1);
            if (filename.EndsWith("js"))
                HeaderContentType("text/javascript", true);
            else if (filename.EndsWith("xsl"))
                HeaderContentType("text/xsl", true);
            else
                HeaderContentType("text/xml", true);
            data = Resources.ResourceManager.GetObject(filename, Resources.Culture) as byte[];
            HeaderContentLength(data.Length);
            HeaderConnectionClosed();
        }
    }
}
