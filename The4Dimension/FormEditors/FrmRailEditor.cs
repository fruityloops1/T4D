using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The4Dimension.FormEditors
{
    public partial class FrmRailEditor : Form
    {
        public Rail edited;
        public FrmRailEditor(Rail rail)
        {
            edited = rail.Clone();
            InitializeComponent();
        }

        bool done = false;
        private void FrmRailEditor_Load(object sender, EventArgs e)
        {

            tName.Text = edited.Name;
            LayerName.Text = edited.LayerName;
            l_id.Value = edited.l_id;
            no.Value = edited.no;
            RClosed.Checked = edited.Closed;
            Type.SelectedIndex = Type.Items.IndexOf(edited.Type);
            done = true;
        }

        private void numericUpDownUpdated(object sender, EventArgs e)
        {
            string railprop = ((NumericUpDown)sender).Name;
            (edited[railprop]) = (Int32)((NumericUpDown)sender).Value;
            return;
        }
        private void tBoxUpdated(object sender, EventArgs e)
        {
            string railprop = ((TextBox)sender).Name;
            if (railprop == "tName")
            {
                edited["Name"] = ((TextBox)sender).Text;
                this.Text = "Rail : " + edited["Name"];
            }
            else
            {
                (edited[railprop]) = ((TextBox)sender).Text;
            }

            return;
        }
        private void checkBoxUpdated(object sender, EventArgs e)
        {
            if (done == true) edited.Closed = RClosed.Checked;
        }
        private void comboBoxUpdated(object sender, EventArgs e)
        {
            if (done == true) edited.Type = Type.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            edited = null;
            this.Close();
        }

        private void RailPointsBtn_Click(object sender, EventArgs e)
        {
            List<Rail.Point> list = edited.Points;
            FrmRailPointEditor railPointEditor = new FrmRailPointEditor(list);
            railPointEditor.ShowDialog();
        }
    }
}
