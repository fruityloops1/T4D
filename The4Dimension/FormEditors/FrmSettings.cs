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


        private List<string> languages;

        public Settings(UserControl1 render)
        {
            string setlng;
            if (Properties.Settings.Default.DotComma != true) { setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            rendera = render;
            InitializeComponent();
            # region Language
            string langpath = Path.GetDirectoryName(Application.ExecutablePath) + "\\LANG\\LANG.xml";
            XmlReader use = XmlReader.Create(langpath);
            LanguageBox.Items.Add("DEFAULT");
            languages = new List<string>();
            while (use.Read())
            {
                string info = "";
                
                if(use.NodeType == XmlNodeType.Element && use.Name == "Lang") 
                {
                    string CurrentFile = use.GetAttribute("file");
                    string xmlinfopath = Path.GetDirectoryName(Application.ExecutablePath) + "\\LANG\\" + CurrentFile + ".xml";
                    if (!File.Exists(xmlinfopath)) { break; }
                    XmlReader xmlinfo = XmlReader.Create(xmlinfopath);
                    languages.Add(CurrentFile);
                    while (xmlinfo.Read())
                    {
                        if (xmlinfo.NodeType == XmlNodeType.Element && xmlinfo.Name == "INFO") 
                        {
                            info = xmlinfo.GetAttribute("lang") + " translation by " + xmlinfo.GetAttribute("author");
                            break;
                        }
                    }
                    LanguageBox.Items.Add(info);
                }
                
            }
            LanguageBox.SelectedIndex = Properties.Settings.Default.CurrentLang;
            #endregion
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
                            case "Settings":
                                CForm = "Settings";
                                break;
                            default:
                                break;
                        }
                    }
                    if (LANG.NodeType == XmlNodeType.EndElement)
                    {
                        switch (LANG.Name)
                        {
                            case "Settings":
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
            #endregion

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
        }
        private void SetDefaultUrl_Click(object sender, EventArgs e)
        {
            URLtxtb.Text = Properties.Settings.Default.DownloadDbLink;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CurrentLang = LanguageBox.SelectedIndex;
            if (LanguageBox.SelectedIndex == 0) 
            { 
                Properties.Settings.Default.CurrentLangName = Properties.Defaults.Default.CurrentLangName; 
            }
            else
            {
                Properties.Settings.Default.CurrentLangName = languages[LanguageBox.SelectedIndex - 1];
            }
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
            rendera.Camspeed = (double)UDCamSpeed.Value;
            Properties.Settings.Default.CamSpeed = (double)UDCamSpeed.Value;
            rendera.CamDistance = (double)UDCamDistance.Value;
            Properties.Settings.Default.CamDistance = (double)UDCamDistance.Value;
            Properties.Settings.Default.UseDesignSound = checkBox1.Checked;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Default_Click(object sender, EventArgs e)
        {
            rendera.Camspeed = Properties.Defaults.Default.CamSpeed;
            Properties.Settings.Default.CamSpeed = Properties.Defaults.Default.CamSpeed;
            rendera.CamDistance = Properties.Defaults.Default.CamDistance;
            Properties.Settings.Default.CamDistance = Properties.Defaults.Default.CamDistance;
            Properties.Settings.Default.CurrentLang = Properties.Defaults.Default.CurrentLang;
            Properties.Settings.Default.CurrentLangName = Properties.Defaults.Default.CurrentLangName;
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
            Properties.Settings.Default.HasAA = Properties.Defaults.Default.HasAA;
            rendera.HasAA = Properties.Defaults.Default.HasAA;
            Properties.Settings.Default.TextFilter = Properties.Defaults.Default.TextFilter;
            rendera.TextureFilter = Properties.Defaults.Default.TextFilter;
            Properties.Settings.Default.UseDesignSound = Properties.Defaults.Default.UseDesignSound;
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
            MessageBox.Show("This feature is experimental and will break children objects with decimal for position, rotation and scale, so use at your own risk");
        
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This will load and save Sound and Design files for levels, as long as it exists at the same folder as the opened Map file (on load) or your level has Design or Sound specific objects/areas (on save).");
        }
    }
}
