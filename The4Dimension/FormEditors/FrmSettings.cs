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
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Properties.Settings.Default.CameraInertia = Properties.Defaults.Default.CameraInertia;
            rendera.CameraInertiaFactor = Properties.Defaults.Default.CameraInertia;
            Properties.Settings.Default.ShowFps = Properties.Defaults.Default.ShowFps;
            rendera.ShowFps = Properties.Defaults.Default.ShowFps;
            Properties.Settings.Default.ShowDbgInfo = Properties.Defaults.Default.ShowDbgInfo;
            rendera.ShowDebugInfo = Properties.Defaults.Default.ShowDbgInfo;
            Properties.Settings.Default.ShowTriCount = Properties.Defaults.Default.ShowTriCount;
            rendera.ShowTriangleCount = Properties.Defaults.Default.ShowTriCount;
            Properties.Settings.Default.ZoomSen = Properties.Defaults.Default.ZoomSen;
            rendera.ZoomSensitivity = Properties.Defaults.Default.ZoomSen;
            rendera.CamMode = Properties.Defaults.Default.CameraMode == 0 ? HelixToolkit.Wpf.CameraMode.Inspect : HelixToolkit.Wpf.CameraMode.WalkAround;
            Properties.Settings.Default.CameraMode = Properties.Defaults.Default.CameraMode;
            Properties.Settings.Default.RotSen = Properties.Defaults.Default.RotSen;
            rendera.RotationSensitivity = Properties.Defaults.Default.RotSen;
            Properties.Settings.Default.AutoMoveCam = Properties.Defaults.Default.AutoMoveCam;
            Properties.Settings.Default.CheckUpdates = Properties.Defaults.Default.CheckUpd;
            Properties.Settings.Default.DownloadDb = Properties.Defaults.Default.DownloadDb;
            Properties.Settings.Default.DownloadDbLink = Properties.Defaults.Default.DownloadDbLnk;
            Properties.Settings.Default.AddObjectOrigin = Properties.Defaults.Default.AddObjOrigin;
            Properties.Settings.Default.DotComma = Properties.Defaults.Default.DotComma;
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
    }
}
