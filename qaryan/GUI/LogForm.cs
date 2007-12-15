using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Qaryan.GUI
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        public LogForm(Stream xmlStream)
            : this()
        {
            webBrowser1.DocumentStream = xmlStream;
        }
    }
}