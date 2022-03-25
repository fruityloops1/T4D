using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.ComponentModel.Design;

namespace The4Dimension
{
    public class Level : Object
    {
        public int lvlnodeid = 0;
        public Dictionary<string, string> lvlparamslist = new Dictionary<string, string>();
    }
    public class World : Object
    {
        public int wid = 0;
        public string wtype = "";
        public Level[] wlvls = new Level[8];
    }
    class CustomStringWriter : System.IO.StringWriter
    {
        private readonly Encoding encoding;

        public CustomStringWriter(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }

    public class CustomStack<T>
    {
        private List<T> items = new List<T>();
        public int MaxItems = 50;

        public int Count
        { get { return items.Count(); } }

        public void Remove(int index)
        {
            items.RemoveAt(index);
        }

        public void Push(T item)
        {
            items.Add(item);
            if (items.Count > MaxItems)
            {
                for (int i = MaxItems; i < items.Count; i++) Remove(0);
            }
        }

        public T Pop()
        {
            if (items.Count > 0)
            {
                T tmp = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                return tmp;
            }
            else return default(T);
        }

        public T[] ToArray()
        {
            return items.ToArray();
        }
    }

    public class UndoAction
    {
        public string actionName;
        public Action<object[]> Action = null;
        object[] Args;

        public void Undo()
        {
            Action.Invoke(Args);
        }

        public override string ToString()
        {
            return actionName;
        }

        public UndoAction(string name, object[] args, Action<object[]> Act)
        {
            actionName = name;
            Args = args;
            Action = Act;
        }
       
    }

    public class ClipBoardItem
    {
        public enum ClipboardType
        {
            NotSet = 0,
            Position = 1,
            Rotation = 2,
            Scale = 3,
            IntArray = 4,
            FullObject = 5,
            Rail = 6,
            ObjectArray = 7
        }

        public Single X = 0;
        public Single Y = 0;
        public Single Z = 0;
        public int[] Args = null;
        public Single[] Scale = null;
        public Single[] Dir = null;
        public Single[] Pos = null;
        public ClipboardType Type = 0;
        public Rail Rail = null;
        public LevelObj[] Objs = null;

        public override string ToString()
        {
            switch (Type)
            {
                case ClipboardType.Position:
                    return String.Format("Position - X:{0} Y:{1} Z:{2}", Pos[0].ToString(), Pos[1].ToString(), Pos[2].ToString());
                case ClipboardType.Rotation:
                    return String.Format("Rotation - X:{0} Y:{1} Z:{2}", Dir[0].ToString(), Dir[1].ToString(), Dir[2].ToString());
                case ClipboardType.Scale:
                    return String.Format("Scale - X:{0} Y:{1} Z:{2}", Scale[0].ToString(), Scale[1].ToString(), Scale[2].ToString());
                case ClipboardType.IntArray:
                    return "Args[]";
                case ClipboardType.Rail:
                    return "Rail - " + Rail.Name;
                case ClipboardType.FullObject:
                    return "Object - " + Objs[0].GetName(true);//db name
                case ClipboardType.ObjectArray:
                    return "Object[" + Objs.Length.ToString() + "]";
                default:
                    return "Not set";
            }
        }

        public string ToString(int ObjectAsChildren)
        {
            switch (Type)
            {
                case ClipboardType.Position:
                    return String.Format("Position - X:{0} Y:{1} Z:{2}", Pos[0].ToString(), Pos[1].ToString(), Pos[2].ToString());
                case ClipboardType.Rotation:
                    return String.Format("Rotation - X:{0} Y:{1} Z:{2}", Dir[0].ToString(), Dir[1].ToString(), Dir[2].ToString());
                case ClipboardType.Scale:
                    return String.Format("Scale - X:{0} Y:{1} Z:{2}", Scale[0].ToString(), Scale[1].ToString(), Scale[2].ToString());
                case ClipboardType.IntArray:
                    return "Args[]";
                case ClipboardType.Rail:
                    return "Rail - " + Rail.Name;
                case ClipboardType.FullObject:
                    if (ObjectAsChildren < 0) return "Object - " + Objs[0].GetName(true); //db name
                    else
                        return "Paste object as children - " + Objs[0].GetName(true); //db name
                case ClipboardType.ObjectArray:
                    return "Object[" + Objs.Length.ToString() + "]";
                default:
                    return "Not set";
            }
        }
    }

    public class AllInfoSection : List<LevelObj>
    {
        public bool IsHidden = false;
        public int GetById(int id)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Prop.ContainsKey("l_id") && ((Node)this[i].Prop["l_id"]).StringValue == id.ToString())
                {
                    return i;
                }
            }
            return -1;
        }
        public List<LevelObj> GetByParentId(int id)//gets a list of all the children of a given parent
        {
            List<LevelObj> Ret = new List<LevelObj>();
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Prop.ContainsKey("AreaParent")) // AreaChildren
                {
                    if (((Node)this[i].Prop["AreaParent"]).StringValue == id.ToString())
                    {
                        Ret.Add(this[i]);
                    }
                }else if (this[i].Prop.ContainsKey("GenerateParent")) // GenerateChildren
                {
                    if (((Node)this[i].Prop["GenerateParent"]).StringValue == id.ToString())
                    {
                        Ret.Add(this[i]);
                    }
                }

            }
            return Ret;
        }
    }

    public class AllRailInfoSection : List<Rail>
    {
        public int GetById(int id)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].l_id == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
    {
        IDictionary _dictionary;

        public DictionaryPropertyGridAdapter(IDictionary d)
        {
            _dictionary = d;
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _dictionary;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        PropertyDescriptorCollection
            System.ComponentModel.ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            ArrayList properties = new ArrayList();
            foreach (DictionaryEntry e in _dictionary)
            {
                properties.Add(new DictionaryPropertyDescriptor(_dictionary, e.Key));
            }

            PropertyDescriptor[] props =
                (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));

            return new PropertyDescriptorCollection(props);
        }
    }

    class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        IDictionary _dictionary;
        object _key;

        internal DictionaryPropertyDescriptor(IDictionary d, object key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        public override Type PropertyType
        {
            get { return _dictionary[_key].GetType(); }
        }

        public override void SetValue(object component, object value)
        {
            _dictionary[_key] = value;
        }

        public override object GetValue(object component)
        {
            return _dictionary[_key];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }

    public class ObjectDb
    {
        public Dictionary<int, string> Categories = new Dictionary<int, string>();
        public Dictionary<string, ObjectDbEntry> Entries = new Dictionary<string, ObjectDbEntry>();
        public Dictionary<string, string> IdToModel = new Dictionary<string, string>();
        public int timestamp;

        public static ObjectDb FromXml(string xml)
        {
            ObjectDb res = new ObjectDb();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode n = doc.SelectSingleNode("/database");
            res.timestamp = int.Parse(n.Attributes["timestamp"].Value);
            foreach (XmlNode node in n.ChildNodes)
            {
                if (node.Name == "categories")
                {
                    foreach (XmlNode subn in node.ChildNodes)
                    {
                        res.Categories.Add(int.Parse(subn.Attributes["id"].InnerText), subn.InnerText);
                    }
                }
                else if (node.Name == "object")
                {
                    ObjectDbEntry tmp = ObjectDbEntry.FromXml(node.ChildNodes);
                    res.Entries.Add(node.Attributes["id"].InnerText, tmp);
                    if (tmp.model.Trim() != "") res.IdToModel.Add(node.Attributes["id"].InnerText, tmp.model);
                }
            }
            return res;
        }
        /* goes unused
        public string GetXml(bool updateTimestamp = true)
        {
            return ASCIIEncoding.UTF8.GetString(GetXmlBytes(updateTimestamp));
        }

        public byte[] GetXmlBytes(bool updateTimestamp = true)
        {
            if (updateTimestamp)
            {
                TimeSpan ts = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
                timestamp = (int)ts.TotalSeconds;
            }
            using (var stream = new MemoryStream())
            {
                using (var xr = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true, Encoding = ASCIIEncoding.UTF8 }))
                {
                    xr.WriteStartDocument();
                    xr.WriteStartElement("database");
                    xr.WriteAttributeString("timestamp", timestamp.ToString());
                    xr.WriteStartElement("categories");
                    foreach (int i in Categories.Keys.ToArray())
                    {
                        xr.WriteStartElement("category");
                        xr.WriteAttributeString("id", i.ToString());
                        xr.WriteString(Categories[i]);
                        xr.WriteEndElement();
                    }
                    xr.WriteEndElement();
                    foreach (string id in Entries.Keys.ToArray())
                    {
                        xr.WriteStartElement("object");
                        xr.WriteAttributeString("id", id);
                        Entries[id].WriteXml(xr);
                        xr.WriteEndElement();
                        xr.WriteRaw("\r\n".ToCharArray(), 0, 1);
                    }
                    xr.WriteEndElement();
                    xr.Close();
                }
                return stream.ToArray();
            }
        }*/

        public class ObjectDbEntry
        {
            public string name, notes, files,model = "" , type = "";
            public int Known, Complete, Category;
            public List<ObjectDbField> Fields = new List<ObjectDbField>();

            public static ObjectDbEntry FromXml(XmlNodeList nodes)
            {
                ObjectDbEntry res = new ObjectDbEntry();
                foreach (XmlNode n in nodes)
                {
                    switch (n.Name)
                    {
                        case "name":
                            res.name = n.InnerText;
                            break;
                        case "type":
                            res.type = n.InnerText;
                            break;
                        case "model":
                            res.model = n.InnerText;
                            break;
                        case "flags":
                            res.Known = int.Parse(n.Attributes["known"].InnerText);
                            res.Complete = int.Parse(n.Attributes["complete"].InnerText);
                            break;
                        case "category":
                            res.Category = int.Parse(n.Attributes["id"].InnerText);
                            break;
                        case "notes":
                            res.notes = n.InnerText;
                            break;
                        case "files":
                            res.files = n.InnerText;
                            break;
                        case "field":
                            res.Fields.Add(ObjectDbField.FromXml(n));
                            break;
                    }
                }
                return res;
            }
            /*unused
            public void WriteXml(XmlWriter xr)
            {
                xr.WriteStartElement("name");
                xr.WriteString(name);
                xr.WriteEndElement();
                xr.WriteStartElement("type");
                xr.WriteString(type);
                xr.WriteEndElement();
                xr.WriteStartElement("model");
                xr.WriteString(model);
                xr.WriteEndElement();
                xr.WriteStartElement("flags");
                xr.WriteAttributeString("known", Known.ToString());
                xr.WriteAttributeString("complete", Complete.ToString());
                xr.WriteEndElement();
                xr.WriteStartElement("category");
                xr.WriteAttributeString("id", Category.ToString());
                xr.WriteEndElement();
                xr.WriteStartElement("notes");
                xr.WriteString(notes);
                xr.WriteEndElement();
                xr.WriteStartElement("files");
                xr.WriteString(files);
                xr.WriteEndElement();
                foreach (ObjectDbField f in Fields) f.WriteXml(xr);
            }
            */
            public class ObjectDbField
            {
                public int id;
                public string type = "int";
                public string name, values, notes;
                /* more unused
                public void WriteXml(XmlWriter xr)
                {
                    xr.WriteStartElement("field");
                    xr.WriteAttributeString("id", id.ToString());
                    xr.WriteAttributeString("type", type);
                    xr.WriteAttributeString("name", name);
                    xr.WriteAttributeString("values", values);
                    xr.WriteAttributeString("notes", notes);
                    xr.WriteEndElement();
                }
                */
                public static ObjectDbField FromXml(XmlNode n)
                {
                    ObjectDbField res = new ObjectDbField();
                    res.id = int.Parse(n.Attributes["id"].InnerText);
                    res.type = n.Attributes["type"].InnerText;
                    res.name = n.Attributes["name"].InnerText;
                    res.values = n.Attributes["values"].InnerText;
                    res.notes = n.Attributes["notes"].InnerText;
                    return res;
                }
            }
        }
    }

    public class NewDb
    {
        public Dictionary<int, string> Categories = new Dictionary<int, string>();
        public Dictionary<int, string> Types = new Dictionary<int, string>();
        public Dictionary<string, NewDbEntry> Entries = new Dictionary<string, NewDbEntry>();// Entries.GetValue()
        public List<EntryProperty> KnownProperties = new List<EntryProperty>();
        public Dictionary<string, string> DBtoId = new Dictionary<string, string>();//Goomba, Kuribo
        public Dictionary<string, string> IdtoDB = new Dictionary<string, string>();//Kuribo, Goomba
        public Dictionary<string, string> IdToModel = new Dictionary<string, string>();// Wanwan, Wanwan
        public decimal version;
        public decimal docpercent(Dictionary<string, string> CCNT)
        {
            decimal ret = 0;
            int doc = 0;
            decimal accnt = CCNT.Count; 
            foreach(NewDbEntry entry in Entries.Values)
            {
                if (CCNT.ContainsKey(entry.filename))
                {
                    doc += 1;
                }
            }
            ret = Math.Round((doc / accnt)*100, 2);
            return ret;
        }
        public static NewDb FromXml(string xml)
        {
            NewDb res = new NewDb();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode n = doc.SelectSingleNode("/database");
            res.version = decimal.Parse(n.Attributes["version"].Value);

            foreach (XmlNode node in n.ChildNodes)
            {
                if (node.Name == "categories")
                {
                    foreach (XmlNode subn in node.ChildNodes)
                    {
                        res.Categories.Add(int.Parse(subn.Attributes["id"].Value), subn.Attributes["name"].Value);
                    }
                }
                else if (node.Name == "types")
                {
                    foreach (XmlNode subn in node.ChildNodes)
                    {
                        res.Types.Add(int.Parse(subn.Attributes["id"].Value), subn.Attributes["name"].Value);
                    }
                }
                else if (node.Name == "objects")
                {
                    foreach (XmlNode s in node.ChildNodes) //object node
                    {
                        NewDbEntry temp = NewDbEntry.FromXml(s);
                        res.Entries.Add(s.Attributes["id"].Value, temp);
                        res.DBtoId.Add(s.Attributes["name"].Value, s.Attributes["id"].Value);
                        res.IdtoDB.Add(s.Attributes["id"].Value, s.Attributes["name"].Value);
                        if (temp.modelname.Trim() != "") res.IdToModel.Add(s.Attributes["id"].Value, temp.modelname); 
                    }
                }else if (node.Name == "properties")
                {
                    
                    foreach (XmlNode s in node.ChildNodes) //property node
                    {
                        if (s.NodeType != XmlNodeType.Comment)
                        {
                        EntryProperty temp = new EntryProperty();

                        temp.name = s.Attributes["name"].Value;
                        temp.data_type = s.Attributes["data_type"].Value;
                        if (s.Attributes["default"] != null) temp.default_value = s.Attributes["default"].Value;
                        temp.text = s.Attributes["text"].Value;
                        if (s.Attributes["info"] != null) temp.info = s.Attributes["info"].Value;
                        if (temp.data_type.Contains("option"))
                        {
                            foreach (XmlNode child in s.ChildNodes)
                            {
                                temp.options.Add(child.Attributes["name"].Value, child.Attributes["value"].Value);
                                temp.revoptions.Add(child.Attributes["value"].Value, child.Attributes["name"].Value);
                            }
                        }
                        else if (temp.data_type.Contains("array"))
                        {
                            foreach (XmlNode child in s.ChildNodes)
                            {
                                temp.array.Add(child.Attributes["name"].Value, child.Attributes["value"].Value);
                            }
                        }

                        res.KnownProperties.Add(temp);
                        }
                    }
                }
            }
            /*
            NewDbEntry t = new NewDbEntry();
            res.Entries.TryGetValue("Kuribo", out t);
            MessageBox.Show(t.dbname);*/
            return res;
        }
        public class NewDbEntry
        {
            public string filename = "";
            public string modelname = "";
            public string dbname = "";
            public string extra = "";
            public List<string> children = new List<string>();
            public int category = 0;
            public int type = 0;
            public List<EntryArg> args = new List<EntryArg>();

            public static NewDbEntry FromXml(XmlNode parent)
            {
                NewDbEntry newentry = new NewDbEntry();
                newentry.filename = parent.Attributes["id"].Value;
                newentry.dbname = parent.Attributes["name"].Value;
                newentry.category = Int32.Parse(parent.Attributes["category"].Value);
                newentry.type = Int32.Parse(parent.Attributes["type"].Value);
                if (parent.Attributes["extra"] != null) newentry.extra = parent.Attributes["extra"].Value;
                for (int i=1; i > 8; i++)//8 is arbitrary, could be any number
                {
                    if (parent.Attributes["child" + i.ToString()] != null)
                    {
                        newentry.children.Add(parent.Attributes["child" + i.ToString()].Value);
                    }
                }

                foreach (XmlNode child in parent.ChildNodes)
                {
                    
                    if (child.NodeType == XmlNodeType.Element)
                    {
                        EntryArg arg = new EntryArg();

                        if (child.Name == "model")
                        {
                            newentry.modelname = child.InnerText;
                        }
                        else
                        {
                            if (child.Name != "property")
                            {
                                arg.type = child.Name;
                                arg.arg_id = int.Parse(child.Attributes["arg"].Value);
                                arg.name = child.Attributes["name"].Value;
                                arg.default_value = child.Attributes["default"].Value;
                                if (child.Attributes["info"] != null)
                                {
                                    arg.info = child.Attributes["info"].Value;
                                }
                                if (arg.type == "int")
                                {
                                    if (child.Attributes["min"] != null)
                                    {
                                        arg.min = Int32.Parse(child.Attributes["min"].Value);
                                    }
                                    if (child.Attributes["max"] != null)
                                    {
                                        arg.max = Int32.Parse(child.Attributes["max"].Value);
                                    }
                                }
                                else if (arg.type == "option")
                                {
                                   
                                    foreach (XmlNode op in child.ChildNodes)
                                    {
                                        arg.options.Add(op.Attributes["name"].Value, op.Attributes["value"].Value);
                                        arg.revoptions.Add(op.Attributes["value"].Value, op.Attributes["name"].Value);
                                    }
                                }
                                newentry.args.Add(arg);


                            }
                        }
                    }
                }
                return newentry;
            }
        }
        public class EntryArg
        {
            public string type = "";
            public int arg_id = 0;
            public string name = "";
            public string default_value = "";
            public string info = "";
            public int max = 0;
            public int min = 0;
            public Dictionary<string, string> options = new Dictionary<string, string>();// Red , -1
            public Dictionary<string, string> revoptions = new Dictionary<string, string>();//-1, Red

        }
        public class EntryProperty
        {
            public string name = "";
            public string data_type = "";
            public string value = "";
            public string default_value = "";
            public string text = "";
            public string info = "";
            public Dictionary<string, string> options = new Dictionary<string, string>();
            public Dictionary<string, string> revoptions = new Dictionary<string, string>();
            public Dictionary<string, string> array = new Dictionary<string, string>();
        }
        public NewDb Copy()
        {
            NewDb newdatabase = new NewDb();

            newdatabase.Categories = new Dictionary<int, string>(Categories);
            newdatabase.Entries = new Dictionary<string, NewDbEntry>(Entries);
            newdatabase.DBtoId = new Dictionary<string, string>(DBtoId);
            newdatabase.IdtoDB = new Dictionary<string, string>(IdtoDB);
            newdatabase.IdToModel = new Dictionary<string, string>(IdToModel);
            newdatabase.Types = new Dictionary<int, string>(Types);
            newdatabase.KnownProperties = new List<EntryProperty>(KnownProperties);
            newdatabase.version = version;

            return newdatabase;
        }
    }
    public class DbCategory
    {
        public string name = "";
        public int id = 0;
        public Dictionary<string, string> objdict = new Dictionary<string, string>();
    }
    public class IndexedProperty<TIndex, TValue>
    {
        readonly Action<TIndex, TValue> SetAction;
        readonly Func<TIndex, TValue> GetFunc;

        public IndexedProperty(Func<TIndex, TValue> getFunc, Action<TIndex, TValue> setAction)
        {
            this.GetFunc = getFunc;
            this.SetAction = setAction;
        }

        public TValue this[TIndex i]
        {
            get
            {
                return GetFunc(i);
            }
            set
            {
                SetAction(i, value);
            }
        }
    }
    public class PropertyPanel
    {
        public Panel panel = new Panel();
        LevelObj oldobj;
        LevelObj obj;
        public LevelObj Obj
        {
            set { oldobj = obj; obj = value; }
            get { return obj; }
            
        }
        public NewDb newDb = new NewDb();
        public Form1 owner;
        public Dictionary<string, NewDb.EntryProperty> known = null;
        Rail oldrail;
        Rail rail;
        public Rail Rail
        {
            set { oldrail = rail; rail = value; }
            get { return rail; }

        }
        public void ClearRails()
        {
            rail = null;
            oldrail = null;
        }        
        
        public void ClearObjs()
        {
            obj = null;
            oldobj = null;
        }
        List<string> keep;
        List<string> remove;
        public void Update()
        {
            if (obj == null)
            {
                if (rail == null || oldrail == null)
                {
                    keep = null;
                    remove = null;
                    return;
                }

            }
            else
            {
                if (oldobj == null)
                {
                    keep = null;
                    remove = null;
                    return;
                }
                keep = new List<string>();
                remove = new List<string>();
                foreach (KeyValuePair<string, object> pair in oldobj.Prop)
                {
                    if (obj.Prop.ContainsKey(pair.Key))
                    {
                        keep.Add(pair.Key);
                    }
                    else
                    {
                        remove.Add(pair.Key);
                    }
                }
            }
        }
        public void Refresh()//gets the new data
        {
            panel.Visible = false;
            Update();
            return;
            if (obj == null)
            {
                if (rail == null)
                {
                    return;
                }
                panel.Controls.Clear();
                int index = 0;
                int x = 0;
                int y = 5;
                Label Railname = new Label();
                Railname.Name = "railname";
                Railname.Text = "Rail : " + rail.Name;
                Railname.Top = y;
                Railname.AutoSize = true;
                panel.Controls.Add(Railname);
                y += 30;
                Label General = new Label();
                General.Text = "General rail properties:";
                General.AutoSize = true;
                General.Top = y;
                y += 30;
                panel.Controls.Add(General);
                Dictionary<string, object> list = new Dictionary<string, object>();
                list.Add("Name",rail.Name);
                list.Add("Closed", rail.Closed);
                list.Add("I_id", rail.l_id);
                list.Add("LayerName", rail.LayerName);
                list.Add("Type", rail.Type);
                list.Add("Points", rail.Points);
                list.Add("no", rail.no);

                //properties
                y += 5;
                Label Args = new Label();
                Args.Text = "Rail arguments:";
                Args.AutoSize = true;
                Args.Top = y;
                Button EditArg = new Button();
                EditArg.Tag = "RailEdit";
                EditArg.Text = "Edit arguments";
                EditArg.AutoSize = true;
                EditArg.Left = Args.Right + 5;
                EditArg.Top = Args.Top;
                EditArg.Click += new System.EventHandler(this.buttonpress);
                
                y += 20;
                panel.Controls.Add(Args);
                panel.Controls.Add(EditArg);
                list.Clear();
                list.Add("Arg", rail.Arg);
                foreach (KeyValuePair<string, object> prop in list)
                {
                    if (prop.Key == "Arg")
                    {
                        y += 10;
                        int z = 0;
                        foreach (int i in (List<int>)prop.Value)
                        {
                            x = 0;
                            int value = i;
                            NumericUpDown upDown = new NumericUpDown();
                            upDown.DecimalPlaces = 0;
                            upDown.Minimum = -1;
                            upDown.Maximum = 1000;
                            upDown.Name = "arg_int" + z;
                            upDown.Value = value;
                            upDown.Enabled = true;
                            upDown.Visible = true;
                            upDown.ValueChanged += new System.EventHandler(this.numupdownupdated);
                            Label label = new Label();
                            label.Height = 15;
                            label.Text = "Arg[" + z.ToString() + "] :";
                            label.Top = y;
                            label.Left = x;
                            label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                            label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                            label.Width = label.Width - 10;
                            x += 100;
                            upDown.Top = y;
                            upDown.Left = x;
                            panel.Controls.Add(label);
                            panel.Controls.Add(upDown);
                            y += 25;
                            z++;
                        }
                        y += 10;
                    }
                }

            }
            else
            {
                int index = 0;
                int x = 0;
                int y = 5;
                if (keep == null)
                {
                    panel.Controls.Clear();

                    Label Objname = new Label();
                    Objname.Name = "objname";
                    if (obj.Prop.ContainsKey("dbname"))
                    {
                        Objname.Text = obj.Prop["dbname"].ToString();
                    }
                    else
                    {
                        Objname.Text = obj.Prop["name"].ToString().Substring(9) + " - (Undocumented object)";
                    }
                    Objname.Top = y;
                    Objname.AutoSize = true;
                    Objname.Font = new System.Drawing.Font(Label.DefaultFont, System.Drawing.FontStyle.Bold);
                    panel.Controls.Add(Objname);
                    y += 30;
                    Label General = new Label();
                    General.Text = "General object properties:";
                    General.AutoSize = true;
                    General.Top = y;
                    y += 30;
                    panel.Controls.Add(General);
                }
                else
                {

                    if (obj.Prop.ContainsKey("dbname"))
                    {
                        panel.Controls["objname"].Text = obj.Prop["dbname"].ToString();
                    }
                    else
                    {
                        panel.Controls["objname"].Text = obj.Prop["name"].ToString().Substring(9) + " - (Undocumented object)";
                    }
                    y += 60;
                }
                LevelObj objecto = new LevelObj();
                var sssd = obj.Prop.OrderBy(d => d.Key);
                foreach (KeyValuePair<string, object> p in sssd)
                {
                    objecto.Prop.Add(p.Key, p.Value);
                }
                foreach (KeyValuePair<string, object> prop in objecto.Prop)
                {
                    if (keep != null && keep.Contains(prop.Key))
                    {
                        if (prop.Value.GetType() == typeof(Single[]))
                        {

                            for (int i = 0; i < 3; i++)
                            {
                                if (prop.Key == "scale")
                                {
                                    ((NumericUpDown)panel.Controls["scale_" + i]).Value = (decimal)((Single[])prop.Value)[i];
                                }
                                else if (prop.Key == "dir")
                                {
                                    ((NumericUpDown)panel.Controls["dir_" + i]).Value = (decimal)((Single[])prop.Value)[i];
                                }
                                else if (prop.Key == "pos")
                                {
                                    ((NumericUpDown)panel.Controls["pos_" + i]).Value = (decimal)((Single[])prop.Value)[i];
                                }
                            }
                            y += 25;

                        }
                    }
                    else
                    {
                        if (known.ContainsKey(prop.Key))
                        {
                            foreach (NewDb.EntryProperty property in newDb.KnownProperties)
                            {
                                if (prop.Key == property.name)
                                {
                                    if (property.data_type == "float_array")//value is treated as array
                                    {
                                        x = 0;
                                        foreach (KeyValuePair<string, string> value in property.array)
                                        {
                                            NumericUpDown upDown = new NumericUpDown();
                                            int axis = 0;
                                            if (value.Key == "X")
                                            {
                                                axis = 0;
                                            }
                                            else if (value.Key == "Y")
                                            {
                                                axis = 1;
                                            }
                                            else
                                            {
                                                axis = 2;
                                            }
                                            upDown.TextAlign = HorizontalAlignment.Center;
                                            upDown.DecimalPlaces = 2;
                                            upDown.Enabled = true;
                                            upDown.Visible = true;
                                            upDown.Width = 50;
                                            if (property.name == "scale")
                                            {
                                                upDown.Minimum = 0;
                                                upDown.Maximum = 100;
                                                upDown.Increment = 0.01M;
                                                upDown.Name = "scale_" + axis;
                                            }
                                            else if (property.name == "dir")
                                            {
                                                upDown.Minimum = -360;
                                                upDown.Maximum = 360;
                                                upDown.Increment = 1;
                                                upDown.Name = "dir_" + axis;
                                            }
                                            else if (property.name == "pos")
                                            {
                                                upDown.Minimum = -100000000;
                                                upDown.Maximum = 100000000;
                                                upDown.Increment = 10;
                                                upDown.Name = "pos_" + axis;
                                            }
                                            upDown.Value = (decimal)((Single[])prop.Value)[axis];
                                            upDown.ValueChanged += new System.EventHandler(this.numupdownupdated);
                                            if (axis == 0)
                                            {
                                                Label label = new Label();
                                                ToolTip labeltt = new ToolTip();
                                                labeltt.SetToolTip(label, newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].info);
                                                label.Height = 15;
                                                label.Text = property.text;
                                                label.Top = y;
                                                label.Left = x;
                                                label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                                label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                                label.Width = label.Width - 10;
                                                panel.Controls.Add(label);
                                                x += 100;
                                            }
                                            else { x += 50; }
                                            upDown.Top = y;
                                            upDown.Left = x;
                                            panel.Controls.Add(upDown);

                                        }
                                        y += 25;
                                    }

                                }
                            }
                        }
                        else
                        {
                            if (prop.Value.GetType() == typeof(Single[]))
                            {
                                x = 0;
                                for (int i = 0; i < 3; i++)
                                {
                                    NumericUpDown upDown = new NumericUpDown();
                                    upDown.TextAlign = HorizontalAlignment.Center;
                                    upDown.DecimalPlaces = 2;
                                    upDown.Enabled = true;
                                    upDown.Visible = true;
                                    upDown.Width = 50;
                                    if (prop.Key == "scale")
                                    {
                                        upDown.Minimum = 0;
                                        upDown.Maximum = 100;
                                        upDown.Increment = 0.01M;
                                        upDown.Name = "scale_" + i;
                                    }
                                    else if (prop.Key == "dir")
                                    {
                                        upDown.Minimum = -360;
                                        upDown.Maximum = 360;
                                        upDown.Increment = 1;
                                        upDown.Name = "dir_" + i;
                                    }
                                    else if (prop.Key == "pos")
                                    {
                                        upDown.Minimum = -100000000;
                                        upDown.Maximum = 100000000;
                                        upDown.Increment = 10;
                                        upDown.Name = "pos_" + i;
                                    }
                                    upDown.Value = (decimal)((Single[])prop.Value)[i];
                                    upDown.ValueChanged += new System.EventHandler(this.numupdownupdated);
                                    if (i == 0)
                                    {
                                        Label label = new Label();
                                        label.Height = 15;
                                        label.Text = prop.Key;
                                        label.Top = y;
                                        label.Left = x;
                                        label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                        label.Width = label.Width - 10;
                                        panel.Controls.Add(label);
                                        x += 100;
                                    }
                                    else { x += 50; }
                                    upDown.Top = y;
                                    upDown.Left = x;


                                    panel.Controls.Add(upDown);

                                }
                                y += 25;
                            }
                        }
                    }
                }
                if (obj.Prop.ContainsKey("pos"))
                {
                    if (keep != null && keep.Contains("pos"))
                    {

                    }
                    else if (known.ContainsKey("pos"))
                    {

                    }
                }
                foreach (KeyValuePair<string, object> prop in objecto.Prop)
                {
                    if (keep != null && keep.Contains(prop.Key))
                    {
                        if (prop.Key == "Rail")
                        {
                            if (obj.Prop["Rail"].ToString().Contains("Unk Type (FF)"))
                            {

                                ((Button)panel.Controls[prop.Key]).Text = "Fix garbage rail data";
                                ((Button)panel.Controls[prop.Key]).Tag = "BrokenRail";
                                ((Label)panel.Controls["lbl_"+prop.Key]).Text = "Undefined rail:";
                            }
                            else
                            {
                                ((Button)panel.Controls[prop.Key]).Tag = "Rail";
                                ((Button)panel.Controls[prop.Key]).Text = "Edit Rail: " + ((Rail)prop.Value).Name;
                                ((Label)panel.Controls["lbl_" + prop.Key]).Text = "Edit Rail: " + prop.Value.ToString();
                            }
                            y += 25;
                        }
                        else if (prop.Value.GetType() == typeof(Node))
                        {
                            if (((Node)prop.Value).NodeType == Node.NodeTypes.String)
                            {
                                if (prop.Key != "MarioType" && prop.Key != "LuigiType")
                                {
                                    ((TextBox)panel.Controls[prop.Key]).Text = prop.Value.ToString().Substring(9);
                                }
                                else
                                {

                                    string a;
                                    NewDb.EntryProperty b;
                                    known.TryGetValue(prop.Key, out b);
                                    newDb.KnownProperties[newDb.KnownProperties.IndexOf(b)].revoptions.TryGetValue(prop.Value.ToString().Substring(9), out a);
                                    ((ComboBox)panel.Controls[prop.Key]).SelectedIndex = ((ComboBox)panel.Controls[prop.Key]).Items.IndexOf(a);
                                }
                            }
                            else if (((Node)prop.Value).NodeType == Node.NodeTypes.Int)
                            {
                                ((NumericUpDown)panel.Controls[prop.Key]).Value = decimal.Parse((prop.Value).ToString().Substring(6));
                            }
                            y += 25;
                        }
                        else if (prop.Value.GetType() == typeof(C0List))
                        {
                            ((Button)panel.Controls[prop.Key]).Text = "Edit C0List";
                            ((Button)panel.Controls[prop.Key]).Tag = prop.Key;
                            y += 25;
                        }
                    }
                    else
                    {
                        if (known.ContainsKey(prop.Key))
                        {
                            foreach (NewDb.EntryProperty property in newDb.KnownProperties)
                            {
                                bool containskeep = false;
                                bool containsremove = false;
                                if (keep != null)
                                {
                                    containskeep = keep.Contains(prop.Key);
                                }
                                if (remove != null)
                                {
                                    containsremove = remove.Contains(prop.Key);
                                }
                                if (prop.Key == property.name && !containskeep && !containsremove)
                                {
                                    if (property.data_type == "int")
                                    {
                                        x = 0;
                                        NumericUpDown upDown = new NumericUpDown();
                                        upDown.DecimalPlaces = 0;
                                        upDown.Minimum = -1;
                                        upDown.Maximum = 1000;
                                        upDown.Value = int.Parse(prop.Value.ToString().Substring("Int : ".Length));
                                        upDown.Enabled = true;
                                        upDown.Visible = true;
                                        upDown.Width = 50;
                                        upDown.Name = property.name;
                                        upDown.ValueChanged += new System.EventHandler(this.numupdownupdated);
                                        Label label = new Label();
                                        ToolTip labeltt = new ToolTip();
                                        labeltt.SetToolTip(label, newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].info);
                                        label.Height = 15;
                                        label.Text = newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].text;
                                        label.Top = y;
                                        label.Left = x;
                                        label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                        label.Width = label.Width - 10;
                                        x += 100;
                                        upDown.Top = y;
                                        upDown.Left = x;
                                        label.Name = "lbl_" + prop.Key.Trim();
                                        panel.Controls.Add(label);
                                        panel.Controls.Add(upDown);
                                        y += 25;
                                        break;
                                    }
                                    else if (property.data_type == "string")
                                    {
                                        x = 0;
                                        TextBox textBox = new TextBox();
                                        textBox.Text = prop.Value.ToString().Substring("String : ".Length);
                                        textBox.Enabled = true;
                                        textBox.Visible = true;
                                        textBox.Name = property.name;
                                        textBox.Tag = textBox.Text;
                                        textBox.Validated += new EventHandler(this.textboxupdated);
                                        Label label = new Label();
                                        ToolTip labeltt = new ToolTip();
                                        labeltt.SetToolTip(label, newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].info);
                                        label.Height = 15;
                                        label.Text = newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].text;
                                        label.Top = y;
                                        label.Left = x;
                                        label.BackColor = System.Drawing.Color.FromArgb(225, 100, 100, 100);
                                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                        label.Width = label.Width - 10;
                                        label.Name = "lbl_" + prop.Key.Trim();
                                        x += 100;
                                        textBox.Top = y;
                                        textBox.Left = x;
                                        panel.Controls.Add(label);
                                        panel.Controls.Add(textBox);
                                        y += 25;
                                        break;
                                    }
                                    else if (property.data_type == "Rail")//opens rail editor form
                                    {
                                        Button button = new Button();
                                        Label label = new Label();
                                        ToolTip labeltt = new ToolTip();

                                        button.Enabled = true;
                                        button.Visible = true;
                                        button.Name = property.name;


                                        if (obj.Prop["Rail"].ToString().Contains("Unk Type (FF)"))
                                        {
                                            button.Text = "Fix garbage rail data";
                                            button.Tag = "BrokenRail";
                                            button.Click += new EventHandler(this.buttonpress);
                                            label.Text = "Undefined rail:";
                                            labeltt.SetToolTip(label, "This rail was added as garbage data in the retail game, so you need to fix it to use it");
                                        }
                                        else
                                        {
                                            button.Tag = "Rail";
                                            button.Text = "Edit Rail: " + prop.Value.ToString();
                                            button.Click += new EventHandler(this.buttonpress);
                                            label.Text = newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].text;
                                            labeltt.SetToolTip(label, newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].info);
                                        }
                                        button.AutoSize = true;
                                        x = 0;

                                        label.Height = 15;

                                        label.Top = y;
                                        label.Left = x;
                                        label.Name = "lbl_Rail";
                                        label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                        label.Width = label.Width - 10;

                                        x += 100;
                                        button.Top = y;
                                        button.Left = x;
                                        panel.Controls.Add(label);
                                        panel.Controls.Add(button);
                                        y += 25;
                                        break;
                                    }
                                    else if (property.data_type == "C0List")//changes view to c0editing
                                    {
                                        Label label = new Label();
                                        ToolTip labeltt = new ToolTip();
                                        labeltt.SetToolTip(label, newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].info);
                                        x = 0;
                                        Button button = new Button();
                                        button.Text = "Edit C0List";
                                        button.Enabled = true;
                                        button.Visible = true;
                                        button.Name = prop.Key.Trim();
                                        button.Click += new EventHandler(this.buttonpress);
                                        button.Tag = "C0List";

                                        label.Height = 15;
                                        label.Text = newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].text;
                                        label.Top = y;
                                        label.Left = x;
                                        label.Name = "lbl_" + prop.Key.Trim();
                                        label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                        label.Width = label.Width - 10;

                                        x += 100;
                                        button.Top = y;
                                        button.Left = x;

                                        panel.Controls.Add(label);
                                        panel.Controls.Add(button);
                                        y += 25;
                                        break;
                                    }
                                    /*else if (property.data_type == "float_array")//value is treated as array
                                    {
                                        y -= 25;
                                        break;
                                    }*/
                                    else if (property.data_type == "string_option")
                                    {
                                        object objname;
                                        obj.Prop.TryGetValue("name", out objname);
                                        x = 0;
                                        ComboBox combobox = new ComboBox();
                                        foreach (string option in property.options.Keys)
                                        {
                                            combobox.Items.Add(option);
                                        }
                                        var valuee = prop.Value;
                                        int value = 0;
                                        string a;
                                        if (newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].revoptions.TryGetValue(prop.Value.ToString().Substring(9), out a))
                                        {
                                            value = combobox.Items.IndexOf(a);
                                        }
                                        else
                                        {
                                            newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].revoptions.TryGetValue(newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].default_value, out a);
                                            value = combobox.Items.IndexOf(a);
                                        }
                                        combobox.SelectedIndex = value;
                                        combobox.DropDownStyle = ComboBoxStyle.DropDownList;
                                        combobox.Name = property.name;
                                        combobox.Tag = property;
                                        combobox.SelectedIndexChanged += new System.EventHandler(this.comboboxupdated);

                                        Label label = new Label();
                                        ToolTip labeltt = new ToolTip();
                                        labeltt.SetToolTip(label, newDb.KnownProperties[newDb.KnownProperties.IndexOf(property)].info);
                                        label.Height = 15;
                                        label.Text = property.text;
                                        label.Top = y;
                                        label.Left = x;
                                        label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                        label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                        label.Width = label.Width - 10;
                                        x += 100;
                                        combobox.Top = y;
                                        combobox.Left = x;
                                        panel.Controls.Add(combobox);
                                        panel.Controls.Add(label);
                                        y += 25;
                                        break;
                                        //newDb.Entries[(string)objname].args[indie].revoptions[((int[])valuee)[newDb.Entries["Kinopio"].args[indie].arg_id]];
                                    }
                                    
                                }
                            }
                        }
                        else
                        {
                            if (prop.Key == "Rail")
                            {
                                Button button = new Button();
                                Label label = new Label();


                                button.Enabled = true;
                                button.Visible = true;
                                button.Name = "Rail";


                                if (obj.Prop["Rail"].ToString().Contains("Unk Type (FF)"))
                                {
                                    ToolTip labeltt = new ToolTip();
                                    button.Text = "Fix garbage rail data";
                                    button.Tag = "BrokenRail";
                                    button.Click += new EventHandler(this.buttonpress);
                                    label.Text = "Undefined rail:";
                                    labeltt.SetToolTip(label, "This rail was added as garbage data in the retail game, so you need to fix it to use it");
                                }
                                else
                                {
                                    button.Tag = "Rail";
                                    button.Text = "Edit Rail: " + ((Rail)prop.Value).Name;
                                    button.Click += new EventHandler(this.buttonpress);
                                    label.Text = "Rail:";
                                }
                                button.AutoSize = true;
                                x = 0;

                                label.Height = 15;

                                label.Top = y;
                                label.Left = x;
                                label.Name = "lbl_Rail";
                                label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                label.Width = label.Width - 10;

                                x += 100;
                                button.Top = y;
                                button.Left = x;
                                panel.Controls.Add(label);
                                panel.Controls.Add(button);
                                y += 25;
                            }
                            else if (prop.Value.GetType() == typeof(Node))
                            {
                                if(((Node)prop.Value).NodeType == Node.NodeTypes.String)
                                {
                                    x = 0;
                                    TextBox textBox = new TextBox();
                                    textBox.Text = prop.Value.ToString().Substring("String : ".Length);
                                    textBox.Enabled = true;
                                    textBox.Visible = true;
                                    textBox.Name = prop.Key.Trim();
                                    textBox.Tag = textBox.Text;
                                    textBox.Validated += new EventHandler(this.textboxupdated);
                                    Label label = new Label();

                                    label.Height = 15;
                                    label.Text = prop.Key + ":";
                                    label.Top = y;
                                    label.Left = x;
                                    label.Name = "lbl_"+ prop.Key.Trim();
                                    label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                    label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                    label.Width = label.Width - 10;

                                    x += 100;
                                    textBox.Top = y;
                                    textBox.Left = x;
                                    panel.Controls.Add(label);
                                    panel.Controls.Add(textBox);
                                    y += 25;
                                }
                                else if (((Node)prop.Value).NodeType == Node.NodeTypes.Int)
                                {
                                    x = 0;
                                    NumericUpDown upDown = new NumericUpDown();
                                    upDown.DecimalPlaces = 0;
                                    upDown.Minimum = -1;
                                    upDown.Maximum = 1000;
                                    upDown.Value = int.Parse(prop.Value.ToString().Substring("Int : ".Length));
                                    upDown.Enabled = true;
                                    upDown.Visible = true;
                                    upDown.Width = 50;
                                    upDown.Name = prop.Key.Trim();
                                    upDown.ValueChanged += new System.EventHandler(this.numupdownupdated);
                                    Label label = new Label();

                                    label.Height = 15;
                                    label.Text = prop.Key + ":";
                                    label.Top = y;
                                    label.Left = x;
                                    label.Name = "lbl_" + prop.Key.Trim();
                                    label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                    label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                    label.Width = label.Width - 10;
                                    x += 100;
                                    upDown.Top = y;
                                    upDown.Left = x;

                                    panel.Controls.Add(label);
                                    panel.Controls.Add(upDown);
                                    y += 25;
                                }
                                
                            }
                            else if (prop.Value.GetType() == typeof(C0List))
                            {

                                Label label = new Label();
                                x = 0;
                                Button button = new Button();
                                button.Text = "Edit C0List";
                                button.Enabled = true;
                                button.Visible = true;
                                button.Name = prop.Key.Trim();
                                button.Click += new EventHandler(this.buttonpress);
                                button.Tag = "C0List";

                                label.Height = 15;
                                label.Text = "C0List:";
                                label.Top = y;
                                label.Left = x;
                                label.Name = "lbl_"+ prop.Key.Trim();
                                label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                label.Width = label.Width - 10;

                                x += 100;
                                button.Top = y;
                                button.Left = x;

                                panel.Controls.Add(label);
                                panel.Controls.Add(button);
                                y += 25;
                            }
                        }
                        index++;
                    }
                }

                if (remove != null)//it's not in this object's prop so it never reaches this point :umokay:
                {
                    foreach (string s in remove)
                    {
                        if (s != "dbname")
                        {
                            panel.Controls.Remove(panel.Controls[panel.Controls.IndexOfKey(s)]);
                            panel.Controls.Remove(panel.Controls[panel.Controls.IndexOfKey("lbl_" + s)]);
                        }
                    }

                }

                if (owner.comboBox1.SelectedItem.ToString() != "StartInfo")
                {
                    y += 5;
                    Label Args = new Label();
                    Args.Text = "Object arguments:";
                    Args.AutoSize = true;
                    Args.Top = y;
                    Args.Name = "ArgSection";
                    //y += 20;

                    if (panel.Controls.IndexOfKey("ArgSection") == -1)
                    {

                        panel.Controls.Add(Args);
                    }
                    y += 20;
                    ArgRefresh();
                }
            }
            Resize();
        }
        public void Resize()
        {
            panel.Visible = false;
            panel.Enabled = true;
            panel.VerticalScroll.Enabled = false;
            panel.VerticalScroll.Visible = false;
            if (panel.Controls.Count == 0)
            {
                return;
            }
            List<int> y = new List<int>();
            List<int> x = new List<int>();
            foreach (Control control in panel.Controls)
            {
                y.Add(control.Top);
                x.Add(control.Right);
            }
           panel.Width = x.Max() + 45;
            if (y.Max() > panel.Height-10)
            {
                panel.Refresh();
                panel.VerticalScroll.Enabled = true;
                panel.VerticalScroll.Visible = true;
                panel.VerticalScroll.SmallChange = 5;
            }
        }
        private void numupdownupdated(object sender, EventArgs e)
        {
            if (((int[])obj.Prop["Arg"]).Length < 10)
            {
                int z = 0;
                int[] array = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                foreach (int iii in (int[])obj.Prop["Arg"])
                {
                    array[z] = iii;
                    z++;
                }
                obj.Prop["Arg"] = array;
            }
            if (((NumericUpDown)sender).Name.Contains("arg"))
            {//is argument
                ((int[])obj.Prop["Arg"])[int.Parse(((NumericUpDown)sender).Name.Substring(7))] = (int)((NumericUpDown)sender).Value;
            }
            else if (((NumericUpDown)sender).Name.Contains("scale") || ((NumericUpDown)sender).Name.Contains("pos") || ((NumericUpDown)sender).Name.Contains("dir"))
            {
                int valud = int.Parse(((NumericUpDown)sender).Name.Substring(((NumericUpDown)sender).Name.Length-1));
                ((Single[])obj.Prop[((NumericUpDown)sender).Name.Substring(0, ((NumericUpDown)sender).Name.Length-2)])[valud] = (Single)((NumericUpDown)sender).Value;
                owner.UpdateOBJPos(owner.ObjectsListBox.SelectedIndex, owner.CurrentAllInfosSection, owner.CurrentAllInfosSectionName);
            }
            else
            {//is general property
                obj.Prop[((NumericUpDown)sender).Name] = new Node(((NumericUpDown)sender).Value.ToString(), "D1");
            }
        }
        private void checkupdated(object sender, EventArgs e)
        {
            if (((int[])obj.Prop["Arg"]).Length < 10)
            {
                int z = 0;
                int[] array = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                foreach (int iii in (int[])obj.Prop["Arg"])
                {
                    array[z] = iii;
                    z++;
                }
                obj.Prop["Arg"] = array;
            }
            int value;
            if (((CheckBox)sender).Checked)
            {
                value = 1;
            }else
            {
                value = -1;
            }
            ((int[])obj.Prop["Arg"])[int.Parse(((CheckBox)sender).Name.Substring(8))] = value;
        }
        private void comboboxupdated(object sender, EventArgs e)
        {
            if (!int.TryParse(((Control)sender).Tag.ToString(), out _))
            {
                string a;
                ((NewDb.EntryProperty)((ComboBox)sender).Tag).options.TryGetValue(((ComboBox)sender).SelectedItem.ToString(), out a);
                //a = newDb.KnownProperties[].options;
                obj.Prop[((Control)sender).Name] = new Node(a, "A0");
            }
            else
            {
                if (((int[])obj.Prop["Arg"]).Length < 10)
                {
                    int z = 0;
                    int[] array = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                    foreach (int iii in (int[])obj.Prop["Arg"])
                    {
                        array[z] = iii;
                        z++;
                    }
                    obj.Prop["Arg"] = array;
                }
                string aaa = obj.Prop["name"].ToString().Substring(9);
                string addaa = ((ComboBox)sender).Name.Substring(10);
                string omg = ((ComboBox)sender).SelectedItem.ToString();
                int value = int.Parse(newDb.Entries[aaa].args[(int)(((ComboBox)sender).Tag)].options[omg]);
                ((int[])obj.Prop["Arg"])[int.Parse(((ComboBox)sender).Name.Substring(10))] = value;
            }
        }
        private void textboxupdated(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                ((TextBox)sender).Text = obj.Prop[((TextBox)sender).Name].ToString().Substring(9);
            }
            if (((TextBox)sender).Name == "name")
            {
                if (((TextBox)sender).Text == (string)((TextBox)sender).Tag)
                {
                    return;
                }
                else
                {
                    ((TextBox)sender).Tag = ((TextBox)sender).Text;
                }
                obj.Prop[((TextBox)sender).Name] = new Node(((TextBox)sender).Text, "A0");
                if (newDb != null)
                {
                    if (newDb.Entries.ContainsKey(((TextBox)sender).Text))
                    {
                        if (obj.Prop.ContainsKey("dbname"))
                        {
                            obj.Prop["dbname"] = newDb.IdtoDB[((TextBox)sender).Text];
                        }
                        else
                        {
                            obj.Prop.Add("dbname", newDb.IdtoDB[((TextBox)sender).Text]);
                        }
                    }
                    else
                    {
                        if (obj.Prop.ContainsKey("dbname"))
                        {
                            obj.Prop.Remove("dbname");
                            //obj.Prop["dbname"] = obj.Prop["name"].ToString().Substring(9);
                        }
                        else
                        {
                            //obj.Prop.Add("dbname", obj.Prop["name"].ToString().Substring(9));

                        }
                    }
                }
                updateinfo();
                string path = owner.GetModelname(owner.CurrentAllInfosSection[owner.ObjectsListBox.SelectedIndex].GetName(true));//db name
                if (!System.IO.File.Exists(path)) path = "models\\UnkBlue.obj";
                foreach (int i in owner.ObjectsListBox.SelectedIndices) owner.render.ChangeModel(owner.CurrentAllInfosSectionName, i, path);
                owner.UpdateOBJPos(owner.ObjectsListBox.SelectedIndex, owner.CurrentAllInfosSection, owner.CurrentAllInfosSectionName);
            }
            obj.Prop[((TextBox)sender).Name] = new Node(((TextBox)sender).Text, "A0");
        }
        private void buttonpress(object sender, EventArgs e)
        {
            if ((string)((Control)sender).Tag == "RailEdit")
            {
                //FormEditors.FrmRailArgsEditor((Rail)sender)
                MessageBox.Show("not yet implemented");
                Resize();
            }
            else if ((string)((Control)sender).Tag == "BrokenRail")
            {
                obj.Prop.Remove("Rail");
                Rail newrail = new Rail();
                obj.Prop.Add("Rail", newrail);
                panel.Controls["lbl_Rail"].Text = "Rail:";
                panel.Controls["Rail"].Text = "Edit Rail: " + newrail.Name;
                panel.Controls["Rail"].Tag = "Rail";
                Resize();
            }
            else if ((string)((Control)sender).Tag == "Rail")
            {

                //FormEditors.FrmRailEditor((Rail)sender)
                MessageBox.Show("not yet implemented");
            }
            else if ((string)((Control)sender).Tag == "C0List")
            {
                if ((string)((Button)sender).Name == "GenerateChildren")
                {
                    if ((C0List)obj.Prop["GenerateChildren"] != null) { owner.EditC0List(((C0List)obj.Prop["GenerateChildren"]).List, true); }
                    ClearObjs();
                    panel.Controls.Clear();
                }
                else if ((string)((Button)sender).Name == "ChildrenArea")
                {
                    if ((C0List)obj.Prop["ChildrenArea"] != null) { owner.EditC0List(((C0List)obj.Prop["ChildrenArea"]).List, true); }
                    ClearObjs();
                    panel.Controls.Clear();
                }
            }
        }
        private void updateinfo()
        {
            if (obj == null)
            {
                panel.Controls["railname"].Text = "Rail : " + rail.Name;
            }
            else
            {
                if (obj.Prop.ContainsKey("dbname"))
                {
                    panel.Controls["objname"].Text = obj.Prop["dbname"].ToString();
                    owner.fullrefresh = false;
                    owner.ObjectsListBox.Items[owner.ObjectsListBox.SelectedIndex] = obj.Prop["dbname"].ToString();
                    owner.fullrefresh = true;
                }
                else
                {
                    panel.Controls["objname"].Text = obj.Prop["name"].ToString().Substring(9) + " - (Undocumented object)";
                    owner.fullrefresh = false;
                    owner.ObjectsListBox.Items[owner.ObjectsListBox.SelectedIndex] = obj.Prop["name"].ToString().Substring(9);
                    owner.fullrefresh = true;
                }


            }
        }
        public void ArgRefresh()
        {
            if (obj != null)
            {
                int y = 0;
                int x = 0;
                List<Control> args = new List<Control>();
                foreach (Control contrl in panel.Controls)
                {
                    args.Add(contrl);
                    if (contrl.Name == "ArgSection")
                    {
                        y = contrl.Top + 20;
                    }
                }
                foreach (Control control in args)
                {
                    if (control.Name.Contains("arg"))
                    {
                        panel.Controls.Remove(control);
                    }
                }
                foreach (KeyValuePair<string, object> prop in obj.Prop)
                {
                    if(prop.Key == "Arg")
                    {
                        object objname;
                        obj.Prop.TryGetValue("name", out objname);
                        y += 10;
                        int indie = 0;
                        if (newDb != null && newDb.Entries.ContainsKey(((Node)objname).StringValue))
                        {
                            foreach (NewDb.EntryArg arg in ((NewDb.NewDbEntry)newDb.Entries[((Node)objname).StringValue]).args)
                            {
                                if (arg.type == "bool")
                                {
                                    CheckBox check = new CheckBox();
                                    var valuee = prop.Value;
                                    int value;
                                    if (((int[])valuee).Length - 1 >= newDb.Entries[((Node)objname).StringValue].args[indie].arg_id)
                                    {
                                        value = (((int[])valuee)[newDb.Entries[((Node)objname).StringValue].args[indie].arg_id]);
                                    }
                                    else
                                    {
                                        value = -1;
                                    }
                                    if (value == -1)
                                    {
                                        check.Checked = false;
                                    }
                                    else
                                    {
                                        check.Checked = true;
                                    }
                                    check.Enabled = true;
                                    check.Visible = true;
                                    check.Top = y;
                                    check.Left = 10;
                                    check.Text = newDb.Entries[((Node)objname).StringValue].args[indie].name;
                                    check.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                    check.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
                                    check.Name = "arg_bool" + newDb.Entries[((Node)objname).StringValue].args[indie].arg_id;
                                    check.CheckedChanged += new System.EventHandler(this.checkupdated);
                                    panel.Controls.Add(check);
                                    y += 25;
                                }
                                else if (arg.type == "option")
                                {
                                    x = 0;
                                    ComboBox combobox = new ComboBox();
                                    foreach (string option in arg.options.Keys)
                                    {
                                        combobox.Items.Add(option);
                                    }
                                    var valuee = prop.Value;
                                    int value = 0;
                                    if (newDb.Entries[((Node)objname).StringValue].args[indie].revoptions.ContainsKey(((int[])valuee)[indie].ToString()))
                                    {
                                        if (((int[])valuee).Length - 1 >= newDb.Entries[((Node)objname).StringValue].args[indie].arg_id)
                                        {
                                            value = combobox.Items.IndexOf(newDb.Entries[((Node)objname).StringValue].args[indie].revoptions[((int[])valuee)[indie].ToString()]);
                                        }
                                        else
                                        {
                                            value = combobox.Items.IndexOf(newDb.Entries[((Node)objname).StringValue].args[indie].revoptions[newDb.Entries[((Node)objname).StringValue].args[indie].default_value]);
                                        }
                                    }
                                    else
                                    {
                                        value = combobox.Items.IndexOf(newDb.Entries[((Node)objname).StringValue].args[indie].revoptions[newDb.Entries[((Node)objname).StringValue].args[indie].default_value]);
                                    }
                                    combobox.SelectedIndex = value;
                                    combobox.DropDownStyle = ComboBoxStyle.DropDownList;
                                    combobox.Name = "arg_option" + newDb.Entries[((Node)objname).StringValue].args[indie].arg_id;

                                    combobox.Tag = indie;
                                    combobox.SelectedIndexChanged += new System.EventHandler(this.comboboxupdated);

                                    Label label = new Label();
                                    label.Height = 15;
                                    label.Text = newDb.Entries[((Node)objname).StringValue].args[indie].name;
                                    label.Top = y;
                                    label.Left = x;
                                    label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                    label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                    label.Width = label.Width - 10;
                                    x += 100;
                                    combobox.Top = y;
                                    combobox.Left = x;
                                    label.Name = "lbl_arg" + indie;
                                    panel.Controls.Add(combobox);
                                    panel.Controls.Add(label);
                                    y += 25;
                                    //newDb.Entries[(string)objname].args[indie].revoptions[((int[])valuee)[newDb.Entries["Kinopio"].args[indie].arg_id]];
                                }
                                else if (arg.type == "int")
                                {
                                    x = 0;
                                    var valuee = prop.Value;
                                    int value;
                                    if (((int[])valuee).Length - 1 >= newDb.Entries[((Node)objname).StringValue].args[indie].arg_id)
                                    {
                                        value = (((int[])valuee)[newDb.Entries[((Node)objname).StringValue].args[indie].arg_id]);
                                    }
                                    else
                                    {
                                        value = -1;
                                    }
                                    NumericUpDown upDown = new NumericUpDown();
                                    upDown.DecimalPlaces = 0;
                                    if (newDb.Entries[((Node)objname).StringValue].args[indie].min != 0)
                                    {
                                        upDown.Minimum = newDb.Entries[((Node)objname).StringValue].args[indie].min;
                                    }
                                    else
                                    {
                                        upDown.Minimum = -1;
                                    }
                                    if (newDb.Entries[((Node)objname).StringValue].args[indie].max != 0)
                                    {
                                        upDown.Maximum = newDb.Entries[((Node)objname).StringValue].args[indie].max;
                                    }
                                    else
                                    {
                                        upDown.Maximum = 1000;
                                    }

                                    upDown.Name = "arg_int" + newDb.Entries[((Node)objname).StringValue].args[indie].arg_id;
                                    upDown.Value = value;
                                    upDown.Enabled = true;
                                    upDown.Visible = true;
                                    upDown.ValueChanged += new System.EventHandler(this.numupdownupdated);
                                    Label label = new Label();
                                    label.Height = 15;
                                    label.Text = newDb.Entries[((Node)objname).StringValue].args[indie].name;
                                    label.Top = y;
                                    label.Left = x;
                                    label.Name = "lbl_arg" + indie;
                                    label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                    label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                    label.Width = label.Width - 10;
                                    x += 100;
                                    upDown.Top = y;
                                    upDown.Left = x;
                                    panel.Controls.Add(label);
                                    panel.Controls.Add(upDown);
                                    y += 25;
                                }

                                indie++;
                            }
                            y += 10;
                            break;
                        }
                        else
                        {
                            int z = 0;
                            foreach (int i in (int[])prop.Value)
                            {
                                x = 0;
                                int value = i;
                                NumericUpDown upDown = new NumericUpDown();
                                upDown.DecimalPlaces = 0;
                                upDown.Minimum = -1;
                                upDown.Maximum = 1000;
                                upDown.Name = "arg_int" + z;
                                upDown.Value = value;
                                upDown.Enabled = true;
                                upDown.Visible = true;
                                upDown.ValueChanged += new System.EventHandler(this.numupdownupdated);
                                Label label = new Label();
                                label.Height = 15;
                                label.Text = "Arg[" + z.ToString() + "] :";
                                label.Name = "lbl_arg" + z;
                                label.Top = y;
                                label.Left = x;
                                label.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                                label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                                label.Width = label.Width - 10;
                                x += 100;
                                upDown.Top = y;
                                upDown.Left = x;
                                panel.Controls.Add(label);
                                panel.Controls.Add(upDown);
                                y += 25;
                                z++;
                            }
                            y += 10;
                            break;
                        }
                        

                    }
                }
                Resize();

            }
            else
            {

            }
        }
    }
}



namespace ExtensionMethods
{
    static class Extensions
    {
        public static Vector3D ToVect(this Point3D p)
        {
            return new Vector3D(p.X, p.Y, p.Z);
        }
    }
}
