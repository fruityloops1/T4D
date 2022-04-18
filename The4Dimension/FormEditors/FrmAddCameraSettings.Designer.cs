using System;
using System.ComponentModel;

namespace The4Dimension.FormEditors
{
    partial class FrmAddCameraSettings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddCameraSettings));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClassCB = new System.Windows.Forms.ComboBox();
            this.CategoryCB = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.IntProp = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label32 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.PropsCB = new System.Windows.Forms.ComboBox();
            this.label34 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.ExpProp = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.ExpLabel = new System.Windows.Forms.Label();
            this.PropertyName = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.ChildNodesCB = new System.Windows.Forms.ComboBox();
            this.BoolProp = new System.Windows.Forms.ComboBox();
            this.SingleProp = new System.Windows.Forms.NumericUpDown();
            this.GameCamToViewportBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IntProp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExpProp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SingleProp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(285, 287);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(6, 287);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Camera Id:";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.setDefaultToolStripMenuItem});
            this.contextMenuStrip1.Name = "ClipBoardMenu";
            this.contextMenuStrip1.Size = new System.Drawing.Size(166, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItem1.Text = "Remove Property";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // setDefaultToolStripMenuItem
            // 
            this.setDefaultToolStripMenuItem.Enabled = false;
            this.setDefaultToolStripMenuItem.Name = "setDefaultToolStripMenuItem";
            this.setDefaultToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.setDefaultToolStripMenuItem.Text = "Set Default";
            // 
            // ClassCB
            // 
            this.ClassCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ClassCB.FormattingEnabled = true;
            this.ClassCB.Items.AddRange(new object[] {
            "Parallel",
            "FixAll",
            "FixAllSpot",
            "Tower",
            "Rail",
            "ParallelTarget",
            "FixPosSpot",
            "FixPos",
            "ParallelVersus",
            "DemoTarget",
            "Anim",
            "Follow"});
            this.ClassCB.Location = new System.Drawing.Point(219, 45);
            this.ClassCB.Name = "ClassCB";
            this.ClassCB.Size = new System.Drawing.Size(121, 21);
            this.ClassCB.TabIndex = 41;
            this.ClassCB.SelectedIndexChanged += new System.EventHandler(this.ClassCB_SelectedIndexChanged);
            // 
            // CategoryCB
            // 
            this.CategoryCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CategoryCB.FormattingEnabled = true;
            this.CategoryCB.Items.AddRange(new object[] {
            "Map",
            "Object",
            "Entrance",
            "Event"});
            this.CategoryCB.Location = new System.Drawing.Point(219, 19);
            this.CategoryCB.Name = "CategoryCB";
            this.CategoryCB.Size = new System.Drawing.Size(121, 21);
            this.CategoryCB.TabIndex = 41;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(161, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Category:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(178, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Class:";
            // 
            // IntProp
            // 
            this.IntProp.Location = new System.Drawing.Point(240, 112);
            this.IntProp.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.IntProp.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.IntProp.Name = "IntProp";
            this.IntProp.Size = new System.Drawing.Size(108, 20);
            this.IntProp.TabIndex = 8;
            this.IntProp.Visible = false;
            this.IntProp.Validated += new System.EventHandler(this.IntProp_Validated);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(81, 22);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(71, 20);
            this.numericUpDown1.TabIndex = 8;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(19, 19);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(38, 13);
            this.label32.TabIndex = 45;
            this.label32.Text = "Name:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(305, 138);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(43, 23);
            this.button3.TabIndex = 44;
            this.button3.Text = "+";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // PropsCB
            // 
            this.PropsCB.FormattingEnabled = true;
            this.PropsCB.Items.AddRange(new object[] {
            "AngleH",
            "AngleV",
            "Category",
            "Class",
            "DashAngleTuner",
            "AddAngleMax",
            "ZoomOutOffsetMax",
            "Distance",
            "InterpoleFrame",
            "IsLimitAngleFix",
            "LimitBoxMax",
            "LimitBoxMin",
            "X",
            "Y",
            "Z",
            "Rotator",
            "AngleMax",
            "SideDegree",
            "SideOffset",
            "UpOffset",
            "UserGroupId",
            "UserName",
            "VelocityOffsetter",
            "MaxOffset",
            "VerticalAbsorber",
            "IsInvalidate",
            "VisionParam",
            "FovyDegree",
            "StereovisionDepth",
            "StereovisionDistance",
            "FarClipDistance",
            "NearClipDistacne",
            "CameraPos",
            "LookAtPos",
            "IsEnable",
            "IsCalcStartPosUseLookAtPos",
            "RailId",
            "HighAngle",
            "LowAngle",
            "PullDistance",
            "PushDistance",
            "TargetLookRate",
            "TargetRadius",
            "MaxOffsetAxisTwo",
            "IsDistanceFix",
            "LimitYMax",
            "LimitYMin",
            "Position",
            "DistanceMax",
            "DistanceMin",
            "FovyVersus",
            "CameraOffset"});
            this.PropsCB.Location = new System.Drawing.Point(66, 16);
            this.PropsCB.Name = "PropsCB";
            this.PropsCB.Size = new System.Drawing.Size(195, 21);
            this.PropsCB.TabIndex = 50;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(215, 84);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(125, 13);
            this.label34.TabIndex = 51;
            this.label34.Text = "(Get explanation from db)";
            this.label34.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(62, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(101, 20);
            this.textBox1.TabIndex = 52;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(8, 51);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(49, 13);
            this.label35.TabIndex = 2;
            this.label35.Text = "Used by:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label35);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.numericUpDown1);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.Controls.Add(this.ClassCB);
            this.groupBox4.Controls.Add(this.CategoryCB);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(354, 81);
            this.groupBox4.TabIndex = 54;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "General:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.treeView1);
            this.groupBox5.Controls.Add(this.ExpProp);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.ExpLabel);
            this.groupBox5.Controls.Add(this.PropertyName);
            this.groupBox5.Controls.Add(this.button5);
            this.groupBox5.Controls.Add(this.button3);
            this.groupBox5.Controls.Add(this.label34);
            this.groupBox5.Controls.Add(this.label32);
            this.groupBox5.Controls.Add(this.PropsCB);
            this.groupBox5.Controls.Add(this.IntProp);
            this.groupBox5.Controls.Add(this.ChildNodesCB);
            this.groupBox5.Controls.Add(this.BoolProp);
            this.groupBox5.Controls.Add(this.SingleProp);
            this.groupBox5.Location = new System.Drawing.Point(6, 90);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(354, 194);
            this.groupBox5.TabIndex = 55;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Properties:";
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(11, 52);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(186, 129);
            this.treeView1.TabIndex = 57;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // ExpProp
            // 
            this.ExpProp.Location = new System.Drawing.Point(240, 140);
            this.ExpProp.Maximum = new decimal(new int[] {
            38,
            0,
            0,
            0});
            this.ExpProp.Minimum = new decimal(new int[] {
            45,
            0,
            0,
            -2147483648});
            this.ExpProp.Name = "ExpProp";
            this.ExpProp.Size = new System.Drawing.Size(59, 20);
            this.ExpProp.TabIndex = 8;
            this.ExpProp.Visible = false;
            this.ExpProp.ValueChanged += new System.EventHandler(this.SingleProp_ValueChanged);
            this.ExpProp.Validated += new System.EventHandler(this.ExpProp_Validated);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(199, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Value :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Visible = false;
            // 
            // ExpLabel
            // 
            this.ExpLabel.AutoSize = true;
            this.ExpLabel.Location = new System.Drawing.Point(202, 142);
            this.ExpLabel.Name = "ExpLabel";
            this.ExpLabel.Size = new System.Drawing.Size(31, 13);
            this.ExpLabel.TabIndex = 4;
            this.ExpLabel.Text = "Exp :";
            this.ExpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ExpLabel.Visible = false;
            // 
            // PropertyName
            // 
            this.PropertyName.AutoSize = true;
            this.PropertyName.Location = new System.Drawing.Point(200, 61);
            this.PropertyName.Name = "PropertyName";
            this.PropertyName.Size = new System.Drawing.Size(0, 13);
            this.PropertyName.TabIndex = 4;
            this.PropertyName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(267, 16);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(73, 23);
            this.button5.TabIndex = 44;
            this.button5.Text = "Add Property";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // ChildNodesCB
            // 
            this.ChildNodesCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChildNodesCB.FormattingEnabled = true;
            this.ChildNodesCB.Location = new System.Drawing.Point(240, 112);
            this.ChildNodesCB.Name = "ChildNodesCB";
            this.ChildNodesCB.Size = new System.Drawing.Size(108, 21);
            this.ChildNodesCB.TabIndex = 58;
            this.ChildNodesCB.Visible = false;
            // 
            // BoolProp
            // 
            this.BoolProp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BoolProp.FormattingEnabled = true;
            this.BoolProp.Items.AddRange(new object[] {
            "True",
            "False"});
            this.BoolProp.Location = new System.Drawing.Point(240, 112);
            this.BoolProp.Name = "BoolProp";
            this.BoolProp.Size = new System.Drawing.Size(108, 21);
            this.BoolProp.TabIndex = 5;
            this.BoolProp.Visible = false;
            this.BoolProp.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // SingleProp
            // 
            this.SingleProp.DecimalPlaces = 6;
            this.SingleProp.Location = new System.Drawing.Point(240, 112);
            this.SingleProp.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.SingleProp.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.SingleProp.Name = "SingleProp";
            this.SingleProp.Size = new System.Drawing.Size(108, 20);
            this.SingleProp.TabIndex = 8;
            this.SingleProp.Visible = false;
            this.SingleProp.ValueChanged += new System.EventHandler(this.SingleProp_ValueChanged);
            this.SingleProp.Validated += new System.EventHandler(this.SingleProp_Validated);
            // 
            // GameCamToViewportBtn
            // 
            this.GameCamToViewportBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GameCamToViewportBtn.Location = new System.Drawing.Point(172, 287);
            this.GameCamToViewportBtn.Name = "GameCamToViewportBtn";
            this.GameCamToViewportBtn.Size = new System.Drawing.Size(31, 23);
            this.GameCamToViewportBtn.TabIndex = 0;
            this.GameCamToViewportBtn.Text = "👁";
            this.GameCamToViewportBtn.UseVisualStyleBackColor = true;
            this.GameCamToViewportBtn.Click += new System.EventHandler(this.PositionCamera);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(347, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(21, 21);
            this.pictureBox1.TabIndex = 56;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "...");
            this.pictureBox1.Visible = false;
            // 
            // FrmAddCameraSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(373, 314);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.GameCamToViewportBtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddCameraSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add camera code";
            this.Load += new System.EventHandler(this.FrmAddCameraSettings_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IntProp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExpProp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SingleProp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        private void ClipBoardMenu_CopyPos_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void pasteValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ClipBoardMenu_opening(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ComboBox ClassCB;
        private System.Windows.Forms.ComboBox CategoryCB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown IntProp;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox PropsCB;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button GameCamToViewportBtn;
        private System.Windows.Forms.ComboBox BoolProp;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown ExpProp;
        private System.Windows.Forms.NumericUpDown SingleProp;
        private System.Windows.Forms.Label PropertyName;
        private System.Windows.Forms.Label ExpLabel;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ComboBox ChildNodesCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ToolStripMenuItem setDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}