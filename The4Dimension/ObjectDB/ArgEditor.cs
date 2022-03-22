using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The4Dimension.ObjectDB
{
    public partial class ArgEditor : Form
    {
        public NewDb.EntryArg arg = null;
        List<NewDb.EntryArg> entryArgs = new List<NewDb.EntryArg>();
        public ArgEditor(List<NewDb.EntryArg> args)
        { // create new argument
            InitializeComponent();
            entryArgs = args;
            radioButton2.Checked = true;
            radioButton1.Checked = true;
            comboBox1.SelectedIndex = 0;
        }

        bool editarg = false;
        int entryindx = 0;

        public ArgEditor(List<NewDb.EntryArg> args, int idx)
        {
            editarg = true;
            entryindx = idx;
            //edit argument
            InitializeComponent();
            entryArgs = args;
            numericUpDown1.Value = args[idx].arg_id;
            textBox1.Text = args[idx].name;
            textBox2.Text = args[idx].info;
            if (args[idx].type == "bool")
            {
                radioButton1.Checked = true;
                comboBox1.SelectedItem = args[idx].default_value;

            }else if (args[idx].type == "int")
            {
                radioButton2.Checked = true;
                numericUpDown3.Value = int.Parse(args[idx].default_value);
                numericUpDown4.Value = args[idx].min;
                numericUpDown5.Value = args[idx].max;
            }
            else
            {
                numericUpDown3.Value = int.Parse(args[idx].default_value);
                radioButton3.Checked = true;
                foreach (string name in args[idx].options.Keys)
                {
                    listView1.Items.Add(name).Name = name;
                    listView1.Items[name].SubItems.Add(args[idx].options[name]);
                }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                switch (((RadioButton)sender).Text)
                {
                    case "Bool":
                        comboBox1.Enabled = true;
                        radioButton2.Checked = false;
                        radioButton3.Checked = false;
                        numericUpDown3.Enabled = false;
                        IntGrp.Enabled = false;
                        OptGrp.Enabled = false;
                        break;
                    case "Int":
                        comboBox1.Enabled = false;
                        radioButton1.Checked = false;
                        radioButton3.Checked = false;
                        numericUpDown3.Enabled = true;
                        IntGrp.Enabled = true;
                        OptGrp.Enabled = false;
                        break;
                    case "Options":
                        comboBox1.Enabled = false;
                        radioButton2.Checked = false;
                        radioButton1.Checked = false;
                        numericUpDown3.Enabled = true;
                        IntGrp.Enabled = false;
                        OptGrp.Enabled = true;
                        break;
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 0) return;
            listView1.SelectedItems[0].Remove();
            listView1.SelectedItems.Clear();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (!editarg)
            {
                for (int i = 0; i < entryArgs.Count; i++)
                {
                    if (entryArgs[i].arg_id == numericUpDown1.Value) { MessageBox.Show("This object already contains an argument of id " + entryArgs[i].arg_id); return; }
                    if (entryArgs[i].name == textBox1.Text) { MessageBox.Show("This object already contains an argument of name \"" + entryArgs[i].name + "\""); return; }
                }
            }
            else
            {
                for (int i = 0; i < entryArgs.Count; i++)
                {
                    if (entryArgs[i].arg_id == numericUpDown1.Value && entryArgs[i].arg_id != entryArgs[entryindx].arg_id) { MessageBox.Show("This object already contains an argument of id " + entryArgs[i].arg_id); return; }
                    if (entryArgs[i].name == textBox1.Text && entryArgs[i].name != entryArgs[entryindx].name) { MessageBox.Show("This object already contains an argument of name \"" + entryArgs[i].name + "\""); return; }
                }
            }
            arg = new NewDb.EntryArg();
            arg.name = textBox1.Text;
            arg.arg_id = (int)numericUpDown1.Value;
            arg.info = textBox2.Text;
            if (radioButton1.Checked)
            {
                arg.default_value = comboBox1.Text;
                arg.type = "bool";
            }
            else if (radioButton2.Checked)
            {
                arg.default_value = numericUpDown3.Value.ToString();
                arg.min = (int)numericUpDown4.Value;
                arg.max = (int)numericUpDown5.Value;
                arg.type = "int";
            }
            else
            {
                arg.default_value = numericUpDown3.Value.ToString();
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    arg.options.Add(listView1.Items[i].Text, listView1.Items[i].SubItems[1].Text);
                    arg.revoptions.Add(listView1.Items[i].SubItems[1].Text, listView1.Items[i].Text);
                }
                if (!arg.revoptions.ContainsKey(arg.default_value)) { MessageBox.Show("This argument doesn't have a default value, please add a new option with the default value you added"); return; }
                arg.type = "option";
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!listView1.Items.ContainsKey(textBox3.Text))
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].SubItems[1].Text == numericUpDown2.Value.ToString()) return;
                }
                listView1.Items.Add(textBox3.Text).Name = textBox3.Text;
                listView1.Items[textBox3.Text].SubItems.Add(numericUpDown2.Value.ToString());
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            arg = null;
            Close();
        }
    }
}
