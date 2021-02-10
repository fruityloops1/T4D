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

namespace The4Dimension
{
    public partial class FrmCredits : Form
    {
        public FrmCredits()
        {
            InitializeComponent();
        }

        private void FrmCredits_Load(object sender, EventArgs e)
        {
            label4.Text = "V. " + Application.ProductVersion;            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
