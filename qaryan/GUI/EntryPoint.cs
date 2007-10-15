using System;
using System.Windows.Forms;

namespace Qaryan.GUI
{
    class EntryPoint
    {
        public static void Main(string[] args)
        {
            /*MbrPlay.SetDatabase("hb2");
            MbrPlay.Play("_ 100\r\nS 100\r\nX 100\r\nt 100\r\n_ 100\r\n",(int)MbrOut.SoundBoard | (int)MbrFlags.Wait,null,0);
            MbrPlay.Unload();*/
            Form1 form1 = new Form1();
            Application.Run(form1);
        }
    }
}
