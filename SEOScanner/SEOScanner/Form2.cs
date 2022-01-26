using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEOScanner
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form1 parentForm;
        public int index;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == FileJSON.FROM_TAG)
            {
                textBox6.ReadOnly = true;
                textBox6.Text = "";
            }
            else if (comboBox1.Text == FileJSON.FROM_ATTRIBUTE)
            {
                textBox6.ReadOnly = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.Text == "Новая запись")
            {
                ListViewItem item = new ListViewItem();
                ListViewItem.ListViewSubItem subitem = new ListViewItem.ListViewSubItem();
                subitem.Text = textBox1.Text;
                item.SubItems.Add(subitem);
                subitem = new ListViewItem.ListViewSubItem();
                subitem.Text = textBox2.Text;
                item.SubItems.Add(subitem);
                subitem = new ListViewItem.ListViewSubItem();
                subitem.Text = textBox3.Text;
                item.SubItems.Add(subitem);
                subitem = new ListViewItem.ListViewSubItem();
                subitem.Text = textBox4.Text;
                item.SubItems.Add(subitem);
                subitem = new ListViewItem.ListViewSubItem();
                subitem.Text = textBox5.Text;
                item.SubItems.Add(subitem);
                subitem = new ListViewItem.ListViewSubItem();
                subitem.Text = comboBox1.Text;
                item.SubItems.Add(subitem);
                subitem = new ListViewItem.ListViewSubItem();
                subitem.Text = textBox6.Text;
                item.SubItems.Add(subitem);
                parentForm.listView2.Items.Add(item);

            }
            if (this.Text == "Редактировать запись")
            {
                parentForm.listView2.Items[index].SubItems[1].Text = textBox1.Text;
                parentForm.listView2.Items[index].SubItems[2].Text = textBox2.Text;
                parentForm.listView2.Items[index].SubItems[3].Text = textBox3.Text;
                parentForm.listView2.Items[index].SubItems[4].Text = textBox4.Text;
                parentForm.listView2.Items[index].SubItems[5].Text = textBox5.Text;
                parentForm.listView2.Items[index].SubItems[6].Text = comboBox1.Text;
                parentForm.listView2.Items[index].SubItems[7].Text = textBox6.Text;
            }
            this.parentForm.updateConfig();
            Close();
        }
    }
}
