/*
 * Created by SharpDevelop.
 * User: Moti Zilberman
 * Date: 4/6/2007
 * Time: 2:09 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using ZedGraph;
using Qaryan.Synths.MBROLA;
using Qaryan.Core;
using Qaryan.Audio;
using System.Reflection;
using MotiZilberman;
using Qaryan.Synths.Native;

namespace Qaryan.GUI
{
    /// <summary>
    /// Description of Form1.
    /// </summary>
    public partial class Form1 : Form
    {
        List<Voice> Voices = new List<Voice>();
        FujisakiForm fujisakiForm;
        TranslitForm translitForm;
        MbrolaForm mbrolaForm;
        NikudHelp nikudHelp;
        string fileName = null;

        QaryanEngine myEngine;

        void LoadVoices()
        {
            int i = toolStripComboBox1.SelectedIndex;
            toolStripComboBox1.Items.Clear();
            Voices.Clear();
            string dir = FileBindings.VoicePath;
            foreach (string file in Directory.GetFiles(dir + "/", "*.xml"))
            {
                Voice voice = new Voice();
                voice.Load(file);
                if (voice.BackendSupported)
                {
                    toolStripComboBox1.Items.Add(voice.DisplayName);
                    Voices.Add(voice);
                }

            }
            if (i < 0)
                toolStripComboBox1.SelectedIndex = toolStripComboBox1.Items.IndexOf(Qaryan.GUI.Settings.Default.Voice);
            else
                toolStripComboBox1.SelectedIndex = i;
            toolStripComboBox1.Items.Add(Resources.GetMoreVoices);
        }

        public Form1()
        {
            myEngine = new QaryanEngine();
            myEngine.LogLine += new LogLineHandler(myEngine_LogLine);
            
            try
            {
                Resources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Settings.Default.Culture);

                Application.CurrentInputLanguage = InputLanguage.FromCulture(System.Globalization.CultureInfo.GetCultureInfo("he-IL"));
            }
            catch
            {
                // HACK: Mono chokes on an exception here but otherwise runs nicely.
                // Until the issue is resolved more elegantly, this catch{} block is what we have.
            }

            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            WindowState = Settings.Default.Form1State;

            if (File.Exists(Path.Combine(FileBindings.EnginePath, Settings.Default.StartupFile)))
            {
                textBox1.Text = File.ReadAllText(Path.Combine(FileBindings.EnginePath, Settings.Default.StartupFile), System.Text.Encoding.UTF8);
                setFileName(Path.Combine(FileBindings.EnginePath, Settings.Default.StartupFile));
                textBox1.Modified = false;
                Settings.Default.StartupFile = "";
                Settings.Default.Save();
            }
            else
            {
                textBox1.Clear();
                setFileName(null);
            }

            toolStripMenuItem1.Checked =
                Settings.Default.RelaxAudibleSchwa;

            strongDageshMenuItem.Checked =
                Settings.Default.DistinguishStrongDagesh;

            everydayMenuItem.Checked =
                Settings.Default.EverydayRegister;

            milelMenuItem.Checked =
                Settings.Default.Milel;

            akanyeIkanyeToolStripMenuItem.Checked =
                Settings.Default.AkanyeIkanye;

            LoadVoices();
            Form1.CheckForIllegalCrossThreadCalls = false;
        }

        void myEngine_LogLine(ILogSource sender, string message, LogLevel visibility)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("{0}\t{1}", sender.Name, message)); 
        }

        PointPairList pcmds, acmds, pitch, pcomp;

        void OnPhraseCommand(double t, double a)
        {
            pcmds.Add(new PointPair(t, a));
        }

        void OnPitchPoint(double t, double p)
        {
            pitch.Add(new PointPair(t, p));
        }

        void OnPhraseComponent(double t, double p)
        {
            pcomp.Add(new PointPair(t, p));
        }

        void OnAccentCommand(double t1, double t2, double a)
        {
            acmds.Add(t1, a);
            acmds.Add(t2, 0);
        }

        System.Windows.Forms.Label MkLabel(string text)
        {
            System.Windows.Forms.Label l = new System.Windows.Forms.Label();
            l.Text = text;
            l.Margin = Padding.Empty;
            l.BorderStyle = BorderStyle.FixedSingle;
            l.Width = 42;
            l.Height = 21;
            l.TextAlign = ContentAlignment.MiddleCenter;
            l.ForeColor = Color.Black;
            l.Font = new Font(l.Font, FontStyle.Bold);
            return l;
        }

        System.Windows.Forms.Label MkLabel()
        {
            System.Windows.Forms.Label l = MkLabel("");
            l.Width = 21;
            l.ForeColor = Color.Blue;
            return l;
        }

        void Form1Load(object sender, EventArgs e)
        {
            Size = Settings.Default.Form1Size;

            fujisakiForm = new FujisakiForm();
            CreateGraph(fujisakiForm.zedGraphControl1);
            fujisakiForm.Visible = Settings.Default.FujisakiFormVisible;
            if (fujisakiForm.Visible)
                fujisakiForm.BringToFront();
            translitForm = new TranslitForm();
            translitForm.Visible = Settings.Default.TranslitFormVisible;
            if (translitForm.Visible)
                translitForm.BringToFront();
            mbrolaForm = new MbrolaForm();
            mbrolaForm.Visible = Settings.Default.MbrolaFormVisible;
            if (mbrolaForm.Visible)
                mbrolaForm.BringToFront();
            nikudHelp = new NikudHelp();
            nikudHelp.Visible = Settings.Default.NikudHelpVisible;
            if (nikudHelp.Visible)
                nikudHelp.BringToFront();
            textBox1.UseNikudMethod = nikudAssistanceToolStripMenuItem.Checked = Settings.Default.UseNikudMethod;

            //            timer1.Enabled = true;
            this.Activate();
        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
            // Set the Titles
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.Title.Text = Resources.GraphTitle;
            myPane.XAxis.Title.Text = "Time, ms";
            myPane.YAxis.Title.Text = "f0(t) [Hz]";
            myPane.Y2Axis.Title.Text = "Cmd";
            myPane.Y2Axis.IsVisible = true;
            myPane.Y2Axis.Scale.Min = 0;
            myPane.Y2Axis.Scale.Max = 1.5;
            LineItem lineItem;
            lineItem = new LineItem("f0(t)");
            lineItem.Line.Width = 2;
            lineItem.Color = Color.DarkBlue;
            lineItem.Symbol.IsVisible = false;

            myPane.CurveList.Add(lineItem);

            lineItem = new LineItem("P. component (t)");
            lineItem.Line.Width = 2;
            lineItem.Color = Color.DarkCyan;
            lineItem.Symbol.IsVisible = false;
            myPane.CurveList.Add(lineItem);

            lineItem = new LineItem("A. cmd");
            lineItem.Color = Color.Chocolate;
            lineItem.Symbol.IsVisible = false;
            lineItem.IsY2Axis = true;
            lineItem.Line.StepType = StepType.ForwardSegment;
            myPane.CurveList.Add(lineItem);

            StickItem stickItem = new StickItem("P. cmd");
            stickItem.Line.Width = 1.5f;
            stickItem.Color = Color.Chartreuse;
            stickItem.Symbol.Type = SymbolType.Diamond;
            stickItem.Symbol.IsVisible = true;
            stickItem.IsY2Axis = true;
            myPane.CurveList.Add(stickItem);

        }

        void updateGraph(ZedGraphControl control)
        {
            control.GraphPane.CurveList[0].Points = pitch;
            control.GraphPane.CurveList[1].Points = pcomp;
            control.GraphPane.CurveList[2].Points = acmds;
            control.GraphPane.CurveList[3].Points = pcmds;
            // Tell ZedGraph to refigure the
            // axes since the data have changed
            control.AxisChange();
            control.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            updateGraph(fujisakiForm.zedGraphControl1);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void יציאהToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        bool initFlag = false;

        TranslitListener translit;

        private void InitTTSObjects()
        {
            if (initFlag)
                return;
            initFlag = true;
            MBROLASynthesizerBase synth = (myEngine.Backend as MbrolaBackend).Synthesizer as MBROLASynthesizerBase;
            acmds = new PointPairList();
            pcmds = new PointPairList();
            pitch = new PointPairList();
            pcomp = new PointPairList();

            FujisakiProcessor fujisaki = (myEngine.Frontend as StandardFrontend).FujisakiProcessor;
            Parser parser = (myEngine.Frontend as StandardFrontend).Parser;

            fujisaki.PhraseCommand += OnPhraseCommand;
            fujisaki.AccentCommand += OnAccentCommand;
            fujisaki.PitchPointComputed += OnPitchPoint;
            fujisaki.PhraseComponentComputed += OnPhraseComponent;
            fujisaki.NoMoreData += delegate { updateGraph(fujisakiForm.zedGraphControl1); };
            
            translit = new TranslitListener(parser);
            translit.TranslitUpdated += new StringEventHandler(translit_TranslitUpdated);
            MBROLAListener mbrolaListener = new MBROLAListener(synth);
            mbrolaListener.TextUpdated += new StringEventHandler(mbrolaListener_TextUpdated);
        }

        private void UpdateTTSOptions()
        {
            translit.Clear();
            mbrolaForm.MbrolaText = "";
            acmds.Clear();
            pcmds.Clear();
            pitch.Clear();
            pcomp.Clear();

            FujisakiProcessor fujisaki = (myEngine.Frontend as StandardFrontend).FujisakiProcessor;
            Segmenter segmenter = (myEngine.Frontend as StandardFrontend).Segmenter;
            Phonetizer phonetizer = (myEngine.Frontend as  StandardFrontend).Phonetizer;
            Parser parser = (myEngine.Frontend as  StandardFrontend).Parser;

            fujisaki.Model.alpha = Settings.Default.FujisakiAlpha;
            fujisaki.Model.beta = Settings.Default.FujisakiBeta;
            fujisaki.Model.gamma = Settings.Default.FujisakiGamma;
            fujisaki.Model.Fb = Settings.Default.FujisakiFb;
           
            segmenter.RelaxAudibleSchwa = Settings.Default.RelaxAudibleSchwa;
            phonetizer.Options.DistinguishStrongDagesh = Settings.Default.DistinguishStrongDagesh;
            phonetizer.Options.Akanye = phonetizer.Options.Ikanye = Settings.Default.AkanyeIkanye;
            //            phonetizer.Options.SingCantillation = checkedListBox1.GetItemChecked(5);
            parser.Options.EverydayRegister = Settings.Default.EverydayRegister;

            if (Settings.Default.Milel)
                segmenter.DefaultStress = Word.Stress.Milel;
            else
                segmenter.DefaultStress = Word.Stress.Milra;
        }

        void mbrolaListener_TextUpdated(object sender, string value)
        {
            mbrolaForm.MbrolaText = value;
        }

        void translit_TranslitUpdated(object sender, string value)
        {
            translitForm.TranslitText = value;
        }

        private void דברToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            if (textBox1.SelectionLength > 0)
                text = textBox1.SelectedText;
            if (!myEngine.IsSpeaking)
            {
                UpdateTTSOptions();
                myEngine.Speak(text);
            }
        }

        private void דברלקובץToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveWavDialog.ShowDialog();
        }

        private void saveWavDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = textBox1.Text;
            if (textBox1.SelectionLength > 0)
                text = textBox1.SelectedText;
            if (!myEngine.IsSpeaking)
            {
                UpdateTTSOptions();
                myEngine.SpeakToWavFile(text, saveWavDialog.FileName);
            }
        }

        private void קולToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadVoices();
        }

        private void גרףהנגנהToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fujisakiForm.Show();
            fujisakiForm.Activate();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            FujisakiProcessor fujisaki = (myEngine.Frontend as StandardFrontend).FujisakiProcessor;
            FujisakiParams paramsWindow = new FujisakiParams();
            paramsWindow.Alpha = fujisaki.Model.alpha;
            paramsWindow.Beta = fujisaki.Model.beta;
            paramsWindow.Gamma = fujisaki.Model.gamma;
            paramsWindow.Fb = fujisaki.Model.Fb;
            if (paramsWindow.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.FujisakiAlpha = fujisaki.Model.alpha = paramsWindow.Alpha;
                Settings.Default.FujisakiBeta = fujisaki.Model.beta = paramsWindow.Beta;
                Settings.Default.FujisakiGamma = fujisaki.Model.gamma = paramsWindow.Gamma;
                Settings.Default.FujisakiFb = fujisaki.Model.Fb = paramsWindow.Fb;
                Settings.Default.Save();
            }
        }

        private void תעתיקToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translitForm.Show();
            translitForm.Activate();
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void textBox1_CtrlEnter(object sender, EventArgs e)
        {
            דברToolStripMenuItem_Click(sender, e);
        }

        private void mBROLAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mbrolaForm.Show();
            mbrolaForm.Activate();
        }

        private void toolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.No;
            if (textBox1.Modified)
                res = MessageBox.Show(Resources.SaveChangesQuery, Resources.AppName, MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
                return;
            if (res == DialogResult.Yes)
                saveMenuItem.PerformClick();
            textBox1.Text = "";
            textBox1.Modified = false;
            setFileName(null);
        }

        private void שמירהMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName == null)
                toolStripMenuItem4.PerformClick();
            else
            {
                File.WriteAllText(fileName, textBox1.Text);
                textBox1.Modified = false;
            }
        }

        void setFileName(string path)
        {
            fileName = path;
            if (fileName != null)
                Text = Path.GetFileName(path) + " - " + Resources.AppName + " " + Application.ProductVersion;
            else
                Text = Resources.Untitled + " - " + Resources.AppName + " " + Application.ProductVersion;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.No;
            if (textBox1.Modified)
                res = MessageBox.Show(Resources.SaveChangesQuery, Resources.AppName, MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
                return;
            if (res == DialogResult.Yes)
                saveMenuItem.PerformClick();
            openFileDialog1.ShowDialog();
        }

        private void פתיחה_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            textBox1.Text = File.ReadAllText(openFileDialog1.FileName, System.Text.Encoding.UTF8);
            setFileName(openFileDialog1.FileName);
            textBox1.Modified = false;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText(saveFileDialog1.FileName, textBox1.Text, System.Text.Encoding.UTF8);
            setFileName(saveFileDialog1.FileName);
            textBox1.Modified = false;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        private void אנגליתToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.No;
            if (textBox1.Modified)
                res = MessageBox.Show(Resources.SaveChangesQuery, Resources.AppName, MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
                return;
            if (res == DialogResult.Yes)
                saveMenuItem.PerformClick();
            Settings.Default.Culture = "en";
            Settings.Default.Save();
            Application.Restart();
        }

        private void עבריתToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.No;
            if (textBox1.Modified)
                res = MessageBox.Show(Resources.SaveChangesQuery, Resources.AppName, MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
                return;
            if (res == DialogResult.Yes)
                saveMenuItem.PerformClick();
            Settings.Default.Culture = "he";
            Settings.Default.Save();
            Application.Restart();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.MbrolaFormVisible = mbrolaForm.Visible;
            Settings.Default.FujisakiFormVisible = fujisakiForm.Visible;
            Settings.Default.TranslitFormVisible = translitForm.Visible;
            Settings.Default.NikudHelpVisible = nikudHelp.Visible;
            Settings.Default.UseNikudMethod = nikudAssistanceToolStripMenuItem.Checked;
            Settings.Default.Form1State = WindowState;
            Settings.Default.Save();

            DialogResult res = DialogResult.No;
            if (textBox1.Modified)
                res = MessageBox.Show(Resources.SaveChangesQuery, Resources.AppName, MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
                e.Cancel = true;
            if (res == DialogResult.Yes)
                saveMenuItem.PerformClick();
        }

        int lastSelectedIndex = -1;

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

               if ((toolStripComboBox1.Items.Count>0) && (toolStripComboBox1.SelectedIndex == toolStripComboBox1.Items.Count - 1))
               {
                   System.Diagnostics.Process p = new System.Diagnostics.Process();
                   p.StartInfo.FileName = Resources.WikiBase + Resources.WikiVoicesPage;
                   p.Start();
                   toolStripComboBox1.SelectedIndex = lastSelectedIndex;
               }
               else
                lastSelectedIndex = toolStripComboBox1.SelectedIndex;
            if ((toolStripComboBox1.SelectedIndex < Voices.Count) && (toolStripComboBox1.SelectedIndex >= 0))
            {
                myEngine.Voice = Voices[toolStripComboBox1.SelectedIndex];
                InitTTSObjects();
            }
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            Settings.Default.Font = textBox1.Font = fontDialog1.Font;
            Settings.Default.Save();
        }

        private void גופןToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
                fontDialog1_Apply(fontDialog1, new EventArgs());
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
                Settings.Default.Form1Size = Size;
        }

        private void ניקודToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nikudHelp.Show();
        }

        private void nikudAssistanceToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.UseNikudMethod = nikudAssistanceToolStripMenuItem.Checked;
        }

        private void toolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem senderItem = (sender as ToolStripMenuItem);
            switch (senderItem.Name)
            {
                case "toolStripMenuItem1":
                    Settings.Default.RelaxAudibleSchwa = senderItem.Checked;
                    break;
                case "strongDageshMenuItem":
                    Settings.Default.DistinguishStrongDagesh = senderItem.Checked;
                    break;
                case "everydayMenuItem":
                    Settings.Default.EverydayRegister = senderItem.Checked;
                    break;
                case "milelMenuItem":
                    Settings.Default.Milel = senderItem.Checked;
                    break;
                case "akanyeIkanyeToolStripMenuItem":
                    Settings.Default.AkanyeIkanye = senderItem.Checked;
                    break;
            }
            Settings.Default.Save();
        }

        static string MakeLangUrlParam()
        {
            return "lang=" + Settings.Default.Culture;
        }

        private void webSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "http://hebtts.sf.net/?" +MakeLangUrlParam();
            p.Start();
        }
    }
}
