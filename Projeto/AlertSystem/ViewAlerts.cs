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
    // Metodo extensão que cria um Zip de 3 listas em vez de o tipico zip de 2. 
   

    public partial class ViewAlerts : Form
    {
        DateTime da = DateTime.Now.AddHours(-24);
        

        public ViewAlerts()
        {
            InitializeComponent();
            ServiceHealthClient web = new ServiceHealthClient();
            var b = web.listPatientWithAlertsBP();
            var h = web.listPatientWithAlertsHR();
            var s = web.listPatientWithAlertsSPO();
            // juntar as 3 listas de valores com alertas
            // z = BloodPressure
            // y =  HeartRate
            // z = Oxygen Saturations
            var listas = b.ZipDeTres(h, s, (w, y, z) => new { w, y, z });
            // metodo implementado na class ExtendMethod.cs

            List<int> lastSNS = new List<int>();
            foreach (var wyz in listas.Where(i => i.w.Date >= da || i.y.Date >= da || i.z.Date >= da).OrderBy(t => t.w.SNS).ThenBy(t => t.z.SNS).ThenBy(t => t.y.SNS))
            {
                

                if (! lastSNS.Contains(wyz.w.SNS))
                {

                    ListViewItem bp = new ListViewItem(Convert.ToString(wyz.w.SNS), 0);
                    bp.SubItems.Add(wyz.w.FirstName + " " + wyz.w.LastName);
                    bp.SubItems.Add(Convert.ToString(wyz.w.Age)); 
                    listView1.Items.Add(bp);
                    

                    
                    lastSNS.Add(wyz.w.SNS);
                }

                if (! lastSNS.Contains(wyz.y.SNS))
                {

                    ListViewItem bp = new ListViewItem(Convert.ToString(wyz.y.SNS), 0);
                    bp.SubItems.Add(wyz.y.FirstName + " " + wyz.y.LastName);
                    listView1.Items.Add(bp);
                    bp.SubItems.Add(Convert.ToString(wyz.y.Age));
                    lastSNS.Add(wyz.y.SNS);
                }

                if (!lastSNS.Contains(wyz.z.SNS))
                {
                    
                    ListViewItem bp = new ListViewItem(Convert.ToString(wyz.z.SNS), 0);
                    bp.SubItems.Add(wyz.z.FirstName + " " + wyz.z.LastName);
                    bp.SubItems.Add(Convert.ToString(wyz.z.Age));
                    listView1.Items.Add(bp);

                    lastSNS.Add(wyz.z.SNS);
                }
            }
        }
       
           
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ViewAlerts_Load(object sender, EventArgs e)
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
    }     
    }

