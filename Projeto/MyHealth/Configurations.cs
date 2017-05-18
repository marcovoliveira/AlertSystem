using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHealth
{
    public partial class Configurations : Form
    {
        public Configurations()
        {
            InitializeComponent();
            retmax.Value = MyHealth.Properties.Settings.Default.retmax;
            delay.Value = MyHealth.Properties.Settings.Default.ndelay / 1000;
            textBox1.Text = MyHealth.Properties.Settings.Default.url;     

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyHealth.Properties.Settings.Default.retmax = Convert.ToInt32(retmax.Value);
            MyHealth.Properties.Settings.Default.ndelay = Convert.ToInt32(delay.Value) * 1000;
            MyHealth.Properties.Settings.Default.url = textBox1.Text; 
            Properties.Settings.Default.Save();
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void Configurations_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
