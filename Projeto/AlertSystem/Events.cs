using AlertSystem.ServiceRefHealth;
using System;
using System.Collections;
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
    public partial class Events : Form
    {
        ServiceRefHealth.Patient p = new Patient();
        ServiceHealthClient web;
        int snsNumber = 0;
        int parsedValue;
        
       
        DateTime da = DateTime.Now.AddDays(-2);
        DateTime dn = DateTime.Now;  
        

        public Events(int sns)
        {
            snsNumber = sns; 

            InitializeComponent();
            dateTimePicker1.CustomFormat = "dd-MMM-yyyy hh:mm:ss";
            dateTimePicker2.CustomFormat = "dd-MMM-yyyy hh:mm:ss";
            dateTimePicker1.Value = da;
            dateTimePicker2.Value = dn; 
           
            checkBox1.Checked = true; 
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            web = new ServiceHealthClient();
            checkBox5.Checked = true;
            checkBox6.Checked = true; 
            
            if (sns != 0)
            {
                p = web.ValidatePatient(sns);
                updateValues(sns, da, dn);
            }
            

        }
        private void updateValues(int sns, DateTime da, DateTime dn)
        {

            if (sns != 0)
            {
                listView1.Items.Clear();
                web = new ServiceHealthClient();
                var a = web.listAlertasUtente(sns, da, dn);
                var b = web.listUserAlertsBPs(sns, da, dn);
                var h = web.listUserAlertsHRs(sns, da, dn);
                var s = web.listUserAlertsSPOes(sns, da, dn);
                if (checkBox6.Checked == true)
                {
                    if (checkBox1.Checked)
                        foreach (Alertas item in a.Where(i => i.Alert == "BP"))
                        {

                            ListViewItem val = new ListViewItem((item.Tipo), 0);
                            val.SubItems.Add(item.Alert);
                            val.SubItems.Add(Convert.ToString(item.Date));


                            listView1.Items.Add(val);
                        }

                    if (checkBox2.Checked)
                        foreach (Alertas item in a.Where(i => i.Alert == "HR"))
                        {

                            ListViewItem val = new ListViewItem((item.Tipo), 0);
                            val.SubItems.Add(item.Alert);
                            val.SubItems.Add(Convert.ToString(item.Date));


                            listView1.Items.Add(val);
                        }

                    if (checkBox3.Checked)
                        foreach (Alertas item in a.Where(i => i.Alert == "SPO"))
                        {

                            ListViewItem val = new ListViewItem((item.Tipo), 0);
                            val.SubItems.Add(item.Alert);
                            val.SubItems.Add(Convert.ToString(item.Date));


                            listView1.Items.Add(val);
                        }
                }
                if (checkBox5.Checked == true)
                {
                    if (b.Any() && checkBox1.Checked == true)
                    {
                        List<BPs> bps = b.OrderBy(i => i.Date).ToList();
                        foreach (BPs item in bps)
                        {

                            if (item.Value2 < 60)
                            {

                                ListViewItem val = new ListViewItem("CriticalEvent! Limit Value", 0);
                                val.SubItems.Add("BP");
                                val.SubItems.Add(Convert.ToString(item.Date));

                                listView1.Items.Add(val);


                            }


                        }
                    }
                    if (h.Any() && checkBox2.Checked == true)
                    {
                        List<HRs> hrs = h.OrderBy(i => i.Date).ToList();
                        foreach (HRs item in hrs)
                        {

                            if (item.Value < 30 || item.Value > 180)
                            {

                                ListViewItem val = new ListViewItem("CriticalEvent! Limit Value", 0);
                                val.SubItems.Add("HR");
                                val.SubItems.Add(Convert.ToString(item.Date));

                                listView1.Items.Add(val);


                            }


                        }
                    }
                    if (s.Any() && checkBox3.Checked == true)
                    {
                        List<SPOes> sps = s.OrderBy(i => i.Date).ToList();
                        foreach (SPOes item in sps)
                        {

                            if (item.Value < 80)
                            {

                                ListViewItem val = new ListViewItem("CriticalEvent! Limit Value", 0);
                                val.SubItems.Add("SPO");
                                val.SubItems.Add(Convert.ToString(item.Date));

                                listView1.Items.Add(val);


                            }


                        }
                    }
                }

                         
                        groupBox1.Visible = true;
                        groupBox2.Visible = true;
                        groupBox3.Visible = true;
                        groupBox4.Visible = true;
                        groupBox5.Visible = true;

                        nameN.Text = p.FirstName + " " + p.LastName;
                        snsN.Text = Convert.ToString(p.SNS);
                        btdN.Text = Convert.ToString(p.Birthdate.Day + "/" + p.Birthdate.Month + "/" + p.Birthdate.Year);
                        ageN.Text = Convert.ToString(p.Age);


                

                    }
                }
            
        


            private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void Events_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            nameN.Text = "";
            snsN.Text = "";
            ageN.Text = "";
            btdN.Text = "";
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
                    updateValues(snsNumber, da, dn);
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


                    var b = web.listUserAlertsBPs(snsNumber, DateTime.Now.AddDays(-3), DateTime.Now);
                    var h = web.listUserAlertsHRs(snsNumber, DateTime.Now.AddDays(-3), DateTime.Now);
                    var s = web.listUserAlertsSPOes(snsNumber, DateTime.Now.AddDays(-3), DateTime.Now);
                   
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ColumnClick(object o, ColumnClickEventArgs e)
        {
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object. Setting this property immediately sorts the 
            // ListView using the ListViewItemComparer object.
            this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
        }
        class ListViewItemComparer : IComparer
        {
            private int col;
            public ListViewItemComparer()
            {
                col = 0;
            }
            public ListViewItemComparer(int column)
            {
                col = column;
            }
            public int Compare(object x, object y)
            {
                return String.Compare(((ListViewItem)y).SubItems[col].Text, ((ListViewItem)x).SubItems[col].Text);
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            updateValues(snsNumber, da, dn);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            updateValues(snsNumber, da, dn);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            updateValues(snsNumber, da, dn);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            updateValues(snsNumber, da, dn);

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            updateValues(snsNumber, da, dn);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            da = DateTime.Now.AddDays(-3); 
            updateValues(snsNumber, da, dn);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime da = dateTimePicker1.Value;
            DateTime dn = dateTimePicker2.Value;
            updateValues(snsNumber, da, dn);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime da = dateTimePicker1.Value;
            DateTime dn = dateTimePicker2.Value;
            updateValues(snsNumber, da, dn);
        }
    }
}
