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
    public partial class FrmAddObj : Form
    {
        public LevelObj Value { get; set; }
        string LayerName;
        string[] CCNT;
        string[] ObjDb;
        Vector3D objPos;

        public FrmAddObj(string[] _CCNT, ObjectDb db, string text, Vector3D SpawnPos)
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
                            case "AddObj":
                                CForm = "FrmAddObj";
                                break;
                            default:
                                break;
                        }
                    }
                    if (LANG.NodeType == XmlNodeType.EndElement)
                    {
                        switch (LANG.Name)
                        {
                            case "AddObj":
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
                        string replace = LANG.ReadElementContentAsString();
                        if (this.Name == CForm)
                        {
                            switch (parent)
                            {
                                default:
                                    ((Label)Controls[label]).Text = replace;
                                    break;
                            }
                        }
                    }
                    else if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("Btn"))
                    {
                        string button = LANG.GetAttribute("name");
                        string parent = LANG.GetAttribute("parent");
                        string replace = LANG.ReadElementContentAsString();
                        if (this.Name == CForm)
                        {
                            switch (parent)
                            {
                                default:
                                    ((Button)Controls[button]).Text = replace;
                                    break;

                            }
                        }

                    }
                    else if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("Chck"))
                    {
                        string cbox = LANG.GetAttribute("name");
                        string parent = LANG.GetAttribute("parent");
                        string replace = LANG.ReadElementContentAsString();
                        if (this.Name == CForm)
                        {
                            switch (parent)
                            {
                                default:
                                    ((CheckBox)Controls[cbox]).Text = replace;
                                    break;
                            }
                        }
                    }

                    else if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("TxtBx"))
                    {
                        string tbx = LANG.GetAttribute("name");
                        string parent = LANG.GetAttribute("parent");
                        string replace = LANG.ReadElementContentAsString();
                        if (this.Name == CForm)
                        {
                            switch (parent)
                            {
                                case "panel1":
                                    Controls[tbx].Text = replace;
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }
            }
            #endregion
            LayerName = text;
            CCNT = _CCNT;           
            comboBox1.Sorted = true;
            objPos = SpawnPos;
            if (db != null)
            {
                List<string> objdb = new List<string>();
                objdb.AddRange(from n in db.Entries.Keys.ToArray() where db.Entries[n].Category != 255 && db.Entries[n].Known == 1 select n);
                ObjDb = objdb.ToArray();
            }
            ObjDb = null;
        }

        void LoadObjList(string[] array)
        {
            comboBox1.Items.Clear();
            if (LayerName == "StartInfo")
            {
                comboBox1.Text = "Mario";
                comboBox1.Enabled = false;
                checkBox1.Enabled = false;
            }
            else if (LayerName == "AreaObjInfo")
            {
                foreach (string s in array)
                {
                    if (!s.EndsWith("*") && s.ToLower().EndsWith("area") && !s.ToLower().Contains("camera")) comboBox1.Items.Add(s);
                }
            }
            else if (LayerName == "CameraAreaInfo")
            {
                foreach (string s in array)
                    if (!s.EndsWith("*") && s.ToLower().EndsWith("area") && s.ToLower().Contains("camera")) comboBox1.Items.Add(s);
            }
            else if (LayerName == "ObjInfo")
            {
                comboBox1.Items.Add("@CameraPositionHelper");
                foreach (string s in array)
                    if (!s.EndsWith("*") && !s.ToLower().EndsWith("area")) comboBox1.Items.Add(s);
            }
            else if (LayerName == "StartEventObjInfo")
            {
                foreach (string s in array)
                    if (!s.EndsWith("*") && s.ToLower().StartsWith("startevent")) comboBox1.Items.Add(s);
            }
            else comboBox1.Items.AddRange(array);
            Value = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "") 
            {
                MessageBox.Show("You can't add nothing as an object!"); return;
            }
            LevelObj obj = new LevelObj();
            if (LayerName != "StartInfo" && LayerName != "AreaObjInfo")
            {
                if (LayerName != "CameraAreaInfo") obj.Prop.Add("ViewId", new Node("-1", "D1"));
                obj.Prop.Add("CameraId", new Node("-1", "D1"));
                if (LayerName == "CameraAreaInfo")
                    obj.Prop.Add("Priority", new Node("-1", "D1"));
            }
            if (LayerName != "StartInfo")
            {
                obj.Prop.Add("l_id", new Node("0", "D1")); 
                if (LayerName != "CameraAreaInfo")
                { 
                        obj.Prop.Add("Arg", new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 });
                }
                obj.Prop.Add("SwitchA", new Node("-1", "D1"));
                obj.Prop.Add("SwitchAppear", new Node("-1", "D1"));
                obj.Prop.Add("SwitchB", new Node("-1", "D1"));
                obj.Prop.Add("SwitchKill", new Node("-1", "D1"));
                obj.Prop.Add("SwitchDeadOn", new Node("-1", "D1"));
            }
            else obj.Prop.Add("MarioNo", new Node("0", "D1"));
            obj.Prop.Add("LayerName", new Node("共通", "A0"));
            obj.Prop.Add("name", new Node(comboBox1.Text, "A0"));
            //obj.Prop.Add("dir_x", new Node("1", "D2"));
            //obj.Prop.Add("dir_y", new Node("1", "D2"));
            //obj.Prop.Add("dir_z", new Node("1", "D2"));
            obj.Prop.Add("dir", new Single[3] { 0, 0, 0 });
            obj.Prop.Add("pos", new Single[3] { (Single)objPos.X , (Single)objPos.Z , (Single) (-objPos.Y) });
            //obj.Prop.Add("pos_x", new Node(objPos.X.ToString(), "D2"));
            //obj.Prop.Add("pos_y", new Node((objPos.Z).ToString(), "D2"));
            //obj.Prop.Add("pos_z", new Node((-objPos.Y).ToString(), "D2"));
            //obj.Prop.Add("scale_x", new Node("1", "D2"));
            //obj.Prop.Add("scale_y", new Node("1", "D2"));
            //obj.Prop.Add("scale_z", new Node("1", "D2"));
            obj.Prop.Add("scale", new Single[3] { 1, 1, 1 });
            Value = obj;
            this.Close();
            
        }

        private void FrmAddObj_Load(object sender, EventArgs e)
        {
            if (checkBox1.Enabled)
            {
                if (Properties.Settings.Default.OnlyKnwonObjs) checkBox1.Checked = true;
                else LoadObjList(CCNT);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ObjDb == null)
            {
                LoadObjList(CCNT);
                return;
            }
            LoadObjList(checkBox1.Checked ? ObjDb : CCNT);
            Properties.Settings.Default.OnlyKnwonObjs = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
