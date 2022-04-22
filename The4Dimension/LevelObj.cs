using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Media.Media3D;

namespace The4Dimension
{
    public class LevelObj : ICloneable
    {
        public Dictionary<string, object> Prop = new Dictionary<string, object>();
        public override string ToString()
        {
            if (Prop == null)
            {
                Prop = new Dictionary<string, object>();
                return "Unknown name LevelObj";
            }
            if (Prop.ContainsKey("name"))
            {
                return Prop["name"].ToString().Substring("String : ".Length);
            }
            else
            {
                if (Prop.ContainsKey("l_id")) return "LevelObj id: " + Prop["l_id"].ToString();
                else return "LevelObj";
            }
        }
        public string GetName(bool dbname = false)
        {
            if (Prop == null)
            {
                Prop = new Dictionary<string, object>();
                return "Unknown name LevelObj";
            }
            if (dbname)
            {
                if (Prop.ContainsKey("dbname"))
                {
                    return Prop["dbname"].ToString();
                }
                else
                {
                    if (Prop.ContainsKey("name")) return Prop["name"].ToString().Substring("String : ".Length);
                    else return "LevelObj";
                }
            }
            else
            {
                if (Prop.ContainsKey("name"))
                {
                    return Prop["name"].ToString().Substring("String : ".Length);
                }
                else
                {
                    if (Prop.ContainsKey("l_id")) return "LevelObj id: " + Prop["l_id"].ToString();
                    else return "LevelObj";
                }
            }

        }

        public LevelObj Clone()
        {
            LevelObj o = new LevelObj();
            foreach (string k in Prop.Keys.ToArray())
            {
                if (Prop[k] is ICloneable) o.Prop.Add(k, ((ICloneable)Prop[k]).Clone());
                else
                    if (Prop[k] is int) { o.Prop.Add(k, Prop[k]); }
                else if (Prop[k] is Single[]) { o.Prop.Add(k, new List<Single>(((Single[])Prop[k])).ToArray()); }
                else if (k.Contains("Children")) { o.Prop.Add(k, Prop[k]); }
                else throw new Exception("Type non cloneable");
            }
            return o;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        [Editor(typeof(LevelObjEditor), typeof(UITypeEditor))]
        [Description("This contains every property of this object")]
        public Dictionary<string, object> ObjectData
        {
            get { return Prop; }
            set { Prop = value; }
        }
    }

    class LevelObjEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            Dictionary<string, object> v = value as Dictionary<string, object>;
            if (svc != null && v != null)
            {
                using (FrmObjEditor form = new FrmObjEditor(v))
                {
                    form.ShowDialog();
                    v = form.Value.Prop;
                }
            }
            return v; // can also replace the wrapper object here
        }
    }


    [Editor(typeof(C0ListEditor), typeof(UITypeEditor))]
    public class C0List : ICloneable
    {
        List<LevelObj> l = new List<LevelObj>();
        public List<LevelObj> List
        {
            get { return l; }
            set { l = value; }
        }

        public C0List Clone()
        {
            C0List C = new C0List();
            foreach (LevelObj lev in l) C.l.Add(lev.Clone());
            return C;
        }

        public override string ToString()
        {
            return "C0 list";
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }

    class C0ListEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            C0List v = value as C0List;
            if (svc != null && v != null)
            {
                ((Form1)System.Windows.Forms.Application.OpenForms["Form1"]).EditC0List(v.List);
            }
            return v;
        }
    }


    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Rail : ICloneable
    {
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        public List<Point3D[]> GetPointArray()
        {
            List<Point3D[]> points = new List<Point3D[]>();
            foreach (Rail.Point p in Points) 
            {
                points.Add(new Point3D[] { new Point3D(p._X[0], -p._Z[0], p._Y[0]), new Point3D(p._X[1], -p._Z[1], p._Y[1]), new Point3D(p._X[2], -p._Z[2], p._Y[2]) }); 
            }
            return points;
        }

        List<int> _Args = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1};
        string _LayerName;
        internal List<Point> _points = new List<Point>();
        //Multi file name ?
        public string _closed;
        int _l_id;
        string _name;
        int _no;
        string _type;

        public Rail(bool Adding = false, Vector3D? BasePosition = null)
        {
            _LayerName = "共通";
            _closed = "FALSE";
            _name = "empty rail";
            _type = "Bezier";
            if (Adding) _points.Add(new Point());
            if (Adding) _points.Add(new Point(1));
            if (BasePosition != null)
            {
                _points[0].setX = (float)((Vector3D)BasePosition).X;
                _points[0].setY = (float)((Vector3D)BasePosition).Z;
                _points[0].setZ = -(float)((Vector3D)BasePosition).Y;
                _points[1].setX = (float)((Vector3D)BasePosition).X;
                _points[1].setY = (float)((Vector3D)BasePosition).Z;
                _points[1].setZ = -(float)((Vector3D)BasePosition).Y;
            }
        }

        public override string ToString()
        {
            return _name;
        }

        public string Name
        {
            set
            {
                _name = JapChars(value);
            }
            get { return _name; }
        }

        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }

        public bool Closed
        {
            set { _closed = value == false ? "OPEN" : "CLOSE"; }
            get { return _closed == "OPEN" ? false : true; }
        }

        public int no
        {
            set { _no = value; }
            get { return _no; }
        }

        public int l_id
        {
            set { _l_id = value; }
            get { return _l_id; }
        }        

        public List<int> Arg
        {
            set { _Args = value; }
            get { return _Args; }
        }

        internal string JapChars(string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            Encoding Enc = Encoding.GetEncoding(932);
            return input;//Enc.GetString(bytes);
        }

        public Rail Clone()
        {
            
            Rail R = new Rail();
            for (int i = 0;i < 10; i++)
            {
                if (R.Arg.Count-1 > i && _Args.Count-1 > i)
                {
                    R._Args[i] = _Args[i];
                }
                else
                {
                    if (_Args.Count - 1 > i)
                    {
                        R._Args.Add(_Args[i]);
                    }
                }
            }
            R.LayerName = (string)_LayerName.Clone();
            foreach (Point p in _points) R._points.Add(p.Clone());
            R._closed = (string)_closed.Clone();
            R.l_id = _l_id;
            R.Name = (string)_name.Clone();
            R.no = _no;
            R.Type = (string)_type.Clone();
            return R;
         }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public string LayerName
        {
            set {
                _LayerName = JapChars(value);
            }
            get { return _LayerName; }
        }

        [Editor(typeof(RailPointEditor), typeof(UITypeEditor))]
        public List<Point> Points
        {
            set { _points = value; }
            get { return _points; }
        }

        // [TypeConverter(typeof(ExpandableObjectConverter))]
        public class Point : ICloneable
        {
            List<int> _Args = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1 };
            int _ID;
            public List<Single> _X = new List<Single>() {0,0,0 };
            public List<Single> _Y = new List<Single>() { 0, 0, 0 };
            public List<Single> _Z = new List<Single>() { 0, 0, 0 };

            public Point(int id = 0)
            {
                _ID = id;
            }

            public override string ToString()
            {
                return "Point ID: " + _ID.ToString();
            }

            public Point Clone()
            {
                Point N = new Point();
                N.Args.Clear();
                foreach (int i in _Args) N._Args.Add(i);
                N.ID = _ID;
                N._X.Clear();
                N._Y.Clear();
                N._Z.Clear();
                foreach (int s in _X) N._X.Add(s);
                foreach (int s in _Y) N._Y.Add(s);
                foreach (int s in _Z) N._Z.Add(s);
                return N;
            }

            public Point Clone_increment()
            {
                Point N = new Point();
                N.Args.Clear();
                foreach (int i in _Args) N._Args.Add(i);
                N.ID = _ID + 1;
                N._X.Clear();
                N._Y.Clear();
                N._Z.Clear();
                foreach (int s in _X) N._X.Add(s + 100);
                foreach (int s in _Y) N._Y.Add(s);
                foreach (int s in _Z) N._Z.Add(s);
                return N;
            }

            object ICloneable.Clone()
            {
                return Clone();
            }

            public List<int> Args
            {
                set { _Args = value; }
                get { return _Args; }
            }

            public int ID
            {
                set { _ID = value; }
                get { return _ID; }
            }

            public Single setX
            {
                set { _X[0] = value; _X[1] = value; _X[2] = value; }
            }

            public Single setY
            {
                set { _Y[0] = value; _Y[1] = value; _Y[2] = value; }
            }

            public Single setZ
            {
                set { _Z[0] = value; _Z[1] = value; _Z[2] = value; }
            }

            public Single X
            {
                set { _X[0] = value; }
                get {
                    if (_X.Count == 0) X = 0;
                    return _X[0];
                }
            }
            public Single X_prev
            {

                set { _X[1] = value; }
                get
                {
                    if (_X.Count == 0) X = 0;
                    return _X[1];
                }
            }

            public Single X_next
            {

                set { _X[2] = value; }
                get
                {
                    if (_X.Count == 0) X = 0;
                    return _X[2];
                }
            }

            public Single Y
            {
                set { _Y[0] = value; }
                get
                {
                    if (_Y.Count == 0) Y = 0;
                    return _Y[0];
                }
            }
            public Single Y_prev
            {

                set { _Y[1] = value; }
                get
                {
                    if (_Y.Count == 0) Y = 0;
                    return _Y[1];
                }
            }

            public Single Y_next
            {

                set { _Y[2] = value; }
                get
                {
                    if (_Y.Count == 0) Y = 0;
                    return _Y[2];
                }
            }
            public Single Z
            {
                set { _Z[0] = value; }
                get
                {
                    if (_Z.Count == 0) Z = 0;
                    return _Z[0];
                }
            }
            public Single Z_prev
            {

                set { _Z[1] = value; }
                get
                {
                    if (_Z.Count == 0) Z = 0;
                    return _Z[1];
                }
            }

            public Single Z_next
            {

                set { _Z[2] = value; }
                get
                {
                    if (_Z.Count == 0) Z = 0;
                    return _Z[2];
                }
            }
            public Point3D ToPoint3D(int part = 0)
            {
                Point3D Ret = new Point3D();
                switch (part)
                {
                    default:
                        Ret = new Point3D(X, Y, Z);
                        break;
                }

                return Ret;
            }

        }
    }

    class RailPointEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            List<Rail.Point> v = value as List<Rail.Point>;
            if (svc != null && v != null)
            {
                using (FrmRailPointEditor form = new FrmRailPointEditor(v))
                {
                    form.ShowDialog();
                    v = form.Value;
                }
            }
            return v; // can also replace the wrapper object here
        }
    }


    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Node : ICloneable
    {
        string _StringValue;
        public string _StringNodeType;
        NodeTypes _NodeType;
        public Dictionary<string, Node> _ChildrenNodes;

        [Description("This is the value of this node as a string, change it respecting the type")]
        public string StringValue
        {
            set
            {
                if (_NodeType == NodeTypes.Int) Int32.Parse(value); //Crashes if the value is invalid
                else if (_NodeType == NodeTypes.Single) Single.Parse(value);
                _StringValue = value;
            }
            get { return _StringValue; }
        }
        [Description("The node type can't be changed, it tells what kind of data contains the node")]
        public NodeTypes NodeType
        {
            //set { _NodeType = value; }
            get { return _NodeType; }
        }

        public override string ToString()
        {
            string Prev = "";
            if (_NodeType == NodeTypes.Empty) Prev += "Empty";
            else if (_NodeType == NodeTypes.String) Prev += "String";
            else if (_NodeType == NodeTypes.Int) Prev += "Int";
            else if (_NodeType == NodeTypes.Single) Prev += "Single";
            else if (_NodeType == NodeTypes.Bool) Prev += "Bool";
            else if (_NodeType == NodeTypes.NodeList) return "Parent Node";
            else Prev += string.Format("Unk Type ({0})", _StringNodeType);
            Prev += " : ";
            Prev += _StringValue;
            return Prev;
        }

        public enum NodeTypes
        {
            String = 0xA0,
            Empty = 0xA1,
            Bool = 0xD0,
            Int = 0xD1,
            Single = 0xD2,
            NodeList = 0xC1,
            Other
        }

        string JapChars(string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            Encoding Enc = Encoding.GetEncoding(932);
            return input; //Enc.GetString(bytes);
        }

        public Node(string _stringValue, string _type)
        {
            _NodeType = NodeTypes.Other;
            if (_type == "A0") _NodeType = NodeTypes.String;
            else if (_type == "A1") _NodeType = NodeTypes.Empty;
            else if (_type == "D0") _NodeType = NodeTypes.Bool;
            else if (_type == "D1") _NodeType = NodeTypes.Int;
            else if (_type == "D2") _NodeType = NodeTypes.Single;
            else if (_type == "C1") _NodeType = NodeTypes.NodeList;
            _StringNodeType = _type;
            ApplyValue(_stringValue, _NodeType);
        }

        public Node(Dictionary<string, Node> nodes, string _type)
        {
            _NodeType = NodeTypes.NodeList;
            _ChildrenNodes = new Dictionary<string, Node>();
            _ChildrenNodes = nodes;
            _StringNodeType = null;
        }

        void ApplyValue(string _stringValue, NodeTypes _type)
        {
            _StringValue = _stringValue;
            switch (_type)
            {
                case NodeTypes.String:
                    _StringValue = JapChars(_StringValue);
                    _NodeType = NodeTypes.String;
                    break;
                case NodeTypes.Empty:
                    _NodeType = NodeTypes.Empty;
                    break;
                default:
                    _NodeType = _type;
                    break;
            }
        }

        public Node Clone()
        {
            return new Node(this._StringValue, this._StringNodeType);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public static bool operator ==(Node first, Node second)
        {
            return first.NodeType == second.NodeType && first.StringValue == second.StringValue;
        }

        public static bool operator !=(Node first, Node second)
        {
            return first.NodeType != second.NodeType || first.StringValue != second.StringValue;
        }

        public override bool Equals(object Obj)
        {
            return (Node)Obj == this;
        }

        //Autogenerated
        public override int GetHashCode()
        {
            int hashCode = 1493277489;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_StringValue);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_StringNodeType);
            hashCode = hashCode * -1521134295 + _NodeType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, Node>>.Default.GetHashCode(_ChildrenNodes);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StringValue);
            hashCode = hashCode * -1521134295 + NodeType.GetHashCode();
            return hashCode;
        }
    }
        /*String = 0xA0,
          Empty = 0xA1,
          Bool = 0xD0,
          Int = 0xD1,
          Single = 0xD2,
          */

    }
