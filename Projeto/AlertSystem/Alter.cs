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
    public partial class Alter : Form
    {
        ServiceRefHealth.Patient p;
        ServiceHealthClient web;
        int oldSNS;
        int parsedValue; 
        

        public Alter(int snsNumber)
        {
            InitializeComponent();
            checkBox1.Visible = false;

            web = new ServiceHealthClient();
            p = web.ValidatePatient(snsNumber);
            if (snsNumber != 0)
            {
                p = web.ValidatePatient(snsNumber);

                MessageBox.Show("Selected the user:  " + p.FirstName);
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox6.Visible = true;
                dateTimePicker1.Visible = true;
                checkBox1.Visible = true;
                button2.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;






                textBox2.Text = p.FirstName;
                textBox3.Text = p.LastName;
                textBox6.Text = Convert.ToString(p.SNS);
                oldSNS = p.SNS;
                dateTimePicker1.Value = p.Birthdate;
                checkBox1.Checked = p.Activo;
            }

            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //inicio do ciclo de validação
            if (!int.TryParse(textBox1.Text, out parsedValue))
            {
                MessageBox.Show("Please insert a valid SNS number!");
                return;
            }
            else
            {
                int snsNumber = int.Parse(textBox1.Text);


           
                web = new ServiceHealthClient();
                p = web.ValidatePatient(snsNumber);
                if (p != null)
                {


                    MessageBox.Show("Selected the user " + p.FirstName);
                    textBox2.Visible = true;
                    textBox3.Visible = true;
                    textBox6.Visible = true;
                    dateTimePicker1.Visible = true;
                    checkBox1.Visible = true;
                    button2.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;






                    textBox2.Text = p.FirstName;
                    textBox3.Text = p.LastName;
                    textBox6.Text = Convert.ToString(p.SNS);
                    oldSNS = p.SNS;
                    dateTimePicker1.Value = p.Birthdate;
                    checkBox1.Checked = p.Activo;







                }
                else
                {
                    MessageBox.Show("Numero de SNS invalido!");


                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ServiceRefHealth.ServiceHealthClient web = new ServiceHealthClient();

            Patient p = new ServiceRefHealth.Patient();
            try
            {

                p.FirstName = textBox2.Text;
                p.LastName = textBox3.Text;
                p.SNS = Int32.Parse(textBox6.Text);
                p.oldSNS = oldSNS; 
                p.Birthdate = DateTime.Parse(dateTimePicker1.Text);
                p.Activo = checkBox1.Checked; 
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Message: " + ex.Message);
            }

            bool result = web.AlterPatient(p);

            if (result == true)
            {

                MessageBox.Show("Registado com sucesso!");
                Close();
            }




        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Alter_Load(object sender, EventArgs e)
        {

        }
    }
}
