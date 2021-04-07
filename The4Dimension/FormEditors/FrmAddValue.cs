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
using System.Xml;

namespace The4Dimension
{
    public partial class FrmAddValue : Form
    {
        LevelObj Source;
        public FrmAddValue(LevelObj s)
        {
            InitializeComponent();
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
                            case "AddProp":
                                CForm = "FrmAddValue";
                                break;
                            default:
                                break;
                        }
                    }
                    if (LANG.NodeType == XmlNodeType.EndElement)
                    {
                        switch (LANG.Name)
                        {
                            case "AddProp":
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

                    else if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("TxtBx"))
                    {
                        string tbx = LANG.GetAttribute("name");
                        string parent = LANG.GetAttribute("parent");
                        string text = LANG.ReadElementContentAsString();
                        if (this.Name == CForm)
                        {
                            switch (parent)
                            {
                                case "panel1":
                                    Controls[tbx].Text = text;
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }
            }
            #endregion
            result = null;
            propertyGrid1.SelectedObject = result;
            resName = null;
            Source = s;
        }

        private void FrmAddValue_Load(object sender, EventArgs e)
        {

        }

        public object result { get; set; }

        public string resName { get; set; }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) { result = new C0List(); propertyGrid1.SelectedObject = (C0List)result; }
            else if (comboBox1.SelectedIndex == 1) { result = new int[1] { 0 }; propertyGrid1.SelectedObject = (int[])result; }
            else if (comboBox1.SelectedIndex == 2) { result = new Rail(); propertyGrid1.SelectedObject = (Rail)result; }
            else if (comboBox1.SelectedIndex == 3) { result = new Node("", "A0"); propertyGrid1.SelectedObject = (Node)result; }
            else if (comboBox1.SelectedIndex == 4) { result = new Node("0", "D1"); propertyGrid1.SelectedObject = (Node)result; }
            else if (comboBox1.SelectedIndex == 5) { result = new Node("0", "D2"); propertyGrid1.SelectedObject = (Node)result; }
            else if (comboBox1.SelectedIndex == 6)
            {
                label2.Visible = true;
                numericUpDown1.Visible = true;
                return;
            }
            label2.Visible = false;
            numericUpDown1.Visible = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            result = new Node("", BitConverter.ToString(new byte[1] { (byte)numericUpDown1.Value }));
            propertyGrid1.SelectedObject = (Node)result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "") { MessageBox.Show("Select a valid type"); return; }
            if (textBox1.Text.Trim() != "" && !Source.Prop.ContainsKey(textBox1.Text.Trim()))
            {
                resName = textBox1.Text.Trim();
                this.Close();
            }
            else MessageBox.Show("This name is not valid or is already in use");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resName = null;
            this.Close();
        }

        private void AddChild_Click(object sender, EventArgs e)
        {
            resName = "GenerateChildren";
            result = new C0List();
            propertyGrid1.SelectedObject = (C0List)result;
            this.Close();
        }
    }
}