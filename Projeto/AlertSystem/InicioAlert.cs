using AlertSystem.ServiceRefHealth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AlertSystem
{
    public partial class StartAlert : Form
    {
        ServiceHealthClient web = new ServiceHealthClient();
        DateTime dataU = Properties.Settings.Default.dataU;
        DateTime dn = DateTime.Now;
        DateTime da = DateTime.Now.AddDays(-2);

       
        int parsedValue; 
        public static int sns;
        public static int snsC; 

        public StartAlert()
        {
            InitializeComponent();
            
            dateTimePicker1.CustomFormat = "dd-MMM-yyyy hh:mm:ss";
            dateTimePicker2.CustomFormat = "dd-MMM-yyyy hh:mm:ss";
            var p = web.listActivePatient();

            foreach (Patient item in p)
            {

                ListViewItem ut = new ListViewItem(Convert.ToString(item.SNS), 0);
                ut.SubItems.Add(item.FirstName + " " + item.LastName);
                ut.SubItems.Add(Convert.ToString(item.Age));
                ut.SubItems.Add(Convert.ToString(item.Birthdate)); 
               
                listView1.Items.Add(ut);


                

            }
            verificaAlertas();
        }
        private void verificaAlertas()
        {
           
            
            var ba = web.listAlertsBPs(da, dn);
            var ha = web.listAlertsHRs(da, dn);
            var sa = web.listAlertsSPOes(da, dn);
            //

           var al = web.listAlertas(da, dn);

            // enventos continuos e intermitentes warnings e critos
            List<Alertas> all = al.ToList();

            foreach (Alertas item in all.Where((i => i.Date > dataU)))
            {
                MessageBox.Show(item.Tipo + " " + Environment.NewLine
                  + "SNS:" + item.SNS + Environment.NewLine
                  + "Name:" + item.FirstName + " " + item.LastName + Environment.NewLine
                  + "Age:" + item.Alert + Environment.NewLine
                  + "Date:" + item.Date + Environment.NewLine);

            }
        
          
           
            //Lista enventos criticos anytime
            List<BPs> bps = ba.Where(i => i.Value2 < 60).ToList();
            List<HRs> hrs = ha.Where(i => i.Value < 30 || i.Value > 180).ToList();
            List<SPOes> spo = sa.Where(i => i.Value < 80).ToList();

            foreach(BPs item in bps.Where(i => i.Date > dataU ))
                {
                MessageBox.Show("Critical Alert! Limit Values " + Environment.NewLine 
                    + "SNS:" + item.SNS + Environment.NewLine
                    + "Name:" + item.FirstName + " " + item.LastName + Environment.NewLine
                    + "Age:" + item.Age + Environment.NewLine
                    + "Type" + "Blood Pressure" + Environment.NewLine
                    + "Value" + item.Value1 +"-"+ item.Value2 + Environment.NewLine
                    + "Date:" + item.Date + Environment.NewLine);
            }
            foreach (HRs item in hrs.Where(i => i.Date > dataU))
            {
                MessageBox.Show("Critical Alert! Limit Values " + Environment.NewLine
                    + "SNS:" + item.SNS + Environment.NewLine
                    + "Name:" + item.FirstName + " " + item.LastName + Environment.NewLine
                    + "Age:" + item.Age + Environment.NewLine
                    + "Type" + "Heart Rate" + Environment.NewLine
                    + "Value" + item.Value + Environment.NewLine
                    + "Date:" + item.Date + Environment.NewLine);
            }
            foreach (SPOes item in spo.Where(i => i.Date > dataU))
            {
                MessageBox.Show("Critical Alert! Limit Values " + Environment.NewLine
                    + "SNS:" + item.SNS + Environment.NewLine
                    + "Name:" + item.FirstName + " " + item.LastName + Environment.NewLine
                    + "Age:" + item.Age + Environment.NewLine
                    + "Type" + "Oxygen Saturation" + Environment.NewLine
                    + "Value" + item.Value + Environment.NewLine
                    + "Date:" + item.Date + Environment.NewLine);
            }


            //data da ultima vez que o programa correu 
            dataU = DateTime.Now; 
            Properties.Settings.Default.dataU = dataU;
            Properties.Settings.Default.Save();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void activesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewAlerts f6 = new ViewAlerts();
            //this.Hide();
            //f2.FormClosed += (s, args) => this.Close();
            f6.Show();
            f6.Focus();
        }

        private void registerToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Registration f2 = new Registration();
            //this.Hide();
            //f2.FormClosed += (s, args) => this.Close();
            f2.Show();
            f2.Focus();
        }

        private void registerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewUser f5 = new ViewUser();
            //this.Hide();
            //f2.FormClosed += (s, args) => this.Close();
            f5.Show();
            f5.Focus();
        }

        private void activesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Actives f3 = new Actives();
            //this.Hide();
            //f2.FormClosed += (s, args) => this.Close();
            f3.Show();
            f3.Focus();
        }

        private void alterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Alter f4 = new Alter(snsC);
            f4.Show();
            f4.Focus();
        }

        private void InicioAlert_Load(object sender, EventArgs e)
        {
          
        }

        private void Chart_Load(object sender, EventArgs e)
        {

            if (chart1.Series.IsUniqueName("BloodPressure"))
            {
                chart1.Series.Add("BloodPressure");
            }

            if (chart1.Series.IsUniqueName("HeartRate"))
            {
                chart1.Series.Add("HeartRate");
            }

            if (chart1.Series.IsUniqueName("OxygenSaturation"))
            {
                chart1.Series.Add("OxygenSaturation");
            }

            

            chart1.Series["BloodPressure"].XValueType = ChartValueType.DateTime;
            chart1.Series["HeartRate"].XValueType = ChartValueType.DateTime;
            chart1.Series["OxygenSaturation"].XValueType = ChartValueType.DateTime;
            chart1.Series["BloodPressure"].ChartType =
                    System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart1.Series["HeartRate"].ChartType =
                    System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart1.Series["OxygenSaturation"].ChartType =
                    System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy HH:mm";
            //chart1.ChartAreas["area"].AxisX.Maximum = Convert.ToDouble(dn);


            chart1.Legends.Add(new Legend("Blood Pressure"));
            chart1.Legends.Add(new Legend("Heart Rate"));
            chart1.Legends.Add(new Legend("Oxygen Saturation"));
            

            chart1.Legends["Blood Pressure"].Docking = Docking.Bottom;
            chart1.Legends["Heart Rate"].Docking = Docking.Bottom;
            chart1.Legends["Oxygen Saturation"].Docking = Docking.Bottom;




            chart1.Titles.Add("Patient Values");

            chart1.ChartAreas[0].AxisX.Title = "Days";
            chart1.ChartAreas[0].AxisY.Title = "Values";
        }


    

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
             
            if (listView1.SelectedItems.Count != 0)
            {
              
                button2.Enabled = true;

                //traz para as variaveis items da lista que foi selecionada 
                sns = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);
                string nome = listView1.SelectedItems[0].SubItems[1].Text;
                string Age = listView1.SelectedItems[0].SubItems[2].Text;
                string bd = listView1.SelectedItems[0].SubItems[3].Text;
                
                //chama metodo passa sns e datas
                updateValues(sns, dn, da);
                snsC = sns;
                //Selected user data
                label9.Text = Convert.ToString(sns);
                label8.Text = nome;
                label10.Text = Convert.ToDateTime(bd).ToString("dd/MM/yyyy");
                label11.Text = Age; 

                
            }
        }
        private void updateValues(int sns, DateTime dn, DateTime da)
        {
            
            chart1.Series["BloodPressure"].Points.Clear();
            chart1.Series["HeartRate"].Points.Clear();
            chart1.Series["OxygenSaturation"].Points.Clear();
            listView2.Items.Clear(); 

            var b = web.listBPs(sns, da, dn);
            
            foreach (BPs item in b)
            {
                chart1.Series["BloodPressure"].Points.AddXY(item.Date.ToOADate(), item.Value1);

                ListViewItem val = new ListViewItem(Convert.ToString("Blood Pressure"), 0);
                val.SubItems.Add(Convert.ToString(item.Date));
                val.SubItems.Add(Convert.ToString(item.Value1 +"-"+ item.Value2));

                listView2.Items.Add(val);

            }
            var h = web.listHRs(sns, da, dn);
            foreach (HRs item in h)
            {
                chart1.Series["HeartRate"].Points.AddXY(item.Date, item.Value );
                ListViewItem val = new ListViewItem(Convert.ToString("Heart Rate"), 0);
                val.SubItems.Add(Convert.ToString(item.Date));
                val.SubItems.Add(Convert.ToString(item.Value));

                listView2.Items.Add(val);

            }
            var s = web.listSPOes(sns, da, dn);
            foreach (SPOes item in s)
            {
                chart1.Series["OxygenSaturation"].Points.AddXY(item.Date, item.Value);
                ListViewItem val = new ListViewItem(Convert.ToString("Oxygen Saturation"), 0);
                val.SubItems.Add(Convert.ToString(item.Date));
                val.SubItems.Add(Convert.ToString(item.Value));

                listView2.Items.Add(val);
            }
            
        

        }

        private void chart1_Click(object sender, EventArgs e)
        {
           
        }

        private void bp_CheckedChanged(object sender, EventArgs e)
        {
            if (bp.Checked == true)
            {
                chart1.Series["BloodPressure"].Enabled = true;
            }
            else if (bp.Checked == false)
            {
                chart1.Series["BloodPressure"].Enabled = false;
            }
        }

        private void hr_CheckedChanged(object sender, EventArgs e)
        {
            if (hr.Checked == true)
            {
                chart1.Series["HeartRate"].Enabled = true;
            }
            else if (hr.Checked == false)
            {
                chart1.Series["HeartRate"].Enabled = false;
            }
        }

        private void spo_CheckedChanged(object sender, EventArgs e)
        {
            if (spo.Checked == true)
            {
                chart1.Series["OxygenSaturation"].Enabled = true;
            }
            else if (spo.Checked == false)
            {
                chart1.Series["OxygenSaturation"].Enabled = false;
            }
           
    }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
            
                DateTime da = DateTime.Now.AddDays(-7);
                updateValues(sns, dn, da);



        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Ciclo de validação 
            if (!int.TryParse(textBox1.Text, out parsedValue))
            {
                MessageBox.Show("Please insert a valid SNS number!");
                return;
            }
            else
            {
                sns = Convert.ToInt32(textBox1.Text);
                var p = web.ValidatePatient(sns);
                //Selected user data
                label9.Text = Convert.ToString(sns);
                label8.Text = p.FirstName + " " + p.LastName;
                label10.Text = Convert.ToDateTime(p.Birthdate).ToString("dd/MM/yyyy");
                label11.Text = Convert.ToString(p.Age);

                if (p != null)
                {
                   
                    updateValues(sns, dn, da);
                    button2.Enabled = true;
                    // so posso passar o sns para outros forms depois de saber que ele é valido 
                    snsC = sns; 

                }
                else
                {
                    MessageBox.Show("SNS number doesn't exist!");
                }


            }
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserStatistics f7 = new UserStatistics(snsC);
            //this.Hide();
            //f2.FormClosed += (s, args) => this.Close();
            f7.Show();
            f7.Focus();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void ColumnClick(object o, ColumnClickEventArgs e)
        {
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object. Setting this property immediately sorts the 
            // ListView using the ListViewItemComparer object.
            this.listView2.ListViewItemSorter = new ListViewItemComparer(e.Column);
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

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
           
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime ds = DateTime.Parse(dateTimePicker1.Text);
            DateTime du = DateTime.Parse(dateTimePicker2.Text);
            updateValues(sns, du, ds);
           
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime ds = DateTime.Parse(dateTimePicker1.Text);
            DateTime du = DateTime.Parse(dateTimePicker2.Text);
            updateValues(sns, du, ds);
        }

        private void eventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Events f8 = new Events(snsC);
            //this.Hide();
            //f2.FormClosed += (s, args) => this.Close();
            f8.Show();
            f8.Focus();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void activesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ViewActives f9 = new ViewActives();
            //this.Hide();
            //f2.FormClosed += (s, args) => this.Close();
            f9.Show();
            f9.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            verificaAlertas();
        }
    }
    }

