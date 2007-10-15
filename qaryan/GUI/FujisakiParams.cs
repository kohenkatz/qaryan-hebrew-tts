using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Qaryan.GUI
{
    public partial class FujisakiParams : Form
    {
        public FujisakiParams()
        {
            InitializeComponent();
        }

        private void FujisakiParams_Load(object sender, EventArgs e)
        {

        }

        public double Alpha
        {
            get
            {
                return double.Parse(textBox3.Text);
            }

            set
            {
                textBox3.Text = value.ToString();
            }
        }

        public double Beta
        {
            get
            {
                return double.Parse(textBox4.Text);
            }

            set
            {
                textBox4.Text = value.ToString();
            }
        }

        public double Gamma
        {
            get
            {
                return double.Parse(textBox5.Text);
            }

            set
            {
                textBox5.Text = value.ToString();
            }
        }

        public double Fb
        {
            get
            {
                return double.Parse(textBox6.Text);
            }

            set
            {
                textBox6.Text = value.ToString();
            }
        }
    }
}