namespace Qaryan.GUI
{
    partial class TranslitForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslitForm));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AccessibleDescription = null;
            this.textBox1.AccessibleName = null;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.BackgroundImage = null;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            // 
            // TranslitForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.textBox1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::Qaryan.GUI.Settings.Default, "TranslitFormPos", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = null;
            this.Icon = null;
            this.Location = global::Qaryan.GUI.Settings.Default.TranslitFormPos;
            this.Name = "TranslitForm";
            this.Resize += new System.EventHandler(this.TranslitForm_Resize);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TranslitForm_FormClosing);
            this.Load += new System.EventHandler(this.TranslitForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
    }
}