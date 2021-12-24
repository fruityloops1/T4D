
namespace The4Dimension.FormEditors
{
    partial class FrmRailEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRailEditor));
            this.Type = new System.Windows.Forms.ComboBox();
            this.RailPointsBtn = new System.Windows.Forms.Button();
            this.Closed = new System.Windows.Forms.CheckBox();
            this.label33 = new System.Windows.Forms.Label();
            this.no = new System.Windows.Forms.NumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.l_id = new System.Windows.Forms.NumericUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.LayerName = new System.Windows.Forms.TextBox();
            this.tName = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.no)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.l_id)).BeginInit();
            this.SuspendLayout();
            // 
            // Type
            // 
            this.Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Type.FormattingEnabled = true;
            this.Type.Items.AddRange(new object[] {
            "Bezier",
            "Linear"});
            this.Type.Location = new System.Drawing.Point(186, 112);
            this.Type.Name = "Type";
            this.Type.Size = new System.Drawing.Size(57, 21);
            this.Type.TabIndex = 6;
            this.Type.SelectedIndexChanged += new System.EventHandler(this.comboBoxUpdated);
            // 
            // RailPointsBtn
            // 
            this.RailPointsBtn.Location = new System.Drawing.Point(30, 162);
            this.RailPointsBtn.Name = "RailPointsBtn";
            this.RailPointsBtn.Size = new System.Drawing.Size(213, 31);
            this.RailPointsBtn.TabIndex = 7;
            this.RailPointsBtn.Text = "Edit rail points";
            this.RailPointsBtn.UseVisualStyleBackColor = true;
            this.RailPointsBtn.Click += new System.EventHandler(this.RailPointsBtn_Click);
            // 
            // Closed
            // 
            this.Closed.AutoSize = true;
            this.Closed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Closed.Location = new System.Drawing.Point(48, 116);
            this.Closed.Name = "Closed";
            this.Closed.Size = new System.Drawing.Size(59, 17);
            this.Closed.TabIndex = 5;
            this.Closed.Text = "Loop?:";
            this.Closed.UseVisualStyleBackColor = true;
            this.Closed.CheckedChanged += new System.EventHandler(this.checkBoxUpdated);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(122, 115);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(58, 13);
            this.label33.TabIndex = 12;
            this.label33.Text = "Rail curve:";
            // 
            // no
            // 
            this.no.Location = new System.Drawing.Point(198, 78);
            this.no.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.no.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.no.Name = "no";
            this.no.Size = new System.Drawing.Size(41, 20);
            this.no.TabIndex = 4;
            this.no.ValueChanged += new System.EventHandler(this.numericUpDownUpdated);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(127, 80);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(65, 13);
            this.label31.TabIndex = 13;
            this.label31.Text = "Second Id?:";
            // 
            // l_id
            // 
            this.l_id.Location = new System.Drawing.Point(73, 78);
            this.l_id.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.l_id.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.l_id.Name = "l_id";
            this.l_id.Size = new System.Drawing.Size(41, 20);
            this.l_id.TabIndex = 3;
            this.l_id.ValueChanged += new System.EventHandler(this.numericUpDownUpdated);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(27, 80);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(40, 13);
            this.label28.TabIndex = 14;
            this.label28.Text = "Rail Id:";
            // 
            // LayerName
            // 
            this.LayerName.Location = new System.Drawing.Point(95, 43);
            this.LayerName.Name = "LayerName";
            this.LayerName.Size = new System.Drawing.Size(119, 20);
            this.LayerName.TabIndex = 2;
            this.LayerName.Validated += new System.EventHandler(this.tBoxUpdated);
            // 
            // tName
            // 
            this.tName.Location = new System.Drawing.Point(95, 17);
            this.tName.Name = "tName";
            this.tName.Size = new System.Drawing.Size(119, 20);
            this.tName.TabIndex = 1;
            this.tName.Validated += new System.EventHandler(this.tBoxUpdated);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(27, 46);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(65, 13);
            this.label29.TabIndex = 15;
            this.label29.Text = "Layer name:";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(54, 20);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(38, 13);
            this.label30.TabIndex = 16;
            this.label30.Text = "Name:";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(12, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 21);
            this.button1.TabIndex = 7;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.Location = new System.Drawing.Point(198, 217);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 21);
            this.button2.TabIndex = 7;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FrmRailEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(277, 246);
            this.Controls.Add(this.Type);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.RailPointsBtn);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.Closed);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.tName);
            this.Controls.Add(this.no);
            this.Controls.Add(this.LayerName);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.l_id);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRailEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmRailEditor";
            this.Load += new System.EventHandler(this.FrmRailEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.no)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.l_id)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox Type;
        private System.Windows.Forms.Button RailPointsBtn;
        private System.Windows.Forms.CheckBox Closed;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.NumericUpDown no;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.NumericUpDown l_id;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox LayerName;
        private System.Windows.Forms.TextBox tName;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}