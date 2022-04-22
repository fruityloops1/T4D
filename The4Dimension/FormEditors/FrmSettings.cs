using ModelViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;
using System.Globalization;
using System.Xml;

namespace The4Dimension.FormEditors
{
    public partial class Settings : Form
    {
        private UserControl1 rendera;

        public Settings(UserControl1 render)
        {
            string setlng;
            if (Properties.Settings.Default.DotComma != true) { setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            rendera = render;
            InitializeComponent();

            checkBox1.Checked = Properties.Settings.Default.UseDesignSound;
            UDCamSpeed.Value = (decimal)Properties.Settings.Default.CamSpeed;
            UDCamDistance.Value = (decimal)Properties.Settings.Default.CamDistance;
            textBox1.Text = Properties.Settings.Default.GamePath;
            CamInertiaUpDown.Value = (decimal)render.CameraInertiaFactor;
            ShowFPS.Checked = render.ShowFps;
            ShowCamDBG.Checked = render.ShowDebugInfo;
            ShowTri.Checked = render.ShowTriangleCount;
            ZoomSenUpDown.Value = (decimal)render.ZoomSensitivity;
            RotSenUpDown.Value = (decimal)render.RotationSensitivity;
            ChckUpdStart.Checked = Properties.Settings.Default.CheckUpdates;
            DwnldObjDBStart.Checked = Properties.Settings.Default.DownloadDb;
            AddOrigin.Checked = Properties.Settings.Default.AddObjectOrigin;// AddObjectOrigin;
            CamToObj.Checked = Properties.Settings.Default.AutoMoveCam;// AutoMoveCam;
            cbCameraMode.SelectedIndex = render.CamMode == HelixToolkit.Wpf.CameraMode.Inspect ? 0 : 1;
            URLtxtb.Text = Properties.Settings.Default.DownloadDbLink;
            dotcomma.Checked = Properties.Settings.Default.DotComma;
            HasAA.Checked = Properties.Settings.Default.HasAA;
            TextFilter.SelectedIndex = Properties.Settings.Default.TextFilter;
            experimental.Checked  = Properties.Settings.Default.ExperimentalFeatures;
            checkBox1.Enabled = Properties.Settings.Default.ExperimentalFeatures;
            checkBox2.Checked = Properties.Settings.Default.UseCamSettings;
            ShowChildren.Checked = Properties.Settings.Default.ShowChildren;
            Backface.Checked = Properties.Settings.Default.BackfaceCull;
            textBox2.Text = Properties.Settings.Default.LayeredFSPath;
            checkBox3.Checked = Properties.Settings.Default.UseLayeredFs;
        }
        private void SetDefaultUrl_Click(object sender, EventArgs e)
        {
            URLtxtb.Text = Properties.Settings.Default.DownloadDbLink;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Properties.Settings.Default.GamePath = textBox1.Text;
            Properties.Settings.Default.CameraInertia = (double)CamInertiaUpDown.Value;
            rendera.CameraInertiaFactor = (double)CamInertiaUpDown.Value;
            Properties.Settings.Default.ShowFps = ShowFPS.Checked;
            rendera.ShowFps = ShowFPS.Checked;
            Properties.Settings.Default.ShowDbgInfo = ShowCamDBG.Checked;
            rendera.ShowDebugInfo = ShowCamDBG.Checked;
            Properties.Settings.Default.ShowTriCount = ShowTri.Checked;
            rendera.ShowTriangleCount = ShowTri.Checked;
            Properties.Settings.Default.ZoomSen = (double)ZoomSenUpDown.Value;
            rendera.ZoomSensitivity = (double)ZoomSenUpDown.Value;
            rendera.CamMode = cbCameraMode.SelectedIndex == 0 ? HelixToolkit.Wpf.CameraMode.Inspect : HelixToolkit.Wpf.CameraMode.WalkAround;
            Properties.Settings.Default.CameraMode = cbCameraMode.SelectedIndex;
            Properties.Settings.Default.RotSen = (double)RotSenUpDown.Value;
            rendera.RotationSensitivity = (double)RotSenUpDown.Value;
            Properties.Settings.Default.AutoMoveCam = CamToObj.Checked;
            Properties.Settings.Default.CheckUpdates = ChckUpdStart.Checked;
            Properties.Settings.Default.DownloadDb = DwnldObjDBStart.Checked;
            //Properties.Settings.Default.DownloadDbLink = URLtxtb.Text;
            Properties.Settings.Default.AddObjectOrigin = AddOrigin.Checked;
            if (URLtxtb.Text != "")
            { 
            Properties.Settings.Default.DownloadDbLink = URLtxtb.Text;
            }
            else { Properties.Settings.Default.DownloadDbLink = "https://cdn.discordapp.com/attachments/749031033395085376/807013958027706378/objectdb.xml"; }
            Properties.Settings.Default.DotComma = dotcomma.Checked;
            Properties.Settings.Default.HasAA = HasAA.Checked;
            rendera.HasAA = HasAA.Checked;
            Properties.Settings.Default.TextFilter = TextFilter.SelectedIndex;
            rendera.TextureFilter = TextFilter.SelectedIndex;

            Properties.Settings.Default.UseDesignSound = checkBox1.Checked;
            Properties.Settings.Default.ExperimentalFeatures = experimental.Checked;
            if (!Properties.Settings.Default.ExperimentalFeatures) Properties.Settings.Default.UseDesignSound = true;
            Properties.Settings.Default.UseCamSettings = checkBox2.Checked;
            Properties.Settings.Default.ShowChildren = ShowChildren.Checked;
            Properties.Settings.Default.BackfaceCull = Backface.Checked;
            rendera.BackfaceCull = Backface.Checked;
            if (!checkBox2.Checked)
            {
                rendera.Camspeed = (double)UDCamSpeed.Value;
                Properties.Settings.Default.CamSpeed = (double)UDCamSpeed.Value;
                rendera.CamDistance = (double)UDCamDistance.Value;
                Properties.Settings.Default.CamDistance = (double)UDCamDistance.Value;
            }
            Properties.Settings.Default.LayeredFSPath = textBox2.Text;
            Properties.Settings.Default.UseLayeredFs = checkBox3.Checked;

            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Default_Click(object sender, EventArgs e)
        {
            rendera.Camspeed = 2000;
            Properties.Settings.Default.CamSpeed = 2000;
            rendera.CamDistance = 200;
            Properties.Settings.Default.CamDistance = 200;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Properties.Settings.Default.CameraInertia = 0.92;
            rendera.CameraInertiaFactor = 0.92;
            Properties.Settings.Default.ShowFps = true;
            rendera.ShowFps = true;
            Properties.Settings.Default.ShowDbgInfo = false;
            rendera.ShowDebugInfo = false;
            Properties.Settings.Default.ShowTriCount = true;
            rendera.ShowTriangleCount = true;
            Properties.Settings.Default.ZoomSen = 2;
            rendera.ZoomSensitivity = 2;
            rendera.CamMode = HelixToolkit.Wpf.CameraMode.Inspect;
            Properties.Settings.Default.CameraMode = 0;
            Properties.Settings.Default.RotSen = 1;
            rendera.RotationSensitivity = 1;
            Properties.Settings.Default.AutoMoveCam = true;
            Properties.Settings.Default.CheckUpdates = true;
            Properties.Settings.Default.DownloadDb = true;
            Properties.Settings.Default.AddObjectOrigin = false;
            Properties.Settings.Default.DotComma = false;
            Properties.Settings.Default.HasAA = true;
            rendera.HasAA = true;
            Properties.Settings.Default.TextFilter = 0;
            rendera.TextureFilter = 0;
            Properties.Settings.Default.UseDesignSound = true;
            Properties.Settings.Default.ExperimentalFeatures = false;
            Properties.Settings.Default.ShowChildren = true;
            Properties.Settings.Default.UseCamSettings = true;
            Properties.Settings.Default.BackfaceCull = true;
            Properties.Settings.Default.BackfaceCull = true;
            Properties.Settings.Default.UseLayeredFs = false;
            
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog fld = new CommonOpenFileDialog  { IsFolderPicker = true};
            fld.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (fld.ShowDialog() != CommonFileDialogResult.Ok) return;
            Cancel.Enabled = true;
            textBox1.Text = fld.FileName;
            if (File.Exists(@"BgmTable.szs"))
            {
                var res = MessageBox.Show("There is already a BgmTable.szs file in this program's folder, do you want to replace it with a new one from the game path you just selected? (Choose no if you edited the BGMs or else you will lose your changes)", "Warning", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    if (File.Exists(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs"))
                    {
                        File.Delete(@"BgmTable.szs");
                        File.Copy(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs", @"BgmTable.szs");
                    }
                    else MessageBox.Show(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs not found !\r\nThe file wasn't replaced");
                }
            }
            if (File.Exists(@"CreatorClassNameTable.szs"))
            {
                var res = MessageBox.Show("There is already a CreatorClassNameTable.szs file in this program's folder, do you want to replace it with a new one from the game path you just selected ? (Choose no if you edited the game objects or else you will lose your changes)", "Warning", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    if (File.Exists(Properties.Settings.Default.GamePath + "\\SystemData\\CreatorClassNameTable.szs"))
                    {
                        File.Delete(@"CreatorClassNameTable.szs");
                        File.Copy(Properties.Settings.Default.GamePath + "\\SystemData\\CreatorClassNameTable.szs", @"CreatorClassNameTable.szs");
                    }
                    else MessageBox.Show(Properties.Settings.Default.GamePath + "\\SystemData\\CreatorClassNameTable.szs not found !\r\nThe file wasn't replaced");
                }
            }
        }

        private void HasAA_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Some changes won't take place until you reload the editor!");
        }

        private void dotcomma_CheckedChanged(object sender, EventArgs e)
        {
            if (dotcomma.Checked==true) 
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void experimental_CheckedChanged(object sender, EventArgs e)
        {
            if (experimental.Checked == true &&!Properties.Settings.Default.ExperimentalFeatures)
            {
                MessageBox.Show("This will enable experimental features like the worldmap editor and the transparentwall generator, so use at your own risk");

            }
            else if (experimental.Checked) checkBox1.Enabled = true;
            else if (experimental.Checked == false && Properties.Settings.Default.ExperimentalFeatures) checkBox1.Enabled = false;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                label14.Enabled = false;
                label16.Enabled = false;
                UDCamDistance.Enabled = false;
                UDCamSpeed.Enabled = false;

            }
            else
            {
                label14.Enabled = true;
                label16.Enabled = true;
                UDCamDistance.Enabled = true;
                UDCamSpeed.Enabled = true;

            }
        }

        private void layerfsbtn_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog fld = new CommonOpenFileDialog { IsFolderPicker = true };
            fld.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (fld.ShowDialog() != CommonFileDialogResult.Ok) return;
            Cancel.Enabled = true;
            textBox2.Text = fld.FileName;
        }
    }
}
