/*
 * Created by SharpDevelop.
 * User: Moti Zilberman
 * Date: 4/6/2007
 * Time: 2:09 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Qaryan.GUI
{
	partial class Form1
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxGeneral = new System.Windows.Forms.TextBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.textBoxIPA = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBoxTokenizer = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBoxParser = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.textBoxAnalyzer = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxSilpr = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.textBoxSynthesizer = new System.Windows.Forms.TextBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveWavDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.קובץToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.שמירהMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.דברToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.השתקToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.דברלקובץToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.יציאהToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.הגדרותToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.קולToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nikudAssistanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.גופןToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.הגדרותToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.הגייתמכפלToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.הגייהיומיומיתToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.הגייהמלעיליתToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.akanyeIkanyeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.תצוגהToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.גרףהנגנהToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.תעתיקToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mBROLAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.שפתממשקמשתמשToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.אנגליתToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.עבריתToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.עזרהToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ניקודToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new Qaryan.GUI.NikudTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.AccessibleDescription = null;
            this.tabControl1.AccessibleName = null;
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.BackgroundImage = null;
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Font = null;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.AccessibleDescription = null;
            this.tabPage2.AccessibleName = null;
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.BackgroundImage = null;
            this.tabPage2.Controls.Add(this.textBoxGeneral);
            this.tabPage2.Font = null;
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxGeneral
            // 
            this.textBoxGeneral.AccessibleDescription = null;
            this.textBoxGeneral.AccessibleName = null;
            resources.ApplyResources(this.textBoxGeneral, "textBoxGeneral");
            this.textBoxGeneral.BackgroundImage = null;
            this.textBoxGeneral.Name = "textBoxGeneral";
            this.textBoxGeneral.ReadOnly = true;
            // 
            // tabPage7
            // 
            this.tabPage7.AccessibleDescription = null;
            this.tabPage7.AccessibleName = null;
            resources.ApplyResources(this.tabPage7, "tabPage7");
            this.tabPage7.BackgroundImage = null;
            this.tabPage7.Controls.Add(this.textBoxIPA);
            this.tabPage7.Font = null;
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // textBoxIPA
            // 
            this.textBoxIPA.AccessibleDescription = null;
            this.textBoxIPA.AccessibleName = null;
            resources.ApplyResources(this.textBoxIPA, "textBoxIPA");
            this.textBoxIPA.BackgroundImage = null;
            this.textBoxIPA.Name = "textBoxIPA";
            this.textBoxIPA.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.AccessibleDescription = null;
            this.tabPage3.AccessibleName = null;
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.BackgroundImage = null;
            this.tabPage3.Controls.Add(this.textBoxTokenizer);
            this.tabPage3.Font = null;
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBoxTokenizer
            // 
            this.textBoxTokenizer.AccessibleDescription = null;
            this.textBoxTokenizer.AccessibleName = null;
            resources.ApplyResources(this.textBoxTokenizer, "textBoxTokenizer");
            this.textBoxTokenizer.BackgroundImage = null;
            this.textBoxTokenizer.Name = "textBoxTokenizer";
            this.textBoxTokenizer.ReadOnly = true;
            // 
            // tabPage4
            // 
            this.tabPage4.AccessibleDescription = null;
            this.tabPage4.AccessibleName = null;
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.BackgroundImage = null;
            this.tabPage4.Controls.Add(this.textBoxParser);
            this.tabPage4.Font = null;
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBoxParser
            // 
            this.textBoxParser.AccessibleDescription = null;
            this.textBoxParser.AccessibleName = null;
            resources.ApplyResources(this.textBoxParser, "textBoxParser");
            this.textBoxParser.BackgroundImage = null;
            this.textBoxParser.Name = "textBoxParser";
            this.textBoxParser.ReadOnly = true;
            // 
            // tabPage5
            // 
            this.tabPage5.AccessibleDescription = null;
            this.tabPage5.AccessibleName = null;
            resources.ApplyResources(this.tabPage5, "tabPage5");
            this.tabPage5.BackgroundImage = null;
            this.tabPage5.Controls.Add(this.textBoxAnalyzer);
            this.tabPage5.Font = null;
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // textBoxAnalyzer
            // 
            this.textBoxAnalyzer.AccessibleDescription = null;
            this.textBoxAnalyzer.AccessibleName = null;
            resources.ApplyResources(this.textBoxAnalyzer, "textBoxAnalyzer");
            this.textBoxAnalyzer.BackgroundImage = null;
            this.textBoxAnalyzer.Name = "textBoxAnalyzer";
            this.textBoxAnalyzer.ReadOnly = true;
            // 
            // tabPage1
            // 
            this.tabPage1.AccessibleDescription = null;
            this.tabPage1.AccessibleName = null;
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.BackgroundImage = null;
            this.tabPage1.Controls.Add(this.textBoxSilpr);
            this.tabPage1.Font = null;
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxSilpr
            // 
            this.textBoxSilpr.AccessibleDescription = null;
            this.textBoxSilpr.AccessibleName = null;
            resources.ApplyResources(this.textBoxSilpr, "textBoxSilpr");
            this.textBoxSilpr.BackgroundImage = null;
            this.textBoxSilpr.Name = "textBoxSilpr";
            this.textBoxSilpr.ReadOnly = true;
            // 
            // tabPage6
            // 
            this.tabPage6.AccessibleDescription = null;
            this.tabPage6.AccessibleName = null;
            resources.ApplyResources(this.tabPage6, "tabPage6");
            this.tabPage6.BackgroundImage = null;
            this.tabPage6.Controls.Add(this.textBoxSynthesizer);
            this.tabPage6.Font = null;
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // textBoxSynthesizer
            // 
            this.textBoxSynthesizer.AccessibleDescription = null;
            this.textBoxSynthesizer.AccessibleName = null;
            resources.ApplyResources(this.textBoxSynthesizer, "textBoxSynthesizer");
            this.textBoxSynthesizer.BackgroundImage = null;
            this.textBoxSynthesizer.Name = "textBoxSynthesizer";
            this.textBoxSynthesizer.ReadOnly = true;
            // 
            // tabPage8
            // 
            this.tabPage8.AccessibleDescription = null;
            this.tabPage8.AccessibleName = null;
            resources.ApplyResources(this.tabPage8, "tabPage8");
            this.tabPage8.BackgroundImage = null;
            this.tabPage8.Controls.Add(this.panel2);
            this.tabPage8.Font = null;
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.AccessibleDescription = null;
            this.panel2.AccessibleName = null;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackgroundImage = null;
            this.panel2.Font = null;
            this.panel2.Name = "panel2";
            // 
            // statusStrip1
            // 
            this.statusStrip1.AccessibleDescription = null;
            this.statusStrip1.AccessibleName = null;
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.BackgroundImage = null;
            this.statusStrip1.Font = null;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel8});
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AccessibleDescription = null;
            this.toolStripStatusLabel1.AccessibleName = null;
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel1.BackgroundImage = null;
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.AccessibleDescription = null;
            this.toolStripStatusLabel2.AccessibleName = null;
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.BackColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel2.BackgroundImage = null;
            this.toolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Padding = new System.Windows.Forms.Padding(3);
            this.toolStripStatusLabel2.Click += new System.EventHandler(this.toolStripStatusLabel2_Click);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.AccessibleDescription = null;
            this.toolStripStatusLabel3.AccessibleName = null;
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            this.toolStripStatusLabel3.BackColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel3.BackgroundImage = null;
            this.toolStripStatusLabel3.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Padding = new System.Windows.Forms.Padding(3);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.AccessibleDescription = null;
            this.toolStripStatusLabel4.AccessibleName = null;
            resources.ApplyResources(this.toolStripStatusLabel4, "toolStripStatusLabel4");
            this.toolStripStatusLabel4.BackColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel4.BackgroundImage = null;
            this.toolStripStatusLabel4.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatusLabel4.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel4.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Padding = new System.Windows.Forms.Padding(3);
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.AccessibleDescription = null;
            this.toolStripStatusLabel5.AccessibleName = null;
            resources.ApplyResources(this.toolStripStatusLabel5, "toolStripStatusLabel5");
            this.toolStripStatusLabel5.BackColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel5.BackgroundImage = null;
            this.toolStripStatusLabel5.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel5.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatusLabel5.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel5.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Padding = new System.Windows.Forms.Padding(3);
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.AccessibleDescription = null;
            this.toolStripStatusLabel6.AccessibleName = null;
            resources.ApplyResources(this.toolStripStatusLabel6, "toolStripStatusLabel6");
            this.toolStripStatusLabel6.BackColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel6.BackgroundImage = null;
            this.toolStripStatusLabel6.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel6.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatusLabel6.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel6.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Padding = new System.Windows.Forms.Padding(3);
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.AccessibleDescription = null;
            this.toolStripStatusLabel7.AccessibleName = null;
            resources.ApplyResources(this.toolStripStatusLabel7, "toolStripStatusLabel7");
            this.toolStripStatusLabel7.BackColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel7.BackgroundImage = null;
            this.toolStripStatusLabel7.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel7.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatusLabel7.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel7.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Padding = new System.Windows.Forms.Padding(3);
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.AccessibleDescription = null;
            this.toolStripStatusLabel8.AccessibleName = null;
            resources.ApplyResources(this.toolStripStatusLabel8, "toolStripStatusLabel8");
            this.toolStripStatusLabel8.BackColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel8.BackgroundImage = null;
            this.toolStripStatusLabel8.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel8.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatusLabel8.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel8.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Padding = new System.Windows.Forms.Padding(3);
            // 
            // saveWavDialog
            // 
            this.saveWavDialog.DefaultExt = "wav";
            resources.ApplyResources(this.saveWavDialog, "saveWavDialog");
            this.saveWavDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveWavDialog_FileOk);
            // 
            // menuStrip1
            // 
            this.menuStrip1.AccessibleDescription = null;
            this.menuStrip1.AccessibleName = null;
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.BackgroundImage = null;
            this.menuStrip1.Font = null;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.קובץToolStripMenuItem,
            this.הגדרותToolStripMenuItem1,
            this.תצוגהToolStripMenuItem,
            this.עזרהToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // קובץToolStripMenuItem
            // 
            this.קובץToolStripMenuItem.AccessibleDescription = null;
            this.קובץToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.קובץToolStripMenuItem, "קובץToolStripMenuItem");
            this.קובץToolStripMenuItem.BackgroundImage = null;
            this.קובץToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.שמירהMenuItem,
            this.toolStripMenuItem4,
            this.toolStripSeparator2,
            this.דברToolStripMenuItem,
            this.השתקToolStripMenuItem,
            this.דברלקובץToolStripMenuItem,
            this.יציאהToolStripMenuItem});
            this.קובץToolStripMenuItem.Name = "קובץToolStripMenuItem";
            this.קובץToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.AccessibleDescription = null;
            this.toolStripMenuItem2.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.BackgroundImage = null;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.ShortcutKeyDisplayString = null;
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click_1);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.AccessibleDescription = null;
            this.toolStripMenuItem3.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            this.toolStripMenuItem3.BackgroundImage = null;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.ShortcutKeyDisplayString = null;
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // שמירהMenuItem
            // 
            this.שמירהMenuItem.AccessibleDescription = null;
            this.שמירהMenuItem.AccessibleName = null;
            resources.ApplyResources(this.שמירהMenuItem, "שמירהMenuItem");
            this.שמירהMenuItem.BackgroundImage = null;
            this.שמירהMenuItem.Name = "שמירהMenuItem";
            this.שמירהMenuItem.ShortcutKeyDisplayString = null;
            this.שמירהMenuItem.Click += new System.EventHandler(this.שמירהMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.AccessibleDescription = null;
            this.toolStripMenuItem4.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            this.toolStripMenuItem4.BackgroundImage = null;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.ShortcutKeyDisplayString = null;
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // דברToolStripMenuItem
            // 
            this.דברToolStripMenuItem.AccessibleDescription = null;
            this.דברToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.דברToolStripMenuItem, "דברToolStripMenuItem");
            this.דברToolStripMenuItem.BackgroundImage = null;
            this.דברToolStripMenuItem.Name = "דברToolStripMenuItem";
            this.דברToolStripMenuItem.Click += new System.EventHandler(this.דברToolStripMenuItem_Click);
            // 
            // השתקToolStripMenuItem
            // 
            this.השתקToolStripMenuItem.AccessibleDescription = null;
            this.השתקToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.השתקToolStripMenuItem, "השתקToolStripMenuItem");
            this.השתקToolStripMenuItem.BackgroundImage = null;
            this.השתקToolStripMenuItem.Name = "השתקToolStripMenuItem";
            this.השתקToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.השתקToolStripMenuItem.Click += new System.EventHandler(this.השתקToolStripMenuItem_Click);
            // 
            // דברלקובץToolStripMenuItem
            // 
            this.דברלקובץToolStripMenuItem.AccessibleDescription = null;
            this.דברלקובץToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.דברלקובץToolStripMenuItem, "דברלקובץToolStripMenuItem");
            this.דברלקובץToolStripMenuItem.BackgroundImage = null;
            this.דברלקובץToolStripMenuItem.Name = "דברלקובץToolStripMenuItem";
            this.דברלקובץToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.דברלקובץToolStripMenuItem.Click += new System.EventHandler(this.דברלקובץToolStripMenuItem_Click);
            // 
            // יציאהToolStripMenuItem
            // 
            this.יציאהToolStripMenuItem.AccessibleDescription = null;
            this.יציאהToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.יציאהToolStripMenuItem, "יציאהToolStripMenuItem");
            this.יציאהToolStripMenuItem.BackgroundImage = null;
            this.יציאהToolStripMenuItem.Name = "יציאהToolStripMenuItem";
            this.יציאהToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.יציאהToolStripMenuItem.Click += new System.EventHandler(this.יציאהToolStripMenuItem_Click);
            // 
            // הגדרותToolStripMenuItem1
            // 
            this.הגדרותToolStripMenuItem1.AccessibleDescription = null;
            this.הגדרותToolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.הגדרותToolStripMenuItem1, "הגדרותToolStripMenuItem1");
            this.הגדרותToolStripMenuItem1.BackgroundImage = null;
            this.הגדרותToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.קולToolStripMenuItem,
            this.nikudAssistanceToolStripMenuItem,
            this.גופןToolStripMenuItem,
            this.toolStripMenuItem5,
            this.הגדרותToolStripMenuItem});
            this.הגדרותToolStripMenuItem1.Name = "הגדרותToolStripMenuItem1";
            this.הגדרותToolStripMenuItem1.ShortcutKeyDisplayString = null;
            // 
            // קולToolStripMenuItem
            // 
            this.קולToolStripMenuItem.AccessibleDescription = null;
            this.קולToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.קולToolStripMenuItem, "קולToolStripMenuItem");
            this.קולToolStripMenuItem.BackgroundImage = null;
            this.קולToolStripMenuItem.Name = "קולToolStripMenuItem";
            this.קולToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.קולToolStripMenuItem.Click += new System.EventHandler(this.קולToolStripMenuItem_Click);
            // 
            // nikudAssistanceToolStripMenuItem
            // 
            this.nikudAssistanceToolStripMenuItem.AccessibleDescription = null;
            this.nikudAssistanceToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.nikudAssistanceToolStripMenuItem, "nikudAssistanceToolStripMenuItem");
            this.nikudAssistanceToolStripMenuItem.BackgroundImage = null;
            this.nikudAssistanceToolStripMenuItem.Checked = true;
            this.nikudAssistanceToolStripMenuItem.CheckOnClick = true;
            this.nikudAssistanceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nikudAssistanceToolStripMenuItem.Name = "nikudAssistanceToolStripMenuItem";
            this.nikudAssistanceToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.nikudAssistanceToolStripMenuItem.CheckedChanged += new System.EventHandler(this.nikudAssistanceToolStripMenuItem_CheckedChanged);
            // 
            // גופןToolStripMenuItem
            // 
            this.גופןToolStripMenuItem.AccessibleDescription = null;
            this.גופןToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.גופןToolStripMenuItem, "גופןToolStripMenuItem");
            this.גופןToolStripMenuItem.BackgroundImage = null;
            this.גופןToolStripMenuItem.Name = "גופןToolStripMenuItem";
            this.גופןToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.גופןToolStripMenuItem.Click += new System.EventHandler(this.גופןToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.AccessibleDescription = null;
            this.toolStripMenuItem5.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
            this.toolStripMenuItem5.BackgroundImage = null;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.ShortcutKeyDisplayString = null;
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // הגדרותToolStripMenuItem
            // 
            this.הגדרותToolStripMenuItem.AccessibleDescription = null;
            this.הגדרותToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.הגדרותToolStripMenuItem, "הגדרותToolStripMenuItem");
            this.הגדרותToolStripMenuItem.BackgroundImage = null;
            this.הגדרותToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.הגייתמכפלToolStripMenuItem,
            this.הגייהיומיומיתToolStripMenuItem,
            this.הגייהמלעיליתToolStripMenuItem,
            this.akanyeIkanyeToolStripMenuItem});
            this.הגדרותToolStripMenuItem.Name = "הגדרותToolStripMenuItem";
            this.הגדרותToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.AccessibleDescription = null;
            this.toolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.BackgroundImage = null;
            this.toolStripMenuItem1.Checked = global::Qaryan.GUI.Settings.Default.RelaxAudibleSchwa;
            this.toolStripMenuItem1.CheckOnClick = true;
            this.toolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.ShortcutKeyDisplayString = null;
            this.toolStripMenuItem1.CheckedChanged += new System.EventHandler(this.toolStripMenuItem1_CheckedChanged);
            // 
            // הגייתמכפלToolStripMenuItem
            // 
            this.הגייתמכפלToolStripMenuItem.AccessibleDescription = null;
            this.הגייתמכפלToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.הגייתמכפלToolStripMenuItem, "הגייתמכפלToolStripMenuItem");
            this.הגייתמכפלToolStripMenuItem.BackgroundImage = null;
            this.הגייתמכפלToolStripMenuItem.Checked = global::Qaryan.GUI.Settings.Default.DistinguishStrongDagesh;
            this.הגייתמכפלToolStripMenuItem.CheckOnClick = true;
            this.הגייתמכפלToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.הגייתמכפלToolStripMenuItem.Name = "הגייתמכפלToolStripMenuItem";
            this.הגייתמכפלToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.הגייתמכפלToolStripMenuItem.CheckedChanged += new System.EventHandler(this.toolStripMenuItem1_CheckedChanged);
            // 
            // הגייהיומיומיתToolStripMenuItem
            // 
            this.הגייהיומיומיתToolStripMenuItem.AccessibleDescription = null;
            this.הגייהיומיומיתToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.הגייהיומיומיתToolStripMenuItem, "הגייהיומיומיתToolStripMenuItem");
            this.הגייהיומיומיתToolStripMenuItem.BackgroundImage = null;
            this.הגייהיומיומיתToolStripMenuItem.Checked = global::Qaryan.GUI.Settings.Default.EverydayRegister;
            this.הגייהיומיומיתToolStripMenuItem.CheckOnClick = true;
            this.הגייהיומיומיתToolStripMenuItem.Name = "הגייהיומיומיתToolStripMenuItem";
            this.הגייהיומיומיתToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.הגייהיומיומיתToolStripMenuItem.CheckedChanged += new System.EventHandler(this.toolStripMenuItem1_CheckedChanged);
            // 
            // הגייהמלעיליתToolStripMenuItem
            // 
            this.הגייהמלעיליתToolStripMenuItem.AccessibleDescription = null;
            this.הגייהמלעיליתToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.הגייהמלעיליתToolStripMenuItem, "הגייהמלעיליתToolStripMenuItem");
            this.הגייהמלעיליתToolStripMenuItem.BackgroundImage = null;
            this.הגייהמלעיליתToolStripMenuItem.Checked = global::Qaryan.GUI.Settings.Default.Milel;
            this.הגייהמלעיליתToolStripMenuItem.CheckOnClick = true;
            this.הגייהמלעיליתToolStripMenuItem.Name = "הגייהמלעיליתToolStripMenuItem";
            this.הגייהמלעיליתToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.הגייהמלעיליתToolStripMenuItem.CheckedChanged += new System.EventHandler(this.toolStripMenuItem1_CheckedChanged);
            // 
            // akanyeIkanyeToolStripMenuItem
            // 
            this.akanyeIkanyeToolStripMenuItem.AccessibleDescription = null;
            this.akanyeIkanyeToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.akanyeIkanyeToolStripMenuItem, "akanyeIkanyeToolStripMenuItem");
            this.akanyeIkanyeToolStripMenuItem.BackgroundImage = null;
            this.akanyeIkanyeToolStripMenuItem.Checked = global::Qaryan.GUI.Settings.Default.AkanyeIkanye;
            this.akanyeIkanyeToolStripMenuItem.CheckOnClick = true;
            this.akanyeIkanyeToolStripMenuItem.Name = "akanyeIkanyeToolStripMenuItem";
            this.akanyeIkanyeToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.akanyeIkanyeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.toolStripMenuItem1_CheckedChanged);
            // 
            // תצוגהToolStripMenuItem
            // 
            this.תצוגהToolStripMenuItem.AccessibleDescription = null;
            this.תצוגהToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.תצוגהToolStripMenuItem, "תצוגהToolStripMenuItem");
            this.תצוגהToolStripMenuItem.BackgroundImage = null;
            this.תצוגהToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.גרףהנגנהToolStripMenuItem,
            this.תעתיקToolStripMenuItem,
            this.mBROLAToolStripMenuItem,
            this.שפתממשקמשתמשToolStripMenuItem});
            this.תצוגהToolStripMenuItem.Name = "תצוגהToolStripMenuItem";
            this.תצוגהToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // גרףהנגנהToolStripMenuItem
            // 
            this.גרףהנגנהToolStripMenuItem.AccessibleDescription = null;
            this.גרףהנגנהToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.גרףהנגנהToolStripMenuItem, "גרףהנגנהToolStripMenuItem");
            this.גרףהנגנהToolStripMenuItem.BackgroundImage = null;
            this.גרףהנגנהToolStripMenuItem.Name = "גרףהנגנהToolStripMenuItem";
            this.גרףהנגנהToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.גרףהנגנהToolStripMenuItem.Click += new System.EventHandler(this.גרףהנגנהToolStripMenuItem_Click);
            // 
            // תעתיקToolStripMenuItem
            // 
            this.תעתיקToolStripMenuItem.AccessibleDescription = null;
            this.תעתיקToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.תעתיקToolStripMenuItem, "תעתיקToolStripMenuItem");
            this.תעתיקToolStripMenuItem.BackgroundImage = null;
            this.תעתיקToolStripMenuItem.Name = "תעתיקToolStripMenuItem";
            this.תעתיקToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.תעתיקToolStripMenuItem.Click += new System.EventHandler(this.תעתיקToolStripMenuItem_Click);
            // 
            // mBROLAToolStripMenuItem
            // 
            this.mBROLAToolStripMenuItem.AccessibleDescription = null;
            this.mBROLAToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.mBROLAToolStripMenuItem, "mBROLAToolStripMenuItem");
            this.mBROLAToolStripMenuItem.BackgroundImage = null;
            this.mBROLAToolStripMenuItem.Name = "mBROLAToolStripMenuItem";
            this.mBROLAToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.mBROLAToolStripMenuItem.Click += new System.EventHandler(this.mBROLAToolStripMenuItem_Click);
            // 
            // שפתממשקמשתמשToolStripMenuItem
            // 
            this.שפתממשקמשתמשToolStripMenuItem.AccessibleDescription = null;
            this.שפתממשקמשתמשToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.שפתממשקמשתמשToolStripMenuItem, "שפתממשקמשתמשToolStripMenuItem");
            this.שפתממשקמשתמשToolStripMenuItem.BackgroundImage = null;
            this.שפתממשקמשתמשToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.אנגליתToolStripMenuItem,
            this.עבריתToolStripMenuItem});
            this.שפתממשקמשתמשToolStripMenuItem.Name = "שפתממשקמשתמשToolStripMenuItem";
            this.שפתממשקמשתמשToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // אנגליתToolStripMenuItem
            // 
            this.אנגליתToolStripMenuItem.AccessibleDescription = null;
            this.אנגליתToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.אנגליתToolStripMenuItem, "אנגליתToolStripMenuItem");
            this.אנגליתToolStripMenuItem.BackgroundImage = null;
            this.אנגליתToolStripMenuItem.Name = "אנגליתToolStripMenuItem";
            this.אנגליתToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.אנגליתToolStripMenuItem.Click += new System.EventHandler(this.אנגליתToolStripMenuItem_Click);
            // 
            // עבריתToolStripMenuItem
            // 
            this.עבריתToolStripMenuItem.AccessibleDescription = null;
            this.עבריתToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.עבריתToolStripMenuItem, "עבריתToolStripMenuItem");
            this.עבריתToolStripMenuItem.BackgroundImage = null;
            this.עבריתToolStripMenuItem.Name = "עבריתToolStripMenuItem";
            this.עבריתToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.עבריתToolStripMenuItem.Click += new System.EventHandler(this.עבריתToolStripMenuItem_Click);
            // 
            // עזרהToolStripMenuItem
            // 
            this.עזרהToolStripMenuItem.AccessibleDescription = null;
            this.עזרהToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.עזרהToolStripMenuItem, "עזרהToolStripMenuItem");
            this.עזרהToolStripMenuItem.BackgroundImage = null;
            this.עזרהToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ניקודToolStripMenuItem});
            this.עזרהToolStripMenuItem.Name = "עזרהToolStripMenuItem";
            this.עזרהToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // ניקודToolStripMenuItem
            // 
            this.ניקודToolStripMenuItem.AccessibleDescription = null;
            this.ניקודToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.ניקודToolStripMenuItem, "ניקודToolStripMenuItem");
            this.ניקודToolStripMenuItem.BackgroundImage = null;
            this.ניקודToolStripMenuItem.Name = "ניקודToolStripMenuItem";
            this.ניקודToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.ניקודToolStripMenuItem.Click += new System.EventHandler(this.ניקודToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AccessibleDescription = null;
            this.toolStrip1.AccessibleName = null;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackgroundImage = null;
            this.toolStrip1.Font = null;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator4,
            this.helpToolStripButton,
            this.toolStripSeparator5,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton4,
            this.toolStripButton3,
            this.toolStripButton5,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.toolStripComboBox1});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.AccessibleDescription = null;
            this.newToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.newToolStripButton, "newToolStripButton");
            this.newToolStripButton.BackgroundImage = null;
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Click += new System.EventHandler(this.toolStripMenuItem2_Click_1);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.AccessibleDescription = null;
            this.openToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.openToolStripButton, "openToolStripButton");
            this.openToolStripButton.BackgroundImage = null;
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.AccessibleDescription = null;
            this.saveToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.saveToolStripButton, "saveToolStripButton");
            this.saveToolStripButton.BackgroundImage = null;
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Click += new System.EventHandler(this.שמירהMenuItem_Click);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.AccessibleDescription = null;
            this.printToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.printToolStripButton, "printToolStripButton");
            this.printToolStripButton.BackgroundImage = null;
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Name = "printToolStripButton";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.AccessibleDescription = null;
            this.toolStripSeparator.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
            this.toolStripSeparator.Name = "toolStripSeparator";
            // 
            // cutToolStripButton
            // 
            this.cutToolStripButton.AccessibleDescription = null;
            this.cutToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.cutToolStripButton, "cutToolStripButton");
            this.cutToolStripButton.BackgroundImage = null;
            this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cutToolStripButton.Name = "cutToolStripButton";
            this.cutToolStripButton.Click += new System.EventHandler(this.cutToolStripButton_Click);
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.AccessibleDescription = null;
            this.copyToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.copyToolStripButton, "copyToolStripButton");
            this.copyToolStripButton.BackgroundImage = null;
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Click += new System.EventHandler(this.copyToolStripButton_Click);
            // 
            // pasteToolStripButton
            // 
            this.pasteToolStripButton.AccessibleDescription = null;
            this.pasteToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.pasteToolStripButton, "pasteToolStripButton");
            this.pasteToolStripButton.BackgroundImage = null;
            this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteToolStripButton.Name = "pasteToolStripButton";
            this.pasteToolStripButton.Click += new System.EventHandler(this.pasteToolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.AccessibleDescription = null;
            this.toolStripSeparator4.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.AccessibleDescription = null;
            this.helpToolStripButton.AccessibleName = null;
            resources.ApplyResources(this.helpToolStripButton, "helpToolStripButton");
            this.helpToolStripButton.BackgroundImage = null;
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Name = "helpToolStripButton";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.AccessibleDescription = null;
            this.toolStripSeparator5.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AccessibleDescription = null;
            this.toolStripButton1.AccessibleName = null;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.BackgroundImage = null;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Qaryan.GUI.Resources.Audio;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.דברToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.AccessibleDescription = null;
            this.toolStripButton2.AccessibleName = null;
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.BackgroundImage = null;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::Qaryan.GUI.Resources.BackgroundSound;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.דברלקובץToolStripMenuItem_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.AccessibleDescription = null;
            this.toolStripButton4.AccessibleName = null;
            resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
            this.toolStripButton4.BackgroundImage = null;
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::Qaryan.GUI.Resources.AppWindow;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.גרףהנגנהToolStripMenuItem_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.AccessibleDescription = null;
            this.toolStripButton3.AccessibleName = null;
            resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
            this.toolStripButton3.BackgroundImage = null;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.תעתיקToolStripMenuItem_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.AccessibleDescription = null;
            this.toolStripButton5.AccessibleName = null;
            resources.ApplyResources(this.toolStripButton5, "toolStripButton5");
            this.toolStripButton5.BackgroundImage = null;
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::Qaryan.GUI.Resources.Textbox;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.mBROLAToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AccessibleDescription = null;
            this.toolStripSeparator3.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AccessibleDescription = null;
            this.toolStripLabel1.AccessibleName = null;
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            this.toolStripLabel1.BackgroundImage = null;
            this.toolStripLabel1.Name = "toolStripLabel1";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.AccessibleDescription = null;
            this.toolStripComboBox1.AccessibleName = null;
            resources.ApplyResources(this.toolStripComboBox1, "toolStripComboBox1");
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AccessibleDescription = null;
            this.textBox1.AccessibleName = null;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.BackgroundImage = null;
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("UseNikudMethod", global::Qaryan.GUI.Settings.Default, "UseNikudMethod", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.HideSelection = false;
            this.textBox1.Name = "textBox1";
            this.textBox1.UseNikudMethod = global::Qaryan.GUI.Settings.Default.UseNikudMethod;
            this.textBox1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox1_PreviewKeyDown);
            this.textBox1.CtrlEnter += new System.EventHandler(this.textBox1_CtrlEnter);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "txt";
            resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.פתיחה_FileOk);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            resources.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // fontDialog1
            // 
            this.fontDialog1.Font = new System.Drawing.Font("David", 26.25F);
            this.fontDialog1.FontMustExist = true;
            this.fontDialog1.ShowApply = true;
            this.fontDialog1.ShowColor = true;
            this.fontDialog1.ShowEffects = false;
            this.fontDialog1.Apply += new System.EventHandler(this.fontDialog1_Apply);
            // 
            // Form1
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::Qaryan.GUI.Settings.Default, "Form1Pos", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = null;
            this.Icon = null;
            this.Location = global::Qaryan.GUI.Settings.Default.Form1Pos;
            this.Name = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		private System.Windows.Forms.TextBox textBoxIPA;
        private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.TextBox textBoxSilpr;
        private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TextBox textBoxSynthesizer;
		private System.Windows.Forms.TextBox textBoxAnalyzer;
		private System.Windows.Forms.TextBox textBoxParser;
		private System.Windows.Forms.TextBox textBoxTokenizer;
		private System.Windows.Forms.TextBox textBoxGeneral;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.SaveFileDialog saveWavDialog;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem קובץToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem שמירהMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem דברToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem השתקToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem דברלקובץToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem יציאהToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem הגדרותToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem קולToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem הגדרותToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem הגייתמכפלToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem הגייהיומיומיתToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem הגייהמלעיליתToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem akanyeIkanyeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem תצוגהToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem גרףהנגנהToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem תעתיקToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mBROLAToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton printToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton cutToolStripButton;
        private System.Windows.Forms.ToolStripButton copyToolStripButton;
        private System.Windows.Forms.ToolStripButton pasteToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.Panel panel1;
        private NikudTextBox textBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripMenuItem שפתממשקמשתמשToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem אנגליתToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem עבריתToolStripMenuItem;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolStripMenuItem גופןToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nikudAssistanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem עזרהToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ניקודToolStripMenuItem;
	}
}
