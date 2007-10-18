namespace Qaryan.GUI
{
    partial class FujisakiForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FujisakiForm));
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.AccessibleDescription = null;
            this.zedGraphControl1.AccessibleName = null;
            resources.ApplyResources(this.zedGraphControl1, "zedGraphControl1");
            this.zedGraphControl1.BackgroundImage = null;
            this.zedGraphControl1.Font = null;
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0;
            this.zedGraphControl1.ScrollMaxX = 0;
            this.zedGraphControl1.ScrollMaxY = 0;
            this.zedGraphControl1.ScrollMaxY2 = 0;
            this.zedGraphControl1.ScrollMinX = 0;
            this.zedGraphControl1.ScrollMinY = 0;
            this.zedGraphControl1.ScrollMinY2 = 0;
            // 
            // FujisakiForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.zedGraphControl1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::Qaryan.GUI.Settings.Default, "FujisakiFormPos", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = null;
            this.Icon = null;
            this.Location = global::Qaryan.GUI.Settings.Default.FujisakiFormPos;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FujisakiForm";
            this.ShowIcon = false;
            this.Resize += new System.EventHandler(this.FujisakiForm_Resize);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FujisakiForm_FormClosing);
            this.Load += new System.EventHandler(this.FujisakiForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public ZedGraph.ZedGraphControl zedGraphControl1;

    }
}