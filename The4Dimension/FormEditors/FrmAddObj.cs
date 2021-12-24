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
        int LayerNum;
        string[] CCNT;
        string[] ObjDb;
        public NewDb ndb = new NewDb();
        Vector3D objPos;
        bool usingdb = false;
        List<DbCategory> categories = new List<DbCategory>();
        Dictionary<string, Dictionary<string, string>> categorisedDB = new Dictionary<string, Dictionary<string, string>>();//category, dbname, name

        public FrmAddObj(string[] _CCNT, NewDb db, string text, Vector3D SpawnPos)
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
            switch (LayerName)
            {
                case "StartInfo":
                    LayerNum = 2;
                    break;
                case "AreaObjInfo":
                    LayerNum = 3;
                    break;
                case "CameraAreaInfo":
                    LayerNum = 4;
                    break;
                case "ObjInfo":
                    LayerNum = 1;
                    break;
                case "StartEventObjInfo":
                    LayerNum = 6;
                    break;
                case "DemoSceneObjInfo":
                    LayerNum = 7;
                    break;
                default:
                    LayerNum = 5;
                    break;
            }
            CCNT = _CCNT;           
            comboBox1.Sorted = true;
            objPos = SpawnPos;
            
            if (db != null)
            {
                ndb = db;
                List<string> objdb = new List<string>();
                foreach (string key in db.Entries.Keys)
                {
                    objdb.Add(db.Entries[key].dbname);
                }
                Dictionary<string, string> aaa = new Dictionary<string, string>();
                aaa.Add("", "");//Goomba, Kuribo
                objdb[ndb.Entries["Kuribo"].category-2] = "";
                //Dictionary<int, string> categories = new Dictionary<int, string>(ndb.Categories);
                DbCategory all = new DbCategory();
                all.name = "All";
                foreach (NewDb.NewDbEntry entry in ndb.Entries.Values)
                {
                    all.objdict.Add(entry.dbname, entry.filename);
                }
                all.id = -1;
                categories.Add(all);
                foreach (KeyValuePair<int, string> keyValuePair in ndb.Categories)
                {
                    DbCategory category = new DbCategory();
                    category.name = keyValuePair.Value;
                    category.id = keyValuePair.Key;
                    foreach (NewDb.NewDbEntry entry in ndb.Entries.Values)
                    {
                        if (entry.category == category.id)
                        {
                            category.objdict.Add(entry.dbname,entry.filename);
                        }
                    }
                    categories.Add(category);
                }

                ObjDb = objdb.ToArray();
            }
            else
            {
                listView1.Enabled = false;
                comboBox2.Enabled = false;
                checkBox1.Checked = false;
            }
        }

        void LoadObjList(string[] array)
        {
            if (!usingdb)
            {
                comboBox1.Items.Clear();
                switch (LayerName)
                {
                    case "StartInfo":
                        comboBox1.Text = "Mario";
                        comboBox1.Enabled = false;
                        checkBox1.Enabled = false;
                        break;
                    case "AreaObjInfo":
                        foreach (string s in array)
                        {
                            if (!s.EndsWith("*") && s.ToLower().EndsWith("area") && !s.ToLower().Contains("camera")) comboBox1.Items.Add(s);
                        }
                        break;
                    case "CameraAreaInfo":
                        foreach (string s in array)
                        {
                            if (!s.EndsWith("*") && s.ToLower().EndsWith("area") && s.ToLower().Contains("camera")) comboBox1.Items.Add(s);
                        }
                        break;
                    case "ObjInfo":
                        comboBox1.Items.Add("@CameraPositionHelper");
                        foreach (string s in array)
                        {
                            if (!s.EndsWith("*") && !s.ToLower().EndsWith("area")) comboBox1.Items.Add(s);
                        }
                        break;
                    case "StartEventObjInfo":
                        foreach (string s in array)
                        {
                            if (!s.EndsWith("*") && s.ToLower().StartsWith("startevent")) comboBox1.Items.Add(s);
                        }
                        break;
                    case "DemoSceneObjInfo":
                        foreach (string s in array)
                        {
                            if (!s.EndsWith("*") && s.ToLower().Contains("demo")) comboBox1.Items.Add(s);
                        }
                        break;
                    default:
                        comboBox1.Items.AddRange(array);
                        break;
                }
            }
            else
            {
                comboBox2.Items.Clear();
                foreach (DbCategory cat in categories)
                {
                    comboBox2.Items.Add(cat.name);
                }
                comboBox2.SelectedIndex = 0;
                listView1.Items.Clear();
                int index = 0;
                foreach (KeyValuePair<string, string> objects in categories[comboBox2.SelectedIndex].objdict)
                {
                    if (LayerNum == ndb.Entries[objects.Value].type/* || LayerNum == 1*/)
                    {
                        listView1.Items.Add(objects.Key);
                        listView1.Items[index].SubItems.Add(objects.Value);
                        if (ndb.Entries[objects.Value].extra != "")
                        {
                            listView1.Items[index].ToolTipText = ndb.Entries[objects.Value].extra;
                        }
                        index++;
                    }
                }
            }
            Value = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!usingdb)
            {
                if (comboBox1.Text == "")
                {
                    MessageBox.Show("You can't add nothing as an object!"); return;
                }
                LevelObj obj = new LevelObj();
                string name = "";
                ndb.DBtoId.TryGetValue(comboBox1.Text, out name);
                if (LayerName == "ObjInfo") { obj.Prop.Add("ClippingGroupId", new Node("-1", "D1")); }
                if (LayerName == "DemoSceneObjInfo")
                {
                    obj.Prop.Add("Action1", new Node("-", "A0"));
                    obj.Prop.Add("Action2", new Node("-", "A0"));
                    obj.Prop.Add("Action3", new Node("-", "A0"));
                    obj.Prop.Add("Action4", new Node("-", "A0"));
                    obj.Prop.Add("Action5", new Node("-", "A0"));
                    obj.Prop.Add("LuigiType", new Node("Common", "A0"));
                    obj.Prop.Add("MarioType", new Node("Common", "A0"));
                    obj.Prop.Add("ModelName", new Node("DemoBird", "A0"));
                    obj.Prop.Add("SuffixName", new Node("-", "A0"));
                }
                if (LayerName != "StartInfo" && LayerName != "AreaObjInfo" && LayerName != "DemoSceneObjInfo")
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
                        obj.Prop.Add("Arg", new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 });

                        if (name != null)
                        {
                            NewDb.NewDbEntry DbEntry = new NewDb.NewDbEntry();
                            ndb.Entries.TryGetValue(name, out DbEntry);
                            object array = new int[10];
                            obj.Prop.TryGetValue("Arg", out array);
                            foreach (NewDb.EntryArg arg in DbEntry.args)
                            {
                                int value = -1;
                                if (arg.type == "bool")
                                {
                                    if (arg.default_value == "true")
                                    {
                                        value = 1;
                                    }
                                    else
                                    {
                                        value = -1;
                                    }
                                }
                                else
                                {
                                    value = int.Parse(arg.default_value);
                                }
                                ((int[])array)[arg.arg_id] = value;
                            }
                        }

                    }
                    obj.Prop.Add("SwitchAppear", new Node("-1", "D1"));
                    obj.Prop.Add("SwitchA", new Node("-1", "D1"));
                    obj.Prop.Add("SwitchB", new Node("-1", "D1"));
                    obj.Prop.Add("SwitchKill", new Node("-1", "D1"));
                    obj.Prop.Add("SwitchDeadOn", new Node("-1", "D1"));
                }
                else obj.Prop.Add("MarioNo", new Node("0", "D1"));
                obj.Prop.Add("MultiFileName", new Node("StageData_tool", "A0"));
                obj.Prop.Add("LayerName", new Node("共通", "A0"));


                if (ndb.DBtoId.ContainsKey(comboBox1.Text))
                {
                    obj.Prop.Add("name", new Node(name, "A0"));
                    obj.Prop.Add("dbname", ndb.Entries[comboBox1.Text].dbname);
                }
                else
                {
                    obj.Prop.Add("name", new Node(comboBox1.Text, "A0"));
                }
                obj.Prop.Add("dir", new Single[3] { 0, 0, 0 });
                obj.Prop.Add("pos", new Single[3] { (Single)objPos.X, (Single)objPos.Z, (Single)(-objPos.Y) });

                obj.Prop.Add("scale", new Single[3] { 1, 1, 1 });
                Value = obj;
                this.Close();
            }
            else
            {
                if (listView1.SelectedIndices.Count == 0)
                {
                    
                    MessageBox.Show("You can't add nothing as an object!"); return;
                }
                LevelObj obj = new LevelObj();
                string name = "";
                ndb.DBtoId.TryGetValue(listView1.Items[listView1.SelectedIndices[0]].Text, out name);
                if (LayerName == "ObjInfo") { obj.Prop.Add("ClippingGroupId", new Node("-1", "D1")); }
                if (LayerName == "DemoSceneObjInfo")
                {
                    obj.Prop.Add("Action1", new Node("-", "A0"));
                    obj.Prop.Add("Action2", new Node("-", "A0"));
                    obj.Prop.Add("Action3", new Node("-", "A0"));
                    obj.Prop.Add("Action4", new Node("-", "A0"));
                    obj.Prop.Add("Action5", new Node("-", "A0"));
                    obj.Prop.Add("LuigiType", new Node("Common", "A0"));
                    obj.Prop.Add("MarioType", new Node("Common", "A0"));
                    obj.Prop.Add("ModelName", new Node("DemoBird", "A0"));
                    obj.Prop.Add("SuffixName", new Node("-", "A0"));
                }
                if (LayerName != "StartInfo" && LayerName != "AreaObjInfo" && LayerName != "DemoSceneObjInfo")
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
                        obj.Prop.Add("Arg", new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 });

                        if (name != null)
                        {
                            NewDb.NewDbEntry DbEntry = new NewDb.NewDbEntry();
                            ndb.Entries.TryGetValue(name, out DbEntry);
                            object array = new int[10];
                            obj.Prop.TryGetValue("Arg", out array);
                            foreach (NewDb.EntryArg arg in DbEntry.args)
                            {
                                int value = -1;
                                if (arg.type == "bool")
                                {
                                    if (arg.default_value == "true")
                                    {
                                        value = 1;
                                    }
                                    else
                                    {
                                        value = -1;
                                    }
                                }
                                else
                                {
                                    value = int.Parse(arg.default_value);
                                }
                                ((int[])array)[arg.arg_id] = value;
                            }
                        }

                    }
                    obj.Prop.Add("SwitchA", new Node("-1", "D1"));
                    obj.Prop.Add("SwitchAppear", new Node("-1", "D1"));
                    obj.Prop.Add("SwitchB", new Node("-1", "D1"));
                    obj.Prop.Add("SwitchKill", new Node("-1", "D1"));
                    obj.Prop.Add("SwitchDeadOn", new Node("-1", "D1"));
                }
                else obj.Prop.Add("MarioNo", new Node("0", "D1"));
                obj.Prop.Add("MultiFileName", new Node("StageData_tool", "A0"));
                obj.Prop.Add("LayerName", new Node("共通", "A0"));


                if (ndb.DBtoId.ContainsKey(listView1.Items[listView1.SelectedIndices[0]].Text))
                {
                    obj.Prop.Add("name", new Node(name, "A0"));
                    obj.Prop.Add("dbname", ndb.Entries[name].dbname);
                }
                else
                {
                    obj.Prop.Add("name", new Node(listView1.Items[listView1.SelectedIndices[0]].Text, "A0"));
                }
                obj.Prop.Add("dir", new Single[3] { 0, 0, 0 });
                obj.Prop.Add("pos", new Single[3] { (Single)objPos.X, (Single)objPos.Z, (Single)(-objPos.Y) });

                obj.Prop.Add("scale", new Single[3] { 1, 1, 1 });
                Value = obj;
                this.Close();
            }
        }

        private void FrmAddObj_Load(object sender, EventArgs e)
        {
            if (checkBox1.Enabled)
            {
                if (Properties.Settings.Default.OnlyKnwonObjs)
                {
                    checkBox1.Checked = true;
                }
                else { checkBox1.Checked = true; checkBox1.Checked = false; }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ObjDb == null)
            {
                LoadObjList(CCNT);
                usingdb = false;
                checkBox1.Checked = false;
                checkBox1.Enabled = false;
                return;
            }
            if (checkBox1.Checked) 
            {
                usingdb = true;
                label3.Enabled = true;
                comboBox2.Enabled = true;
                listView1.Enabled = true;
                label1.Enabled = false;
                comboBox1.Enabled = false;
            }
            else
            {
                usingdb = false;
                label3.Enabled = false;
                label1.Enabled = true;
                comboBox1.Enabled = true;
                comboBox2.Enabled = false;
                listView1.Enabled = false;
            }
            LoadObjList(usingdb ? ObjDb : CCNT);
            Properties.Settings.Default.OnlyKnwonObjs = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            int index = 0;
            foreach (KeyValuePair<string, string> objects in categories[comboBox2.SelectedIndex].objdict)
            {
                if (LayerNum == ndb.Entries[objects.Value].type || LayerNum == 1)
                {
                    listView1.Items.Add(objects.Key);
                    listView1.Items[index].SubItems.Add(objects.Value);
                    if (ndb.Entries[objects.Value].extra != "")
                    {
                        listView1.Items[index].ToolTipText = ndb.Entries[objects.Value].extra;
                    }
                    index++;
                }
            }
        }

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
