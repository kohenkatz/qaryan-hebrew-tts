using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Qaryan.GUI
{
    public partial class FujisakiForm : Form
    {
        public FujisakiForm()
        {
            InitializeComponent();
        }

        private void FujisakiForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void FujisakiForm_Load(object sender, EventArgs e)
        {
            Size = Settings.Default.FujisakiFormSize;
        }

        private void FujisakiForm_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
                Settings.Default.FujisakiFormSize= Size;
        }
    }
}