using ModelViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Xml;
using The4Dimension;

namespace The4Dimension.FormEditors
{
    public partial class FrmAddCameraSettings : Form
    {

        string XmlFile;
        int CameraId;
        int TextInsertIndex = -1;
        public List<byte> camparamfilenew;
        public Form1 parent;
        Dictionary<string, string> PropertyTypes;
        public Camera3DL Ret;
        public List<FrmCameraViewer> CamPreviews = new List<FrmCameraViewer>();

        public FrmAddCameraSettings(Camera3DL camera, bool createnewcam = false)
        {
            InitializeComponent();
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Image = SystemIcons.Warning.ToBitmap();
            CategoryCB.SelectedIndex = CategoryCB.Items.IndexOf(camera.Category);
            ClassCB.SelectedIndex = ClassCB.Items.IndexOf(camera.Class);
            numericUpDown1.Value = camera.UserGroupId;
            numericUpDown1.Enabled = !createnewcam;
            textBox1.Text = camera.UserName;
            foreach (string key in camera.Attributes.Keys)
            {
                treeView1.Nodes.Add(key).Name = key;
                treeView1.Nodes[key].Tag = (camera.Attributes[key].StringValue);
                treeView1.Nodes[key].ToolTipText = camera.Attributes[key].NodeType.ToString();
                if (camera.Attributes[key].NodeType == Node.NodeTypes.NodeList)
                {
                    
                    foreach (string child in camera.Attributes[key]._ChildrenNodes.Keys)
                    {

                        treeView1.Nodes[key].Nodes.Add(child).Name = child;
                        treeView1.Nodes[key].Nodes[child].Tag = (camera.Attributes[key]._ChildrenNodes[child].StringValue);
                        treeView1.Nodes[key].Nodes[child].ToolTipText = camera.Attributes[key]._ChildrenNodes[child].NodeType.ToString();
                        if (camera.Attributes[key]._ChildrenNodes[child].NodeType == Node.NodeTypes.NodeList)
                        {
                            foreach (string cchild in camera.Attributes[key]._ChildrenNodes[child]._ChildrenNodes.Keys)
                            {

                                treeView1.Nodes[key].Nodes[child].Nodes.Add(cchild).Name = cchild;
                                treeView1.Nodes[key].Nodes[child].Nodes[cchild].Tag = (camera.Attributes[key]._ChildrenNodes[child]._ChildrenNodes[cchild].StringValue);
                                treeView1.Nodes[key].Nodes[child].Nodes[cchild].ToolTipText = camera.Attributes[key]._ChildrenNodes[child]._ChildrenNodes[cchild].NodeType.ToString();
                                
                            }
                        }
                    }
                }
            }
            treeView1.ExpandAll();
            CameraId = camera.UserGroupId;
            //label1.Text = label1.Text + " " + CameraId;

        }

        void recursive(string key, Node cam, bool repeat = false)
        {
            if (repeat)
            {
                if (cam.NodeType == Node.NodeTypes.Int || cam.NodeType == Node.NodeTypes.Single)
                {
                    if (cam.StringValue.Contains("3,402823E+38")) { ((NumericUpDown)Controls[key]).Value = 1000000; }
                    else if (cam.StringValue.Contains("-3,402823E+38")) { ((NumericUpDown)Controls[key]).Value = -1000000; }
                    else
                    {
                        ((NumericUpDown)Controls[key]).Value = (decimal)((int)float.Parse(cam.StringValue));
                    }
                }
                else if (cam.NodeType == Node.NodeTypes.Bool)
                {
                    ((CheckBox)Controls[key]).Checked = cam.StringValue == "True" ? true : false;
                }
            }
            else
            {
                if (cam.NodeType == Node.NodeTypes.NodeList)
                {
                    foreach (string tmpkey in cam._ChildrenNodes.Keys)
                    {
                        recursive(tmpkey, cam, true);
                    }
                }
                else if(cam.NodeType == Node.NodeTypes.Int || cam.NodeType == Node.NodeTypes.Single)
                {
                    if (cam.StringValue.Contains("3,402823E+38")) { ((NumericUpDown)Controls[key]).Value = 1000000; }
                    else if (cam.StringValue.Contains("-3,402823E+38")) { ((NumericUpDown)Controls[key]).Value = -1000000; }
                    else
                    {
                        ((NumericUpDown)Controls[key]).Value = (decimal)((int)float.Parse(cam.StringValue));
                    }
                }
                else if (cam.NodeType == Node.NodeTypes.Bool)
                {
                    ((CheckBox)Controls[key]).Checked = cam.StringValue == "True" ? true : false;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ret = null;
            this.Close();
        }

        private void FrmAddCameraSettings_Load(object sender, EventArgs e)
        {
            string setlng;
            if (Properties.Settings.Default.DotComma != true){ setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            /*TextInsertIndex = XmlFile.IndexOf("<C0 Name=\"CameraParams\">");
            if (TextInsertIndex == -1)
            {
                MessageBox.Show("Failed to get CameraParams node position !");
                this.Close();
            }
            TextInsertIndex += "<C0 Name=\"CameraParams\">".Length;*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ret = new Camera3DL();
            Ret.Category = CategoryCB.Text;
            Ret.Class = ClassCB.Text;
            Ret.UserGroupId = (int)numericUpDown1.Value;
            Ret.UserName = textBox1.Text;
            foreach (TreeNode Node in treeView1.Nodes)
            {

                Node temp;
                if (Node.ToolTipText == "Single")
                {
                    temp = new Node((string)Node.Tag, "D2");
                }
                else if (Node.ToolTipText == "Int")
                {
                    temp = new Node((string)Node.Tag, "D1");
                }
                else if (Node.ToolTipText == "Bool")
                {
                    temp = new Node((string)Node.Tag, "D0");
                }
                else if (Node.ToolTipText == "NodeList")
                {
                    Dictionary<string, Node> templist = ProcessChildren(Node);
                    //process children first then add to list then add 
                    temp = new Node(templist, "C1");
                }
                else
                {
                    temp = null;
                }
                Ret.Attributes.Add(Node.Name, temp);
            }
            this.Close();
        }

        private Dictionary<string, Node> ProcessChildren(TreeNode node)
        {
            Dictionary<string, Node> ret = new Dictionary<string, Node>();
            foreach (TreeNode CNode in node.Nodes)
            {
                Node temp;
                if (CNode.ToolTipText == "Single")
                {
                    temp = new Node((string)CNode.Tag, "D2");
                }
                else if (CNode.ToolTipText == "Int")
                {
                    temp = new Node((string)CNode.Tag, "D1");
                }
                else if (CNode.ToolTipText == "Bool")
                {
                    temp = new Node((string)CNode.Tag, "D0");
                }
                else// if (Node.ToolTipText == "NodeList")
                {
                    Dictionary<string, Node> templist = ProcessChildren(CNode);

                    temp = new Node(templist, "C1");
                }
                ret.Add(CNode.Name, temp);
            }
            return ret;
        }

        private void ClassCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> propertiesPerClass = new List<string>() { "AngleH", "AngleV", "DashAngleTuner", "Distance", "InterpoleFrame", "IsLimitAngleFix", "Rotator","SideDegree", "SideOffset", "UpOffset","VelocityOffsetter", "VerticalAbsorber","VisionParam", "IsCalcStartPosUseLookAtPos", "IsDistanceFix"};
            if (ClassCB.Text.Contains("Target"))
            {
                if (ClassCB.Text.Contains("Demo"))
                {
                    propertiesPerClass.AddRange(new string[] {"CameraOffset" });
                }
                else
                {
                    propertiesPerClass.AddRange(new string[] { "CameraOffset" });
                }
            }
            else if (ClassCB.Text.Contains("Versus"))
            {
                propertiesPerClass.AddRange(new string[] { "DistanceMax", "DistanceMin", "FovyVersus" });
            }
            else if (ClassCB.Text.Contains("Tower"))
            {
                propertiesPerClass.AddRange(new string[] { "LimitYMax", "LimitYMin", "Position" });
                //XYZ.Text = "Rotation center position:";
            }
            else if (ClassCB.Text.Contains("Rail"))
            {
                propertiesPerClass.AddRange(new string[] { "RailId" });

            }
            else if (ClassCB.Text.Contains("Spot"))
            {

            }
            else if (ClassCB.Text.Contains("All"))
            {
                propertiesPerClass.AddRange(new string[] { "CameraPos", "LookAtPos" });
               // XYZ.Text = "Camera position:";
            }
            else if (ClassCB.Text.Contains("Pos"))
            {

                propertiesPerClass.AddRange(new string[] { "CameraPos" });
                //XYZ.Text = "Camera position:";
            }
            else if (ClassCB.Text.Contains("Parallel"))
            {
                propertiesPerClass.AddRange(new string[] { "LimitBoxMax", "LimitBoxMin" });
                //XYZ.Text = "Box 1st vertex pos:";
            }
            else if (ClassCB.Text.Contains("Follow"))
            {
                propertiesPerClass.AddRange(new string[] { "LimitBoxMax", "LimitBoxMin", "HighAngle", "LowAngle", "PullDistance", "PushDistance"});

            }
            else if (ClassCB.Text.Contains("Anim"))
            {
                //propertiesPerClass.AddRange(new string[] {  });

            }
            PropsCB.Items.Clear();

            pictureBox1.Visible = false;
            toolTip1.SetToolTip(pictureBox1, "");
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (!propertiesPerClass.Contains(node.Text))
                {
                    pictureBox1.Visible = true;
                    toolTip1.SetToolTip(pictureBox1, "Some of the properties given to this camera may not work with the selected class");
                    break;
                }
            }
            PropsCB.Items.AddRange(propertiesPerClass.ToArray());
        }

        private void PositionCamera(object sender, EventArgs e)
        {
            Camera3DL CamPos = new Camera3DL();
            CamPos.Category = CategoryCB.Text;
            CamPos.Class = ClassCB.Text;
            CamPos.UserGroupId = (int)numericUpDown1.Value;
            CamPos.UserName = textBox1.Text;
            foreach (TreeNode Node in treeView1.Nodes)
            {

                Node temp;
                if (Node.ToolTipText == "Single")
                {
                    temp = new Node((string)Node.Tag, "D2");
                }
                else if (Node.ToolTipText == "Int")
                {
                    temp = new Node((string)Node.Tag, "D1");
                }
                else if (Node.ToolTipText == "Bool")
                {
                    temp = new Node((string)Node.Tag, "D0");
                }
                else if (Node.ToolTipText == "NodeList")
                {
                    Dictionary<string, Node> templist = ProcessChildren(Node);
                    //process children first then add to list then add 
                    temp = new Node(templist, "C1");
                }
                else
                {
                    temp = null;
                }
                CamPos.Attributes.Add(Node.Name, temp);
            }
            FrmCameraViewer camview = new FrmCameraViewer(parent.render);
            CamPreviews.Add(camview);
            //camview.render = control1;
            //parent.render.CameraFrom3DLCamera(CamPos.GetAsValues());
            camview.Show();
            camview.GameCamToViewport(CamPos);
        }

        private void ViewShow(string type, string propname = "")
        {

            PropertyName.Text = propname;
            if (type == "Single")
            {
                PropertyName.Text = propname;
                SingleProp.Visible = true;
                SingleProp.Enabled = true;
                ExpProp.Visible = true; ExpProp.Enabled = true;
                ExpLabel.Visible = true;

            }else
            {
                ExpLabel.Visible = false;
                SingleProp.Visible = false;
                SingleProp.Enabled = false;
                ExpProp.Visible = false; ExpProp.Enabled = false;
            }
            if (type == "Int")
            {
                IntProp.Enabled = true; IntProp.Visible = true;
            }
            else
            {
                IntProp.Enabled = false; IntProp.Visible = false;
            }
            if (type == "Bool")
            {
                BoolProp.Enabled = true; BoolProp.Visible = true;
            }
            else
            {
                BoolProp.Enabled = false; BoolProp.Visible = false;
            }
            if (type == "NodeList")
            {
                button3.Enabled = true; button3.Visible = true;
                ChildNodesCB.Enabled = true; ChildNodesCB.Visible = true;
                ChildNodesCB.Items.Clear();
                string[] children = new string[0];
                if (propname == "DashAngleTuner")
                {
                    children = new string[] { "AddAngleMax", "ZoomOutOffsetMax" };
                }
                else if (propname == "LimitBoxMax" || propname == "LimitBoxMin" || propname == "CameraPos" || propname == "LookAtPos" || propname == "Position")
                {
                    children = new string[] { "X", "Y","Z" };
                }
                else if (propname == "VelocityOffsetter")
                {
                    children = new string[] { "MaxOffset", "MaxOffsetAxisTwo" };
                }
                else if (propname == "VerticalAbsorber")
                {
                    children = new string[] { "IsInvalidate" };
                }
                else if (propname == "MaxOffsetAxisTwo")
                {
                    children = new string[] { "X", "Y" };
                }
                else if (propname == "Rotator")
                {
                    children = new string[] { "AngleMax", "IsEnable" };
                }
                else if (propname == "VisionParam")
                {
                    children = new string[] { "FarClipDistance", "FovyDegree", "NearClipDistacne", "StereovisionDepth", "StereovisionDistance" };
                }
                ChildNodesCB.Items.AddRange(children);

            }
            else
            {
                button3.Enabled = false; button3.Visible = false;
                ChildNodesCB.Enabled = false; ChildNodesCB.Visible = false;
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ViewShow(treeView1.SelectedNode.ToolTipText, treeView1.SelectedNode.Name);
            label5.Visible = true;
            if (treeView1.SelectedNode.ToolTipText == "Bool")
            {
                BoolProp.SelectedIndex = BoolProp.Items.IndexOf(((string)treeView1.SelectedNode.Tag));
            }
            else if (treeView1.SelectedNode.ToolTipText == "Int")
            {
                IntProp.Value = int.Parse(((string)treeView1.SelectedNode.Tag));
            }
            else if (treeView1.SelectedNode.ToolTipText == "Single")
            {
                if (((string)treeView1.SelectedNode.Tag).Contains("E"))
                {
                    string dec = ((string)treeView1.SelectedNode.Tag).Substring(0, ((string)treeView1.SelectedNode.Tag).IndexOf("E"));
                    string exp = ((string)treeView1.SelectedNode.Tag).Substring(((string)treeView1.SelectedNode.Tag).IndexOf("E") + 1);
                    ExpProp.Value = decimal.Parse(exp);
                    SingleProp.Value = decimal.Parse(dec);
                }
                else
                {
                    ExpProp.Value = 0;
                    SingleProp.Value = (decimal)Single.Parse(((string)treeView1.SelectedNode.Tag));
                }
            }
            else if (treeView1.SelectedNode.ToolTipText == "NodeList")
            {
                ChildNodesCB.SelectedIndex = 0;
            }
            else
            {
                label5.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.ToolTipText == "NodeList")
            {
                // treeView1.SelectedNode ChildNodesCB
                if (ChildNodesCB.SelectedIndex == -1) { MessageBox.Show("Select a property to add first!"); return; }
                if (treeView1.SelectedNode.Nodes.ContainsKey(ChildNodesCB.Text)) { MessageBox.Show("The selected property is already in this object!"); return; }

                treeView1.SelectedNode.Nodes.Add(ChildNodesCB.Text).Name = ChildNodesCB.Text;
                treeView1.SelectedNode.Expand();
                // if it's a specific property we give it default properties
                // this needs to change but will stay like this for now
                string a = ChildNodesCB.Text;
                if (a == "DashAngleTuner" || a == "LimitBoxMax" || a == "LimitBoxMin" || a == "Rotator" || a == "VelocityOffsetter" || a == "VerticalAbsorber" || a == "VisionParam" || a == "CameraPos" || a == "LookAtPos" || a == "MaxOffsetAxisTwo" || a == "Position")
                {
                    treeView1.SelectedNode.Nodes[a].Tag = "0";
                    treeView1.SelectedNode.Nodes[a].ToolTipText = "NodeList";
                }
                else if (a == "IsLimitAngleFix" || a == "IsInvalidate" || a == "IsEnable" || a == "IsDistanceFix" || a == "IsCalcStartPosUseLookAtPos")
                {
                    treeView1.SelectedNode.Nodes[a].Tag = "False";
                    treeView1.SelectedNode.Nodes[a].ToolTipText = "Bool";
                }
                else if (a == "InterpoleFrame" || a == "RailId")
                {
                    treeView1.SelectedNode.Nodes[a].Tag = "0";
                    treeView1.SelectedNode.Nodes[a].ToolTipText = "Int";
                }
                else
                {
                    treeView1.SelectedNode.Nodes[a].Tag = "0";
                    treeView1.SelectedNode.Nodes[a].ToolTipText = "Single";
                }
            }
            else
            {
                if (treeView1.SelectedNode.ToolTipText == "Single")
                {
                    if (ExpProp.Value != 0)
                    {
                        treeView1.SelectedNode.Tag = SingleProp.Value.ToString() + "E" +( (ExpProp.Value>0) ? ("+"+ExpProp.Value.ToString()) : ExpProp.Value.ToString());
                    }
                    else
                    {
                        treeView1.SelectedNode.Tag = SingleProp.Value.ToString();
                    }
                }
                else if (treeView1.SelectedNode.ToolTipText == "Int")
                {

                    treeView1.SelectedNode.Tag = IntProp.Value.ToString();
                }else if (treeView1.SelectedNode.ToolTipText == "Bool")
                {

                    treeView1.SelectedNode.Tag = BoolProp.Text;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (PropsCB.SelectedIndex == -1) { MessageBox.Show("Select a property to add first!"); return; }
            if (treeView1.Nodes.ContainsKey(PropsCB.Text)) { MessageBox.Show("The selected property is already in this object!"); return; }
            treeView1.Nodes.Add(PropsCB.Text).Name = PropsCB.Text;
            // if it's a specific property we give it default properties
            // this needs to change but will stay like this for now
            string a = PropsCB.Text;
            if (a == "DashAngleTuner" || a == "LimitBoxMax" || a == "LimitBoxMin" || a == "Rotator" || a == "VelocityOffsetter" || a == "VerticalAbsorber" || a == "VisionParam" || a == "CameraPos" || a == "LookAtPos" || a == "MaxOffsetAxisTwo" || a == "Position")
            {
                treeView1.Nodes[a].ToolTipText = "NodeList";
                if (a == "LimitBoxMax" || a == "LimitBoxMin" || a == "CameraPos" || a == "LookAtPos" || a == "Position")
                {
                    treeView1.Nodes[a].Nodes.Add("X").Name = "X"; treeView1.Nodes[a].Nodes["X"].Tag = 0.ToString(); treeView1.Nodes[a].Nodes["X"].ToolTipText = "Single";
                    treeView1.Nodes[a].Nodes.Add("Y").Name = "Y"; treeView1.Nodes[a].Nodes["Y"].Tag = 0.ToString(); treeView1.Nodes[a].Nodes["Y"].ToolTipText = "Single";
                    treeView1.Nodes[a].Nodes.Add("Z").Name = "Z"; treeView1.Nodes[a].Nodes["Z"].Tag = 0.ToString(); treeView1.Nodes[a].Nodes["Z"].ToolTipText = "Single";
                }
                else if (a == "MaxOffsetAxisTwo")
                {
                    treeView1.Nodes[a].Nodes.Add("X").Name = "X"; treeView1.Nodes[a].Nodes["X"].Tag = 0.ToString(); treeView1.Nodes[a].Nodes["X"].ToolTipText = "Single";
                    treeView1.Nodes[a].Nodes.Add("Y").Name = "Y"; treeView1.Nodes[a].Nodes["Y"].Tag = 0.ToString(); treeView1.Nodes[a].Nodes["Y"].ToolTipText = "Single";
                }
                treeView1.Nodes[a].Expand();
            }
            else if (a == "IsLimitAngleFix" || a == "IsInvalidate" || a == "IsEnable" || a == "IsDistanceFix" || a== "IsCalcStartPosUseLookAtPos")
            {
                treeView1.Nodes[a].Tag = "False";
                treeView1.Nodes[a].ToolTipText = "Bool";
            }
            else if (a == "InterpoleFrame" || a == "AngleMax" || a == "RailId")
            {
                treeView1.Nodes[a].Tag = "0";
                treeView1.Nodes[a].ToolTipText = "Int";
            }
            else
            {
                treeView1.Nodes[a].Tag = "0";
                treeView1.Nodes[a].ToolTipText = "Single";
            }


        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            treeView1.SelectedNode.Remove();
            treeView1.Refresh();
        }

        private void SingleProp_Validated(object sender, EventArgs e)
        {
            if (ExpProp.Value != 0)
            {
                treeView1.SelectedNode.Tag = SingleProp.Value.ToString() + "E" + ((ExpProp.Value > 0) ? ("+" + ExpProp.Value.ToString()) : ExpProp.Value.ToString());
            }
            else
            {
                treeView1.SelectedNode.Tag = SingleProp.Value.ToString();
            }
        }

        private void IntProp_Validated(object sender, EventArgs e)
        {
            treeView1.SelectedNode.Tag = IntProp.Value.ToString();
        }

        private void ExpProp_Validated(object sender, EventArgs e)
        {
            if (ExpProp.Value != 0)
            {
                treeView1.SelectedNode.Tag = SingleProp.Value.ToString() + "E" + ((ExpProp.Value > 0) ? ("+" + ExpProp.Value.ToString()) : ExpProp.Value.ToString());
            }
            else
            {
                treeView1.SelectedNode.Tag = SingleProp.Value.ToString();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeView1.SelectedNode.Tag = BoolProp.Text;
        }

        private void SingleProp_ValueChanged(object sender, EventArgs e)
        {
            if (ExpProp.Value == 38)
            {
                if (SingleProp.Value > 3.402823M)
                {
                    SingleProp.Value = 3.402823M;
                }
                else if (SingleProp.Value < -3.402823M)
                {
                    SingleProp.Value = 3.402823M;
                }
                SingleProp.Maximum = 3.402823M;
                SingleProp.Minimum = -3.402823M;
            }
            else
            {
                SingleProp.Maximum = 1000000;
                SingleProp.Minimum = -1000000;
            }
        }
    }
}