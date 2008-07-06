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
            this.components = new System.ComponentModel.Container();
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
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.speakMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speakToFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.voiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nikudAssistanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.strongDageshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.everydayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.milelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.akanyeIkanyeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.translitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mbrolaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.langMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hebrewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nikudMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new Qaryan.GUI.NikudTextBox();
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
            this.fileMenuItem,
            this.settingsMenuItem,
            this.viewMenuItem,
            this.helpMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.AccessibleDescription = null;
            this.fileMenuItem.AccessibleName = null;
            resources.ApplyResources(this.fileMenuItem, "fileMenuItem");
            this.fileMenuItem.BackgroundImage = null;
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.saveMenuItem,
            this.toolStripMenuItem4,
            this.toolStripSeparator2,
            this.speakMenuItem,
            this.stopMenuItem,
            this.speakToFileMenuItem,
            this.exitMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.ShortcutKeyDisplayString = null;
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
            // saveMenuItem
            // 
            this.saveMenuItem.AccessibleDescription = null;
            this.saveMenuItem.AccessibleName = null;
            resources.ApplyResources(this.saveMenuItem, "saveMenuItem");
            this.saveMenuItem.BackgroundImage = null;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.ShortcutKeyDisplayString = null;
            this.saveMenuItem.Click += new System.EventHandler(this.שמירהMenuItem_Click);
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
            // speakMenuItem
            // 
            this.speakMenuItem.AccessibleDescription = null;
            this.speakMenuItem.AccessibleName = null;
            resources.ApplyResources(this.speakMenuItem, "speakMenuItem");
            this.speakMenuItem.BackgroundImage = null;
            this.speakMenuItem.Name = "speakMenuItem";
            this.speakMenuItem.ShortcutKeyDisplayString = null;
            this.speakMenuItem.Click += new System.EventHandler(this.דברToolStripMenuItem_Click);
            // 
            // stopMenuItem
            // 
            this.stopMenuItem.AccessibleDescription = null;
            this.stopMenuItem.AccessibleName = null;
            resources.ApplyResources(this.stopMenuItem, "stopMenuItem");
            this.stopMenuItem.BackgroundImage = null;
            this.stopMenuItem.Name = "stopMenuItem";
            this.stopMenuItem.ShortcutKeyDisplayString = null;
            this.stopMenuItem.Click += new System.EventHandler(this.השתקToolStripMenuItem_Click);
            // 
            // speakToFileMenuItem
            // 
            this.speakToFileMenuItem.AccessibleDescription = null;
            this.speakToFileMenuItem.AccessibleName = null;
            resources.ApplyResources(this.speakToFileMenuItem, "speakToFileMenuItem");
            this.speakToFileMenuItem.BackgroundImage = null;
            this.speakToFileMenuItem.Name = "speakToFileMenuItem";
            this.speakToFileMenuItem.ShortcutKeyDisplayString = null;
            this.speakToFileMenuItem.Click += new System.EventHandler(this.דברלקובץToolStripMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.AccessibleDescription = null;
            this.exitMenuItem.AccessibleName = null;
            resources.ApplyResources(this.exitMenuItem, "exitMenuItem");
            this.exitMenuItem.BackgroundImage = null;
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.ShortcutKeyDisplayString = null;
            this.exitMenuItem.Click += new System.EventHandler(this.יציאהToolStripMenuItem_Click);
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.AccessibleDescription = null;
            this.settingsMenuItem.AccessibleName = null;
            resources.ApplyResources(this.settingsMenuItem, "settingsMenuItem");
            this.settingsMenuItem.BackgroundImage = null;
            this.settingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.voiceMenuItem,
            this.nikudAssistanceToolStripMenuItem,
            this.fontMenuItem,
            this.toolStripMenuItem5,
            this.settingsMenuItem1});
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.ShortcutKeyDisplayString = null;
            // 
            // voiceMenuItem
            // 
            this.voiceMenuItem.AccessibleDescription = null;
            this.voiceMenuItem.AccessibleName = null;
            resources.ApplyResources(this.voiceMenuItem, "voiceMenuItem");
            this.voiceMenuItem.BackgroundImage = null;
            this.voiceMenuItem.Name = "voiceMenuItem";
            this.voiceMenuItem.ShortcutKeyDisplayString = null;
            this.voiceMenuItem.Click += new System.EventHandler(this.קולToolStripMenuItem_Click);
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
            // fontMenuItem
            // 
            this.fontMenuItem.AccessibleDescription = null;
            this.fontMenuItem.AccessibleName = null;
            resources.ApplyResources(this.fontMenuItem, "fontMenuItem");
            this.fontMenuItem.BackgroundImage = null;
            this.fontMenuItem.Name = "fontMenuItem";
            this.fontMenuItem.ShortcutKeyDisplayString = null;
            this.fontMenuItem.Click += new System.EventHandler(this.גופןToolStripMenuItem_Click);
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
            // settingsMenuItem1
            // 
            this.settingsMenuItem1.AccessibleDescription = null;
            this.settingsMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.settingsMenuItem1, "settingsMenuItem1");
            this.settingsMenuItem1.BackgroundImage = null;
            this.settingsMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.strongDageshMenuItem,
            this.everydayMenuItem,
            this.milelMenuItem,
            this.akanyeIkanyeToolStripMenuItem});
            this.settingsMenuItem1.Name = "settingsMenuItem1";
            this.settingsMenuItem1.ShortcutKeyDisplayString = null;
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
            // strongDageshMenuItem
            // 
            this.strongDageshMenuItem.AccessibleDescription = null;
            this.strongDageshMenuItem.AccessibleName = null;
            resources.ApplyResources(this.strongDageshMenuItem, "strongDageshMenuItem");
            this.strongDageshMenuItem.BackgroundImage = null;
            this.strongDageshMenuItem.Checked = global::Qaryan.GUI.Settings.Default.DistinguishStrongDagesh;
            this.strongDageshMenuItem.CheckOnClick = true;
            this.strongDageshMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.strongDageshMenuItem.Name = "strongDageshMenuItem";
            this.strongDageshMenuItem.ShortcutKeyDisplayString = null;
            this.strongDageshMenuItem.CheckedChanged += new System.EventHandler(this.toolStripMenuItem1_CheckedChanged);
            // 
            // everydayMenuItem
            // 
            this.everydayMenuItem.AccessibleDescription = null;
            this.everydayMenuItem.AccessibleName = null;
            resources.ApplyResources(this.everydayMenuItem, "everydayMenuItem");
            this.everydayMenuItem.BackgroundImage = null;
            this.everydayMenuItem.Checked = global::Qaryan.GUI.Settings.Default.EverydayRegister;
            this.everydayMenuItem.CheckOnClick = true;
            this.everydayMenuItem.Name = "everydayMenuItem";
            this.everydayMenuItem.ShortcutKeyDisplayString = null;
            this.everydayMenuItem.CheckedChanged += new System.EventHandler(this.toolStripMenuItem1_CheckedChanged);
            // 
            // milelMenuItem
            // 
            this.milelMenuItem.AccessibleDescription = null;
            this.milelMenuItem.AccessibleName = null;
            resources.ApplyResources(this.milelMenuItem, "milelMenuItem");
            this.milelMenuItem.BackgroundImage = null;
            this.milelMenuItem.Checked = global::Qaryan.GUI.Settings.Default.Milel;
            this.milelMenuItem.CheckOnClick = true;
            this.milelMenuItem.Name = "milelMenuItem";
            this.milelMenuItem.ShortcutKeyDisplayString = null;
            this.milelMenuItem.CheckedChanged += new System.EventHandler(this.toolStripMenuItem1_CheckedChanged);
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
            // viewMenuItem
            // 
            this.viewMenuItem.AccessibleDescription = null;
            this.viewMenuItem.AccessibleName = null;
            resources.ApplyResources(this.viewMenuItem, "viewMenuItem");
            this.viewMenuItem.BackgroundImage = null;
            this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphMenuItem,
            this.translitMenuItem,
            this.mbrolaMenuItem,
            this.langMenuItem});
            this.viewMenuItem.Name = "viewMenuItem";
            this.viewMenuItem.ShortcutKeyDisplayString = null;
            // 
            // graphMenuItem
            // 
            this.graphMenuItem.AccessibleDescription = null;
            this.graphMenuItem.AccessibleName = null;
            resources.ApplyResources(this.graphMenuItem, "graphMenuItem");
            this.graphMenuItem.BackgroundImage = null;
            this.graphMenuItem.Name = "graphMenuItem";
            this.graphMenuItem.ShortcutKeyDisplayString = null;
            this.graphMenuItem.Click += new System.EventHandler(this.גרףהנגנהToolStripMenuItem_Click);
            // 
            // translitMenuItem
            // 
            this.translitMenuItem.AccessibleDescription = null;
            this.translitMenuItem.AccessibleName = null;
            resources.ApplyResources(this.translitMenuItem, "translitMenuItem");
            this.translitMenuItem.BackgroundImage = null;
            this.translitMenuItem.Name = "translitMenuItem";
            this.translitMenuItem.ShortcutKeyDisplayString = null;
            this.translitMenuItem.Click += new System.EventHandler(this.תעתיקToolStripMenuItem_Click);
            // 
            // mbrolaMenuItem
            // 
            this.mbrolaMenuItem.AccessibleDescription = null;
            this.mbrolaMenuItem.AccessibleName = null;
            resources.ApplyResources(this.mbrolaMenuItem, "mbrolaMenuItem");
            this.mbrolaMenuItem.BackgroundImage = null;
            this.mbrolaMenuItem.Name = "mbrolaMenuItem";
            this.mbrolaMenuItem.ShortcutKeyDisplayString = null;
            this.mbrolaMenuItem.Click += new System.EventHandler(this.mBROLAToolStripMenuItem_Click);
            // 
            // langMenuItem
            // 
            this.langMenuItem.AccessibleDescription = null;
            this.langMenuItem.AccessibleName = null;
            resources.ApplyResources(this.langMenuItem, "langMenuItem");
            this.langMenuItem.BackgroundImage = null;
            this.langMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishMenuItem,
            this.hebrewMenuItem});
            this.langMenuItem.Name = "langMenuItem";
            this.langMenuItem.ShortcutKeyDisplayString = null;
            // 
            // englishMenuItem
            // 
            this.englishMenuItem.AccessibleDescription = null;
            this.englishMenuItem.AccessibleName = null;
            resources.ApplyResources(this.englishMenuItem, "englishMenuItem");
            this.englishMenuItem.BackgroundImage = null;
            this.englishMenuItem.Name = "englishMenuItem";
            this.englishMenuItem.ShortcutKeyDisplayString = null;
            this.englishMenuItem.Click += new System.EventHandler(this.אנגליתToolStripMenuItem_Click);
            // 
            // hebrewMenuItem
            // 
            this.hebrewMenuItem.AccessibleDescription = null;
            this.hebrewMenuItem.AccessibleName = null;
            resources.ApplyResources(this.hebrewMenuItem, "hebrewMenuItem");
            this.hebrewMenuItem.BackgroundImage = null;
            this.hebrewMenuItem.Name = "hebrewMenuItem";
            this.hebrewMenuItem.ShortcutKeyDisplayString = null;
            this.hebrewMenuItem.Click += new System.EventHandler(this.עבריתToolStripMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.AccessibleDescription = null;
            this.helpMenuItem.AccessibleName = null;
            resources.ApplyResources(this.helpMenuItem, "helpMenuItem");
            this.helpMenuItem.BackgroundImage = null;
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nikudMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.ShortcutKeyDisplayString = null;
            // 
            // nikudMenuItem
            // 
            this.nikudMenuItem.AccessibleDescription = null;
            this.nikudMenuItem.AccessibleName = null;
            resources.ApplyResources(this.nikudMenuItem, "nikudMenuItem");
            this.nikudMenuItem.BackgroundImage = null;
            this.nikudMenuItem.Name = "nikudMenuItem";
            this.nikudMenuItem.ShortcutKeyDisplayString = null;
            this.nikudMenuItem.Click += new System.EventHandler(this.ניקודToolStripMenuItem_Click);
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
            this.toolStripComboBox1,
            this.toolStripButton6});
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
            // toolStripButton6
            // 
            this.toolStripButton6.AccessibleDescription = null;
            this.toolStripButton6.AccessibleName = null;
            resources.ApplyResources(this.toolStripButton6, "toolStripButton6");
            this.toolStripButton6.BackgroundImage = null;
            this.toolStripButton6.Checked = true;
            this.toolStripButton6.CheckOnClick = true;
            this.toolStripButton6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Name = "toolStripButton6";
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
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ltr_1_000001.bmp");
            this.imageList1.Images.SetKeyName(1, "ltr_1_000002.bmp");
            this.imageList1.Images.SetKeyName(2, "ltr_1_000003.bmp");
            this.imageList1.Images.SetKeyName(3, "ltr_1_000004.bmp");
            this.imageList1.Images.SetKeyName(4, "ltr_1_000005.bmp");
            this.imageList1.Images.SetKeyName(5, "ltr_1_000006.bmp");
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
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
            this.Location = global::Qaryan.GUI.Settings.Default.Form1Pos;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1Load);
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
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
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem speakMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speakToFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem voiceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem strongDageshMenuItem;
        private System.Windows.Forms.ToolStripMenuItem everydayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem milelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem akanyeIkanyeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphMenuItem;
        private System.Windows.Forms.ToolStripMenuItem translitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mbrolaMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem langMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hebrewMenuItem;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolStripMenuItem fontMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nikudAssistanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nikudMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
	}
}
