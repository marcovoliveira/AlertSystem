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
    public partial class ViewUser : Form
    {
        public ViewUser()
        {
            InitializeComponent();
            ServiceHealthClient web = new ServiceHealthClient();
            var p = web.listPatient();

            foreach (Patient item in p)
            {

                ListViewItem ut = new ListViewItem(Convert.ToString(item.SNS), 0);
                ut.SubItems.Add(item.FirstName + " " + item.LastName);
                ut.SubItems.Add(Convert.ToString(item.Age));
                ut.SubItems.Add(Convert.ToString(item.Birthdate));
                ut.SubItems.Add(Convert.ToString(item.Activo));


                listView1.Items.Add(ut);
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
    }
}
