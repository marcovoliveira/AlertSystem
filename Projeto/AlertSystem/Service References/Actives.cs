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
    public partial class Actives : Form
    {
        ServiceHealthClient web = new ServiceHealthClient();

        public Actives()
        {
            InitializeComponent(); 
            var p = web.listActivePatient();
            
            foreach (Patient item in p)
            {
                
                    ListViewItem ut = new ListViewItem(Convert.ToString(item.SNS), 0);
                    ut.SubItems.Add(item.FirstName +" "+ item.LastName);
               
                    listView1.Items.Add(ut);
                }
                
            }
                



        

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void Form3_Load(object sender, EventArgs e)
        {

            
        
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            
            
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
  }

