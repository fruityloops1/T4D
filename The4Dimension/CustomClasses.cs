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
        public List<LevelObj> GetbyScenarioX(string scenario) //gets scenarios with the exact name Common, SubCommon, 1, 2, 3
        {
            List<LevelObj> Ret = new List<LevelObj>();
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Prop.ContainsKey("LayerName")) // Scenario
                {
                    if (((Node)this[i].Prop["LayerName"]).StringValue == scenario)
                    {
                        Ret.Add(this[i]);
                    }
                }

            }
            return Ret;
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
                } else if (this[i].Prop.ContainsKey("GenerateParent")) // GenerateChildren
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
        public bool IsHidden = false;

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
            public string name, notes, files, model = "", type = "";
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
            foreach (NewDbEntry entry in Entries.Values)
            {
                if (CCNT.ContainsKey(entry.filename))
                {
                    doc += 1;
                }
            }
            ret = Math.Round((doc / accnt) * 100, 2);
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
                } else if (node.Name == "properties")
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
                for (int i = 1; i > 8; i++)//8 is arbitrary, could be any number
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

    public class Camera3DL : Object
    {
        public int UserGroupId = 0;
        public string Category = "Map";
        public string Class = "Parallel";
        public string UserName = "CameraArea";
        public Dictionary<string, Node> Attributes = new Dictionary<string, Node>();

        public Camera3DL(bool newcamsetup = false)
        {
            if (!newcamsetup) return;
            Attributes.Add("AngleH", new Node("0", "D2"));
            Attributes.Add("AngleV", new Node("25", "D2"));
            Attributes.Add("Distance", new Node("1200", "D2"));
            Attributes.Add("UpOffset", new Node("100", "D2"));

            Dictionary<string, Node> VsParam = new Dictionary<string, Node>();
            VsParam.Add("FovyDegree", new Node("45", "D2"));
            VsParam.Add("StereovisionDepth", new Node("0,4", "D2"));
            VsParam.Add("StereovisionDistance", new Node("650", "D2"));

            Attributes.Add("VisionParam", new Node(VsParam, "C1"));
        }
        public XmlWriter CamToXml(XmlWriter xr)
        {
            xr.WriteStartElement("C1");
            List<string> OrderedAttributes = new List<string>() { "UserGroupId", "Category", "Class", "UserName" };
            OrderedAttributes.AddRange(Attributes.Keys);
            OrderedAttributes.Sort();
            for (int a = 0; a < OrderedAttributes.Count; a++)
            {
                if (OrderedAttributes[a] == "Class"|| OrderedAttributes[a] == "Category" || OrderedAttributes[a] == "UserName")
                {

                    xr.WriteStartElement("A0");
                    xr.WriteAttributeString("Name", OrderedAttributes[a]);
                    xr.WriteAttributeString("StringValue", ((OrderedAttributes[a] == "Class") ? Class: ((OrderedAttributes[a] == "Category") ? Category:UserName)) );
                    xr.WriteEndElement();
                }
                else if (OrderedAttributes[a] == "UserGroupId")
                {
                    xr.WriteStartElement("D1");
                    xr.WriteAttributeString("Name", "UserGroupId");
                    xr.WriteAttributeString("StringValue", UserGroupId.ToString());
                    xr.WriteEndElement();
                }
                else
                {
                    string elementType = "0";
                    if (Attributes[OrderedAttributes[a]].NodeType == Node.NodeTypes.Bool) elementType = "D0";
                    if (Attributes[OrderedAttributes[a]].NodeType == Node.NodeTypes.Int) elementType = "D1";
                    if (Attributes[OrderedAttributes[a]].NodeType == Node.NodeTypes.Single) elementType = "D2";
                    if (Attributes[OrderedAttributes[a]].NodeType == Node.NodeTypes.NodeList) elementType = "C1";

                    if (Attributes[OrderedAttributes[a]].NodeType != Node.NodeTypes.NodeList)
                    {

                        xr.WriteStartElement(elementType);
                        xr.WriteAttributeString("Name", OrderedAttributes[a]);
                        xr.WriteAttributeString("StringValue", Attributes[OrderedAttributes[a]].StringValue);
                        xr.WriteEndElement();
                    }
                    else
                    {
                        xr.WriteStartElement(elementType);
                        xr.WriteAttributeString("Name", OrderedAttributes[a]);
                        //xr.WriteAttributeString("StringValue", Attributes[OrderedAttributes[a]].StringValue);
                        foreach (string childkey in Attributes[OrderedAttributes[a]]._ChildrenNodes.Keys)
                        {


                            if (Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey].NodeType == Node.NodeTypes.Bool) elementType = "D0";
                            if (Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey].NodeType == Node.NodeTypes.Int) elementType = "D1";
                            if (Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey].NodeType == Node.NodeTypes.Single) elementType = "D2";
                            if (Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey].NodeType == Node.NodeTypes.NodeList) elementType = "C1";



                            if (Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey].NodeType != Node.NodeTypes.NodeList)
                            {

                                xr.WriteStartElement(elementType);
                                xr.WriteAttributeString("Name", childkey);
                                xr.WriteAttributeString("StringValue", Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey].StringValue);
                                xr.WriteEndElement();
                            }
                            else
                            {
                                xr.WriteStartElement(elementType);
                                xr.WriteAttributeString("Name", childkey);
                                //xr.WriteAttributeString("StringValue", Attributes[OrderedAttributes[a]].StringValue);
                                foreach (string cchildkey in Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey]._ChildrenNodes.Keys)
                                {


                                    if (Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey]._ChildrenNodes[cchildkey].NodeType == Node.NodeTypes.Bool) elementType = "D0";
                                    if (Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey]._ChildrenNodes[cchildkey].NodeType == Node.NodeTypes.Int) elementType = "D1";
                                    if (Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey]._ChildrenNodes[cchildkey].NodeType == Node.NodeTypes.Single) elementType = "D2";
                                    if (Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey]._ChildrenNodes[cchildkey].NodeType == Node.NodeTypes.NodeList) elementType = "C1";


                                    xr.WriteStartElement(elementType);
                                    xr.WriteAttributeString("Name", cchildkey);
                                    xr.WriteAttributeString("StringValue", Attributes[OrderedAttributes[a]]._ChildrenNodes[childkey]._ChildrenNodes[cchildkey].StringValue);
                                    xr.WriteEndElement();
                                }
                                xr.WriteEndElement();
                            }
                        }
                        
                        xr.WriteEndElement();
                    }
                }
            }

            xr.WriteEndElement();
            return xr;
        }

        public Dictionary<string, object> GetAsValues() //use this for the usercontrol camera
        {
            Dictionary<string, object> Ret = new Dictionary<string, object>();
            Ret.Add("Category", Category);
            Ret.Add("Class", Class);
            Ret.Add("UserGroupId", UserGroupId);
            Ret.Add("UserName", UserName);
            foreach (string s in this.Attributes.Keys)
            {
                if (Attributes[s].NodeType == Node.NodeTypes.Bool)
                {
                    Ret.Add(s, Attributes[s].StringValue == "True" ? true : false);
                }else if (Attributes[s].NodeType == Node.NodeTypes.Int)
                {
                    Ret.Add(s, int.Parse(Attributes[s].StringValue));
                }else if (Attributes[s].NodeType == Node.NodeTypes.Single)
                {
                    Ret.Add(s, Single.Parse(Attributes[s].StringValue));
                }
                else if (Attributes[s].NodeType == Node.NodeTypes.NodeList)
                {
                    Dictionary<string, object> list = new Dictionary<string, object>();
                    foreach (string node in Attributes[s]._ChildrenNodes.Keys)
                    {
                        if (Attributes[s]._ChildrenNodes[node].NodeType == Node.NodeTypes.Bool)
                        {
                            list.Add(node, Attributes[s]._ChildrenNodes[node].StringValue == "True" ? true : false);
                        }else if (Attributes[s]._ChildrenNodes[node].NodeType == Node.NodeTypes.Int)
                        {
                            list.Add(node, int.Parse(Attributes[s]._ChildrenNodes[node].StringValue));
                        }else if (Attributes[s]._ChildrenNodes[node].NodeType == Node.NodeTypes.Single)
                        {
                            list.Add(node, Single.Parse(Attributes[s]._ChildrenNodes[node].StringValue));
                        }
                    }
                    Ret.Add(s, list);
                }
            }


            return Ret;
        }
        public Camera3DL Clone()
        {
            Camera3DL cam = new Camera3DL();
            cam.UserGroupId = UserGroupId;
            cam.Category = Category;
            cam.Class = Class;
            cam.UserName = UserName;
            foreach(string s in Attributes.Keys)
            {
                cam.Attributes.Add(s, Attributes[s]);
            }
            return cam;
        }
    }
    public class CameraParams : List<Camera3DL>
    {
        public bool ContainsName(string UserName)
        {
            for (int i = 0; i< Count; i++)
            {
                if (this[i].UserName == UserName)
                {
                    return true;
                }
            }
            return false;
        }
        public bool ContainsId(int UserGroupId)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].UserGroupId == UserGroupId)
                {
                    return true;
                }
            }
            return false;
        }
        public int IndexById(int UserGroupId)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].UserGroupId == UserGroupId)
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetIndex(string UserName, int UserGroupId)
        {
            if (UserName == "EntranceCameraObj") UserName = "Entrance";
            if (UserName.Contains("OutOnly")) UserName = UserName.Substring(0, "OutOnly".Length);
            for (int i = 0; i < Count; i++)
            {
                if (this[i].UserGroupId == UserGroupId && this[i].UserName.Contains(UserName)) //We use contains because most objects use the object and an action for the UserName
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public class MultiTreeView : System.Windows.Forms.TreeView
    {
        //From CodeProject

        #region Selected Node(s) Properties

        public MultiTreeView()
        {
            m_SelectedNodes = new List<TreeNode>();
            base.SelectedNode = null;
        }

        private List<TreeNode> m_SelectedNodes = null;
        public List<TreeNode> SelectedNodes
        {
            get
            {
                return m_SelectedNodes;
            }
            set
            {
                ClearSelectedNodes();
                if (value != null)
                {
                    foreach (TreeNode node in value)
                    {
                        ToggleNode(node, true);
                    }
                }
            }
        }

        // Note we use the new keyword to Hide the native treeview's 
        // SelectedNode property.
        private TreeNode m_SelectedNode;
        public new TreeNode SelectedNode
        {
            get
            {
                return m_SelectedNode;
            }
            set
            {
                ClearSelectedNodes();
                if (value != null)
                {
                    SelectNode(value);
                }
            }
        }

        #endregion

        #region Overridden Events

        protected override void OnGotFocus(EventArgs e)
        {
            // Make sure at least one node has a selection
            // this way we can tab to the ctrl and use the
            // keyboard to select nodes
            try
            {
                if (m_SelectedNode == null && this.TopNode != null)
                {
                    ToggleNode(this.TopNode, true);
                }

                base.OnGotFocus(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // If the user clicks on a node that was not
            // previously selected, select it now.
            try
            {
                base.SelectedNode = null;

                TreeNode node = this.GetNodeAt(e.Location);
                if (node != null)
                {
                    //Allow user to click on image
                    int leftBound = node.Bounds.X; // - 20; 
                                                   // Give a little extra room
                    int rightBound = node.Bounds.Right + 10;
                    if (e.Location.X > leftBound && e.Location.X < rightBound)
                    {
                        if (ModifierKeys ==
                            Keys.None && (m_SelectedNodes.Contains(node)))
                        {
                            // Potential Drag Operation
                            // Let Mouse Up do select
                        }
                        else
                        {
                            SelectNode(node);
                        }
                    }
                }

                base.OnMouseDown(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // If you clicked on a node that WAS previously
            // selected then, reselect it now. This will clear
            // any other selected nodes. e.g. A B C D are selected
            // the user clicks on B, now A C & D are no longer selected.
            try
            {
                // Check to see if a node was clicked on
                TreeNode node = this.GetNodeAt(e.Location);
                if (node != null)
                {
                    if (ModifierKeys == Keys.None && m_SelectedNodes.Contains(node))
                    {
                        // Allow user to click on image
                        int leftBound = node.Bounds.X; // - 20; 
                                                       // Give a little extra room
                        int rightBound = node.Bounds.Right + 10;
                        if (e.Location.X > leftBound && e.Location.X < rightBound)
                        {
                            SelectNode(node);
                        }
                    }
                }

                base.OnMouseUp(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            // If the user drags a node and the node being dragged is NOT
            // selected, then clear the active selection, select the
            // node being dragged and drag it. Otherwise if the node being
            // dragged is selected, drag the entire selection.
            try
            {
                TreeNode node = e.Item as TreeNode;

                if (node != null)
                {
                    if (!m_SelectedNodes.Contains(node))
                    {
                        SelectSingleNode(node);
                        ToggleNode(node, true);
                    }
                }

                base.OnItemDrag(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            // Never allow base.SelectedNode to be set!
            try
            {
                base.SelectedNode = null;
                e.Cancel = true;

                base.OnBeforeSelect(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            // Never allow base.SelectedNode to be set!
            try
            {
                base.OnAfterSelect(e);
                base.SelectedNode = null;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Handle all possible key strokes for the control.
            // including navigation, selection, etc.

            base.OnKeyDown(e);

            if (e.KeyCode == Keys.ShiftKey) return;

            //this.BeginUpdate();
            bool bShift = (ModifierKeys == Keys.Shift);

            try
            {
                // Nothing is selected in the tree, this isn't a good state
                // select the top node
                if (m_SelectedNode == null && this.TopNode != null)
                {
                    ToggleNode(this.TopNode, true);
                }

                // Nothing is still selected in the tree, 
                // this isn't a good state, leave.
                if (m_SelectedNode == null) return;

                if (e.KeyCode == Keys.Left)
                {
                    if (m_SelectedNode.IsExpanded && m_SelectedNode.Nodes.Count > 0)
                    {
                        // Collapse an expanded node that has children
                        m_SelectedNode.Collapse();
                    }
                    else if (m_SelectedNode.Parent != null)
                    {
                        // Node is already collapsed, try to select its parent.
                        SelectSingleNode(m_SelectedNode.Parent);
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (!m_SelectedNode.IsExpanded)
                    {
                        // Expand a collapsed node's children
                        m_SelectedNode.Expand();
                    }
                    else
                    {
                        // Node was already expanded, select the first child
                        SelectSingleNode(m_SelectedNode.FirstNode);
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    // Select the previous node
                    if (m_SelectedNode.PrevVisibleNode != null)
                    {
                        SelectNode(m_SelectedNode.PrevVisibleNode);
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    // Select the next node
                    if (m_SelectedNode.NextVisibleNode != null)
                    {
                        SelectNode(m_SelectedNode.NextVisibleNode);
                    }
                }
                else if (e.KeyCode == Keys.Home)
                {
                    if (bShift)
                    {
                        if (m_SelectedNode.Parent == null)
                        {
                            // Select all of the root nodes up to this point
                            if (this.Nodes.Count > 0)
                            {
                                SelectNode(this.Nodes[0]);
                            }
                        }
                        else
                        {
                            // Select all of the nodes up to this point under 
                            // this nodes parent
                            SelectNode(m_SelectedNode.Parent.FirstNode);
                        }
                    }
                    else
                    {
                        // Select this first node in the tree
                        if (this.Nodes.Count > 0)
                        {
                            SelectSingleNode(this.Nodes[0]);
                        }
                    }
                }
                else if (e.KeyCode == Keys.End)
                {
                    if (bShift)
                    {
                        if (m_SelectedNode.Parent == null)
                        {
                            // Select the last ROOT node in the tree
                            if (this.Nodes.Count > 0)
                            {
                                SelectNode(this.Nodes[this.Nodes.Count - 1]);
                            }
                        }
                        else
                        {
                            // Select the last node in this branch
                            SelectNode(m_SelectedNode.Parent.LastNode);
                        }
                    }
                    else
                    {
                        if (this.Nodes.Count > 0)
                        {
                            // Select the last node visible node in the tree.
                            // Don't expand branches incase the tree is virtual
                            TreeNode ndLast = this.Nodes[0].LastNode;
                            while (ndLast.IsExpanded && (ndLast.LastNode != null))
                            {
                                ndLast = ndLast.LastNode;
                            }
                            SelectSingleNode(ndLast);
                        }
                    }
                }
                else if (e.KeyCode == Keys.PageUp)
                {
                    // Select the highest node in the display
                    int nCount = this.VisibleCount;
                    TreeNode ndCurrent = m_SelectedNode;
                    while ((nCount) > 0 && (ndCurrent.PrevVisibleNode != null))
                    {
                        ndCurrent = ndCurrent.PrevVisibleNode;
                        nCount--;
                    }
                    SelectSingleNode(ndCurrent);
                }
                else if (e.KeyCode == Keys.PageDown)
                {
                    // Select the lowest node in the display
                    int nCount = this.VisibleCount;
                    TreeNode ndCurrent = m_SelectedNode;
                    while ((nCount) > 0 && (ndCurrent.NextVisibleNode != null))
                    {
                        ndCurrent = ndCurrent.NextVisibleNode;
                        nCount--;
                    }
                    SelectSingleNode(ndCurrent);
                }
                else
                {
                    // Assume this is a search character a-z, A-Z, 0-9, etc.
                    // Select the first node after the current node that
                    // starts with this character
                    string sSearch = ((char)e.KeyValue).ToString();

                    TreeNode ndCurrent = m_SelectedNode;
                    while ((ndCurrent.NextVisibleNode != null))
                    {
                        ndCurrent = ndCurrent.NextVisibleNode;
                        if (ndCurrent.Text.StartsWith(sSearch))
                        {
                            SelectSingleNode(ndCurrent);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                this.EndUpdate();
            }
        }

        #endregion

        #region Helper Methods

        private void SelectNode(TreeNode node)
        {
            try
            {
                this.BeginUpdate();

                if (m_SelectedNode == null || ModifierKeys == Keys.Control)
                {
                    // Ctrl+Click selects an unselected node, 
                    // or unselects a selected node.
                    bool bIsSelected = m_SelectedNodes.Contains(node);
                    ToggleNode(node, !bIsSelected);
                }
                else if (ModifierKeys == Keys.Shift)
                {
                    // Shift+Click selects nodes between the selected node and here.
                    TreeNode ndStart = m_SelectedNode;
                    TreeNode ndEnd = node;

                    if (ndStart.Parent == ndEnd.Parent)
                    {
                        // Selected node and clicked node have same parent, easy case.
                        if (ndStart.Index < ndEnd.Index)
                        {
                            // If the selected node is beneath 
                            // the clicked node walk down
                            // selecting each Visible node until we reach the end.
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.NextVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                        else if (ndStart.Index == ndEnd.Index)
                        {
                            // Clicked same node, do nothing
                        }
                        else
                        {
                            // If the selected node is above the clicked node walk up
                            // selecting each Visible node until we reach the end.
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.PrevVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                    }
                    else
                    {
                        // Selected node and clicked node have same parent, hard case.
                        // We need to find a common parent to determine if we need
                        // to walk down selecting, or walk up selecting.

                        TreeNode ndStartP = ndStart;
                        TreeNode ndEndP = ndEnd;
                        int startDepth = Math.Min(ndStartP.Level, ndEndP.Level);

                        // Bring lower node up to common depth
                        while (ndStartP.Level > startDepth)
                        {
                            ndStartP = ndStartP.Parent;
                        }

                        // Bring lower node up to common depth
                        while (ndEndP.Level > startDepth)
                        {
                            ndEndP = ndEndP.Parent;
                        }

                        // Walk up the tree until we find the common parent
                        while (ndStartP.Parent != ndEndP.Parent)
                        {
                            ndStartP = ndStartP.Parent;
                            ndEndP = ndEndP.Parent;
                        }

                        // Select the node
                        if (ndStartP.Index < ndEndP.Index)
                        {
                            // If the selected node is beneath 
                            // the clicked node walk down
                            // selecting each Visible node until we reach the end.
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.NextVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                        else if (ndStartP.Index == ndEndP.Index)
                        {
                            if (ndStart.Level < ndEnd.Level)
                            {
                                while (ndStart != ndEnd)
                                {
                                    ndStart = ndStart.NextVisibleNode;
                                    if (ndStart == null) break;
                                    ToggleNode(ndStart, true);
                                }
                            }
                            else
                            {
                                while (ndStart != ndEnd)
                                {
                                    ndStart = ndStart.PrevVisibleNode;
                                    if (ndStart == null) break;
                                    ToggleNode(ndStart, true);
                                }
                            }
                        }
                        else
                        {
                            // If the selected node is above 
                            // the clicked node walk up
                            // selecting each Visible node until we reach the end.
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.PrevVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                    }
                }
                else
                {
                    // Just clicked a node, select it
                    SelectSingleNode(node);
                }

                OnAfterSelect(new TreeViewEventArgs(m_SelectedNode));
            }
            finally
            {
                this.EndUpdate();
            }
        }

        private void ClearSelectedNodes()
        {
            try
            {
                foreach (TreeNode node in m_SelectedNodes)
                {
                    node.BackColor = this.BackColor;
                    node.ForeColor = this.ForeColor;
                }
            }
            finally
            {
                m_SelectedNodes.Clear();
                m_SelectedNode = null;
            }
        }

        private void SelectSingleNode(TreeNode node)
        {
            if (node == null)
            {
                return;
            }

            ClearSelectedNodes();
            ToggleNode(node, true);
            node.EnsureVisible();
        }

        private void ToggleNode(TreeNode node, bool bSelectNode)
        {
            if (bSelectNode)
            {
                m_SelectedNode = node;
                if (!m_SelectedNodes.Contains(node))
                {
                    m_SelectedNodes.Add(node);
                }
                node.BackColor = System.Drawing.Color.FromArgb(System.Windows.SystemColors.HighlightColor.R, System.Windows.SystemColors.HighlightColor.G, System.Windows.SystemColors.HighlightColor.B);
                node.ForeColor = System.Drawing.Color.FromArgb(System.Windows.SystemColors.HighlightTextColor.R, System.Windows.SystemColors.HighlightTextColor.G, System.Windows.SystemColors.HighlightTextColor.B);
            }
            else
            {
                m_SelectedNodes.Remove(node);
                node.BackColor = this.BackColor;
                node.ForeColor = this.ForeColor;
            }
        }

        private void HandleException(Exception ex)
        {
            // Perform some error handling here.
            // We don't want to bubble errors to the CLR.
            MessageBox.Show(ex.Message);
        }

        #endregion

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
