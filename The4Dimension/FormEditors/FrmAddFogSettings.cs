using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace The4Dimension.FormEditors
{
    public partial class FrmAddFogSettings : Form
    {
        string XmlFile;
        string XmlP1;
        string XmlP2;
        int FogID;
        int TextInsertIndex = -1;
        int FogScenario = 0; //0 common, 1 2 3 scenario
        public List<byte> fogparamfilenew;
        int startAt = -1;
        bool newfogfile = false;
        public FrmAddFogSettings(string Base, int ID, int scenario, bool noareas = false)
        {
            InitializeComponent();
            XmlFile = Base;
            FogID = ID;
            FogScenario = scenario;
            panel2.BackColor = Color.FromArgb(255,255,255);
            label2.Text = label2.Text + " " + FogID.ToString();
            comboBox1.SelectedIndex = 0;
            newfogfile = noareas;
            comboBox1.Enabled = noareas;
        }

        public FrmAddFogSettings(string Base, int ID, int scenario, int index)
        {

            InitializeComponent();
            XmlFile = Base;
            FogID = ID;
            FogScenario = scenario;
            comboBox1.Enabled = newfogfile;

            Single Red = 0;
            Single Green = 0;
            Single Blue = 0;
           
            XmlReader reader = XmlReader.Create(new StringReader(XmlFile));
            while (reader.Read())
            {
                if (reader.Name == "D1")
                {
                    if (reader.GetAttribute("Name") == "Area Id")
                    {
                        reader.MoveToNextAttribute();
                        if (reader.GetAttribute("StringValue") == ID.ToString())
                        {
                            while (reader.Read() && reader.Name != "C1")
                            {
                                if (reader.GetAttribute("Name") == "ColorB") Blue = Single.Parse(reader.GetAttribute("StringValue"));
                                if (reader.GetAttribute("Name") == "ColorG") Green = Single.Parse(reader.GetAttribute("StringValue"));
                                if (reader.GetAttribute("Name") == "ColorR") Red = Single.Parse(reader.GetAttribute("StringValue"));
                                if (reader.GetAttribute("Name") == "Density") numericUpDown1.Value = decimal.Parse(reader.GetAttribute("StringValue"));
                                if (reader.GetAttribute("Name") == "InterpFrame") numericUpDown2.Value = decimal.Parse(reader.GetAttribute("StringValue"));
                                if (reader.GetAttribute("Name") == "MaxDepth") numericUpDown3.Value = decimal.Parse(reader.GetAttribute("StringValue"));
                                if (reader.GetAttribute("Name") == "MinDepth") numericUpDown4.Value = decimal.Parse(reader.GetAttribute("StringValue"));
                            }

                        }
                    }
                }
            }
            panel2.BackColor = Color.FromArgb((int)(Red*255), (int)(Green*255), (int)(Blue*255));
            label2.Text = label2.Text + " " + FogID.ToString();
            comboBox1.SelectedIndex = 0;
            startAt = index;
        }




        private void panel2_BackColorChanged(object sender, EventArgs e)
        {
            RedUD.Value = panel2.BackColor.R;
            BlueUD.Value = panel2.BackColor.B;
            GreenUD.Value = panel2.BackColor.G;
            RedBar.Value = panel2.BackColor.R;
            GreenBar.Value = panel2.BackColor.G;
            BlueBar.Value = panel2.BackColor.B;
        }

        private void RedBar_ValueChanged(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(RedBar.Value, panel2.BackColor.G, panel2.BackColor.B);
        }

        private void GreenBar_ValueChanged(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(panel2.BackColor.R, GreenBar.Value, panel2.BackColor.B);
        }

        private void BlueBar_ValueChanged(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(panel2.BackColor.R, panel2.BackColor.G, BlueBar.Value);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FrmAddFogSettings_Load(object sender, EventArgs e)
        {
            string setlng;
            if (Properties.Settings.Default.DotComma != true) { setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            TextInsertIndex = XmlFile.IndexOf("<C0 Name=\"FogAreas\">");
            if (TextInsertIndex == -1)
            {
                MessageBox.Show("Failed to get FogAreas node position!");
                this.Close();
            }
            TextInsertIndex += "<C0 Name=\"FogAreas\">".Length;
        }
        private void Ok_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            string Red = ((decimal)(RedUD.Value / 255)).ToString();
            string Blue = ((decimal)(BlueUD.Value / 255)).ToString();
            string Green = ((decimal)(GreenUD.Value / 255)).ToString();
            string str = "\r\n";
            str += "<C1>\r\n";
            str += "<D1 Name=\"Area Id\" StringValue=\""+FogID.ToString()+"\" />\r\n";
            str += "<D2 Name=\"ColorB\" StringValue=\""+Blue+"\" />\r\n";
            str += "<D2 Name=\"ColorG\" StringValue=\""+Green+"\" />\r\n";
            str += "<D2 Name=\"ColorR\" StringValue=\""+Red+"\" />\r\n";
            str += "<D2 Name=\"Density\" StringValue=\"" + numericUpDown1.Value.ToString() + "\" />\r\n";
            str += "<D1 Name=\"InterpFrame\" StringValue=\"" + numericUpDown2.Value.ToString() + "\" />\r\n";
            str += "<D2 Name=\"MaxDepth\" StringValue=\"" + numericUpDown3.Value.ToString() + "\" />\r\n";
            str += "<D2 Name=\"MinDepth\" StringValue=\"" + numericUpDown4.Value.ToString() + "\" />\r\n";
            str += "</C1>\r\n";
            if (startAt != -1)
            {
                int endAt = -1;
                
                XmlP1 = XmlFile.Substring(0, startAt-20);

                for (int i = 0; i < 10000; i++)
                {
                    int a = XmlFile.IndexOf("<D1 Name=\"Area Id\" StringValue=\"" + i.ToString() + "\" />");
                    if ((a > startAt&& a<endAt) || (a>startAt && endAt==-1)) endAt = a;
                }
                //if (endAt > -1) endAt -= ("</C1>\r\n" + "<C1>\r\n").Length;
                XmlP2 = XmlFile.Substring((endAt != -1) ? endAt-20 : (XmlFile.LastIndexOf("<A0 Name=\"FogType\"")- ("</C1>\r\n" + "<C1>\r\n").Length));

                XmlFile = XmlP1 + XmlP2;
            }
            XmlFile = XmlFile.Insert(TextInsertIndex, str);
            TextInsertIndex = XmlFile.IndexOf("<A0 Name=\"FogType\" StringValue=\"");
            if (newfogfile) XmlFile = XmlFile.Insert(TextInsertIndex+32, comboBox1.Text);
            fogparamfilenew = BymlConverter.GetByml(XmlFile).ToList();
            this.Close();
        }
    }
}
