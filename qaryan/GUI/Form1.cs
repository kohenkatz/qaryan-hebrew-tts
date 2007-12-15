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
using System.Xml;
using System.Xml.Xsl;

namespace Qaryan.GUI
{
    /// <summary>
    /// Description of Form1.
    /// </summary>
    public partial class Form1 : Form
    {
        List<MBROLAVoice> Voices = new List<MBROLAVoice>();
        FujisakiForm fujisakiForm;
        TranslitForm translitForm;
        MbrolaForm mbrolaForm;
        NikudHelp nikudHelp;
        string fileName = null;

        void LoadVoices()
        {
            int i = toolStripComboBox1.SelectedIndex;
            toolStripComboBox1.Items.Clear();
            Voices.Clear();
            string dir = FileBindings.VoicePath;
            foreach (string file in Directory.GetFiles(dir + "/", "*.xml"))
            {
                MBROLAVoice voice = new MBROLAVoice();
                voice.LoadFromXml(file);
                if (voice.Activate())
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
            Resources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Settings.Default.Culture);
            Application.CurrentInputLanguage = InputLanguage.FromCulture(System.Globalization.CultureInfo.GetCultureInfo("he-IL"));
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

            ‰‚ÈÈ˙ÓÎÙÏToolStripMenuItem.Checked =
                Settings.Default.DistinguishStrongDagesh;

            ‰‚ÈÈ‰ÈÂÓÈÂÓÈ˙ToolStripMenuItem.Checked =
                Settings.Default.EverydayRegister;

            ‰‚ÈÈ‰ÓÏÚÈÏÈ˙ToolStripMenuItem.Checked =
                Settings.Default.Milel;

            akanyeIkanyeToolStripMenuItem.Checked =
                Settings.Default.AkanyeIkanye;

            LoadVoices();
            Form1.CheckForIllegalCrossThreadCalls = false;
            fujisaki = new FujisakiProcessor();

            fujisaki.PhraseCommand += OnPhraseCommand;
            fujisaki.AccentCommand += OnAccentCommand;
            fujisaki.PitchPointComputed += OnPitchPoint;
            fujisaki.PhraseComponentComputed += OnPhraseComponent;
            fujisaki.NoMoreData += delegate { updateGraph(fujisakiForm.zedGraphControl1); };
        }

        Tokenizer tokenizer = new Tokenizer();
        Parser parser = new Parser();
        Segmenter segmenter = new Segmenter();
        Phonetizer phonetizer = new Phonetizer();
        MBROLATranslator translator = new MBROLATranslator();
        MBROLASynthesizer synth = new MBROLASynthesizer();
        AudioTarget target;
        FujisakiProcessor fujisaki;

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

        List<ProgressAnim> progAnims;

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
            string[] componentStrings = new string[] {
                "TOK","PAR","SEG","PHO","FUJ","TRA","MBR","AUD"
            };
            progAnims = new List<ProgressAnim>();
            for (int j = 0; j < componentStrings.Length; j++)
            {
                flowLayoutPanel1.Controls.Add(MkLabel(componentStrings[j]));
                if (j < componentStrings.Length - 1)
                {
                    ProgressAnim anim = new ProgressAnim();
                    anim.Animation = imageList1;
                    anim.Width = anim.Height = 21;
                    anim.Margin = Padding.Empty;
                    flowLayoutPanel1.Controls.Add(anim);
                    progAnims.Add(anim);

                    flowLayoutPanel1.Controls.Add(MkLabel());

                    anim = new ProgressAnim();
                    anim.Animation = imageList1;
                    anim.Width = anim.Height = 21;
                    anim.Margin = Padding.Empty;
                    flowLayoutPanel1.Controls.Add(anim);
                    progAnims.Add(anim);
                }
            }
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

        private void ÈˆÈ‡‰ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnDoneProducing(object sender, EventArgs e)
        {
            ToolStripStatusLabel sl = null;

            if (sender == tokenizer)
                sl = toolStripStatusLabel1;
            else if (sender == parser)
                sl = toolStripStatusLabel2;

            else if (sender == segmenter)
                sl = toolStripStatusLabel3;

            else if (sender == translator)
                sl = toolStripStatusLabel6;

            else if (sender == synth)
                sl = toolStripStatusLabel7;

            else if (sender == phonetizer)
                sl = toolStripStatusLabel4;

            else if (sender == fujisaki)
                sl = toolStripStatusLabel5;

            if (sl != null)
                sl.BackColor = Color.Red;
        }

        private void OnDSoundAudioFinished()
        {
            toolStripStatusLabel8.BackColor = Color.Red;
        }

        XmlLogger logger;

        private void InitTTSObjects()
        {
            logger = new XmlLogger();
            XmlLog = new MemoryStream();

            logger.Add(fujisaki);
            logger.Writer = XmlWriter.Create(XmlLog);
            logger.Start();



            
       
            


            toolStripStatusLabel1.BackColor = toolStripStatusLabel2.BackColor = toolStripStatusLabel3.BackColor = toolStripStatusLabel4.BackColor = toolStripStatusLabel5.BackColor = toolStripStatusLabel6.BackColor = toolStripStatusLabel7.BackColor = toolStripStatusLabel8.BackColor = Color.Green;

            tokenizer = new Tokenizer();
            logger.Add(tokenizer);
            tokenizer.DoneProducing += OnDoneProducing;
            parser = new Parser();
            logger.Add(parser);
            parser.DoneProducing += OnDoneProducing;
            segmenter = new Segmenter();
            logger.Add(segmenter);
            segmenter.DoneProducing += OnDoneProducing;
            phonetizer = new Phonetizer();
            logger.Add(phonetizer);
            phonetizer.DoneProducing += OnDoneProducing;
            translator = new MBROLATranslator();
            logger.Add(translator);
            translator.DoneProducing += OnDoneProducing;
            synth = new MBROLASynthesizer();
            synth.DoneProducing += OnDoneProducing;
            logger.Add(synth);

            tokenizer.ItemProduced += delegate(Producer<Token> p, ItemEventArgs<Token> e)
                {
                    progAnims[0].Ping();
                };
            tokenizer.DoneProducing += delegate(object sender, EventArgs e)
            {
                progAnims[0].Stop();
            };
            parser.ItemConsumed += delegate(ThreadedConsumer<Token> c, ItemEventArgs<Token> e)
                {
                    progAnims[1].Ping();
                };
            parser.DoneConsuming += delegate(object sender, EventArgs e)
                {
                    progAnims[1].Stop();
                };
            parser.ItemProduced += delegate(Producer<SpeechElement> p, ItemEventArgs<SpeechElement> e)
                {
                    progAnims[2].Ping();
                };
            parser.DoneProducing += delegate(object sender, EventArgs e)
            {
                progAnims[2].Stop();
            };
            segmenter.ItemConsumed += delegate(ThreadedConsumer<SpeechElement> c, ItemEventArgs<SpeechElement> e)
                {
                    progAnims[3].Ping();
                };
            segmenter.DoneConsuming += delegate(object sender, EventArgs e)
                {
                    progAnims[3].Stop();
                };
            segmenter.ItemProduced += delegate(Producer<Segment> p, ItemEventArgs<Segment> e)
                {
                    progAnims[4].Ping();
                };
            segmenter.DoneProducing += delegate(object sender, EventArgs e)
            {
                progAnims[4].Stop();
            };
            phonetizer.ItemConsumed += delegate(ThreadedConsumer<Segment> c, ItemEventArgs<Segment> e)
                {
                    progAnims[5].Ping();
                };
            phonetizer.DoneConsuming += delegate(object sender, EventArgs e)
                {
                    progAnims[5].Stop();
                };
            phonetizer.ItemProduced += delegate(Producer<Phone> p, ItemEventArgs<Phone> e)
                {
                    progAnims[6].Ping();
                };
            phonetizer.DoneProducing += delegate(object sender, EventArgs e)
            {
                progAnims[6].Stop();
            };
            fujisaki.ItemConsumed += delegate(ThreadedConsumer<Phone> c, ItemEventArgs<Phone> e)
                {
                    progAnims[7].Ping();
                };
            fujisaki.DoneConsuming += delegate(object sender, EventArgs e)
                {
                    progAnims[7].Stop();
                };
            fujisaki.ItemProduced += delegate(Producer<Phone> p, ItemEventArgs<Phone> e)
                {
                    progAnims[8].Ping();
                };
            fujisaki.DoneProducing += delegate(object sender, EventArgs e)
            {
                progAnims[8].Stop();
            };
            translator.ItemConsumed += delegate(ThreadedConsumer<Phone> c, ItemEventArgs<Phone> e)
                {
                    progAnims[9].Ping();
                };
            translator.DoneConsuming += delegate(object sender, EventArgs e)
                {
                    progAnims[9].Stop();
                };
            translator.ItemProduced += delegate(Producer<MBROLAElement> p, ItemEventArgs<MBROLAElement> e)
                {
                    progAnims[10].Ping();
                };
            translator.DoneProducing += delegate(object sender, EventArgs e)
            {
                progAnims[10].Stop();
            };
            synth.ItemConsumed += delegate(ThreadedConsumer<MBROLAElement> c, ItemEventArgs<MBROLAElement> e)
                {
                    progAnims[11].Ping();
                };
            synth.DoneConsuming += delegate(object sender, EventArgs e)
                {
                    progAnims[11].Stop();
                };
            synth.ItemProduced += delegate(Producer<AudioBufferInfo> p, ItemEventArgs<AudioBufferInfo> e)
                {
                    progAnims[12].Ping();
                };
            synth.DoneProducing += delegate(object sender, EventArgs e)
            {
                progAnims[12].Stop();
            };
            acmds = new PointPairList();
            pcmds = new PointPairList();
            pitch = new PointPairList();
            pcomp = new PointPairList();

            //            fujisaki = new FujisakiProcessor();
            fujisaki.Model.alpha = Settings.Default.FujisakiAlpha;
            fujisaki.Model.beta = Settings.Default.FujisakiBeta;
            fujisaki.Model.gamma = Settings.Default.FujisakiGamma;
            fujisaki.Model.Fb = Settings.Default.FujisakiFb;
            fujisaki.Reset();

            fujisaki.DoneProducing += OnDoneProducing;

            fujisaki.PhraseCommand += OnPhraseCommand;
            fujisaki.AccentCommand += OnAccentCommand;
            fujisaki.PitchPointComputed += OnPitchPoint;
            fujisaki.PhraseComponentComputed += OnPhraseComponent;
            fujisaki.NoMoreData += delegate { updateGraph(fujisakiForm.zedGraphControl1); };

            translator.Voice = synth.Voice = Voices[toolStripComboBox1.SelectedIndex];
            segmenter.RelaxAudibleSchwa = Settings.Default.RelaxAudibleSchwa;
            phonetizer.Options.DistinguishStrongDagesh = Settings.Default.DistinguishStrongDagesh;
            phonetizer.Options.Akanye = phonetizer.Options.Ikanye = Settings.Default.AkanyeIkanye;
            //            phonetizer.Options.SingCantillation = checkedListBox1.GetItemChecked(5);
            parser.Options.EverydayRegister = Settings.Default.EverydayRegister;

            if (Settings.Default.Milel)
                segmenter.DefaultStress = Word.Stress.Milel;
            else
                segmenter.DefaultStress = Word.Stress.Milra;
            TranslitListener translit = new TranslitListener(parser);
            translit.TranslitUpdated += new StringEventHandler(translit_TranslitUpdated);
            MBROLAListener mbrolaListener = new MBROLAListener(synth);
            mbrolaListener.TextUpdated += new StringEventHandler(mbrolaListener_TextUpdated);
        }

        void mbrolaListener_TextUpdated(object sender, string value)
        {
            mbrolaForm.MbrolaText = value;
        }

        void translit_TranslitUpdated(object sender, string value)
        {
            translitForm.TranslitText = value;
        }

        MemoryStream XmlLog;


        private void „·¯ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitTTSObjects();
            string text = textBox1.Text;
            if (textBox1.SelectionLength > 0)
                text = textBox1.SelectedText;

            target = new DSoundAudioTarget(this);
            logger.Add(target);
            target.AudioFinished += OnDSoundAudioFinished;
            target.ItemConsumed += delegate(ThreadedConsumer<AudioBufferInfo> c, ItemEventArgs<AudioBufferInfo> ee)
                {
                    progAnims[13].Ping();
                };
            target.DoneConsuming += delegate(object s, EventArgs ee)
                {
                    progAnims[13].Stop();
                };

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.Encoding = System.Text.Encoding.UTF8;

            LogForm logForm = new LogForm(XmlLog);

            target.AudioFinished += delegate
            {
                XslCompiledTransform xslt = new XslCompiledTransform(true);
                MemoryStream strr = new MemoryStream(Resources.QaryanLog);
                strr.Seek(0, SeekOrigin.Begin);
                XsltSettings xslts = new XsltSettings();
                xslts.EnableScript = true;
                xslts.EnableDocumentFunction = false;
                xslt.Load(XmlReader.Create(strr), xslts, null);
                strr.Close();
                strr = new MemoryStream();
                //  xslt.Load(XmlReader.Create());
                logger.Stop();
                logger.Writer = null;
                FileStream fs = File.Create("logs/log1.xml");
                XmlLog.WriteTo(fs);
                fs.Close();
                XmlLog.Seek(0, SeekOrigin.Begin);
                XmlReader reader = XmlReader.Create(XmlLog);
                strr.Seek(0, SeekOrigin.Begin);
                XmlWriter writer = XmlWriter.Create(strr);
                xslt.Transform(reader, writer);
                strr.Seek(0, SeekOrigin.Begin);
                logForm.webBrowser1.DocumentStream = strr;
                logForm.Show();
            };
            tokenizer.Run(text);
            parser.Run(tokenizer);
            segmenter.Run(parser);
            phonetizer.Run(segmenter);
            fujisaki.Run(phonetizer);
            translator.Run(fujisaki);
            synth.Run(translator);
            target.Run(synth);
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void „·¯Ï˜Â·ıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveWavDialog.ShowDialog();
        }


        private void OnFileAudioFinished()
        {
            OnDSoundAudioFinished();
            System.Diagnostics.Process.Start(((WaveFileAudioTarget)target).Filename);
        }

        private void saveWavDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            InitTTSObjects();
            string text = textBox1.Text;
            if (textBox1.SelectionLength > 0)
                text = textBox1.SelectedText;

            target = new WaveFileAudioTarget();
            logger.Add(target);     
            ((WaveFileAudioTarget)target).Filename = saveWavDialog.FileName;

            target.ItemConsumed += delegate(ThreadedConsumer<AudioBufferInfo> c, ItemEventArgs<AudioBufferInfo> ee)
                {
                    progAnims[13].Ping();
                };
            target.DoneConsuming += delegate(object s, EventArgs ee)
                {
                    progAnims[13].Stop();
                };

            target.AudioFinished += OnFileAudioFinished;

            tokenizer.Run(text);
            parser.Run(tokenizer);
            segmenter.Run(parser);
            phonetizer.Run(segmenter);
            fujisaki.Run(phonetizer);
            translator.Run(fujisaki);
            synth.Run(translator);
            target.Run(synth);
        }

        private void ˜ÂÏToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadVoices();
        }

        private void ‰˘˙˜ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (target != null)
                target.Stop();
        }

        private void ‚¯Û‰‚‰ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fujisakiForm.Show();
            fujisakiForm.Activate();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
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

        private void ˙Ú˙È˜ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translitForm.Show();
            translitForm.Activate();
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void textBox1_CtrlEnter(object sender, EventArgs e)
        {
            „·¯ToolStripMenuItem_Click(sender, e);
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
                ˘ÓÈ¯‰MenuItem.PerformClick();
            textBox1.Text = "";
            textBox1.Modified = false;
            setFileName(null);
        }

        private void ˘ÓÈ¯‰MenuItem_Click(object sender, EventArgs e)
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
                ˘ÓÈ¯‰MenuItem.PerformClick();
            openFileDialog1.ShowDialog();
        }

        private void Ù˙ÈÁ‰_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
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

        private void ‡‚ÏÈ˙ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.No;
            if (textBox1.Modified)
                res = MessageBox.Show(Resources.SaveChangesQuery, Resources.AppName, MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
                return;
            if (res == DialogResult.Yes)
                ˘ÓÈ¯‰MenuItem.PerformClick();
            Settings.Default.Culture = "en";
            Settings.Default.Save();
            Application.Restart();
        }

        private void Ú·¯È˙ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.No;
            if (textBox1.Modified)
                res = MessageBox.Show(Resources.SaveChangesQuery, Resources.AppName, MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
                return;
            if (res == DialogResult.Yes)
                ˘ÓÈ¯‰MenuItem.PerformClick();
            Settings.Default.Culture = "he";
            Settings.Default.Save();
            Application.Restart();
        }

        private void ˘Ù˙ÓÓ˘˜Ó˘˙Ó˘ToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                ˘ÓÈ¯‰MenuItem.PerformClick();
        }

        int lastSelectedIndex = -1;

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (toolStripComboBox1.SelectedIndex == toolStripComboBox1.Items.Count - 1)
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = Resources.WikiBase + Resources.WikiVoicesPage;
                p.Start();
                toolStripComboBox1.SelectedIndex = lastSelectedIndex;
            }
            else
                lastSelectedIndex = toolStripComboBox1.SelectedIndex;
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            Settings.Default.Font = textBox1.Font = fontDialog1.Font;
            Settings.Default.Save();
        }

        private void ‚ÂÙÔToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
                fontDialog1_Apply(fontDialog1, new EventArgs());
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
                Settings.Default.Form1Size = Size;
        }

        private void ‡Â„Â˙ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void È˜Â„ToolStripMenuItem_Click(object sender, EventArgs e)
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
                case "‰‚ÈÈ˙ÓÎÙÏToolStripMenuItem":
                    Settings.Default.DistinguishStrongDagesh = senderItem.Checked;
                    break;
                case "‰‚ÈÈ‰ÈÂÓÈÂÓÈ˙ToolStripMenuItem":
                    Settings.Default.EverydayRegister = senderItem.Checked;
                    break;
                case "‰‚ÈÈ‰ÓÏÚÈÏÈ˙ToolStripMenuItem":
                    Settings.Default.Milel = senderItem.Checked;
                    break;
                case "akanyeIkanyeToolStripMenuItem":
                    Settings.Default.AkanyeIkanye = senderItem.Checked;
                    break;
            }
            Settings.Default.Save();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (ProgressAnim anim in progAnims)
            {
                anim.Refresh();
            }
        }

    }
}
