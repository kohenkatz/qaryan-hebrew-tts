using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Qaryan.GUI
{
    public partial class TranslitForm : Form
    {
        public TranslitForm()
        {
            InitializeComponent();
        }

        public string TranslitText
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

        private void TranslitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void TranslitForm_Load(object sender, EventArgs e)
        {
            Size = Settings.Default.TranslitFormSize;
        }

        private void TranslitForm_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
                Settings.Default.TranslitFormSize = Size;
        }
    }

    
}