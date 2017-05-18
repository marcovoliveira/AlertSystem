using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhysiologicParametersDll;
using MyHealth.ServiceRefHealth;
using System.Text.RegularExpressions;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Diagnostics;

namespace MyHealth
{
    public partial class MyHealthApp : Form
    {
        
        ServiceRefHealth.Patient p;
        ServiceHealthClient web;
        private BackgroundWorker bw = new BackgroundWorker();
        enum DataType { Normal, Alerts };
        int parsedValue;

        int NB;
        int NH;
        int NS;

        //Relogios alertas BP
        Stopwatch stopwatchBW = new Stopwatch();
        Stopwatch stopwatchBC = new Stopwatch();
        Stopwatch stopwatchBW2 = new Stopwatch();
        Stopwatch stopwatchBC2 = new Stopwatch();

        //Relogios alertas para HR
        Stopwatch stopwatchHW = new Stopwatch();
        Stopwatch stopwatchHC = new Stopwatch();
        Stopwatch stopwatchHW2 = new Stopwatch();
        Stopwatch stopwatchHC2 = new Stopwatch();

        //Relogios alertas para SPO
        Stopwatch stopwatchSW = new Stopwatch();
        Stopwatch stopwatchSC = new Stopwatch();
        Stopwatch stopwatchSW2 = new Stopwatch();
        Stopwatch stopwatchS2 = new Stopwatch();


        SpeechSynthesizer sSynth = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();

        private void Form1_Load(object sender, EventArgs e)
        {
            sns.Text = MyHealth.Properties.Settings.Default.snsH.ToString();

        }

        private PhysiologicParametersDll.PhysiologicParametersDll dll = null;

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            DataType dt = (DataType)e.Argument;
           
            if (dt == DataType.Normal)
            {
                dll.Initialize(myProcessedMethod, MyHealth.Properties.Settings.Default.ndelay, checkBlood.Checked, checkHeart.Checked, checkOxy.Checked);
            }
            else if (dt == DataType.Alerts)
            {
                dll.InitializeWithAlerts(myProcessedMethod, MyHealth.Properties.Settings.Default.ndelay, checkBlood.Checked, checkHeart.Checked, checkOxy.Checked);
            }
        }

        public MyHealthApp()
        {
            InitializeComponent();
            start();
            bw.DoWork += new DoWorkEventHandler(DoWork);

        }
        private void start()
        {

            
            Choices sList = new Choices();
            sList.Add(new string[] { "hello", "find", "start", "monitoring", "login", "exit", "close", "quit",});
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
           /* if (e.Result.Text == "exit")
            {
                Application.Exit();
            }
            */
            if (e.Result.Text == "find")
            {
               
                findTerms.PerformClick(); 
            }
            if (e.Result.Text == "login")
            {

               buttonValidate.PerformClick();
            }
            if (e.Result.Text == "start" || e.Result.Text == "monitoring")
            {

                button1.PerformClick();
            }
          


        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }



        private void buttonValidate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(sns.Text, out parsedValue))
            {
                MessageBox.Show("Please insert a valid SNS number!");
                return;
            }
            else
            {
                richTextBox2.Clear();
                //validate junto do web service se o utente existe
                //se existir mostrar mensagem de boas vindas
                int snsNumber = int.Parse(sns.Text);



                System.Console.WriteLine(snsNumber);
                web = new ServiceHealthClient();
                p = web.ValidatePatient(snsNumber);


                if (p != null)
                {


                    MessageBox.Show("Bem vindo Sr.(a)" + p.FirstName);
                    groupBox1.Visible = true;

                    //gravar ultimo sns inserido que acedeu a aplicação com sucesso
                    Properties.Settings.Default.snsH = snsNumber;
                    Properties.Settings.Default.Save();

                    string dataU = "Name: " + p.FirstName + "."
                    + Environment.NewLine + "Surname: " + p.LastName + "."
                    + Environment.NewLine + "SNS: " + p.SNS + "."
                    + Environment.NewLine + "Birthdate: " + p.Birthdate.Day + "/" + p.Birthdate.Month + "/" + p.Birthdate.Year + "."
                    + Environment.NewLine + "Age: " + p.Age;


                    richTextBox2.AppendText(dataU);
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Visible = true;
                    // se o utente fez login, então é um utente ativo na aplicação 
                    p.Activo = true;
                    web.ActivatePatient(p);


                }

                else
                {
                    MessageBox.Show("SNS Number doesn't exist! ");
                    button1.Enabled = false;

                }
            }
        }


        private void button1_Click(object sender, EventArgs e)

        {
            
            


            if (checkBox1.Checked)
            {
                
                dll = new PhysiologicParametersDll.PhysiologicParametersDll();
                
                bw.RunWorkerAsync(DataType.Alerts);
                
            }
            else
            {
               
                dll = new PhysiologicParametersDll.PhysiologicParametersDll();
                
                bw.RunWorkerAsync(DataType.Normal);
            }
        }

         
        private void myProcessedMethod(string message)
        {
            
            
            var values = message.Split(';');
            ServiceRefHealth.ServiceHealthClient web = new ServiceHealthClient();
            BPs b = new ServiceRefHealth.BPs();
            SPOes s = new ServiceRefHealth.SPOes();
            HRs h = new ServiceRefHealth.HRs();
            Alertas a = new ServiceRefHealth.Alertas(); 
   
            //comunicar entre threads
            this.BeginInvoke(new MethodInvoker(delegate
            {

                if (values[0].Equals("BP"))
                {
                    var valuesBP = values[1].Split('-');
                    label3.Text = values[1];

                    b.Value1 = Convert.ToInt32(valuesBP[0]);
                    b.Value2 = Convert.ToInt32(valuesBP[1]);
                    b.Date = Convert.ToDateTime(values[2]);
                    b.SNS = Properties.Settings.Default.snsH;
                    label6.Text = values[2];
                    

                    //alertas para hiper e hipotensão
                    if (b.Value1 > 180 || b.Value1 < 100 || b.Value2 < 60)
                    {


                        b.Alert = true;
                        label3.ForeColor = System.Drawing.Color.Red;
                        sSynth.Speak("Blood Pressure Alert!");

                    // fora da gama normal e dos limites criticos
                        if (b.Value2 >= 30 && b.Value2 < 60 || b.Value1 > 180)
                        {
                            if (stopwatchBW.IsRunning)
                            {
                                if (stopwatchBW.ElapsedMilliseconds > 600000)
                                {
                                    a.Date = Convert.ToDateTime(values[2]);
                                    a.SNS = Properties.Settings.Default.snsH;
                                    a.Bp = b;
                                    a.Alert = "BP";
                                    a.Tipo = "WarningContinuous";
                                    web.AddAlerts(a);
                                    stopwatchBW.Restart();
                                }
                            }
                            else
                            {
                                stopwatchBW.Start();

                            }

                            if (stopwatchBC.IsRunning)
                            {
                                if (stopwatchBC.ElapsedMilliseconds > 3600000)
                                {
                                    a.Date = Convert.ToDateTime(values[2]);
                                    a.SNS = Properties.Settings.Default.snsH;
                                    a.Bp = b;
                                    a.Alert = "BP";
                                    a.Tipo = "CriticalContinous";
                                    web.AddAlerts(a);
                                    stopwatchBC.Restart();
                                }

                            }
                            else
                            {
                                stopwatchBC.Start();
                            }

                            if (stopwatchBW2.IsRunning)
                            {
                                if (stopwatchBW2.ElapsedMilliseconds > 1800000 && NB >= 600000)
                                {

                                    a.Date = Convert.ToDateTime(values[2]);
                                    a.SNS = Properties.Settings.Default.snsH;
                                    a.Bp = b;
                                    a.Alert = "BP";
                                    a.Tipo = "WarningIntermitente";
                                    web.AddAlerts(a);
                                    stopwatchBW2.Restart();
                                    NB = 0;
                                }
                                if (stopwatchBW2.ElapsedMilliseconds > 1800000 && NB < 600000)
                                {
                                    NB = 0;
                                    stopwatchBW2.Restart();
                                }
                                else
                                {
                                    NB += MyHealth.Properties.Settings.Default.ndelay * 3;
                                }
                            }
                            else
                            {
                                stopwatchBW2.Start();
                                NB += MyHealth.Properties.Settings.Default.ndelay * 3;
                            }
                        }
                    }
                    else
                    {
                        b.Alert = false;
                        label3.ForeColor = System.Drawing.Color.Green;
                    }
                    web.AddValuesBP(b);

                }

                if (values[0].Equals("SPO2"))
                {
                    label4.Text = values[1];

                    s.Value = Convert.ToInt32(values[1]);
                    s.Date = Convert.ToDateTime(values[2]);// + "-" + values[3] + "-" + values[4]);
                    s.SNS = Properties.Settings.Default.snsH;
                    label7.Text = values[2];
                    if (s.Value < 90)
                    {
                        s.Alert = true;
                        label4.ForeColor = System.Drawing.Color.Red;
                        sSynth.Speak("Oxygen Saturation Alert!");

                        if (s.Value > 80 && s.Value < 90)
                        {
                            if (stopwatchSW.IsRunning)
                            {
                                if (stopwatchSW.ElapsedMilliseconds > 600000)
                                {
                                    a.Date = Convert.ToDateTime(values[2]);
                                    a.SNS = Properties.Settings.Default.snsH;
                                    a.Spoe = s;
                                    a.Alert = "SPO";
                                    a.Tipo = "WarningContinuous";
                                    web.AddAlerts(a);
                                    stopwatchSW.Restart();
                                }
                            }
                            else
                            {
                                stopwatchSW.Start();

                            }

                            if (stopwatchSC.IsRunning)
                            {
                                if (stopwatchSC.ElapsedMilliseconds > 3600000)
                                {
                                    a.Date = Convert.ToDateTime(values[2]);
                                    a.SNS = Properties.Settings.Default.snsH;
                                    a.Spoe = s;
                                    a.Alert = "SPO";
                                    a.Tipo = "CriticalContinous";
                                    web.AddAlerts(a);
                                    stopwatchSC.Restart();
                                }

                            }
                            else
                            {
                                stopwatchSC.Start();
                            }

                            if (stopwatchSW2.IsRunning)
                            {
                                if (stopwatchSW2.ElapsedMilliseconds > 1800000 && NS >= 600000)
                                {

                                    a.Date = Convert.ToDateTime(values[2]);
                                    a.SNS = Properties.Settings.Default.snsH;
                                    a.Spoe = s;
                                    a.Alert = "SPO";
                                    a.Tipo = "WarningIntermitente";
                                    web.AddAlerts(a);
                                    stopwatchSW2.Restart();
                                    NS = 0;
                                }
                                if (stopwatchSW2.ElapsedMilliseconds > 1800000 && NS < 600000)
                                {
                                    NS = 0;
                                    stopwatchSW2.Restart();
                                }
                                else
                                {
                                    NS += MyHealth.Properties.Settings.Default.ndelay * 3;
                                }
                            }
                            else
                            {
                                stopwatchSW2.Start();
                                NS += MyHealth.Properties.Settings.Default.ndelay * 3;
                            }
                        }
                    }

                    else
                    {
                        s.Alert = false;
                        label4.ForeColor = System.Drawing.Color.Green;
                    }
                    web.AddValuesSPO(s);
                }
                



                if (values[0].Equals("HR"))
                    {
                        label5.Text = values[1];

                        h.Value = Convert.ToInt32(values[1]);
                        h.Date = Convert.ToDateTime(values[2]);// + "-" + values[3] + "-" + values[4]);
                        h.SNS = Properties.Settings.Default.snsH;
                        label8.Text = values[2];
                        if (h.Value < 60 || h.Value > 120)
                        {

                            h.Alert = true;
                            label5.ForeColor = System.Drawing.Color.Red;                        
                            sSynth.Speak("Heart Rate Alert!");

                        if (h.Value < 60 && h.Value >= 30 || h.Value > 120 && h.Value <= 180)
                            {
                                if (stopwatchHW.IsRunning)
                                {
                                    if (stopwatchHW.ElapsedMilliseconds > 600000)
                                    {
                                                a.Date = Convert.ToDateTime(values[2]);
                                                 a.SNS = Properties.Settings.Default.snsH;
                                                a.Hr = h;
                                                a.Alert = "HR";
                                                a.Tipo = "WarningContinuous";
                                                web.AddAlerts(a);
                                                stopwatchHW.Restart();
                                    }
                                }
                                else
                                {
                                    stopwatchHW.Start();

                                }

                                if (stopwatchHC.IsRunning)
                                {
                                    if (stopwatchHC.ElapsedMilliseconds > 3600000)
                                    {
                                                a.Date = Convert.ToDateTime(values[2]);
                                                a.SNS = Properties.Settings.Default.snsH;
                                                a.Hr = h;
                                                a.Alert = "HR";
                                                a.Tipo = "CriticalContinous";
                                                web.AddAlerts(a);
                                                stopwatchHC.Restart();
                                    }

                                }
                                else
                                {
                                    stopwatchHC.Start();
                                }

                                if (stopwatchHW2.IsRunning)
                                {
                                    if (stopwatchHW2.ElapsedMilliseconds > 1800000 && NH >= 600000)
                                    {

                                                a.Date = Convert.ToDateTime(values[2]);
                                                a.SNS = Properties.Settings.Default.snsH;
                                                a.Hr = h;
                                                a.Alert = "HR";
                                                a.Tipo = "WarningIntermitente";
                                                web.AddAlerts(a);
                                                stopwatchSW2.Restart();
                                                NH = 0;
                                }
                                    if (stopwatchHW2.ElapsedMilliseconds > 1800000 && NH < 600000)
                                    {
                                        NH = 0;
                                        stopwatchHW2.Restart();
                                    }
                                    else
                                    {
                                        NH += MyHealth.Properties.Settings.Default.ndelay * 3;
                                    }
                                }
                                else
                                {
                                    stopwatchHW2.Start();
                                    NH += MyHealth.Properties.Settings.Default.ndelay * 3;
                                }
                            }
                        }
                        else
                        {

                            h.Alert = false;
                            label5.ForeColor = System.Drawing.Color.Green;
                        }
                        web.AddValuesHR(h);
                    }           
            }
                
                ));

            

        }

      

        private void idUtente_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            


        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void registerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
            Find f2 = new Find();
            //this.Hide();
            //f2.FormClosed += (s, args) => this.Close();
            f2.Show();
            f2.Focus();
            sRecognize.RecognizeAsyncStop();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click_2(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBlood_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void configurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configurations f3 = new Configurations();
            //this.Hide();
            //f2.FormClosed += (s, args) => this.Close();
            f3.Show();
            f3.Focus();
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dll.Stop(); 
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            pBuilder.ClearContent(); 
            pBuilder.AppendText(richTextBox2.Text);
           
            sSynth.Speak(pBuilder);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            sRecognize.RecognizeAsyncStop();
            MessageBox.Show("Voice recognition sucefully stoped!");
        }
    }
}
