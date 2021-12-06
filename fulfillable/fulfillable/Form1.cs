using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace fulfillable
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs s)
        {
           
                listBox1.Items.Clear();
                Tuple<string,List<OrdersHelper.OrdersData>> orders = OrdersHelper.Get_orders(tBOrder.Text);
                if (orders.Item2 != null)
                {
                    listBox1.Items.Add(string.Concat(("product_id").PadRight(20, ' '), ("quantity").PadRight(25, ' '), ("priority").PadRight(20, ' '), "created_at"));
                    foreach (var data in orders.Item2)
                    {
                        string Lines = String.Concat((data.product_id.PadRight(30, ' ')),
                                       (data.quantity.ToString().PadRight(30, ' ')),
                                       (data.priorityName.PadRight(30, ' ')),
                                        data.created_at.ToString());
                        listBox1.Items.Add(Lines);
                    }


                }
                else listBox1.Items.Add(orders.Item1);
           
        }
    }
}
