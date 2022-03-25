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
        string InfosName;
        int LayerNum;
        string[] CCNT;
        string[] ObjDb;
        public NewDb ndb = new NewDb();
        List<string> DesignSoundList = new List<string>() { "SoundEmitArea", "SoundEmitObj", "BgmChangeArea", "AudioEffectChangeArea", "AudioVolumeSettingArea", "LightArea", "FogAreaCameraPos", "FogArea" };
        Vector3D objPos;
        Vector3D objScale;
        Vector3D objDir;
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
            Setup(false, _CCNT, db, text, SpawnPos);

        }
        public FrmAddObj(string[] _CCNT, NewDb db, string text, LevelObj editObj)
        {

            InitializeComponent();
            Setup(true, _CCNT, db, text, default, editObj);

        }

        void Setup(bool ObjEdit, string[] _CCNT, NewDb db, string text, Vector3D SpawnPos = new Vector3D(), LevelObj editObj = null )
        {
            InfosName = text;
            switch (InfosName)
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
            if (ObjEdit)
            {
                objPos = new Vector3D(((Single[])editObj.Prop["pos"])[0], ((Single[])editObj.Prop["pos"])[1], ((Single[])editObj.Prop["pos"])[2]);
                objScale = new Vector3D(((Single[])editObj.Prop["scale"])[0], ((Single[])editObj.Prop["scale"])[1], ((Single[])editObj.Prop["scale"])[2]);
                objDir = new Vector3D(((Single[])editObj.Prop["dir"])[0], ((Single[])editObj.Prop["dir"])[1], ((Single[])editObj.Prop["dir"])[2]);
                Genpos0.Value = (decimal)objPos.X;
                Genpos1.Value = (decimal)objPos.Y;
                Genpos2.Value = (decimal)objPos.Z;
                Genscale0.Value = (decimal)objScale.X;
                Genscale1.Value = (decimal)objScale.Y;
                Genscale2.Value = (decimal)objScale.Z;
                Gendir0.Value = (decimal)objDir.X;
                Gendir1.Value = (decimal)objDir.Y;
                Gendir2.Value = (decimal)objDir.Z;
                Genl_id.Value = decimal.Parse(((Node)editObj.Prop["l_id"]).StringValue);
                for (int i = 0; i < 10; i++)
                {
                    ((NumericUpDown)Controls["groupBox2"].Controls["Defarg_int" + i]).Value = ((int[])editObj.Prop["Arg"])[i];
                }

                switch (((Node)editObj.Prop["LayerName"]).StringValue)
                {
                    case "共通":
                        LayerNameCB.SelectedIndex = 0;
                        break;
                    case "シナリオ1":
                        LayerNameCB.SelectedIndex = 1;
                        break;
                    case "シナリオ2":
                        LayerNameCB.SelectedIndex = 2;
                        break;
                    case "シナリオ3":
                        LayerNameCB.SelectedIndex = 3;
                        break;
                    case "共通サブ":
                        LayerNameCB.SelectedIndex = 4;
                        break;
                    default:
                        LayerNameCB.SelectedIndex = 0;
                        break;

                }

            }
            else
            {
                objPos = SpawnPos;
                Genpos0.Value = (decimal)objPos.X;
                Genpos1.Value = (decimal)objPos.Z;
                Genpos2.Value = (decimal)-objPos.Y;
                Genscale0.Value = 1; ; Genscale1.Value = 1; ; Genscale2.Value = 1;
                LayerNameCB.SelectedIndex = 0;
                for (int i = 0; i < 10; i++)
                {
                    ((NumericUpDown)Controls["groupBox2"].Controls["Defarg_int" + i]).Value = -1;
                }
            }
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
                //objdb[ndb.Entries["Mario"].category - 2] = "";
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
                            category.objdict.Add(entry.dbname, entry.filename);
                        }
                    }
                    categories.Add(category);
                }
                foreach (KeyValuePair<int, string> valuePair in ndb.Types)
                {
                    ObjectTypeCB.Items.Add(valuePair.Value);
                }
                ObjectTypeCB.SelectedIndex = ObjectTypeCB.Items.IndexOf(text);
                ObjDb = objdb.ToArray();
            }
            else
            {
                listView1.Enabled = false;
                comboBox2.Enabled = false;
                checkBox1.Checked = false;
                ObjectTypeCB.Items.Add(text);
                ObjectTypeCB.SelectedIndex = 0;
            }
            comboBox1.Items.Clear();
            LoadObjList(CCNT);
            if (ObjEdit && ndb.Entries.ContainsKey(((Node)editObj.Prop["name"]).StringValue))
            {
               if (checkBox1.Checked)
                {
                    listView1.Items[listView1.Items.IndexOfKey(ndb.IdtoDB[((Node)editObj.Prop["name"]).StringValue])].Selected = true;
                }
            }
            else if (ObjEdit)
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(((Node)editObj.Prop["name"]).StringValue);
            }
        }
        void LoadObjList(string[] array)
        {
            if (!usingdb)
            {
                switch (InfosName)
                {
                    case "StartInfo":
                        comboBox1.Text = "Mario";
                        comboBox1.Enabled = false;
                        checkBox1.Enabled = false;
                        break;
                    case "AreaObjInfo":
                        foreach (string s in array)
                        {
                            if ((!s.EndsWith("*") && s.ToLower().EndsWith("area") &&  !s.ToLower().Contains("camera")) || s.Equals("FogAreaCameraPos") ) comboBox1.Items.Add(s);
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
            if (usingdb)
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
            if ((comboBox1.Text == "" && !usingdb) || (listView1.SelectedIndices.Count == 0 && usingdb))
            {
                MessageBox.Show("You can't add nothing as an object!"); return;
            }
            LevelObj obj = new LevelObj();
            string name = null;
            ndb.DBtoId.TryGetValue(!usingdb ? comboBox1.Text : listView1.Items[listView1.SelectedIndices[0]].Text, out name); // ndb.DBtoId.TryGetValue(listView1.Items[listView1.SelectedIndices[0]].Text, out name);
            if (InfosName == "ObjInfo" || InfosName == "GoalObjInfo") { obj.Prop.Add("ClippingGroupId", new Node("-1", "D1"));
                obj.Prop.Add("GenerateParent", new Node("-1", "D1"));
            }
            if (InfosName == "AreaObjInfo") { obj.Prop.Add("AreaParent", new Node("-1", "D1")); }
            if (InfosName == "DemoSceneObjInfo")
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
            if (InfosName != "StartInfo" && InfosName != "AreaObjInfo" && InfosName != "DemoSceneObjInfo")
            {
                if (InfosName != "CameraAreaInfo") obj.Prop.Add("ViewId", new Node("-1", "D1"));
                obj.Prop.Add("CameraId", new Node("-1", "D1"));
            }
            if (InfosName == "CameraAreaInfo" || InfosName == "AreaObjInfo")
            {
                obj.Prop.Add("Priority", new Node("-1", "D1"));
            }
            if (InfosName != "StartInfo")
            {

                obj.Prop.Add("l_id", new Node("0", "D1"));
                if (InfosName != "CameraAreaInfo")
                {
                    int[] args = new int[10];


                    for (int i = 0; i < 10; i++)
                    {
                        args[i] = (int)((NumericUpDown)Controls["groupBox2"].Controls["Defarg_int" + i]).Value;
                    }

                    obj.Prop.Add("Arg", args);

                    if (name != null && usingdb)
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
            if (DesignSoundList.Contains(textBox1.Text))
            {
                obj.Prop.Add("MultiFileName", new Node("StageData_mul", "A0")); //design and sound objects use this
            }
            else
            {
                obj.Prop.Add("MultiFileName", new Node("StageData_tool", "A0"));
            }
            switch (LayerNameCB.SelectedIndex)
            {
                default:
                    obj.Prop.Add("LayerName", new Node("共通", "A0"));
                    break;
                case 1:
                    obj.Prop.Add("LayerName", new Node("シナリオ1", "A0"));
                    break;
                case 2:
                    obj.Prop.Add("LayerName", new Node("シナリオ2", "A0"));
                    break;
                case 3:
                    obj.Prop.Add("LayerName", new Node("シナリオ3", "A0"));
                    break;
                case 4:
                    obj.Prop.Add("LayerName", new Node("共通サブ", "A0"));
                    break;

            }



            if (name != null)
            {
                obj.Prop.Add("name", new Node(name, "A0"));
                obj.Prop.Add("dbname", ndb.Entries[name].dbname);
            }
            else
            {
                obj.Prop.Add("name", new Node(!usingdb ? comboBox1.Text : listView1.Items[listView1.SelectedIndices[0]].Text, "A0"));
            }
            obj.Prop.Add("dir", new Single[3] { (Single)Gendir0.Value, (Single)Gendir1.Value, (Single)Gendir2.Value });
            obj.Prop.Add("pos", new Single[3] { (Single)Genpos0.Value, (Single)Genpos1.Value, (Single)(Genpos2.Value) });

            obj.Prop.Add("scale", new Single[3] { (Single)Genscale0.Value, (Single)Genscale1.Value, (Single)Genscale2.Value });
            Value = obj;
            this.Close();

            /*if (!usingdb)
            {
                if (comboBox1.Text == "")
                {
                    MessageBox.Show("You can't add nothing as an object!"); return;
                }
                LevelObj obj = new LevelObj();
                string name = "";
                ndb.DBtoId.TryGetValue(comboBox1.Text, out name);
                if (InfosName == "ObjInfo") { obj.Prop.Add("ClippingGroupId", new Node("-1", "D1")); }
                if (InfosName == "DemoSceneObjInfo")
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
                if (InfosName != "StartInfo" && InfosName != "AreaObjInfo" && InfosName != "DemoSceneObjInfo")
                {
                    if (InfosName != "CameraAreaInfo") obj.Prop.Add("ViewId", new Node("-1", "D1"));
                    obj.Prop.Add("CameraId", new Node("-1", "D1"));
                    if (InfosName == "CameraAreaInfo")
                        obj.Prop.Add("Priority", new Node("-1", "D1"));
                }
                if (InfosName != "StartInfo")
                {

                    obj.Prop.Add("l_id", new Node("0", "D1"));
                    if (InfosName != "CameraAreaInfo")
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


                if (ndb.IdtoDB.ContainsKey(comboBox1.Text))
                {
                    obj.Prop.Add("name", new Node(comboBox1.Text, "A0"));
                    obj.Prop.Add("dbname", ndb.IdtoDB[comboBox1.Text]);

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
                LevelObj obj = new LevelObj();//
                string name = "";//
                ndb.DBtoId.TryGetValue(listView1.Items[listView1.SelectedIndices[0]].Text, out name);
                if (InfosName == "ObjInfo") { obj.Prop.Add("ClippingGroupId", new Node("-1", "D1")); }
                if (InfosName == "DemoSceneObjInfo")
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
                }//
                if (InfosName != "StartInfo" && InfosName != "AreaObjInfo" && InfosName != "DemoSceneObjInfo")
                {
                    if (InfosName != "CameraAreaInfo") obj.Prop.Add("ViewId", new Node("-1", "D1"));
                    obj.Prop.Add("CameraId", new Node("-1", "D1"));
                    if (InfosName == "CameraAreaInfo")
                        obj.Prop.Add("Priority", new Node("-1", "D1"));
                }//
                if (InfosName != "StartInfo")
                {

                    obj.Prop.Add("l_id", new Node("0", "D1"));//
                    if (InfosName != "CameraAreaInfo")
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
            }*/
        }

        private void FrmAddObj_Load(object sender, EventArgs e)
        {
            if (checkBox1.Enabled)
            {
                if (Properties.Settings.Default.OnlyKnownObjs)
                {
                    checkBox1.Checked = true;
                }
                else { checkBox1.Checked = true; checkBox1.Checked = false; }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (checkBox1.Checked) 
            {
                usingdb = true;
                label3.Enabled = true;
                //label1.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = true;
                listView1.Enabled = true;
                LoadObjList(ObjDb);
            }
            else
            {
                usingdb = false;
                label3.Enabled = false;
                //label1.Enabled = true;
                comboBox1.Enabled = true;
                comboBox2.Enabled = false;
                listView1.Enabled = false;
            }
            //LoadObjList(usingdb ? ObjDb : CCNT);
            Properties.Settings.Default.OnlyKnownObjs = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.Text;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                textBox1.Text = "";
            }
            else
            {
                textBox1.Text = listView1.Items[listView1.SelectedIndices[0]].SubItems[1].Text;
            }
        }

        private void pasteValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteValue(Form1.clipboard[Form1.clipboard.Count - 1]);
            ClipBoardMenu.Close();
        }

        private void copyPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyValue("pos");
        }
        private void copyRotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyValue("dir");
        }
        private void copyScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyValue("scale");
        }

        private void ClipBoardMenu_CopyArgs_Click(object sender, EventArgs e)
        {
            CopyValue("Arg");
        }

        void CopyValue(string value)
        {
            ClipBoardItem cl = new ClipBoardItem();
            if (value == "pos")
            {
                cl.Type = ClipBoardItem.ClipboardType.Position;
                Single[] ds = new Single[] { (Single)Genpos0.Value, (Single)Genpos1.Value, (Single)Genpos2.Value };
                cl.Pos = ds;
            }
            if (value == "dir")
            {
                cl.Type = ClipBoardItem.ClipboardType.Rotation;
                Single[] ds = new Single[] { (Single)Gendir0.Value, (Single)Gendir1.Value, (Single)Gendir2.Value };
                cl.Dir = ds;
            }
            if (value == "scale")
            {
                cl.Type = ClipBoardItem.ClipboardType.Scale;
                Single[] ds = new Single[] { (Single)Genscale0.Value, (Single)Genscale1.Value, (Single)Genscale2.Value };
                cl.Scale = ds;
            }
            else if (value == "Arg")
            {
                cl.Type = ClipBoardItem.ClipboardType.IntArray;
                cl.Args = new int[] { (int)Defarg_int0.Value, (int)Defarg_int1.Value, (int)Defarg_int2.Value, (int)Defarg_int3.Value, (int)Defarg_int4.Value, (int)Defarg_int5.Value, (int)Defarg_int6.Value, (int)Defarg_int7.Value, (int)Defarg_int8.Value, (int)Defarg_int9.Value };
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
            if (itm.Type == ClipBoardItem.ClipboardType.Position)
            {
                Genpos0.Value = (decimal)itm.Pos[0];
                Genpos1.Value = (decimal)itm.Pos[1];
                Genpos2.Value = (decimal)itm.Pos[2];
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.Rotation)
            {
                Gendir0.Value = (decimal)itm.Dir[0];
                Gendir1.Value = (decimal)itm.Dir[1];
                Gendir2.Value = (decimal)itm.Dir[2];
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.Scale)
            {
                Genscale0.Value = (decimal)itm.Scale[0];
                Genscale1.Value = (decimal)itm.Scale[1];
                Genscale2.Value = (decimal)itm.Scale[2];
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.IntArray)
            {
                for (int i = 0; i < itm.Args.Length; i++)
                {
                    ((NumericUpDown)Controls["groupBox2"].Controls["Defarg_int" + i]).Value = itm.Args[i];
                }
            }
            else
            {
                MessageBox.Show("Can't paste this!");
                return;
            }
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

    }
}
