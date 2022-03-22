using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The4Dimension.FormEditors
{
    public partial class FrmAddFogSettings : Form
    {
        string XmlFile;
        int FogID;
        int TextInsertIndex = -1;
        int FogScenario = 0; //0 common, 1 2 3 scenario
        public List<byte> fogparamfilenew;
        public FrmAddFogSettings(string Base, int ID, int scenario)
        {
            InitializeComponent();
            XmlFile = Base;
            FogID = ID;
            FogScenario = scenario;
            panel2.BackColor = Color.FromArgb(255,255,255);
            label2.Text = label2.Text + " " + FogID.ToString();
            comboBox1.SelectedIndex = 0;
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


            XmlFile = XmlFile.Insert(TextInsertIndex, str);
            TextInsertIndex = XmlFile.IndexOf("<A0 Name=\"FogType\" StringValue=\"");
            XmlFile = XmlFile.Insert(TextInsertIndex+32, comboBox1.Text);
            fogparamfilenew = BymlConverter.GetByml(XmlFile).ToList();
            this.Close();
        }
    }
}
