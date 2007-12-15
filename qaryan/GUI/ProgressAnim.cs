using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Qaryan.GUI
{
    public partial class ProgressAnim : Control
    {
        public ProgressAnim()
        {
            InitializeComponent();
            BackColor = Color.White;
        }
        bool _Forward = true;

        public bool Forward
        {
            get
            {
                return _Forward;
            }
            set
            {
                _Forward = value;
                if (Forward)
                    frame = 0;
                else
                    frame = _Animation.Images.Count - 1;
            }
        }

        ImageList _Animation;

        public ImageList Animation
        {
            get
            {
                return _Animation;
            }
            set
            {
                if (!animating)
                    _Animation = value;
            }
        }

        int pingCount = 0;
        int frame = 0;
        bool animating = false;
        System.Threading.Timer timer;
        bool pinged = false;

        public void Stop()
        {
            pinged = false;
        }

        public void Ping()
        {
            pingCount++;
            pinged = true;
            if (!animating)
            {
                animating = true;
                timer = new System.Threading.Timer(delegate
                {
                    this.Refresh();
                });
                timer.Change(0, 50);
            }

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            //            base.OnPaint(pe);
            // TODO: Add custom paint code here]
            if (_Animation == null)
                return;
            if (animating)
            {
                if (Forward)
                {
                    frame++;
                    if (frame >= _Animation.Images.Count)
                        frame = 0;
                }
                else
                {
                    frame--;
                    if (frame <0)
                        frame = _Animation.Images.Count-1;
                }
                pingCount/=2;
                pingCount--;
                if ((!pinged) && (pingCount<=0))
                {
                    frame = 0;
                    animating = false;
                    timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    timer = null;
                }
            }
            if (frame < _Animation.Images.Count)
                pe.Graphics.DrawImage(_Animation.Images[frame], new Rectangle(new Point(0, 0), Size));
        }
    }
}
