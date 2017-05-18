using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MyHealth
{
    public partial class Find : Form
    {
        
        
        // const int retmax = 5;
        string conteudo = "";
        SpeechSynthesizer sSynth = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();

        public Find()

        {
            InitializeComponent();
            start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear(); 

            string urlfinal = MyHealth.Properties.Settings.Default.url.ToString() + textBox1.Text + "&retmax=" + MyHealth.Properties.Settings.Default.retmax.ToString();

            var result = new WebClient();   
            result.DownloadStringAsync(new Uri(urlfinal));
            result.DownloadStringCompleted += Result_DownloadStringCompleted;
        }
        private void start()
        {


            Choices sList = new Choices();
            sList.Add(new string[] { "hello", "find", "start", "monitoring", "login", "exit", "close", "quit", "zero", "one","uno", "two", "three", "four", "five", "six", "clear", "cancer", "virus","bacteria", "broken", "leg", "read" });
            Grammar gr = new Grammar(new GrammarBuilder(sList));
            try
            {
                sRecognize.RequestRecognizerUpdate();
                sRecognize.LoadGrammar(gr);
                sRecognize.SpeechRecognized += sRecognize_SpeechRecognized;
                sRecognize.SetInputToDefaultAudioDevice();
                sRecognize.RecognizeAsync(RecognizeMode.Multiple);
                sRecognize.Recognize();


            }

            catch
            {
                return;
            }
        }
        private void sRecognize_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "exit")
            {
                Application.Exit();
            }
            if (e.Result.Text == "find")
            {

                button1.PerformClick(); 
            }
          
        
            
        if (e.Result.Text == "zero" && listView1.Items.Count != 0)
            {
                listView1.SelectedIndices.Clear();
                listView1.Items[0].Selected = true;
                listView1_SelectedIndexChanged(sender, e);
            }

            if (e.Result.Text == "one" || e.Result.Text == "uno" && listView1.Items.Count != 0)
            {
                listView1.SelectedIndices.Clear();
                listView1.Items[1].Selected = true;
                listView1_SelectedIndexChanged(sender, e);
            }

            if (e.Result.Text == "two" && listView1.Items.Count != 0)
            {
                listView1.SelectedIndices.Clear();
                listView1.Items[2].Selected = true;
                listView1_SelectedIndexChanged(sender, e);
            }

            if (e.Result.Text == "three" && listView1.Items.Count != 0)
            {
                listView1.SelectedIndices.Clear();
                listView1.Items[3].Selected = true;
                listView1_SelectedIndexChanged(sender, e);
            }

            if (e.Result.Text == "four" && listView1.Items.Count != 0)
            {
                listView1.SelectedIndices.Clear();
                listView1.Items[4].Selected = true;
                listView1_SelectedIndexChanged(sender, e);
            }

            if (e.Result.Text == "five" && listView1.Items.Count != 0)
            {
                listView1.SelectedIndices.Clear();
                listView1.Items[5].Selected = true;
                listView1_SelectedIndexChanged(sender, e);
            }

            if (e.Result.Text == "six" && listView1.Items.Count != 0)
            {
                listView1.SelectedIndices.Clear();
                listView1.Items[6].Selected = true;
                listView1_SelectedIndexChanged(sender, e);
            }

            if (e.Result.Text == "clear")
            {
                textBox1.Clear();
            }    
            if (e.Result.Text == "read")
            {
                button2.PerformClick(); 
            }               
            else
            {
                textBox1.Text = e.Result.Text.ToString();
            }


        }

        private void Result_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            
            conteudo = e.Result;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(conteudo);

            XmlNodeList rankings = doc.SelectNodes("//document/@rank");
            XmlNodeList summary = doc.SelectNodes("//document/content[@name='FullSummary']");
            XmlNodeList titles = doc.SelectNodes("//document/content[@name='title']");

           
            if (rankings.Count == 0)
            {
                
                MessageBox.Show("Search not found.");
                textBox1.Clear();
            }
            else
            {
                for (int i = 0; i < MyHealth.Properties.Settings.Default.retmax; i++)
                {
                    
                    ListViewItem linha = new ListViewItem(rankings[i].InnerText, 0);
                    linha.SubItems.Add(Regex.Replace(titles[i].InnerText, @"<[^>]*>", String.Empty));
                    linha.SubItems.Add(summary[i].InnerText);
                    
                    listView1.Items.Add(linha);
                    
                }
            }
        }


        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {


            webBrowser2.DocumentText = conteudo;
           
        }
        
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count != 0)
            {
                
                string rank = listView1.SelectedItems[0].SubItems[0].Text;
                string title = listView1.SelectedItems[0].SubItems[1].Text;
                string summary = listView1.SelectedItems[0].SubItems[2].Text;
                
                
                webBrowser2.DocumentText = (title + "." + Environment.NewLine + summary); 
            }
        }

        private void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
          
            string naoDeixarLerHtml = Regex.Replace(webBrowser2.DocumentText, @"<[^>]*>", String.Empty);

            pBuilder.ClearContent();
            
            pBuilder.AppendText(naoDeixarLerHtml);

            sSynth.Speak(pBuilder);

        }

        private void Find_Load(object sender, EventArgs e)
        {

        }
    }
}
