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
using System.Xml;
using The4Dimension;

namespace The4Dimension.FormEditors
{
    public partial class FrmAddCameraSettings : Form
    {

        string XmlFile;
        int CameraId;
        int TextInsertIndex = -1;
        Form1 owner;
        private Dictionary<string, string> strings;
        public FrmAddCameraSettings(string xml, int camId, Form1 own)
        {
            strings = new Dictionary<string, string>();
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
                            case "AddCam":
                                CForm = "FrmAddCameraSettings";
                                break;
                            default:
                                break;
                        }
                    }
                    if (LANG.NodeType == XmlNodeType.EndElement)
                    {
                        switch (LANG.Name)
                        {
                            case "AddCam":
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
                                    strings.Add(var,text);
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
            else 
            {
                strings.Add("pointpos", "Point position:");
                strings.Add("campos", "Camera position:");
                strings.Add("boxV1", "Box 1st vertex pos:");
                strings.Add("boxV2", "Box 2nd vertex pos:");
                strings.Add("unused", "Unused:");
                strings.Add("rotcent", "Rotation center position:");
            }
            #endregion

            XmlFile = xml;
            CameraId = camId;
            label1.Text = label1.Text + " " + camId.ToString();
            owner = own;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmAddCameraSettings_Load(object sender, EventArgs e)
        {
            string setlng;
            if (Properties.Settings.Default.DotComma != true){ setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            TextInsertIndex = XmlFile.IndexOf("<C0 Name=\"CameraParams\">");
            if (TextInsertIndex == -1)
            {
                MessageBox.Show("Failed to get CameraParams node position !");
                this.Close();
            }
            TextInsertIndex += "<C0 Name=\"CameraParams\">".Length;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            string type;
            if (ChckBxRailCam.Checked != true)
            {
                if (ChckBxFixCam.Checked == true)
                {
                    switch (FixedTypeBox.SelectedIndex)
                    {
                        case 0:
                            type = "FixAll";
                            break;
                        case 1:
                            type = "Parallel";
                            break;
                        case 2:
                            type = "FixPos";
                            break;
                        case 3:
                            type = "Tower";
                            break;
                        default:
                            type = "Parallel";
                            break;
                    }
                }
                else {type = "Parallel"; }
            }
            else {  type = "Rail"; }
            string str = "\r\n";
            str += "<C1>\r\n";
            str += "<D2 Name=\"AngleH\" StringValue=\"" + numericUpDown3.Value.ToString() + "\" />\r\n";
            str += "<D2 Name=\"AngleV\" StringValue=\"" + numericUpDown2.Value.ToString() + "\" />\r\n";
            if (ChckBxFixCam.Checked == true)
            {
                switch (FixedTypeBox.SelectedIndex)
                {
                    case 0:
                        str += "<C1 Name=\"CameraPos\">\r\n          <D2 Name=\"X\" StringValue=\"" + FixX.Value.ToString() + "\" />\r\n          <D2 Name=\"Y\" StringValue=\"" + FixY.Value.ToString() + "\" />\r\n          <D2 Name=\"Z\" StringValue=\"" + FixZ.Value.ToString() + "\" />\r\n        </C1>\r\n";
                        break;
                    case 2:
                        str += "<C1 Name=\"CameraPos\">\r\n          <D2 Name=\"X\" StringValue=\"" + FixX.Value.ToString() + "\" />\r\n          <D2 Name=\"Y\" StringValue=\"" + FixY.Value.ToString() + "\" />\r\n          <D2 Name=\"Z\" StringValue=\"" + FixZ.Value.ToString() + "\" />\r\n        </C1>\r\n";
                        break;
                    default:
                        break;
                }
            }

            str += "<A0 Name=\"Category\" StringValue=\"Map\" />\r\n<A0 Name=\"Class\" StringValue=\""+ type +"\"/>\r\n";
            str += "<C1 Name=\"DashAngleTuner\">\r\n          <D2 Name=\"AddAngleMax\" StringValue=\""+numericUpDown5.Value.ToString() + "\" />\r\n          <D2 Name=\"ZoomOutOffsetMax\" StringValue=\""+numericUpDown6.Value.ToString()+"\" />\r\n        </C1>\r\n";
            str += "<D2 Name=\"Distance\" StringValue=\"" + numericUpDown4.Value.ToString() + "\" />\r\n";
            if (ChckBxRailCam.Checked == true)
            {
                str += "<D0 Name = \"IsCalcStartPosUseLookAtPos\" StringValue=\"True\" />\r\n        <D1 Name=\"RailId\" StringValue=\"" + ((RailId.Value).ToString()) + "\" />\r\n";
            }
            if (ChckBxFixCam.Checked == true) 
            {
                switch (FixedTypeBox.SelectedIndex)
                {
                    case 0:
                        str += "<C1 Name=\"LookAtPos\">\r\n          <D2 Name=\"X\" StringValue=\"" + FixX2.Value.ToString() + "\" />\r\n          <D2 Name=\"Y\" StringValue=\"" + FixY2.Value.ToString() + "\" />\r\n          <D2 Name=\"Z\" StringValue=\"" + FixZ2.Value.ToString() + "\" />\r\n        </C1>\r\n";
                        break;
                    case 1:
                        str += "<D0 Name=\"IsLimitAngleFix\" StringValue=\"True\" />\r\n";
                        str += "<C1 Name=\"LimitBoxMax\">\r\n          <D2 Name=\"X\" StringValue=\"" + FixX.Value.ToString() + "\" />\r\n          <D2 Name=\"Y\" StringValue=\"" + FixY.Value.ToString() + "\" />\r\n          <D2 Name=\"Z\" StringValue=\"" + FixZ.Value.ToString() + "\" />\r\n        </C1>\r\n";
                        str += "<C1 Name=\"LimitBoxMin\">\r\n          <D2 Name=\"X\" StringValue=\"" + FixX2.Value.ToString() + "\" />\r\n          <D2 Name=\"Y\" StringValue=\"" + FixY2.Value.ToString() + "\" />\r\n          <D2 Name=\"Z\" StringValue=\"" + FixZ2.Value.ToString() + "\" />\r\n        </C1>\r\n";
                        break;
                    case 2:
                        break;
                    case 3:
                        str += "<C1 Name=\"Position\">\r\n          <D2 Name=\"X\" StringValue=\"" + FixX.Value.ToString() + "\" />\r\n          <D2 Name=\"Y\" StringValue=\"" + FixY.Value.ToString() + "\" />\r\n          <D2 Name=\"Z\" StringValue=\"" + FixZ.Value.ToString() + "\" />\r\n        </C1>\r\n";
                        break;
                    default:
                        break;
                }
            }
            str += "<C1 Name=\"Rotator\">\r\n          <D2 Name=\"AngleMax\" StringValue=\""+numericUpDown1.Value.ToString()+"\" />\r\n        </C1>\r\n";
            str += "<D1 Name=\"UserGroupId\" StringValue=\"" + CameraId.ToString() + "\" />\r\n";
            str += "<A0 Name=\"UserName\" StringValue=\"CameraArea\" />\r\n</C1>\r\n";
            XmlFile = XmlFile.Insert(TextInsertIndex,str);
            owner.SzsFiles["CameraParam.byml"] = BymlConverter.GetByml(XmlFile);
            this.Close();
        }

        private void ChckBxRailCam_CheckedChanged(object sender, EventArgs e)
        {

            RailId.Maximum = owner.AllRailInfos.Count;
            if (ChckBxRailCam.Checked == true)
            {

                    ChckBxFixCam.Checked = false;
                    //ChckBxFixCam.Enabled = false;
                RailId.Enabled = true;
            }
            else
            {
                ChckBxFixCam.Enabled = true;
                RailId.Enabled = false;
            }

        }
        private void ChckBxFixCam_CheckedChanged(object sender, EventArgs e)
        {
            if (ChckBxFixCam.Checked == true)
            {

                ChckBxRailCam.Checked = false;
                FixedTypeBox.SelectedIndex = 0;
                //ChckBxRailCam.Enabled = false;
                FixedTypeBox.Enabled = true;
                FixX.Enabled = true;
                FixY.Enabled = true;
                FixZ.Enabled = true;
                FixX2.Enabled = true;
                FixY2.Enabled = true;
                FixZ2.Enabled = true;
            }
            else
            {
                ChckBxRailCam.Enabled = true;
                FixedTypeBox.Enabled = false;
                FixX.Enabled = false;
                FixY.Enabled = false;
                FixZ.Enabled = false;    
                FixX2.Enabled = false;
                FixY2.Enabled = false;
                FixZ2.Enabled = false;
            }
        }

        private void ShowHideXYZ2(object sender, EventArgs e)
        {
            switch (FixedTypeBox.SelectedIndex)
            {
                case 0://first label is for CamPos, second for Point position (LookAtPos) label9 and label11
                    label9.Text = strings["campos"];
                    label11.Text = strings["pointpos"];
                    FixX2.Enabled = true;
                    FixY2.Enabled = true;
                    FixZ2.Enabled = true;
                    break;
                case 1://first label is for CamPos, second for Point position (LookAtPos) label9 and label11
                    label9.Text = strings["boxV1"];
                    label11.Text = strings["boxV2"];
                    FixX2.Enabled = true;
                    FixY2.Enabled = true;
                    FixZ2.Enabled = true;
                    break;
                case 2://first label is for CamPos, second for Point position (LookAtPos) label9 and label11
                    label9.Text = strings["campos"];
                    label11.Text = strings["unused"];
                    FixX2.Enabled = false;
                    FixY2.Enabled = false;
                    FixZ2.Enabled = false;
                    break;
                case 3://first label is for CamPos, second for Point position (LookAtPos) label9 and label11
                    label9.Text = strings["rotcent"];
                    label11.Text = strings["unused"];
                    FixX2.Enabled = false;
                    FixY2.Enabled = false;
                    FixZ2.Enabled = false;
                    break;
                default:
                    FixedTypeBox.SelectedIndex = 0;
                    break;
            }
        }
    }
}