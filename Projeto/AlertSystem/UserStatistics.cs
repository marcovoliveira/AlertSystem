using AlertSystem.ServiceRefHealth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlertSystem
{
    public partial class UserStatistics : Form
    {
        ServiceRefHealth.Patient p = new Patient();
        ServiceHealthClient web;
        int snsNumber = 0;
        int parsedValue;
        DateTime da = DateTime.Now.AddDays(-2);


        public UserStatistics(int sns)
        {
            web = new ServiceHealthClient();
            snsNumber = sns;
            if (snsNumber != 0)
            {
                p = web.ValidatePatient(sns);
            }

            InitializeComponent();
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // duas formas de limpar labels
            nameN.Text = "";
            snsN.Text = "";
            ageN.Text = "";
            btdN.Text = "";

            //2ªforma
            bpmax.ResetText();
            bpmed.ResetText();
            bpmin.ResetText();
            spomax.ResetText(); 
            spomed.ResetText();
            spomin.ResetText();
            hrmax.ResetText();
            hrmed.ResetText();
            hrmin.ResetText();
            
            //validate junto do web service se o utente existe
            //se existir mostrar mensagem de boas vindas



            if (!int.TryParse(textBox1.Text, out parsedValue))
            {
                MessageBox.Show("Please insert a valid SNS number!");
                return;
            }
            else
            {
                snsNumber = Convert.ToInt32(textBox1.Text);



                web = new ServiceHealthClient();
                p = web.ValidatePatient(snsNumber);


                if (p != null)
                {

                    MessageBox.Show("You selected the user " + p.FirstName);
                    groupBox1.Visible = true;
                    groupBox2.Visible = true;
                    groupBox3.Visible = true;
                    groupBox4.Visible = true;
                    groupBox5.Visible = true;

                    nameN.Text = p.FirstName + " " + p.LastName;
                    snsN.Text = Convert.ToString(p.SNS);
                    btdN.Text = Convert.ToString(p.Birthdate.Day + "/" + p.Birthdate.Month + "/" + p.Birthdate.Year);
                    ageN.Text = Convert.ToString(p.Age);

                    var b = web.listBPs(snsNumber, DateTime.Now.AddDays(-3), DateTime.Now);
                    var h = web.listHRs(snsNumber, DateTime.Now.AddDays(-3), DateTime.Now);
                    var s = web.listSPOes(snsNumber, DateTime.Now.AddDays(-3), DateTime.Now);
                    update(b, s, h);
                }
            }
        }
        //metodo update para ter o codigo mais limpo e evitar repetição 
        private void update(BPs[] b, SPOes[] s, HRs[] h)
        {
          
            

            //
            //Verifica se existe valores de medição, caso não existe da a informação
            if (b.Any())
            {
                List<BPs> bps = b.OrderBy(i => i.Value1).ToList();
                bpmax.Text = Convert.ToString(bps.Last().Value1 + "-" + bps.Last().Value2);

                List<BPs> bpss = b.OrderByDescending(i => i.Value1).ToList();
                bpmin.Text = Convert.ToString(bpss.Last().Value1 + "-" + bps.Last().Value2);

                bpmed.Text = Convert.ToString(Math.Round(b.Average(i => i.Value1), 1) + "-" + Math.Round(b.Average(i => i.Value2), 1));
            }
            else
            {
                bpmax.Text = "N/A";
                bpmin.Text = "N/A";
                bpmed.Text = "N/A";
            }
            if (h.Any())
            {

                List<HRs> hrs = h.OrderBy(i => i.Value).ToList();
                hrmax.Text = Convert.ToString(hrs.Last().Value);

                List<HRs> hrss = h.OrderByDescending(i => i.Value).ToList();
                hrmin.Text = Convert.ToString(hrss.Last().Value);

                hrmed.Text = Convert.ToString(Math.Round(h.Average(i => i.Value), 1));
            }
            else
            {
                hrmax.Text = "N/A";
                hrmin.Text = "N/A";
                hrmed.Text = "N/A";
            }
            if (s.Any())
            {
                List<SPOes> spo = s.OrderBy(i => i.Value).ToList();
                spomax.Text = Convert.ToString(spo.Last().Value);

                spomed.Text = Convert.ToString(Math.Round(s.Average(i => i.Value), 1));

                List<SPOes> spoes = s.OrderByDescending(i => i.Value).ToList();
                spomin.Text = Convert.ToString(spoes.Last().Value);
            }
            else
            {
                spomax.Text = "N/A";
                spomed.Text = "N/A";
                spomin.Text = "N/A";
            }

            }
   
     
        private void label4_Click(object sender, EventArgs e)
        {


        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void UserStatistics_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat =  "dd-MMM-yyyy hh:mm:ss";
            dateTimePicker2.CustomFormat =  "dd-MMM-yyyy hh:mm:ss";

            if (snsNumber != 0)
            {


                MessageBox.Show("You selected the user " + p.FirstName);
                groupBox1.Visible = true;
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = true;
                groupBox5.Visible = true;

                nameN.Text = p.FirstName + " " + p.LastName;
                snsN.Text = Convert.ToString(p.SNS);
                btdN.Text = Convert.ToString(p.Birthdate.Day + "/" + p.Birthdate.Month + "/" + p.Birthdate.Year);
                ageN.Text = Convert.ToString(p.Age);

                // a limitação da vizualização dos registos para o parametro global como a stora sugeriu 
                // assim apenas vai buscar os valores de um utente dos ultimos 3 messes para o calculo da estatistica
                var b = web.listBPs(snsNumber, DateTime.Now.AddMonths(-3), DateTime.Now);
                var h = web.listHRs(snsNumber, DateTime.Now.AddMonths(-3), DateTime.Now);
                var s = web.listSPOes(snsNumber, DateTime.Now.AddMonths(-3), DateTime.Now);
                update(b, s, h);

            }  
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime ds = DateTime.Parse(dateTimePicker1.Text);
            DateTime du = DateTime.Parse(dateTimePicker2.Text);
            dateChage(ds, du);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // registos dos ultimos 2 dias
            var b = web.listBPs(snsNumber, da, DateTime.Now);
            var h = web.listHRs(snsNumber, da, DateTime.Now);
            var s = web.listSPOes(snsNumber, da, DateTime.Now);

            update(b, s, h);
        }

        private void dateChage(DateTime ds, DateTime du)
        {
            var b = web.listBPs(snsNumber, ds, du);
            var h = web.listHRs(snsNumber, ds, du);
            var s = web.listSPOes(snsNumber, ds, du);

            update(b, s, h);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime ds = DateTime.Parse(dateTimePicker1.Text);
            DateTime du = DateTime.Parse(dateTimePicker2.Text); 
            dateChage(ds, du);
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
    }

