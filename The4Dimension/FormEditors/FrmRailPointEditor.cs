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
using System.Windows.Media.Media3D;
using System.Xml;

namespace The4Dimension
{
    public partial class FrmRailPointEditor : Form
    {
        public List<Rail.Point> Value { get; set; }
        public List<Rail.Point> OldValue = new List<Rail.Point>();
        Form1 owner;
        private Dictionary<string, string> strings;
        public FrmRailPointEditor(List<Rail.Point> list)
        {
            strings = new Dictionary<string, string>(); 
            InitializeComponent();
            Value = list;
            foreach (Rail.Point p in Value) OldValue.Add(p.Clone());
            
        }

        private void FrmObjEditor_Load(object sender, EventArgs e)
        {
            #region Translation
            if (Properties.Settings.Default.CurrentLang != 0)
            {
                string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\LANG\\" + Properties.Settings.Default.CurrentLangName + ".xml";
                XmlReader LANG = XmlReader.Create(path);
                string CForm = null;
                while (LANG.Read())
                {

                    if (LANG.NodeType == XmlNodeType.Element)
                    {
                        switch (LANG.Name)
                        {
                            case "RailEditor":
                                CForm = "FrmRailPointEditor";
                                break;
                            default:
                                break;
                        }
                    }
                    if (LANG.NodeType == XmlNodeType.EndElement)
                    {
                        switch (LANG.Name)
                        {
                            case "RailEditor":
                                CForm = null;
                                break;
                            default:
                                break;
                        }
                    }

                    if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("Lbl"))
                    {
                        string label = LANG.GetAttribute("name");
                        string parent = LANG.GetAttribute("parent");
                        string text = LANG.ReadElementContentAsString();
                        if (this.Name == CForm)
                        {
                            switch (parent)
                            {
                                default:
                                    ((Label)Controls[label]).Text = text;
                                    break;
                            }
                        }
                    }
                    else if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("Var"))
                    {
                        string var = LANG.GetAttribute("name");
                        string parent = LANG.GetAttribute("parent");
                        string text = LANG.ReadElementContentAsString();
                        if (this.Name == CForm)
                        {
                            switch (parent)
                            {
                                default:
                                    strings.Add(var, text);
                                    break;
                            }
                        }
                    }
                    else if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("Btn"))
                    {
                        string button = LANG.GetAttribute("name");
                        string parent = LANG.GetAttribute("parent");
                        string text = LANG.ReadElementContentAsString();
                        if (this.Name == CForm)
                        {
                            switch (parent)
                            {
                                default:
                                    ((Button)Controls[button]).Text = text;
                                    break;

                            }
                        }

                    }
                    else if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("Chck"))
                    {
                        string cbox = LANG.GetAttribute("name");
                        string parent = LANG.GetAttribute("parent");
                        string text = LANG.ReadElementContentAsString();
                        if (this.Name == CForm)
                        {
                            switch (parent)
                            {
                                default:
                                    ((CheckBox)Controls[cbox]).Text = text;
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                //Do not edit these unless you find translation errors, change the ones in your preferred language xml instead
                strings.Add("Name", "Edit point list");
                strings.Add("2Point", "A rail must have at least 2 points!");
                strings.Add("NoPaste", "You can't paste this here");
            }
            #endregion
            owner = (Form1)Application.OpenForms["Form1"];
            RefreshListBox();
            if (listBox1.Items.Count > 0) listBox1.SelectedIndex = 0;
            this.Text = strings["Name"];
        }

        void RefreshListBox()
        {
            propertyGrid1.SelectedObject = null;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Value.ToArray());
            listBox1.SelectedIndex = -1;
        }

        private void pasteValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteValue(Form1.clipboard[Form1.clipboard.Count - 1]);
            ClipBoardMenu.Close();
        }

        private void copyPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyValue("pos_");
        }

        private void ClipBoardMenu_CopyArgs_Click(object sender, EventArgs e)
        {
            CopyValue("Arg");
        }

        void CopyValue(string value)
        {
            if (listBox1.SelectedIndex == -1) return;
            ClipBoardItem cl = new ClipBoardItem();
            if (value == "pos_")
            {
                cl.Type = ClipBoardItem.ClipboardType.Position;
                Single[] ds = new Single[] { Value[listBox1.SelectedIndex].X, Value[listBox1.SelectedIndex].Y,  Value[listBox1.SelectedIndex].Z};
                cl.Pos = ds;
            }
            else if (value == "Arg")
            {
                cl.Type = ClipBoardItem.ClipboardType.IntArray;
                cl.Args = Value[listBox1.SelectedIndex].Args.ToArray();
            }
            Form1.clipboard.Add(cl);
            if (Form1.clipboard.Count > 10) Form1.clipboard.RemoveAt(0);
            ClipBoardMenu_Paste.DropDownItems.Clear();
            List<ToolStripMenuItem> Items = new List<ToolStripMenuItem>();
            for (int i = 0; i < Form1.clipboard.Count; i++)
            {
                ToolStripMenuItem btn = new ToolStripMenuItem();
                btn.Name = "ClipboardN" + i.ToString();
                btn.Text = Form1.clipboard[i].ToString();
                btn.Click += QuickClipboardItem_Click;
                Items.Add(btn);
            }
            Items.Reverse();
            ClipBoardMenu_Paste.DropDownItems.AddRange(Items.ToArray());
        }

        private void QuickClipboardItem_Click(object sender, EventArgs e)
        {
            string SenderName = ((ToolStripMenuItem)sender).Name;
            int index = int.Parse(SenderName.Substring("ClipboardN".Length));
            PasteValue(Form1.clipboard[index]);
        }

        void PasteValue(ClipBoardItem itm)
        {
            if (listBox1.SelectedIndex == -1) return;
            if (itm.Type == ClipBoardItem.ClipboardType.Position)
            {
                Value[listBox1.SelectedIndex].X = itm.Pos[0];
                Value[listBox1.SelectedIndex].Y = itm.Pos[1];
                Value[listBox1.SelectedIndex].Z = itm.Pos[2];
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.IntArray)
            {
                Value[listBox1.SelectedIndex].Args = itm.Args.ToList();
            }
            else
            {
                MessageBox.Show(strings["NoPaste"]);
                return;
            }
            propertyGrid1.Refresh();
        }

        private void ClipBoardMenu_opening(object sender, CancelEventArgs e)
        {
            ClipBoardMenu_Paste.DropDownItems.Clear();
            List<ToolStripMenuItem> Items = new List<ToolStripMenuItem>();
            for (int i = 0; i < Form1.clipboard.Count; i++)
            {
                ToolStripMenuItem btn = new ToolStripMenuItem();
                btn.Name = "ClipboardN" + i.ToString();
                btn.Text = Form1.clipboard[i].ToString();
                btn.Click += QuickClipboardItem_Click;
                Items.Add(btn);
            }
            Items.Reverse();
            ClipBoardMenu_Paste.DropDownItems.AddRange(Items.ToArray());
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            if (listBox1.SelectedItems.Count > 1) propertyGrid1.Enabled = false;
            else
            {
                propertyGrid1.Enabled = true;
                propertyGrid1.SelectedObject = Value[listBox1.SelectedIndex];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Value.Add(new Rail.Point(Value.Count));
            RefreshListBox();
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            if (listBox1.Items.Count - listBox1.SelectedItems.Count +1 < 3)
            {
                MessageBox.Show(strings["2Point"]);
                return;
            }
            foreach (Rail.Point i in listBox1.SelectedItems)
            {
                Value.Remove(i);
            }
            RefreshListBox();
        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            if (owner.comboBox1.Text == "AllRailInfos") owner.RailPointsChanged(OldValue);
        }

        Point3D[] GetPointArray()
        {
            List<Point3D> points = new List<Point3D>();
            foreach (Rail.Point p in Value) points.Add(new Point3D(p.X, -p.Z, p.Y));
            return points.ToArray();
        }

        private void property_changed(object s, PropertyValueChangedEventArgs e)
        {
            if (owner.comboBox1.Text == "AllRailInfos") owner.UpdateSelectedRailView();
            listBox1.Items[listBox1.SelectedIndex] = Value[listBox1.SelectedIndex].ToString();
        }
    }
}
