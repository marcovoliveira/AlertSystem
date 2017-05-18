using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlertSystem.ServiceRefHealth;
using System.Diagnostics;

namespace AlertSystem
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Criar validação para o formulario de registo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ServiceRefHealth.ServiceHealthClient web = new ServiceHealthClient();

            Patient p = new ServiceRefHealth.Patient();


            try
            {
                p.FirstName = textBox1.Text;
                p.LastName = textBox2.Text;
                p.SNS = Int32.Parse(textBox3.Text);
                p.Birthdate = DateTime.Parse(dateTimePicker1.Text);
            }
            catch (Exception ex)
            {
                 MessageBox.Show("Exception Message: " + ex.Message);
            }

            bool result = web.AddPatient(p);
            
            if (result == true)
            {

                MessageBox.Show("Registado com sucesso!");
                Close();
            } 
            else
            {
                MessageBox.Show("Erro no registo!");
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear(); 


            }
        
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
