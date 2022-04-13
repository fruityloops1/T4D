using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The4Dimension
{
    public partial class ObjectDbEditor : Form
    {
        ObjectDb database;
        NewDb ndb;
        public NewDb NewDatabase;
        public Dictionary<string, string> CCNT = new Dictionary<string, string>();
        public ObjectDbEditor(ObjectDb db)
        {
            InitializeComponent();
            database = db;
        }
        public ObjectDbEditor(NewDb db)
        {
            InitializeComponent();
            ndb = db;
        }

        bool firstdelete = true;
        private void ObjectDbEditor_Load(object sender, EventArgs e)
        {
            listView1.MultiSelect = false;
            comboBox1.Items.AddRange(ndb.Categories.Values.ToArray());
            comboBox2.Items.AddRange(ndb.Categories.Values.ToArray());
            comboBox3.Items.AddRange(ndb.Types.Values.ToArray());
            comboBox1.Text = "All";
            listView1.View = View.Details;
            //listView1.HeaderStyle = ColumnHeaderStyle.None;
            UpdateResults();
            groupBox2.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            if (Properties.Settings.Default.ExperimentalFeatures)
            {
                button5.Enabled = true;
                button5.Visible = true;

            }
        }

        void fromCCNT()
        {
            foreach (string ccntentry in CCNT.Keys)
            {
                if (!ndb.Entries.ContainsKey(ccntentry))
                {
                    NewDb.NewDbEntry entry = new NewDb.NewDbEntry();
                    entry.filename = ccntentry;
                    entry.dbname = "DB"+ccntentry;
                    entry.modelname = "";
                    entry.type = 1;
                    entry.category = 0;
                    ndb.Entries.Add(entry.filename, entry);
                    ndb.IdtoDB.Add(entry.filename, entry.dbname);
                    ndb.DBtoId.Add(entry.dbname, entry.filename);
                    ndb.IdToModel.Add(entry.filename, entry.modelname);
                }
            }
            UpdateResults();
        }
        void UpdateResults(bool clear = true)
        {
            groupBox2.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            int Category;
            if (comboBox1.Text == "All") Category = -1;
            else Category = ndb.Categories.Keys.ToArray()[ndb.Categories.Values.ToList().IndexOf(comboBox1.Text)];
            List<string> Results = new List<string>();
            if (textBox1.Text.Trim() == "") Results = ndb.Entries.Keys.ToList();
            else
            {
                foreach (string s in ndb.Entries.Keys.ToArray())
                {
                    if (s.ToLower().StartsWith(textBox1.Text.Trim().ToLower()) || s.ToLower() == textBox1.Text.Trim().ToLower()) Results.Add(s);
                    else if (ndb.IdtoDB[s].ToLower().StartsWith(textBox1.Text.Trim().ToLower()) || ndb.IdtoDB[s].ToLower() == textBox1.Text.Trim().ToLower()) Results.Add(s);
                    else if(s.ToLower().Contains(textBox1.Text.Trim().ToLower())) Results.Add(s);
                    else if(ndb.IdtoDB[s].ToLower().Contains(textBox1.Text.Trim().ToLower())) Results.Add(s);

                }
            }
            if (Category != -1)
            {
                List<string> _Results = new List<string>();
                foreach (string s in Results)
                {
                    if (ndb.Entries[s].category == Category) _Results.Add(s);
                }
                Results = _Results;
            }
            if (clear)
            {
                listView1.Items.Clear();
                foreach (string s in Results)
                {
                    listView1.Items.Add(ndb.IdtoDB[s]).Name = ndb.IdtoDB[s];
                    listView1.Items[ndb.IdtoDB[s]].SubItems.Add(s);
                    Color c = Color.Red;
                    //if (database.Entries[s].Known == 1) c = ndb.Entries[s].Complete == 0 ? Color.Orange : Color.Green;
                    //listView1.Items[listView1.Items.Count - 1].ForeColor = c;
                }
            }
            else
            {
                foreach (string s in Results)
                {
                    if (!listView1.Items.ContainsKey(ndb.IdtoDB[s]))
                    {
                        listView1.Items.Add(ndb.IdtoDB[s]).Name = ndb.IdtoDB[s];
                        listView1.Items[ndb.IdtoDB[s]].SubItems.Add(s);
                        Color c = Color.Red;
                        //if (database.Entries[s].Known == 1) c = ndb.Entries[s].Complete == 0 ? Color.Orange : Color.Green;
                        //listView1.Items[listView1.Items.Count - 1].ForeColor = c;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            ArgList.Items.Clear();
            UpdateResults();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateResults();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                groupBox2.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                ArgList.Items.Clear();
                return;
            }
            groupBox2.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            //ObjectDb.ObjectDbEntry entry = database.Entries[listView1.SelectedItems[0].Text];
            NewDb.NewDbEntry dbEntry = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text];
            if (dbEntry.category == 0)
            {
                //label4.Text += "This object is not documented";
                //return;
            }
            //if (entry.Complete == 0) label4.Text += "(This object is not fully known)\r\n";
            Txt_filename.Text = dbEntry.filename;
            Txt_dbname.Text = dbEntry.dbname;
            Txt_extra.Text = dbEntry.extra;
            Txt_model.Text = dbEntry.modelname;
            comboBox2.SelectedItem = ndb.Categories[dbEntry.category];
            comboBox3.SelectedItem = ndb.Types[dbEntry.type];

            ArgList.Items.Clear();
            foreach (NewDb.EntryArg arg in dbEntry.args)
            {
                ArgList.Items.Add(arg.arg_id.ToString()).Name = arg.arg_id.ToString();
                ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.name);
                ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.type);
                ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.info);
            }


           /* if (dbEntry.Fields.Count > 0)
            {
                label4.Text += "\r\n\r\nArgs:";
                for (int i = 0; i < entry.Fields.Count; i++)
                {
                    label4.Text += "\r\nArg[" + entry.Fields[i].id + "] Name: " + entry.Fields[i].name + "  Type: " + entry.Fields[i].type + "\r\n  Notes:" + entry.Fields[i].notes + "\r\n  Values:" + entry.Fields[i].values;
                }
            }
            label4.Text += "\r\n\r\nFiles:" + entry.files + "\r\n\r\nObject category: " + database.Categories[entry.Category];*/
        }
        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = ((ListView)sender).Columns[e.ColumnIndex].Width;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Add a blank object to the database 
            NewDb.NewDbEntry entry = new NewDb.NewDbEntry();
            if (!ndb.Entries.ContainsKey("NewObject"))
            {
                entry.filename = "NewObject";
                entry.dbname = "New Object";
                entry.type = 1;
                entry.category = 0;
                ndb.Entries.Add(entry.filename, entry);
                ndb.IdtoDB.Add(entry.filename, entry.dbname);
                ndb.DBtoId.Add(entry.dbname, entry.filename);
                ndb.IdToModel.Add(entry.filename, entry.modelname);
            }
            else
            {
                for (int i = 0; i<ndb.Entries.Count; i++)
                {
                    if (!ndb.Entries.ContainsKey("NewObject" + i))
                    {
                        entry.filename = "NewObject"+i;
                        entry.dbname = "New Object"+i;
                        break;
                    }
                }
                entry.type = 1;
                entry.category = 0;
                ndb.Entries.Add(entry.filename, entry);
                ndb.IdtoDB.Add(entry.filename, entry.dbname);
                ndb.DBtoId.Add(entry.dbname, entry.filename);
                ndb.IdToModel.Add(entry.filename, entry.modelname);
            }
            UpdateResults();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //remove selected object from database after warning
            if (( !firstdelete || MessageBox.Show("Are you sure you want to remove this object from the database? (you will still be able to access it after saving the database in the backup file)","Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)&&listView1.SelectedItems.Count > 0)
            {
                int index = ndb.Entries.Keys.ToList().IndexOf(listView1.SelectedItems[0].SubItems[1].Text);
                ndb.Entries.Remove(listView1.SelectedItems[0].SubItems[1].Text);
                string tempdb = ndb.IdtoDB.Values.ToList()[index];
                string temp = ndb.DBtoId.Values.ToList()[index];
                ndb.IdToModel.Remove(temp);
                ndb.IdtoDB.Remove(temp);
                ndb.DBtoId.Remove(tempdb);
                listView1.Items.Remove(listView1.SelectedItems[0]);

                UpdateResults();
                if (index > 0)
                {
                    listView1.Items[index - 1].Selected = true;
                }
                firstdelete = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //duplicate
            NewDb.NewDbEntry entry = new NewDb.NewDbEntry();
            entry.filename = listView1.SelectedItems[0].SubItems[1].Text + "_c";
            entry.dbname = listView1.SelectedItems[0].Text + "_Dup";
            entry.modelname = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].modelname;

            foreach (NewDb.EntryArg arga in ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args)
            {
                entry.args.Add(arga);
            }            
            entry.type = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].type;
            entry.category = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].category;
            entry.extra = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].extra;
            ndb.Entries.Add(entry.filename, entry);
            ndb.IdtoDB.Add(entry.filename, entry.dbname);
            ndb.DBtoId.Add(entry.dbname, entry.filename);
            ndb.IdToModel.Add(entry.filename, entry.modelname);

            UpdateResults(false);

            listView1.Items[listView1.Items.IndexOfKey(entry.dbname)].Selected = true;

            groupBox2.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox4_Validated(sender, null);
            }
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            if (((TextBox)sender).Name.Substring(4) == "filename")
            {
                if (ndb.Entries.ContainsKey(((TextBox)sender).Text))
                {
                    if (((TextBox)sender).Focused)
                    {
                        MessageBox.Show("An object with this filename already exists!");
                    }
                    ((TextBox)sender).Text = listView1.SelectedItems[0].SubItems[1].Text;
                }
                else
                {
                    NewDb.NewDbEntry entry = new NewDb.NewDbEntry();
                    entry.filename = ((TextBox)sender).Text;
                    entry.dbname = listView1.SelectedItems[0].Text;
                    entry.modelname = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].modelname;
                    entry.args = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args;
                    entry.type = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].type;
                    entry.category = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].category;
                    entry.extra = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].extra;
                    ndb.Entries.Add(entry.filename, entry);
                    ndb.Entries.Remove(listView1.SelectedItems[0].SubItems[1].Text);
                    string tempdb = ndb.IdtoDB[listView1.SelectedItems[0].SubItems[1].Text];
                    string temp = ndb.DBtoId[tempdb];
                    ndb.IdToModel.Remove(temp);
                    ndb.IdtoDB.Remove(temp);
                    ndb.DBtoId.Remove(tempdb);
                    ndb.IdtoDB.Add(entry.filename, entry.dbname);
                    ndb.DBtoId.Add(entry.dbname, entry.filename);
                    ndb.IdToModel.Add(entry.filename, entry.modelname);
                    //remove old entry, add again with new info
                    listView1.SelectedItems[0].SubItems[1].Text = ((TextBox)sender).Text; listView1.SelectedItems[0].SubItems[1].Name = ((TextBox)sender).Text;

                }

            }
            else if (((TextBox)sender).Name.Substring(4) == "dbname")
            {
                if (ndb.DBtoId.ContainsKey(((TextBox)sender).Text))
                {
                    if (((TextBox)sender).Focused)
                    {
                        MessageBox.Show("An object with this database name already exists!");
                    }
                    ((TextBox)sender).Text = listView1.SelectedItems[0].Text;
                }
                else
                {
                    NewDb.NewDbEntry entry = new NewDb.NewDbEntry();
                    entry.filename = listView1.SelectedItems[0].SubItems[1].Text;
                    entry.dbname = ((TextBox)sender).Text;
                    entry.modelname = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].modelname;
                    entry.args = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args;
                    entry.type = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].type;
                    entry.category = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].category;
                    entry.extra = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].extra;

                    ndb.Entries.Remove(listView1.SelectedItems[0].SubItems[1].Text);
                    ndb.Entries.Add(entry.filename, entry);
                    string tempdb = ndb.IdtoDB[listView1.SelectedItems[0].SubItems[1].Text];
                    string temp = ndb.DBtoId[tempdb];
                    ndb.IdtoDB.Remove(temp);
                    ndb.DBtoId.Remove(tempdb);
                    ndb.IdtoDB.Add(entry.filename, entry.dbname);
                    ndb.DBtoId.Add(entry.dbname, entry.filename);
                    //remove old entry, add again with new info
                    listView1.SelectedItems[0].Text = ((TextBox)sender).Text;
                }
            }
            else if (((TextBox)sender).Name.Substring(4) == "extra")
            {
                ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].extra = ((TextBox)sender).Text;
            }
            else if (((TextBox)sender).Name.Substring(4) == "model")
            {
                ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].modelname = ((TextBox)sender).Text;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //save database from database contents
            NewDatabase = ndb;
            Close();
        }

        private void Args_DoubleClick(object sender, EventArgs e)
        {
            //open arg editor window and edit current arg
            ObjectDB.ArgEditor A = new ObjectDB.ArgEditor(ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args, ArgList.SelectedItems[0].Index);
            A.ShowDialog();
            if (A.arg == null) return;
            ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args.RemoveAt(ArgList.SelectedItems[0].Index);
            ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args.Add(A.arg);
            NewDb.EntryArg arg = A.arg;

            ArgList.SelectedItems[0].Remove();
            ArgList.Items.Add(arg.arg_id.ToString()).Name = arg.arg_id.ToString();
            ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.name);
            ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.type);
            ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.info);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //object category

            ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].type = comboBox3.SelectedIndex+1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //object type 

            ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].category = comboBox2.SelectedIndex;
        }

        private void addArgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectDB.ArgEditor A = new ObjectDB.ArgEditor(ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args);
            A.ShowDialog();

            if (A.arg == null) return;
            ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args.Add(A.arg);
            NewDb.EntryArg arg = A.arg;
            ArgList.Items.Add(arg.arg_id.ToString()).Name = arg.arg_id.ToString();
            ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.name);
            ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.type);
            ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.info);
        }

        private void removeArgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ArgList.SelectedItems.Count < 1) return;
            int indx = ArgList.SelectedItems[0].Index;
            ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args.RemoveAt(ArgList.SelectedItems[0].Index);
            ArgList.SelectedItems[0].Remove();
            if (indx > 0)
            {
                ArgList.Items[indx - 1].Selected = true;
            }
        }

        private void ArgList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                removeArgToolStripMenuItem_Click(null, null);
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                button2_Click(null, null);
            }
        }

        private void CCNT_Click(object sender, EventArgs e)
        {
            fromCCNT();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (ArgList.SelectedIndices.Count == 1 && ArgList.SelectedItems[0].Index!=0)
            {
                NewDb.EntryArg temparg = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args[ArgList.SelectedItems[0].Index];
                ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args.RemoveAt(ArgList.SelectedItems[0].Index);
                ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args.Insert(ArgList.SelectedItems[0].Index - 1, temparg);
                int oldindx = ArgList.SelectedItems[0].Index - 1;
                ArgList.Items.Clear();

                NewDb.NewDbEntry dbEntry = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text];
                foreach (NewDb.EntryArg arg in dbEntry.args)
                {
                    ArgList.Items.Add(arg.arg_id.ToString()).Name = arg.arg_id.ToString();
                    ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.name);
                    ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.type);
                    ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.info);
                }
                ArgList.Items[oldindx].Selected = true;
            }
        }

        private void MoveArgDownBtn_Click(object sender, EventArgs e)
        {

            if (ArgList.SelectedIndices.Count == 1 && ArgList.SelectedItems[0].Index != ArgList.Items.Count-1)
            {
                NewDb.EntryArg temparg = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args[ArgList.SelectedItems[0].Index];
                ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args.RemoveAt(ArgList.SelectedItems[0].Index);
                ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text].args.Insert(ArgList.SelectedItems[0].Index + 1, temparg);
                int oldindx = ArgList.SelectedItems[0].Index + 1;
                ArgList.Items.Clear();

                NewDb.NewDbEntry dbEntry = ndb.Entries[listView1.SelectedItems[0].SubItems[1].Text];
                foreach (NewDb.EntryArg arg in dbEntry.args)
                {
                    ArgList.Items.Add(arg.arg_id.ToString()).Name = arg.arg_id.ToString();
                    ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.name);
                    ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.type);
                    ArgList.Items[ArgList.Items.IndexOfKey(arg.arg_id.ToString())].SubItems.Add(arg.info);
                }
                ArgList.Items[oldindx].Selected = true;
            }
        }
    }
}
