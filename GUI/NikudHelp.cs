using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Qaryan.GUI
{
    public partial class NikudHelp : Form
    {
        public NikudHelp()
        {
            InitializeComponent();
        }

        private void NikudHelp_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}