﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace The4Dimension.FormEditors
{
    public partial class FrmStringInput : Form
    {
        public string Result = null;
        public FrmStringInput()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Result = null;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "AllRailInfos") { MessageBox.Show("You can't use this name !"); return; }
            Result = comboBox1.Text;
            this.Close();
        }

        private void FrmStringInput_Load(object sender, EventArgs e)
        {

        }
    }
}
