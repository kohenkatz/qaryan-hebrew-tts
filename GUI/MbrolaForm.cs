using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Qaryan.GUI
{
    public partial class MbrolaForm : Form
    {
        public MbrolaForm()
        {
            InitializeComponent();
        }

        public string MbrolaText
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }
        }

        private void MbrolaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void MbrolaForm_Load(object sender, EventArgs e)
        {
            Size = Settings.Default.MbrolaFormSize;
        }

        private void MbrolaForm_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
                Settings.Default.MbrolaFormSize = Size;
        }
    }
}