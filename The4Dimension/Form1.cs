using ModelViewer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Windows.Input;
using System.IO;
using LibEveryFileExplorer.Files.SimpleFileSystem;
using ExtensionMethods;
using System.Net;
using The4Dimension.ObjectDB;
using The4Dimension.FormEditors;
using System.Globalization;
using System.Threading;

namespace The4Dimension
{
    public partial class Form1 : Form
    {
        public static string ObjectDbLink = "https://raw.githubusercontent.com/exelix11/TheFourthDimension/master/ObjectsDb.xml";
        public UserControl1 render = new UserControl1();
        public UserControlNEW renderr = new UserControlNEW();
        public Dictionary<string, string> LevelNameNum = new Dictionary<string, string>(); //WX-X, stageName
        int APP_VER = Int32.Parse(Application.ProductVersion.Replace(".", ""));
        string LoadedFile = "";
        bool AutoMoveCam = true;
        bool AddObjectOrigin = false;
        public static int ReleaseId = 11;
        private Dictionary<string, string> strings;
        public Form1(string FileLoad = "")
        {
            string setlng;
            if (Properties.Settings.Default.DotComma != true) { setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            try
            {
                strings = new Dictionary<string, string>();
                InitializeComponent();
                #region Language
                if (!Directory.Exists("LANG"))
                {
                    MessageBox.Show("A language folder will be created, you'll be able to change the language later in the settings");
                    Directory.CreateDirectory("LANG");
                    File.WriteAllText(@"LANG\LANG.xml", Properties.Resources.LANG);
                    File.WriteAllText(@"LANG\EN.xml", Properties.Resources.EN);
                }
                if (!File.Exists(@"LANG\LANG.xml")) { File.WriteAllText(@"LANG\LANG.xml", Properties.Resources.LANG); }
                if (!File.Exists(@"LANG\EN.xml")) { File.WriteAllText(@"LANG\EN.xml", Properties.Resources.EN); }
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
                                case "MainMenu":
                                    CForm = "Form1";
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (LANG.NodeType == XmlNodeType.EndElement)
                        {
                            switch (LANG.Name)
                            {
                                case "MainMenu":
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
                                    case "splitContainer1.Panel1":
                                        ((Label)splitContainer1.Panel1.Controls[label]).Text = text;
                                        break;
                                    case "C0EditingPanel":
                                        ((Label)C0EditingPanel.Controls[label]).Text = text;
                                        break;
                                    case "splitContainer1.Panel2":
                                        ((Label)splitContainer1.Panel2.Controls[label]).Text = text;
                                        break;
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
                                    case "splitContainer1.Panel1":
                                        ((Button)splitContainer1.Panel1.Controls[button]).Text = text;
                                        break;
                                    case "C0EditingPanel":
                                        ((Button)C0EditingPanel.Controls[button]).Text = text;
                                        break;
                                    case "splitContainer1.Panel2":
                                        ((Button)splitContainer1.Panel2.Controls[button]).Text = text;
                                        break;
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
                                    case "splitContainer1.Panel1":
                                        ((CheckBox)splitContainer1.Panel1.Controls[cbox]).Text = text;
                                        break;
                                    case "C0EditingPanel":
                                        ((CheckBox)C0EditingPanel.Controls[cbox]).Text = text;
                                        break;
                                    case "splitContainer1.Panel2":
                                        ((CheckBox)splitContainer1.Panel2.Controls[cbox]).Text = text;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("Strp"))
                        {
                            string smenu = LANG.GetAttribute("name");
                            string parent = LANG.GetAttribute("parent");
                            string text = LANG.ReadElementContentAsString();
                            if (this.Name == CForm)
                            {
                                switch (parent)
                                {
                                    case "menu":
                                        menuStrip1.Items[smenu].Text = text;
                                        break;
                                    case "bymlconv":
                                        bymlConverterToolStripMenuItem1.DropDownItems[smenu].Text = text;
                                        break;
                                    case "file":
                                        fileToolStripMenuItem.DropDownItems[smenu].Text = text;
                                        break;
                                    case "help":
                                        helpToolStripMenuItem.DropDownItems[smenu].Text = text;
                                        break;
                                    case "clipboard":
                                        ClipBoardMenu.Items[smenu].Text = text;
                                        break;
                                    case "tools":
                                        otherToolStripMenuItem.DropDownItems[smenu].Text = text;
                                        break;
                                    case "find":
                                        findToolStripMenuItem.DropDownItems[smenu].Text = text;
                                        break;
                                    case "SaveAs":
                                        saveAsToolStripMenuItem.DropDownItems[smenu].Text = text;
                                        break;
                                    case "OBJbySWITCH":
                                        objectBySwitchToolStripMenuItem.DropDownItems[smenu].Text = text;
                                        break;
                                    default:
                                        ToolStripItem Menu = menuStrip1.Items["fileToolStripMenuItem"];
                                        text = "aga";
                                        Menu.Text = text;
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
                                        panel1.Controls[tbx].Text = text;
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }
                        else if (LANG.NodeType == XmlNodeType.Element && LANG.Name.Equals("Var"))
                        {
                            string var = LANG.GetAttribute("name");
                            string parent = LANG.GetAttribute("parent");
                            string text = LANG.ReadElementContentAsString();
                            if (this.Name == CForm)
                            {
                                switch (parent)
                                {
                                    default:
                                        strings.Add(var, text);
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    strings.Add("Hotkey", "Hotkeys list:\r\n" +
                   " Ctrl + O : Open File\r\n" +
                   " Ctrl + S : Save File\r\n" +
                   " Ctrl + Z : Undo\r\n" +
                   " Alt + S : Settings\r\n" +
                   " Space : Move the camera to the selected object\r\n" +
                   " Ctrl + D : Duplicate selected object\r\n" +
                   " + : Add a new object\r\n" +
                   " Del : Delete selected object\r\n" +
                   " Ctrl + R : Round the selected object position to a multiple of 100 (like Ctrl + alt + drag, but without dragging)\r\n" +
                   " Ctrl + F : Open the search menu\r\n" +
                   " C : If the selected object has a GenerateChildren C0List edit it\r\n" +
                   " B : If you are editing a C0List go back\r\n\r\n" +
                   "In the Objects list:\r\n" +
                   " Click once on an object to select it\r\n" +
                   " Double click an object to select it and move the camera to it\r\n" +
                   " Left click to deselect every object\r\n" +
                   "In the 3D view:\r\n" +
                   " Ctrl + drag : Move object\r\n" +
                   " Ctrl + Alt + drag : Move object snapping every 100 units\r\n" +
                   " Ctrl + Shift + drag : Move object snapping every 50 units\r\n" +
                   " (With a rail selected) N : Add a new point at the end of the rail\r\n" +
                   " -Every other combination without having to press Ctrl\r\n");
                }

                #endregion
                #region StageList 
                string[] lines = Properties.Resources.AllStageList.Split(Environment.NewLine[0]);
                int nextIndex = -1;
                for (int i = 1; i < 3; i++)
                {
                    for (int y = 1; y < 6; y++)
                    {
                        LevelNameNum.Add("W" + i.ToString() + "-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                    }
                }
                for (int i = 3; i < 8; i++)
                {
                    for (int y = 1; y < 7; y++)
                    {
                        LevelNameNum.Add("W" + i.ToString() + "-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                    }
                }
                for (int y = 1; y < 10; y++)
                {
                    LevelNameNum.Add("W8-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                }
                for (int y = 1; y < 6; y++)
                {
                    LevelNameNum.Add("S1-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                }
                for (int i = 10; i < 17; i++)
                {
                    for (int y = 1; y < 7; y++)
                    {
                        LevelNameNum.Add("S" + (i - 8).ToString() + "-" + y.ToString() + "  (" + lines[++nextIndex].Trim() + ")", lines[nextIndex].Trim());
                    }
                }
                LevelNameNum.Add("S8-Championship", lines[++nextIndex].Trim());
                string[] spstg = Properties.Resources.SpecialStageList.Split(Environment.NewLine[0]);
                nextIndex = 0;
                int spstgindex = 0;
                for (int i = 0; i < spstg.Length; i++)
                {
                    if (spstg[i].Trim().StartsWith("Demo"))
                    {
                        LevelNameNum.Add("Cutscene " +  ((++nextIndex)).ToString() + " (" + spstg[spstgindex].Trim() + ")" , spstg[spstgindex].Trim());
                        if(!spstg[i+1].Trim().StartsWith("Demo")){ nextIndex = 0; }
                    }
                    else if (spstg[i].Trim().StartsWith("KinopioHouse"))
                    {
                        LevelNameNum.Add("Toad house " + ((++nextIndex)).ToString() + " (" + spstg[spstgindex].Trim() + ")", spstg[spstgindex].Trim());
                        if (!spstg[i + 1].Trim().StartsWith("KinopioHouse")){ nextIndex = 0; }
                    }
                    else if (spstg[i].Trim().StartsWith("KinopioGame"))
                    {
                        LevelNameNum.Add("Gameover house " + ((++nextIndex)).ToString() + " (" + spstg[spstgindex].Trim() + ")", spstg[spstgindex].Trim());
                        if (!spstg[i + 1].Trim().StartsWith("KinopioGame")){ nextIndex = 0; }
                    }                    
                    else if (spstg[i].Trim().StartsWith("Mystery"))
                    {
                        LevelNameNum.Add("Mystery box " + ((++nextIndex)).ToString() + " (" + spstg[spstgindex].Trim() + ")", spstg[spstgindex].Trim());
                        if (i != spstg.Length - 1) 
                        { if (!spstg[i + 1].Trim().StartsWith("Mystery")) { nextIndex = 0; } }
                        
                    }
                    ++spstgindex;
                }
                #endregion


                while (Properties.Settings.Default.OpenRecent.Count > 9)
                {
                    Properties.Settings.Default.OpenRecent.RemoveAt(0);
                }
                int d = Properties.Settings.Default.OpenRecent.Count;

                for (int i = Properties.Settings.Default.OpenRecent.Count; i > 0; i--)
                    {
                        ToolStripMenuItem btn = new ToolStripMenuItem();
                        btn.Name = "OpnRec" + (Properties.Settings.Default.OpenRecent.Count-i+1);
                        btn.Text = (Properties.Settings.Default.OpenRecent.Count-i+1).ToString() + ". " + Properties.Settings.Default.OpenRecent[i-1];
                        btn.Click += OpenRecent_click;
                        openRecentToolStripMenuItem.DropDownItems.Add(btn);
                }
                ToolStripMenuItem clr = new ToolStripMenuItem();
                clr.Name = "OpnRecClr";
                clr.Text = "Clear";
                clr.Click += OpenRecentClear;
                openRecentToolStripMenuItem.DropDownItems.Add(clr);


                render.CamDistance =Properties.Settings.Default.CamDistance;
                render.Camspeed =Properties.Settings.Default.CamSpeed;
                KeyPreview = true;
                elementHost1.Child = render;
                elementHost2.Visible = false;
                elementHost2.Child = renderr;

                /*  */
                    render.MouseLeftButtonDown += render_LeftClick;
                render.MouseMove += render_MouseMove;
                render.MouseLeftButtonDown += render_MouseLeftButtonDown;
                render.MouseLeftButtonUp += render_MouseLeftButtonUp;
                render.KeyDown += render_KeyDown;
                render.KeyUp += render_KeyUP;
                render.CameraInertiaFactor = Properties.Settings.Default.CameraInertia;
                render.ShowFps = Properties.Settings.Default.ShowFps;
                render.ShowTriangleCount = Properties.Settings.Default.ShowTriCount;
                render.ShowDebugInfo = Properties.Settings.Default.ShowDbgInfo;
                render.CamMode = Properties.Settings.Default.CameraMode == 0 ? HelixToolkit.Wpf.CameraMode.Inspect : HelixToolkit.Wpf.CameraMode.WalkAround;
                render.ZoomSensitivity = Properties.Settings.Default.ZoomSen;
                render.RotationSensitivity = Properties.Settings.Default.RotSen;
                render.HasAA = Properties.Settings.Default.HasAA;
                render.TextureFilter = Properties.Settings.Default.TextFilter;
                AutoMoveCam = Properties.Settings.Default.AutoMoveCam;
                AddObjectOrigin = Properties.Settings.Default.AddObjectOrigin;
                
                Focus();
                if (FileLoad != "")
                {
                    LoadedFile = FileLoad;
                }
            }
            catch (Exception ex)
            {
                string err = "There was an error in the application.\r\n" +
                    "________________________________________\r\n" +
                    ex.Message + "\r\n\r\n" + ex.StackTrace + "\r\n";
                File.WriteAllText("Error.log", err);
                MessageBox.Show("There was an error in the application:\r\n" + ex.Message);
                MessageBox.Show("A log of the error was saved in the same folder of this application.");
            }
        }
        public bool UseDesignSound = Properties.Settings.Default.UseDesignSound;
        public Dictionary<string, byte[]> SzsFiles = null;
        public Dictionary<string, AllInfoSection> AllInfos = new Dictionary<string, AllInfoSection>();
        public List<Rail> AllRailInfos = new List<Rail>();
        public Dictionary<string, int> higestID = new Dictionary<string, int>();
        public Dictionary<string, string> CreatorClassNameTable = new Dictionary<string, string>();
        public CustomStack<UndoAction> Undo = new CustomStack<UndoAction>();
        public static List<ClipBoardItem> clipboard = new List<ClipBoardItem>();
        public static Encoding DefEnc = Encoding.GetEncoding("Shift-JIS");
        public ObjectDb ObjectDatabase = null;
        List<String> CustomModels = new List<string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("models"))
            {
                if (MessageBox.Show("You must convert every model from the game before you can use the editor, convert now ? (you need to have the extracted ROMFS of the game on your pc)", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show("Select the ROMFS folder (This folder should contain ObjectData, SystemData, StageData, etc...)\r\nKeep this folder in the same path, from there will be loaded the levels, some files needed for editing the levels and the bgm.\r\nThe content of the folder won't be modified unless you save edited levels there");
                    ModelDumper dlg = new ModelDumper();
                    dlg.ShowDialog();
                    if (!Directory.Exists("models")) Application.Exit();
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Directory.CreateDirectory("models");
                    File.WriteAllBytes(@"models\baseModels.zip", Properties.Resources.BaseModels);
                    System.IO.Compression.ZipFile.ExtractToDirectory(@"models\baseModels.zip", @"models");
                    File.Delete(@"models\baseModels.zip");
                    Directory.CreateDirectory(@"models\Tex");
                    MessageBox.Show("You won't be able to see any model, to do the procedure delete the models folder");
                }
            }
            if (!Properties.Settings.Default.DownloadDb) LoadObjectDatabase();
            LoadCreatorClassNameTable();
            if (LoadedFile != "") LoadFile(LoadedFile);
            else SetUiLock(false);
            if (Properties.Settings.Default.CheckUpdates || Properties.Settings.Default.DownloadDb)
            {
                StatusLbl.Visible = true;
                downloadLatestObjectDatabaseToolStripMenuItem.Enabled = true;
                StartupChecks.RunWorkerAsync();
            }
            if (Properties.Settings.Default.CheckUpdates)
            {
                StatusLbl.Text = "Checking updates";
                if (Properties.Settings.Default.DownloadDb) StatusLbl.Text += " and downloading database..."; else StatusLbl.Text += "...";
            }
            else if (Properties.Settings.Default.DownloadDb) StatusLbl.Text = "Downloading database...";
            if (Directory.Exists("CustomModels"))
            {
                foreach (string s in Directory.EnumerateFiles("CustomModels"))
                    if (s.EndsWith(".obj")) CustomModels.Add(Path.GetFileNameWithoutExtension(s));
            }
        }

        void UnloadLevel()
        {
            render.UnloadLevel();
            SzsFiles = null;

            C0ListEditingStack = new Stack<List<LevelObj>>();
            IsEditingC0List = false;
            SelectionIndex = new Stack<int>();
            InitialAllInfosSection = -1;
            C0EditingPanel.Visible = false;

            AllInfos = new Dictionary<string, AllInfoSection>();
            AllRailInfos = new List<Rail>();
            higestID = new Dictionary<string, int>();
            Undo = new CustomStack<UndoAction>();
            comboBox1.Items.Clear();
            ObjectsListBox.Items.Clear();
            propertyGrid1.SelectedObject = null;
            //if (MessageBox.Show("Keep clipboard ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) clipboard = new List<ClipBoardItem>();
            LoadedFile = "";
            this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + LoadedFile;
            otherToolStripMenuItem.DropDownItems.Clear();
            SetUiLock(false);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Title = "Open a level file";
            opn.Filter = "Supported formats (.szs, .byml, .xml)|*.szs; *.byml; *.xml|Every file|*.*";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                LoadFile(opn.FileName);
            }
        }

        private void openFromLevelNameWXXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.GamePath.Trim() == "")
            {
                MessageBox.Show("To use this function you must set the game ROMFS path");
                return;
            }
            else if (!Directory.Exists(Properties.Settings.Default.GamePath + "\\StageData"))
            {
                MessageBox.Show("Folder " + Properties.Settings.Default.GamePath + "\\StageData Not found!\r\nProbably your Romfs dump is incomplete or was modified");
                return;
            }
            FrmLevNameOpen f = new FrmLevNameOpen(LevelNameNum);
            f.ShowDialog();
            if (f.res == null) return;
            else
            {
                if (File.Exists(Properties.Settings.Default.GamePath + "\\StageData\\" + LevelNameNum[f.res])) LoadFile(Properties.Settings.Default.GamePath + "\\StageData\\" + LevelNameNum[f.res]);
                else MessageBox.Show(Properties.Settings.Default.GamePath + "\\StageData\\" + LevelNameNum[f.res] + " Not found!\r\nProbably your Romfs dump is incomplete or was modified");
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnloadLevel();
            elementHost1.Show();
            comboBox1.Items.Add("AllRailInfos");
            higestID.Add("AllRailInfos", 0);
            render.AddKey("AllRailInfos");
            comboBox1.SelectedIndex = 0;
            SetupSZS();
            SetUiLock(true);
            saveToolStripMenuItem.Enabled = false;
            lblDescription.Text = "";
            lblDescription.Tag = -1;
        }

        void SetUiLock(bool Lock)
        {
            splitContainer1.Enabled = Lock;
            saveAsXmlToolStripMenuItem.Enabled = Lock;
            saveAsBymlToolStripMenuItem1.Enabled = Lock;
            saveToolStripMenuItem.Enabled = Lock;
            generate2DSectionToolStripMenuItem.Enabled = Lock;
            UndoMenu.Enabled = Lock;
            findToolStripMenuItem.Enabled = Lock;
            label3.Text = "";
            if (Lock) ZoomCheckWarning.Start(); else ZoomCheckWarning.Stop();
            OtherLevelDataMenu.Enabled = Lock;
            saveAsSZSToolStripMenuItem.Enabled = Lock;
            generatePreloadFileListToolStripMenuItem.Enabled = Lock;
        }

        #region FileLoading
        public List<string> opnfiles = new List<string>();
        public void LoadFile(string FilePath) //Checks the file type and then loads the file
        {

            if (Path.GetExtension(FilePath).ToLower() == ".xml")//xml and byml originally tried to load levels, now only xml does this as it makes more sense for byml to be readable, like when dropping a byml file to t4d.exe
            {/*
                LoadedFile = FilePath;
                SetUiLock(true);
                OpenFile(File.ReadAllText(FilePath, DefEnc));
                SetupSZS();*/
            }
            else if (Path.GetExtension(FilePath).ToLower() == ".byml")
            {
                LoadedFile = FilePath;
                FormEditors.FrmXmlEditor frm = new FormEditors.FrmXmlEditor(BymlConverter.GetXml(LoadedFile), LoadedFile, false);
                frm.ShowDialog();
                if (frm.XmlRes != null) { File.WriteAllBytes(FilePath, BymlConverter.GetByml(frm.XmlRes)); }
                /* Original function was less useful
                LoadedFile = FilePath;
                SetUiLock(true);
                OpenFile(BymlConverter.GetXml(FilePath));
                SetupSZS();*/
            }
            else if (Path.GetExtension(FilePath).ToLower() == ".szs")//load the main level and then try to load the design and sound
            {
                UnloadLevel();
                elementHost1.Show();
                LoadedFile = FilePath;
                int Scenario = 1;
                if (Int32.TryParse(FilePath.Substring(FilePath.Count() - 5, 1), out _))
                {
                    Scenario = Int32.Parse(FilePath.Substring(FilePath.Count() - 5, 1));
                }
                string DesignFile = FilePath.Substring(0, FilePath.Count()-8)+"Design"+ Scenario.ToString()+".szs";
                string SoundFile = FilePath.Substring(0, FilePath.Count() - 8) + "Sound" + Scenario.ToString() + ".szs";
                OtherLevelDataMenu.DropDownItems.Clear();
                SzsFiles = new Dictionary<string, byte[]>();
                CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
                NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
                SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(FilePath)));
                int index = 0;
                List<ToolStripMenuItem> OtherFiles = new List<ToolStripMenuItem>();
                byte[] StageData = null;
                opnfiles.Add(FilePath);
                foreach (SFSFile f in SzsArch.ToFileSystem().Files)
                {
                    if (f.FileName.ToLower() == "stagedata.byml") StageData = f.Data;
                    else
                    {
                        ToolStripMenuItem btn = new ToolStripMenuItem();
                        btn.Name = "LoadFile" + index.ToString();
                        btn.Text = f.FileName;
                        btn.Click += LoadFileList_click;
                        OtherFiles.Add(btn);
                        SzsFiles.Add(f.FileName, f.Data);
                    }
                    index++;
                }
                OtherLevelDataMenu.DropDownItems.AddRange(OtherFiles.ToArray());
                if (StageData != null)
                {
                    Debug.Print("Size : " + (StageData.Length / 1024).ToString());

                    OpenFile(BymlConverter.GetXml(StageData),"Map", (!File.Exists(DesignFile)&&!File.Exists(SoundFile)));
                }
                else
                {
                    MessageBox.Show("StageData.byml not found in the file !");
                    SzsFiles = null;
                    SetUiLock(false);
                }
                if ((File.Exists(DesignFile) || File.Exists(SoundFile))&& UseDesignSound == true)
                {
                    if (File.Exists(DesignFile))
                    {
                        //SzsFiles = new Dictionary<string, byte[]>();
                        y = new CommonCompressors.YAZ0();
                        SzsArch = new NDS.NitroSystem.FND.NARC();
                        SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(DesignFile)));
                        OtherFiles = new List<ToolStripMenuItem>();
                        StageData = null;
                        opnfiles.Add(DesignFile);
                        foreach (SFSFile f in SzsArch.ToFileSystem().Files)
                        {
                            if (f.FileName.ToLower() == "stagedata.byml") StageData = f.Data;
                            else
                            {
                                ToolStripMenuItem btn = new ToolStripMenuItem();
                                btn.Name = "LoadFile" + index.ToString();
                                btn.Text = f.FileName;
                                btn.Click += LoadFileList_click;
                                OtherFiles.Add(btn);
                                SzsFiles.Add(f.FileName, f.Data);
                            }
                            index++;
                        }
                        OtherLevelDataMenu.DropDownItems.AddRange(OtherFiles.ToArray());
                        if (StageData != null)
                        {
                            Debug.Print("Size : " + (StageData.Length / 1024).ToString());
                            OpenFile(BymlConverter.GetXml(StageData),"Design",!File.Exists(SoundFile));
                        }
                        else
                        {
                            MessageBox.Show("StageData.byml not found in the file !");
                            SzsFiles = null;
                            SetUiLock(false);
                        }
                    }
                    if (File.Exists(SoundFile))
                    {
                        //SzsFiles = new Dictionary<string, byte[]>();
                        y = new CommonCompressors.YAZ0();
                        SzsArch = new NDS.NitroSystem.FND.NARC();
                        SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(SoundFile)));
                        OtherFiles = new List<ToolStripMenuItem>();
                        StageData = null;
                        opnfiles.Add(SoundFile);
                        foreach (SFSFile f in SzsArch.ToFileSystem().Files)
                        {
                            if (f.FileName.ToLower() == "stagedata.byml") StageData = f.Data;
                            else
                            {
                                ToolStripMenuItem btn = new ToolStripMenuItem();
                                btn.Name = "LoadFile" + index.ToString();
                                btn.Text = f.FileName;
                                btn.Click += LoadFileList_click;
                                OtherFiles.Add(btn);
                                SzsFiles.Add(f.FileName, f.Data);
                            }
                            index++;
                        }
                        OtherLevelDataMenu.DropDownItems.AddRange(OtherFiles.ToArray());
                        if (StageData != null)
                        {
                            Debug.Print("Size : " + (StageData.Length / 1024).ToString());
                            OpenFile(BymlConverter.GetXml(StageData),"Sound",true);
                        }
                        else
                        {
                            MessageBox.Show("StageData.byml not found in the file !");
                            SzsFiles = null;
                            SetUiLock(false);
                        }
                    }
                    this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + FilePath.Substring(0, FilePath.Count() - 8) + " - Scenario " + Scenario.ToString();
                }
                else { this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + FilePath; }

                SetUiLock(true);

                if (!Properties.Settings.Default.OpenRecent.Contains(LoadedFile))
                {
                    Properties.Settings.Default.OpenRecent.Add(LoadedFile);
                    while (Properties.Settings.Default.OpenRecent.Count>9)
                    {
                        Properties.Settings.Default.OpenRecent.RemoveAt(0);
                    }
                    int d = Properties.Settings.Default.OpenRecent.Count;
                }
                else
                {
                    Properties.Settings.Default.OpenRecent.RemoveAt(Properties.Settings.Default.OpenRecent.IndexOf(LoadedFile));
                    Properties.Settings.Default.OpenRecent.Add(LoadedFile);
                }
                openRecentToolStripMenuItem.DropDownItems.Clear();
                for (int i = Properties.Settings.Default.OpenRecent.Count; i > 0; i--)
                {
                    ToolStripMenuItem btn = new ToolStripMenuItem();
                    btn.Name = "OpnRec" + (Properties.Settings.Default.OpenRecent.Count - i + 1);
                    btn.Text = (Properties.Settings.Default.OpenRecent.Count - i + 1).ToString() + ". " + Properties.Settings.Default.OpenRecent[i - 1];
                    btn.Click += OpenRecent_click;
                    openRecentToolStripMenuItem.DropDownItems.Add(btn);

                }
                ToolStripMenuItem clr = new ToolStripMenuItem();
                clr.Name = "OpnRecClr";
                clr.Text = "Clear";
                clr.Click += OpenRecentClear;
                openRecentToolStripMenuItem.DropDownItems.Add(clr);
            }
            else
            {
                LoadedFile = "";
                MessageBox.Show("File type not supported !");
                SetUiLock(false);
            }
            
        }

        private void OpenRecent_click(object sender, EventArgs e)
        {
            string filepath = (((ToolStripMenuItem)sender).Text);
            LoadFile(filepath.Substring(3));
        }
        private void OpenRecentClear(object sender, EventArgs e)
        {
            Properties.Settings.Default.OpenRecent.Clear();
            openRecentToolStripMenuItem.DropDownItems.Clear();
            ToolStripMenuItem clr = new ToolStripMenuItem();
            clr.Name = "OpnRecClr";
            clr.Text = "Clear";
            clr.Click += OpenRecentClear;
            openRecentToolStripMenuItem.DropDownItems.Add(clr);
        }
        private void LoadFileList_click(object sender, EventArgs e)
        {
            string name = ((ToolStripMenuItem)sender).Text;
            FormEditors.FrmXmlEditor frm = new FormEditors.FrmXmlEditor(BymlConverter.GetXml(SzsFiles[name]), name, false);
            frm.ShowDialog();
            if (frm.XmlRes != null) SzsFiles[name] = BymlConverter.GetByml(frm.XmlRes);
        }
        
        public void LoadObjectDatabase()
        {
            ObjectDatabase = null;
            if (!File.Exists(@"objectdb.xml"))
            {
                MessageBox.Show("The object database wasn't found, some objects may not appear, and you won't be able to get informations about how to use objects, you can download the database from Help -> Download latest object database");
                return;
            } else
            {
                try { ObjectDatabase = ObjectDb.FromXml(File.ReadAllText(@"objectdb.xml")); }
                catch (Exception ex) { MessageBox.Show("Could't load the objects database: \r\n\r\n" + ex.Message); }
            }
        }

        public void LoadCreatorClassNameTable()
        {
            CreatorClassNameTable.Clear();
            if (!File.Exists(@"CreatorClassNameTable.szs"))
            {
                if (Properties.Settings.Default.GamePath.Trim() != "" && File.Exists(Properties.Settings.Default.GamePath + "\\SystemData\\CreatorClassNameTable.szs"))
                {
                    File.Copy(Properties.Settings.Default.GamePath + "\\SystemData\\CreatorClassNameTable.szs", @"CreatorClassNameTable.szs");
                }
                else
                {
                    if (Properties.Settings.Default.GamePath.Trim() == "") MessageBox.Show("to add objects to the game, and get list of every objects in the editor you need CreatorClassNameTable.szs in the same folder as this program, this file is placed inside GameRomFS:SystemData\\CreatorClassNameTable.szs");
                    else MessageBox.Show(Properties.Settings.Default.GamePath + "\\SystemData\\CreatorClassNameTable.szs not found.\r\nProbably your Romfs dump is incomplete or was modified.\r\nWithout this file you can only duplicate or delete objects.");
                    creatorClassNameTableEditorToolStripMenuItem.Enabled = false;
                    return;
                }
            }
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(@"CreatorClassNameTable.szs")));
            string ConvertedCCN = BymlConverter.GetXml(SzsArch.ToFileSystem().Files[0].Data);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(ConvertedCCN);
            XmlNode n = xml.SelectSingleNode("/Root/C0");
            foreach (XmlNode C1Block in n.ChildNodes)
            {
                string ClassName = "";
                string ObjName = "";
                foreach (XmlNode SubNode in C1Block.ChildNodes)
                {
                    if (SubNode.Attributes["Name"].Value == "ClassName")
                        ClassName = SubNode.Attributes["StringValue"].Value;
                    else ObjName = SubNode.Attributes["StringValue"].Value;
                }
                CreatorClassNameTable.Add(ObjName, ClassName);
            }
        }

        void OpenFile(string XmlText, string Type, bool Last)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XmlText);
            XmlNode n = xml.SelectSingleNode("/Root/C1/C1");
            if (n.Attributes["Name"].Value == "AllInfos") ProcessAllInfos(n.ChildNodes); else throw new Exception("Not The AllInfos node !");
            n = xml.SelectNodes("/Root/C1/C1")[1];
            if (n.Attributes["Name"].Value == "AllRailInfos") ProcessRailInfos(n.ChildNodes); else throw new Exception("Not The AllRailInfos node !");
            if (Type == "Map")
            {
                comboBox1.Items.AddRange(AllInfos.Keys.ToArray());
                comboBox1.Items.Add("AllRailInfos");
                render.AddKey("AllRailInfos");
                LoadRailsModels(AllRailInfos);
            }

            /*xml.Load(System.IO.Path.GetDirectoryName(file) + "\\PreLoadFileList1.xml");
            n = xml.SelectSingleNode("/Root/C1");
            foreach (XmlNode subnode in n.ChildNodes)
            {
                if (subnode.ChildNodes.Count == 2 && subnode.ChildNodes[1].Attributes["StringValue"].Value == "Archive")
                {
                    string FileName = subnode.ChildNodes[0].Attributes["StringValue"].Value.Split('/')[1];
                    FileName = FileName.Substring(0, FileName.Length - 4);
                    Models.Add(FileName.ToLower(), "models\\" + FileName + ".obj");
                }
            } Reading the file list doesn't seem to be useful for now
            */
            if (Last)
            {
                foreach (string k in AllInfos.Keys.ToArray())
                {
                    render.AddKey(k);
                    if (k == "AreaObjInfo") LoadModels(AllInfos[k], k, "models\\UnkYellow.obj");
                    else if (k == "CameraAreaInfo") LoadModels(AllInfos[k], k, "models\\UnkGreen.obj");
                    else LoadModels(AllInfos[k], k);
                }
                if (AllInfos.ContainsKey("AreaObjInfo")) HideLayer("AreaObjInfo");
                if (AllInfos.ContainsKey("CameraAreaInfo")) HideLayer("CameraAreaInfo");
                checkBox1.Checked = true;
                checkBox1.CheckedChanged += checkBox1_CheckedChanged;
                if (comboBox1.Items.Contains("ObjInfo")) comboBox1.Text = "ObjInfo"; else comboBox1.Text = comboBox1.Items[0].ToString();
                if (ObjectsListBox.Items.Count > 0)
                {
                    ObjectsListBox.SelectedIndex = 0;
                    render.CameraToObj(CurrentAllInfosSectionName, 0);
                }
            }
        }

        void LoadRailsModels(List<Rail> source)
        {
            foreach (Rail r in source)
            {
                render.addRail(r.GetPointArray(), 5, -1);
            }
        }

        void LoadRailsModels(Rail source, int at = -1)
        {
            List<Point3D> points = new List<Point3D>();
            foreach (Rail.Point p in source.Points) points.Add(new Point3D(p.X, -p.Z, p.Y));
            render.addRail(points.ToArray(), 5, at);
        }

        void LoadModels(List<LevelObj> Source, string Type, string PlaceHolderMod = "models\\UnkBlue.obj", int at = -1)
        {
            for (int i = 0; i < Source.Count; i++)
            {
                string Path = GetModelname(Source[i].ToString());
                if (!System.IO.File.Exists(Path)) Path = PlaceHolderMod;
                Single X, Y, Z, ScaleX, ScaleY, ScaleZ, RotX, RotY, RotZ, A, B, C;
                Single[] scale_XDD;
                Single[] rotation;
                Single[] position;
                if (Source[i].Prop.ContainsKey("pos") == true)
                { position = (Single[])Source[i].Prop["pos"]; }
                else
                {
                    if (Source[i].Prop["pos_x"] != null)
                    {
                        A = Single.Parse(((Node)Source[i].Prop["pos_x"]).StringValue);
                        B = Single.Parse(((Node)Source[i].Prop["pos_y"]).StringValue);
                        C = Single.Parse(((Node)Source[i].Prop["pos_z"]).StringValue);
                        position = new Single[3] { A, B, C };

                        Source[i].Prop.Add("pos", position);
                        Source[i].Prop.Remove("pos_x");
                        Source[i].Prop.Remove("pos_y");
                        Source[i].Prop.Remove("pos_z");
                    }
                    else
                    {
                        position = new Single[3] { 1, 1, 1 };
                    }
                }
                X = position[0];
                Y = position[1];
                Z = position[2];
                //X = Single.Parse(((Node)Source[i].Prop["pos_x"]).StringValue);
                //Y = Single.Parse(((Node)Source[i].Prop["pos_y"]).StringValue);
                //Z = Single.Parse(((Node)Source[i].Prop["pos_z"]).StringValue);
                if (Source[i].Prop.ContainsKey("scale") == true)
                { scale_XDD = (Single[])Source[i].Prop["scale"]; }
                else
                {
                    if (Source[i].Prop["scale_x"] != null)
                    {
                        A = Single.Parse(((Node)Source[i].Prop["scale_x"]).StringValue);
                        B = Single.Parse(((Node)Source[i].Prop["scale_y"]).StringValue);
                        C = Single.Parse(((Node)Source[i].Prop["scale_z"]).StringValue);
                        scale_XDD = new Single[3] { A,B,C };

                        Source[i].Prop.Add("scale", scale_XDD);
                        Source[i].Prop.Remove("scale_x");
                        Source[i].Prop.Remove("scale_y");
                        Source[i].Prop.Remove("scale_z");

                        //cl.Scale = (Single[])((Single[])GetListByName(type)[index].Prop["scale"]).Clone();
                        /*Single[] test;
                        test = (Single[])((Single[])Source[i].Prop["scale"]).Clone();
                        string text;
                            text = test[2].ToString();
                        MessageBox.Show("test " + text);*/
                    }
                    else
                    {
                        scale_XDD = new Single[3] { 1, 1, 1 };
                    }
                }
                ScaleX = scale_XDD[0];
                ScaleY = scale_XDD[1];
                ScaleZ = scale_XDD[2];
                if (Source[i].Prop.ContainsKey("dir") == true)
                { rotation = (Single[])Source[i].Prop["dir"]; }
                else
                {
                    if (Source[i].Prop["dir_x"] != null)
                    {
                        A = Single.Parse(((Node)Source[i].Prop["dir_x"]).StringValue);
                        B = Single.Parse(((Node)Source[i].Prop["dir_y"]).StringValue);
                        C = Single.Parse(((Node)Source[i].Prop["dir_z"]).StringValue);
                        rotation = new Single[3] { A, B, C };

                        Source[i].Prop.Add("dir", rotation);
                        Source[i].Prop.Remove("dir_x");
                        Source[i].Prop.Remove("dir_y");
                        Source[i].Prop.Remove("dir_z");
                    }
                    else
                    {
                        rotation = new Single[3] { 1, 1, 1 };
                    }
                }
                RotX = rotation[0];
                RotY = rotation[1];
                RotZ = rotation[2];
                render.addModel(Path, Type, new Vector3D(X, -Z, Y), new Vector3D(ScaleX, ScaleZ, ScaleY), RotX, -RotZ, RotY, at);
            }
        }

        public void AddChildrenModels(C0List tmp, bool area)
        {
            string setlng;
            if (Properties.Settings.Default.DotComma != true) { setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            if (tmp.List.Count > 0)
            {
                List<string> modelsPaths = new List<string>();
                List<Vector3D> Pos = new List<Vector3D>();
                List<Vector3D> Rot = new List<Vector3D>();
                List<Vector3D> Scale = new List<Vector3D>();
                foreach (LevelObj o in tmp.List)
                {
                    string Path;
                    if (area) Path = "models\\UnkYellow.obj"; else
                    {
                        Path = GetModelname(o.ToString());
                        if (!System.IO.File.Exists(Path)) Path = "models\\UnkRed.obj";
                    }
                    Single X, Y, Z, ScaleX, ScaleY, ScaleZ, RotX, RotY, RotZ, A, B, C;
                    Single[] scale_XDD;
                    Single[] rotation;
                    Single[] position;
                    if (o.Prop.ContainsKey("pos") == true)
                    { position = (Single[])o.Prop["pos"]; }
                    else
                    {
                        if (o.Prop["pos_x"] != null)
                        {
                            A = Single.Parse(((Node)o.Prop["pos_x"]).StringValue);
                            B = Single.Parse(((Node)o.Prop["pos_y"]).StringValue);
                            C = Single.Parse(((Node)o.Prop["pos_z"]).StringValue);
                            position = new Single[3] { A, B, C };

                            o.Prop.Add("pos", position);
                            o.Prop.Remove("pos_x");
                            o.Prop.Remove("pos_y");
                            o.Prop.Remove("pos_z");
                        }
                        else
                        {
                            position = new Single[3] { 1, 1, 1 };
                        }
                    }
                    X = position[0];
                    Y = position[1];
                    Z = position[2];
                    //X = Single.Parse(((Node)Source[i].Prop["pos_x"]).StringValue);
                    //Y = Single.Parse(((Node)Source[i].Prop["pos_y"]).StringValue);
                    //Z = Single.Parse(((Node)Source[i].Prop["pos_z"]).StringValue);
                    if (o.Prop.ContainsKey("scale") == true)
                    { scale_XDD = (Single[])o.Prop["scale"]; }
                    else
                    {
                        if (o.Prop["scale_x"] != null)
                        {
                            A = Single.Parse(((Node)o.Prop["scale_x"]).StringValue);
                            B = Single.Parse(((Node)o.Prop["scale_y"]).StringValue);
                            C = Single.Parse(((Node)o.Prop["scale_z"]).StringValue);
                            scale_XDD = new Single[3] { A, B, C };

                            o.Prop.Add("scale", scale_XDD);
                            o.Prop.Remove("scale_x");
                            o.Prop.Remove("scale_y");
                            o.Prop.Remove("scale_z");
                        }
                        else
                        {
                            scale_XDD = new Single[3] { 1, 1, 1 };
                        }
                    }
                    ScaleX = scale_XDD[0];
                    ScaleY = scale_XDD[1];
                    ScaleZ = scale_XDD[2];
                    if (o.Prop.ContainsKey("dir") == true)
                    { rotation = (Single[])o.Prop["dir"]; }
                    else
                    {
                        if (o.Prop["dir_x"] != null)
                        {
                            A = Single.Parse(((Node)o.Prop["dir_x"]).StringValue);
                            B = Single.Parse(((Node)o.Prop["dir_y"]).StringValue);
                            C = Single.Parse(((Node)o.Prop["dir_z"]).StringValue);
                            rotation = new Single[3] { A, B, C };

                            o.Prop.Add("dir", rotation);
                            o.Prop.Remove("dir_x");
                            o.Prop.Remove("dir_y");
                            o.Prop.Remove("dir_z");
                        }
                        else
                        {
                            rotation = new Single[3] { 1, 1, 1 };
                        }
                    }
                    RotX = rotation[0];
                    RotY = rotation[1];
                    RotZ = rotation[2];
                    Pos.Add(new Vector3D(X, -Z, Y));
                    Rot.Add(new Vector3D(RotX, -RotZ, RotY));
                    Scale.Add(new Vector3D(ScaleX, ScaleZ, ScaleY));
                    modelsPaths.Add(Path);
                }
                render.AddTmpObjects(Pos, Scale, Rot, modelsPaths, area ? "TmpAreaChildrenObjs" : "TmpChildrenObjs");
            }
        }

        string GetModelname(string ObjName)
        {
            if (CustomModels.Contains(ObjName)) return "CustomModels\\" + ObjName + ".obj";
            else if (ObjectDatabase != null && ObjectDatabase.IdToModel.ContainsKey(ObjName)) return "models\\" + ObjectDatabase.IdToModel[ObjName] + ".obj";
            else return "models\\" + ObjName + ".obj";
        }

        void ProcessAllInfos(XmlNodeList xml)
        {
            for (int i = 0; i < xml.Count; i++)
            {
                ProcessAllOBJECTS(xml[i].ChildNodes, xml[i].Attributes["Name"].Value);
            }
        }

        void ProcessRailInfos(XmlNodeList xml)
        {
            if (!higestID.ContainsKey("AllRailInfos")) higestID.Add("AllRailInfos", 0);
            for (int i = 0; i < xml.Count; i++)
            {
                foreach (XmlNode node in xml[i].ChildNodes) AllRailInfos.Add(LoadRail(node.ChildNodes, "AllRailInfos"));
            }
        }

        void ProcessAllOBJECTS(XmlNodeList xml, string Type)
        {
            if (!AllInfos.ContainsKey(Type)) AllInfos.Add(Type, new AllInfoSection());
            foreach (XmlNode N in xml) GetListByName(Type).Add(LoadOBJECT(N.ChildNodes, Type));
        }

        Rail LoadRail(XmlNodeList xml, string Type)
        {
            if (!higestID.ContainsKey(Type)) higestID.Add(Type, 0);
            Rail Ret = new Rail();
            List<int> Args = new List<int>();
            for (int i = 0; i < xml.Count; i++)
            {
                XmlNode xNode = xml[i];
                if (xNode.NodeType == XmlNodeType.Element)
                {
                    if (xNode.Attributes["Name"].Value.StartsWith("Arg")) Args.Add(Int32.Parse(xNode.Attributes["StringValue"].Value));
                    else if (xNode.Attributes["Name"].Value == "LayerName") Ret.LayerName = xNode.Attributes["StringValue"].Value;
                    else if (xNode.Attributes["Name"].Value == "closed") Ret._closed = xNode.Attributes["StringValue"].Value;
                    else if (xNode.Attributes["Name"].Value == "l_id") Ret.l_id = Int32.Parse(xNode.Attributes["StringValue"].Value);
                    else if (xNode.Attributes["Name"].Value == "name") Ret.Name = xNode.Attributes["StringValue"].Value;
                    else if (xNode.Attributes["Name"].Value == "no") Ret.no = Int32.Parse(xNode.Attributes["StringValue"].Value);
                    else if (xNode.Attributes["Name"].Value == "type") Ret.Type = xNode.Attributes["StringValue"].Value;
                    else if (xNode.Attributes["Name"].Value == "Points")
                    {
                        XmlNodeList PointsList = xNode.ChildNodes;
                        foreach (XmlNode Points in PointsList)
                        {
                            Rail.Point P = new Rail.Point();
                            List<int> _Args = new List<int>();
                            List<Single> _X = new List<Single>();
                            List<Single> _Y = new List<Single>();
                            List<Single> _Z = new List<Single>();
                            foreach (XmlNode Point in Points.ChildNodes)
                            {
                                if (Point.Attributes["Name"].Value.StartsWith("Arg")) _Args.Add(Int32.Parse(Point.Attributes["StringValue"].Value));
                                if (Point.Attributes["Name"].Value.EndsWith("_x")) _X.Add(Single.Parse(Point.Attributes["StringValue"].Value));
                                if (Point.Attributes["Name"].Value.EndsWith("_y")) _Y.Add(Single.Parse(Point.Attributes["StringValue"].Value));
                                if (Point.Attributes["Name"].Value.EndsWith("_z")) _Z.Add(Single.Parse(Point.Attributes["StringValue"].Value));
                                if (Point.Attributes["Name"].Value == "id") P.ID = (Int32.Parse(Point.Attributes["StringValue"].Value));
                            }
                            P.Args = _Args;
                            P._X = _X;
                            P._Y = _Y;
                            P._Z = _Z;
                            Ret.Points.Add(P);
                        }
                    }
                    if (xNode.Attributes["Name"].Value == "l_id") if (Int32.Parse(xNode.Attributes["StringValue"].Value) > higestID[Type]) higestID[Type] = Int32.Parse(xNode.Attributes["StringValue"].Value);
                }
            }
            if (Args.Count != 0) Ret.Arg = Args;
            return Ret;
        }

        LevelObj LoadOBJECT(XmlNodeList xml, string Type)
        {
            if (!higestID.ContainsKey(Type)) higestID.Add(Type, 0);
            LevelObj Ret = new LevelObj();
            List<int> Args = new List<int>();
            for (int i = 0; i < xml.Count; i++)
            {
                XmlNode xNode = xml[i];
                if (xNode.NodeType == XmlNodeType.Element)
                {
                    if (xNode.Attributes["Name"].Value.StartsWith("Arg")) Args.Add(Int32.Parse(xNode.Attributes["StringValue"].Value));
                    else
                    {
                        if (xNode.Name == "C1")
                        {
                            if (xNode.Attributes["Name"].Value == "Rail") Ret.Prop.Add("Rail", LoadRail(xNode.ChildNodes, "AllRailInfos"));
                            else throw new Exception("C1 type not implemented :(");
                        }
                        else if (xNode.Name == "C0")
                        {
                            C0List c0Section = new C0List();
                            XmlNodeList objList = xNode.ChildNodes;
                            foreach (XmlNode Object in objList)
                            {
                                c0Section.List.Add(LoadOBJECT(Object.ChildNodes, Type));
                            }
                            Ret.Prop.Add(xNode.Attributes["Name"].Value, c0Section);
                        }
                        else
                            Ret.Prop.Add(xNode.Attributes["Name"].Value, new Node(xNode.Attributes["StringValue"].Value, xNode.Name));
                        if (xNode.Attributes["Name"].Value == "l_id") if (Int32.Parse(xNode.Attributes["StringValue"].Value) > higestID[Type]) higestID[Type] = Int32.Parse(xNode.Attributes["StringValue"].Value);
                    }
                }
            }
            if (Args.Count != 0) Ret.Prop.Add("Arg", Args.ToArray());
            return Ret;
        }
        #endregion

        #region ObjectsEvents

        #region Find
        private void objectByIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput();
            f.ShowDialog();
            if (f.Res == null) return;
            FindIndex(comboBox1.Text, "l_id", Convert.ToInt32(f.Res));
        }

        private void objectByCameraIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput();
            f.ShowDialog();
            if (f.Res == null) return;
            FindIndex(comboBox1.Text, "CameraId", Convert.ToInt32(f.Res));
        }

        private void Switch___FindClick(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput();
            f.ShowDialog();
            if (f.Res == null) return;
            FindIndex(comboBox1.Text, ((ToolStripMenuItem)sender).Text, Convert.ToInt32(f.Res));
        }

        private void objectByViewIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput();
            f.ShowDialog();
            if (f.Res == null) return;
            FindIndex(comboBox1.Text, "ViewId", Convert.ToInt32(f.Res));
        }

        private void objectByRailNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput(true);
            f.ShowDialog();
            if (f.Res == null || (string)f.Res == "") return;
            FindIndex(comboBox1.Text, "Rail", (string)f.Res);
        }
        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         /*   if (comboBox1.Text != "AllRailInfos")
            {
                if (ObjectsListBox.Items.Count < )
                {
                    panel1.Visible = false;
                    panel1.Enabled = false;
                }
                else
                {
                    panel1.Visible = true;
                    panel1.Enabled = true;
                }
            }
            else
            {*/
                panel1.Visible = false;
                panel1.Enabled = false;
            button4.Visible = false;
            button5.Visible = false;
            /*  }*/
            IncType.SelectedIndex = 0;
            IncAxis.SelectedIndex = 0;
            btn_cameraCode.Visible = false;
            ObjectsListBox.Items.Clear();
            propertyGrid1.SelectedObject = null;
            if (!AllInfos.ContainsKey(comboBox1.Text))
            {
                if (comboBox1.Text == "AllRailInfos")
                {
                    checkBox1.Visible = false;
                    checkBox2.Visible = false;
                    for (int i = 0; i < AllRailInfos.Count; i++) ObjectsListBox.Items.Add(AllRailInfos[i].ToString());
                }
                ObjectsListBox.SelectionMode = SelectionMode.One;
                return;
            }
            else
            {
                ObjectsListBox.SelectionMode = SelectionMode.MultiExtended;
                if (comboBox1.Text == "AreaObjInfo" || comboBox1.Text == "CameraAreaInfo")
                {
                    checkBox1.Visible = true;
                    checkBox2.Visible = false;
                    if (((AllInfoSection)CurrentAllInfosSection).IsHidden) checkBox1.Checked = true; else checkBox1.Checked = false;
                }
                else
                {
                    checkBox1.Visible = false;
                    checkBox2.Visible = true;
                }
            }
            for (int i = 0; i < CurrentAllInfosSection.Count; i++) ObjectsListBox.Items.Add(CurrentAllInfosSection[i].ToString());
        }

        private void render_LeftClick(object sender, MouseButtonEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control || RenderIsDragging) return;
            object[] indexes = render.GetOBJ(sender, e); //indexes[0] string, [1] int
            if (indexes[0] == null) return; //this means indexes[0] = -1
            if ((string)indexes[0] == "SelectedRail" || (string)indexes[0] == "TmpChildrenObjs" || (string)indexes[0] == "TmpAreaChildrenObjs" || (IsEditingC0List && (string)indexes[0] != "C0EditingListObjs")) return;
            if ((ModifierKeys & Keys.Shift) == Keys.Shift && (string)indexes[0] == CurrentAllInfosSectionName)
            {
                ObjectsListBox.SelectedIndices.Add((int)indexes[1]);
            }
            else
            {
                if (RenderIsDragging == true) { return; }
                if (DraggingAxis[0] == true) { return; }
                if (DraggingAxis[1] == true) { return; }
                if (DraggingAxis[2] == true) { return; }
                if ((string)indexes[0] != "C0EditingListObjs") comboBox1.SelectedIndex = comboBox1.Items.IndexOf((string)indexes[0]);
                ObjectsListBox.ClearSelected();
                ObjectsListBox.SelectedIndex = (int)indexes[1];
            }
        }

        bool RenderIsDragging = false;
        object[] DraggingArgs = null;
        bool[] DraggingAxis = new bool[5];
        Vector3D StartPos;

        private void render_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) //Render hotkeys
        {
            if (e.Key == Key.X)
            {
                if (RenderIsDragging == true)
                {
                    if (DraggingAxis[0] == false && (DraggingAxis[1] == false || DraggingAxis[2] == false) && (string)DraggingArgs[0] != "SelectedRail")
                    { 
                        DraggingAxis[0] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "X");
                    }
                    else if (DraggingAxis[0] == true && (DraggingAxis[1] == false || DraggingAxis[2] == false))
                    {
                        DraggingAxis[0] = false;
                        render.removeAxis("X");
                    }
                    else if (DraggingAxis[0] == false && (DraggingAxis[1] == true || DraggingAxis[2] == true))
                    {
                        DraggingAxis[0] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "X");
                        render.removeAxis("Y");
                        DraggingAxis[1] = false;
                        render.removeAxis("Z");
                        DraggingAxis[2] = false;
                    }
                }
            }
            if (e.Key == Key.Y)
            {
                if (RenderIsDragging == true)
                {
                    if (DraggingAxis[1] == false && (DraggingAxis[0] == false || DraggingAxis[2] == false)&& (string)DraggingArgs[0]!="SelectedRail")
                    {
                        DraggingAxis[1] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "Y");
                    }
                    else if (DraggingAxis[1] == true && (DraggingAxis[0] == false || DraggingAxis[2] == false))
                    {
                        DraggingAxis[1] = false;
                        render.removeAxis("Y");
                    }
                    else if (DraggingAxis[1] == false && (DraggingAxis[0] == true || DraggingAxis[2] == true))
                    {
                        DraggingAxis[1] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "Y");
                        render.removeAxis("X");
                        DraggingAxis[0] = false;
                        render.removeAxis("Z");
                        DraggingAxis[2] = false;
                    }
                }
            }
            if (e.Key == Key.S)
            {
                if (RenderIsDragging == true)
                {
                    if (DraggingAxis[3] == false)
                    {
                        DraggingAxis[4] = false;
                        DraggingAxis[3] = true;
                    }
                    else { DraggingAxis[3] = false; }
                }
            }
            if (e.Key == Key.R)
            {
                if (RenderIsDragging == true)
                {
                    if (DraggingAxis[4] == false)
                    {
                        DraggingAxis[4] = true;
                        DraggingAxis[3] = false;
                    }
                    else { DraggingAxis[4] = false; }
                }
            }
            if (e.Key == Key.Z)
            {
                if (Undo.Count > 0 && RenderIsDragging==false) Undo.Pop().Undo();
                else if (RenderIsDragging == true)
                {
                    if (DraggingAxis[2] == false && (DraggingAxis[0] == false || DraggingAxis[1] == false) && (string)DraggingArgs[0] != "SelectedRail")
                    {
                        DraggingAxis[2] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "Z");
                    }
                    else if (DraggingAxis[2] == true && (DraggingAxis[0] == false || DraggingAxis[1] == false))
                    {
                        DraggingAxis[2] = false;
                        render.removeAxis("Z");
                    }
                    else if (DraggingAxis[2] == false && (DraggingAxis[0] == true || DraggingAxis[1] == true))
                    {
                        DraggingAxis[2] = true;
                        Single[] sing = ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"]);
                        Point3D axiss = new Point3D(sing[0], -sing[2], sing[1]);
                        render.addAxis(axiss, "Z");
                        render.removeAxis("X");
                        DraggingAxis[0] = false;
                        render.removeAxis("Y");
                        DraggingAxis[1] = false;
                    }
                }
                return;
            }
            else if (e.Key == Key.B && comboBox1.Text != "AllRailInfos")
            {
                if (IsEditingC0List) C0ListEditorGoBack();
            }
            if (ObjectsListBox.SelectedIndex == -1) return;
            if (comboBox1.Text == "AllRailInfos")
            {
                if (propertyGrid1.SelectedObject is Rail && e.Key == Key.N)
                {
                    Rail tmp = (Rail)propertyGrid1.SelectedObject;
                    tmp.Points.Add(tmp.Points[tmp.Points.Count - 1].Clone_increment());
                    propertyGrid1.SelectedObject = AllRailInfos[ObjectsListBox.SelectedIndex];
                    UpdateRailpos(ObjectsListBox.SelectedIndex, AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray());
                    render.SelectRail(AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray());
                }
            }
            if (e.Key == Key.Space) render.CameraToObj(CurrentAllInfosSectionName, ObjectsListBox.SelectedIndex);
            else if (e.Key == Key.OemPlus) { if (Btn_AddObj.Enabled == true) BtnAddObj_Click(null, null); } //Add obj
            else if (e.Key == Key.D && ObjectsListBox.SelectedItems.Count == 1) button2_Click(null, null); //Duplicate
            else if (e.Key == Key.Delete) button3_Click(null, null); //Delete obj
            else if (e.Key == Key.F) findToolStripMenuItem.ShowDropDown();
            else if (e.Key == Key.R && comboBox1.Text != "AllRailInfos") //Round selected object position to a multiple of 100
            {
                if (RenderIsDragging) return;
                string type = comboBox1.Text;
                int id = ObjectsListBox.SelectedIndex;
                ((Single[])GetListByName(type)[id].Prop["pos"])[0] = ((Single)Math.Round((((Single[])GetListByName(type)[id].Prop["pos"])[0]) / 100d, 0) * 100);
                ((Single[])GetListByName(type)[id].Prop["pos"])[1] = ((Single)Math.Round((((Single[])GetListByName(type)[id].Prop["pos"])[1]) / 100d, 0) * 100);
                ((Single[])GetListByName(type)[id].Prop["pos"])[2] = ((Single)Math.Round((((Single[])GetListByName(type)[id].Prop["pos"])[2]) / 100d, 0) * 100);
                UpdateOBJPos(id, GetListByName(type), type);
                propertyGrid1.Refresh();
            }
            else if (e.Key == Key.C && comboBox1.Text != "AllRailInfos" && ObjectsListBox.SelectedItems.Count == 1)
            {
                if (CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop.ContainsKey("GenerateChildren"))
                {
                    EditC0List(CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"] as C0List);
                }
            }
            else return;
        }

        private void Listbox_keyDown(object sender, System.Windows.Forms.KeyEventArgs e) //Listbox hotkeys
        {
            if (e.KeyCode == Keys.Z && e.Control)
            {
                if (Undo.Count > 0) Undo.Pop().Undo();
                return;
            }
            else if (e.KeyCode == Keys.B && comboBox1.Text != "AllRailInfos")
            {
                if (IsEditingC0List) C0ListEditorGoBack();
            }
            if (ObjectsListBox.SelectedIndex == -1) return;
            if (e.KeyCode == Keys.Space) render.CameraToObj(CurrentAllInfosSectionName, ObjectsListBox.SelectedIndex);
            else if (e.KeyCode == Keys.D && e.Control && ObjectsListBox.SelectedItems.Count == 1) button2_Click(null, null); //Duplicate
            else if (e.KeyCode == Keys.Delete) button3_Click(null, null); //Delete obj
            else if (e.KeyCode == Keys.F && e.Control) findToolStripMenuItem.ShowDropDown();
            else if (comboBox1.Text != "AllRailInfos" && e.KeyCode == Keys.Oemplus) { if (Btn_AddObj.Enabled == true) BtnAddObj_Click(null, null); } //Add obj            
            else if (comboBox1.Text != "AllRailInfos" && e.KeyCode == Keys.R && e.Control) //Round selected object position to a multiple of 100
            {
                if (RenderIsDragging) return;
                string type = comboBox1.Text;
                int id = ObjectsListBox.SelectedIndex;
                ((Single[])GetListByName(type)[id].Prop["pos"])[0] = ((Single)Math.Round((((Single[])GetListByName(type)[id].Prop["pos"])[0]) / 100d, 0) * 100);
                ((Single[])GetListByName(type)[id].Prop["pos"])[1] = ((Single)Math.Round((((Single[])GetListByName(type)[id].Prop["pos"])[1]) / 100d, 0) * 100);
                ((Single[])GetListByName(type)[id].Prop["pos"])[2] = ((Single)Math.Round((((Single[])GetListByName(type)[id].Prop["pos"])[2]) / 100d, 0) * 100);
                UpdateOBJPos(id, GetListByName(type), type);
                propertyGrid1.Refresh();
            }
            else if (e.KeyCode == Keys.C && comboBox1.Text != "AllRailInfos" && ObjectsListBox.SelectedItems.Count == 1)
            {
                if (CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop.ContainsKey("GenerateChildren"))
                {
                    EditC0List(CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"] as C0List);
                }
            }
            else return;
            e.SuppressKeyPress = true;
        }


        private void render_KeyUP(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                if (DraggingArgs != null) endDragging();
                RenderIsDragging = false;
                DraggingArgs = null;
                propertyGrid1.Refresh();
            }
        }

        void UpdateRailpos(int id, Point3D[] Points)
        {
            render.UpdateRailpos(id, Points);
            if (comboBox1.SelectedItem.ToString() == "AllRailInfos" && ObjectsListBox.SelectedIndex != -1) render.SelectRail(AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray());
        }

        private void render_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed || (ModifierKeys & Keys.Control) != Keys.Control || !RenderIsDragging) { RenderIsDragging = false; return; }
            int RoundTo = (ModifierKeys & Keys.Alt) == Keys.Alt ? 100 : ((ModifierKeys & Keys.Shift) == Keys.Shift ? 50 : 0);
            Vector3D NewPos = render.Drag(DraggingArgs, e, RoundTo);
            if (NewPos == null) return;
            if ((string)DraggingArgs[0] == "SelectedRail")
            {
                AllRailInfos[ObjectsListBox.SelectedIndex].Points[(int)DraggingArgs[1]].X = (float)NewPos.X;
                AllRailInfos[ObjectsListBox.SelectedIndex].Points[(int)DraggingArgs[1]].Y = (float)NewPos.Z;
                AllRailInfos[ObjectsListBox.SelectedIndex].Points[(int)DraggingArgs[1]].Z = -(float)NewPos.Y;
                UpdateRailpos(ObjectsListBox.SelectedIndex, AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray());
            }
            else if ((string)DraggingArgs[0] == "TmpChildrenObjs")
            {
                ((Single[])((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[0] = (Single)NewPos.X;
                ((Single[])((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[1] = (Single)NewPos.Z;
                ((Single[])((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[2] = -(Single)NewPos.Y;
                UpdateOBJPos((int)DraggingArgs[1], null, "TmpChildrenObjs");
            }
            else if ((string)DraggingArgs[0] != "AllRailInfos")
            {
                string type = "pos";
                //if (DraggingAxis[3]==true) { type = "scale"; }
                //if (DraggingAxis[4]==true) { type = "dir"; }
                if ((DraggingAxis[0] == false && DraggingAxis[1] == false && DraggingAxis[2] == false)|| (DraggingAxis[0] == true && DraggingAxis[1] == true && DraggingAxis[2] == true))
                {
                    ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[0] = (Single)NewPos.X;
                    ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[1] = (Single)NewPos.Z;
                    ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[2] = -(Single)NewPos.Y;
                    //render.removeAxis();

                }
                else
                {
                    if(DraggingAxis[0] == true)
                    {


                        ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[0] = (Single)NewPos.X;
                    }
                    if (DraggingAxis[1] == true)
                    {
                        ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[1] = (Single)NewPos.Z;
                    }
                    if (DraggingAxis[2] == true)
                    {
                        ((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop[type])[2] = -(Single)NewPos.Y;
                    }
                }

                UpdateOBJPos((int)DraggingArgs[1], GetListByName((string)DraggingArgs[0]), (string)DraggingArgs[0]);
            }
            DraggingArgs[2] = NewPos;
        }

        void endDragging()
        {
            if (DraggingArgs[0] == null || DraggingArgs[1] == null || DraggingArgs[2] == null) return;
            if (IsEditingC0List && (string)DraggingArgs[0] != "C0EditingListObjs") return;

            if (DraggingAxis[0] == true) { render.removeAxis("X"); DraggingAxis[0] = false; }
            if (DraggingAxis[1] == true) { render.removeAxis("Y"); DraggingAxis[1] = false; }
            if (DraggingAxis[2] == true) { render.removeAxis("Z"); DraggingAxis[2] = false; }
            if ((string)DraggingArgs[0] == "SelectedRail")
            {
                Action<object[]> act;
                act = (object[] args) =>
                {
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].X = (float)((Vector3D)args[2]).X;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].Y = (float)((Vector3D)args[2]).Z;
                    AllRailInfos[(int)args[0]].Points[(int)args[1]].Z = -(float)((Vector3D)args[2]).Y;
                    UpdateRailpos((int)args[0], AllRailInfos[(int)args[0]].GetPointArray());
                    propertyGrid1.Refresh();
                };
                Undo.Push(new UndoAction("Moved " + ObjectsListBox.SelectedItem.ToString() + "'s point[" + DraggingArgs[1].ToString() + "] : ", new object[] { ObjectsListBox.SelectedIndex, (int)DraggingArgs[1], StartPos }, act));
            }
            else if ((string)DraggingArgs[0] == "TmpChildrenObjs")
            {
                Action<object[]> act;
                act = (object[] args) =>
                {
                    render.ClearSelection();
                    List<LevelObj> type = (List<LevelObj>)args[0];
                    int id = (int)args[1];
                    int idInList = (int)args[2];
                    Vector3D pos = (Vector3D)args[3];
                    SetComboboxToLevObjList(type);
                    ObjectsListBox.SelectedIndex = id;
                    ((Single[])((C0List)type[id].Prop["GenerateChildren"]).List[idInList].Prop["pos"])[0]= (Single)pos.X;
                    ((Single[])((C0List)type[id].Prop["GenerateChildren"]).List[idInList].Prop["pos"])[1]= (Single)pos.Y;
                    ((Single[])((C0List)type[id].Prop["GenerateChildren"]).List[idInList].Prop["pos"])[2]= (Single)pos.Z;
                    propertyGrid1.Refresh();
                };
                Undo.Push(new UndoAction("Moved children object of: " + CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString(), new object[] { CurrentAllInfosSection, ObjectsListBox.SelectedIndex, (int)DraggingArgs[1] , StartPos }, act));
            }
            else if ((string)DraggingArgs[0] != "AllRailInfos")
            {
                Action<object[]> act;
                act = (object[] args) =>
                {
                    render.ClearSelection();
                    List<LevelObj> type = (List<LevelObj>)args[0];
                    int id = (int)args[1];
                    Vector3D pos = (Vector3D)args[2];
                    ((Single[])type[id].Prop["pos"])[0] = (Single)pos.X; //These values were stored directly
                    ((Single[])type[id].Prop["pos"])[1] = (Single)pos.Y;
                    ((Single[])type[id].Prop["pos"])[2] = (Single)pos.Z;
                    string typename = (string)args[3];
                    if (typename != "C0EditingListObjs" || type.GetHashCode() == CurrentAllInfosSection.GetHashCode())
                    {
                        UpdateOBJPos(id, type, typename, true);
                    }
                    if (ObjectsListBox.SelectedIndex == id) RefreshTmpChildrenObjects();
                    propertyGrid1.Refresh();
                };
                Undo.Push(new UndoAction("Moved object : " + GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].ToString(), new object[] { GetListByName((string)DraggingArgs[0]), DraggingArgs[1], StartPos, DraggingArgs[0] }, act));
            }
        }

        private void render_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DraggingArgs != null) endDragging();
            RenderIsDragging = false;
            DraggingArgs = null;
            propertyGrid1.Refresh();
        }

        private void render_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) != Keys.Control || RenderIsDragging) return;
            RenderIsDragging = true;
            DraggingArgs = render.GetOBJ(sender, e);
            if (DraggingArgs[0] == null) { RenderIsDragging = false; return; }
            if ((string)DraggingArgs[0] == "SelectedRail" && !IsEditingC0List)
            {
                StartPos = AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray()[(int)DraggingArgs[1]].ToVect();
                return;
            }
            else if ((string)DraggingArgs[0] == "TmpChildrenObjs")
            {
                StartPos = new Vector3D(
                    ((Single[])((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[0],
                    ((Single[])((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[1],
                    ((Single[])((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"]).List[(int)DraggingArgs[1]].Prop["pos"])[2]);
            }
            else if ((string)DraggingArgs[0] == "C0EditingListObjs")
            {
                ObjectsListBox.ClearSelected();
                ObjectsListBox.SelectedIndex = (int)DraggingArgs[1];
                    StartPos = new Vector3D((Single)((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[0],
                       (Single)((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[1],
                       (Single)((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[2]);
            }
            else if (!IsEditingC0List)
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf((string)DraggingArgs[0]);
                ObjectsListBox.ClearSelected();
                ObjectsListBox.SelectedIndex = (int)DraggingArgs[1];
                if ((string)DraggingArgs[0] != "AllRailInfos")
                    StartPos = new Vector3D(
                    float.Parse(((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[0].ToString()),
                    float.Parse(((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[1].ToString()),
                    float.Parse(((Single[])GetListByName((string)DraggingArgs[0])[(int)DraggingArgs[1]].Prop["pos"])[2].ToString()));
            }
            else { RenderIsDragging = false; return; }
        }

        private void ZoomCheckWarning_Tick(object sender, EventArgs e)
        {
            //Maybe not the best way to accomplish this, but it works...
            double d = Math.Abs(render.TooCloseCheck());
            if (d <= 0.1) label3.Text = "You are zooming in too much, the camera may glitch, zoom out to fix.";
            else if (d > 1000000)
            {
                if (ObjectsListBox.SelectedIndex == -1) label3.Text = "You are too far from the level, select an object and press space.";
                else label3.Text = "You are too far from the selected object, press space.";
            }
            else label3.Text = "";
        }

        #region Hiding layers
        void HideLayer(string layerName)
        {
            AllInfos[layerName].IsHidden = true;
            render.HideGroup(layerName);
        }

        void ShowLayer(string layerName)
        {
            AllInfos[layerName].IsHidden = false;
            for (int i = 0; i < AllInfos[layerName].Count; i++) UpdateOBJPos(i, AllInfos[layerName], layerName);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                ShowLayer(comboBox1.Text);
            else HideLayer(comboBox1.Text);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            RefreshTmpChildrenObjects();
        }
        #endregion

        int AreaObjOldSelection = -1;
        int CameraAreaOldSelection = -1;

        private void ObjectsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_cameraCode.Visible = false;
            render.ClearTmpObjects();
            render.UnselectRail();
            render.ClearSelection();
            lblDescription.Text = "";
            lblDescription.Tag = -1;
            if (ObjectsListBox.SelectedIndex < 0)
            {
                panel1.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                return;
            }
            else
            {
                if (comboBox1.SelectedItem != "AllRailInfos")
                {
                    panel1.Visible = true;
                    panel1.Enabled = true;
                }

                button4.Visible = true;
                button5.Visible = true;
            }
            if (ObjectsListBox.SelectedItems.Count > 1)
            {

                Btn_CopyObjs.Visible = true;
                Btn_Duplicate.Visible = false;
                button4.Enabled = false;
                btn_delObj.Text = "Delete objects";
                if (comboBox1.Text != "AllRailInfos")
                {
                    object[] SelectedObjects = new object[ObjectsListBox.SelectedItems.Count];
                    int index = 0;
                    foreach (int i in ObjectsListBox.SelectedIndices) SelectedObjects[index++] = new DictionaryPropertyGridAdapter(CurrentAllInfosSection[i].Prop);
                    propertyGrid1.SelectedObjects = SelectedObjects;
                    render.SelectObjs(CurrentAllInfosSectionName, ObjectsListBox.SelectedIndices);
                }
                else propertyGrid1.SelectedObject = null;
                return;
            }
            else
            {
                Btn_CopyObjs.Visible = false;
                Btn_Duplicate.Visible = true;
                button4.Enabled = true;
                btn_delObj.Text = "Delete object";
                if (ObjectDatabase != null) UpdateHint();
            }

            if (comboBox1.Text == "AreaObjInfo")
            {
                propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter(CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop);
                if (((AllInfoSection)CurrentAllInfosSection).IsHidden)
                {
                    if (AreaObjOldSelection != -1 && AreaObjOldSelection < AllInfos["AreaObjInfo"].Count) render.ChangeTransform(comboBox1.Text, AreaObjOldSelection, render.Positions[comboBox1.Text][AreaObjOldSelection], new Vector3D(0, 0, 0), 0, 0, 0, false);
                    UpdateOBJPos(ObjectsListBox.SelectedIndex, CurrentAllInfosSection, comboBox1.Text);
                }
                AreaObjOldSelection = ObjectsListBox.SelectedIndex;
                if (CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString() == "FogArea") { btn_cameraCode.Visible = true; btn_cameraCode.Text = "Edit fog area"; }

                return;
            }
            else if (comboBox1.Text == "CameraAreaInfo")
            {
                propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter(CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop);
                if (((AllInfoSection)CurrentAllInfosSection).IsHidden)
                {
                    if (CameraAreaOldSelection != -1 && CameraAreaOldSelection < AllInfos["CameraAreaInfo"].Count) render.ChangeTransform(comboBox1.Text, CameraAreaOldSelection, render.Positions[comboBox1.Text][CameraAreaOldSelection], new Vector3D(0, 0, 0), 0, 0, 0, false);
                    UpdateOBJPos(ObjectsListBox.SelectedIndex, CurrentAllInfosSection, comboBox1.Text);
                }
                CameraAreaOldSelection = ObjectsListBox.SelectedIndex;
                if (CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString() == "CameraArea") { btn_cameraCode.Visible = true; btn_cameraCode.Text = "Edit camera code"; }
                return;
            }
            else if (comboBox1.Text == "AllRailInfos")
            {
                propertyGrid1.SelectedObject = AllRailInfos[ObjectsListBox.SelectedIndex];
                UpdateRailpos(ObjectsListBox.SelectedIndex, AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray());
                render.SelectRail(AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray());
             }
            else
            {
                render.SelectObjs(CurrentAllInfosSectionName, ObjectsListBox.SelectedIndices);
                propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter(CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop);
                RefreshTmpChildrenObjects();
            }
        }

        private void RefreshTmpChildrenObjects()
        {
            render.ClearTmpObjects();
            if (ObjectsListBox.SelectedIndex == -1) return;
            if (checkBox2.Checked && CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop.ContainsKey("GenerateChildren"))
            {
                AddChildrenModels((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"], false);
            }
            if (checkBox2.Checked && CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop.ContainsKey("AreaChildren"))
            {
                AddChildrenModels((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["AreaChildren"], true);
            }
        }

        private void ObjectsListBox_Doubleclick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (ObjectsListBox.SelectedIndex < 0) return;
            if (ObjectsListBox.SelectedItems.Count > 1) return;
            render.CameraToObj(CurrentAllInfosSectionName, ObjectsListBox.SelectedIndex);
        }

        void UpdateHint()
        {
            if (comboBox1.Text == "AllRailInfos")
            {
                lblDescription.Text = "";
                lblDescription.Tag = -1;
                return;
            }
            if (ObjectDatabase.Entries.ContainsKey(ObjectsListBox.SelectedItem.ToString()))
            {
                lblDescription.Text = ObjectDatabase.Entries[ObjectsListBox.SelectedItem.ToString()].notes;
                if (ObjectDatabase.Entries[ObjectsListBox.SelectedItem.ToString()].Known == 0)
                {
                    lblDescription.Text = "This object is not documented";
                    lblDescription.Tag = -1;
                }
                else
                {
                    if (ObjectDatabase.Entries[ObjectsListBox.SelectedItem.ToString()].Complete == 0)
                    {
                        lblDescription.Text += "\r\nThis object entry is not completed";
                    }
                    lblDescription.Tag = 1;
                    lblDescription.Text += "\r\n(Click for more)";
                }
            }
            else
            {
                lblDescription.Text = "This object is not in the database";
                lblDescription.Tag = -1;
            }
        }

        private void bymlXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "Byml File|*.Byml|*.*|*.*";
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "Xml file|*.xml";
            if (opn.ShowDialog() == DialogResult.OK && sav.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sav.FileName, BymlConverter.GetXml(File.ReadAllBytes(opn.FileName)), DefEnc);
            }
        }

        private void xmlBymlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "Xml file|*.xml|*.*|*.*";
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "Byml File|*.Byml";
            if (opn.ShowDialog() == DialogResult.OK && sav.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sav.FileName, BymlConverter.GetByml(File.ReadAllText(opn.FileName, DefEnc)));
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            FrmCredits c = new FrmCredits();
            c.ShowDialog();
        }

        private void hotkeysListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(strings["Hotkey"]);
        }

        private void Undo_loading(object sender, EventArgs e)
        {
            UndoMenu.DropDownItems.Clear();
            List<ToolStripMenuItem> Items = new List<ToolStripMenuItem>();
            int count = 0;
            foreach (UndoAction act in Undo.ToArray().Reverse())
            {
                ToolStripMenuItem btn = new ToolStripMenuItem();
                btn.Name = "Undo" + count.ToString();
                btn.Text = act.ToString();
                btn.Click += UndoListItem_Click;
                btn.MouseEnter += UndoListItem_MouseEnter;
                Items.Add(btn);
                count++;
            }
            UndoMenu.DropDownItems.AddRange(Items.ToArray());
        }

        private void UndoListItem_MouseEnter(object sender, EventArgs e)
        {
            string SenderName = ((ToolStripMenuItem)sender).Name;
            int index = int.Parse(SenderName.Substring("Undo".Length));
            for (int i = 0; i < UndoMenu.DropDownItems.Count; i++)
            {
                if (i < index) UndoMenu.DropDownItems[i].BackColor = Color.LightBlue;
                else UndoMenu.DropDownItems[i].BackColor = SystemColors.Control;
            }
        }

        private void UndoListItem_Click(object sender, EventArgs e)
        {
            string SenderName = ((ToolStripMenuItem)sender).Name;
            int index = int.Parse(SenderName.Substring("Undo".Length));
            for (int i = 0; i <= index; i++)
            {
                Undo.Pop().Undo();
            }
            UndoMenu.HideDropDown();
        }

        private void gbatempThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://gbatemp.net/threads/wip-the-fourth-dimension-a-super-mario-3d-land-level-editor.424001/");
        }

        private void Form1_closing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void creatorClassNameTableEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmCCNTEdit f = new FormEditors.FrmCCNTEdit(CreatorClassNameTable, this);
            f.ShowDialog();
            LoadCreatorClassNameTable();
        }

        private void stagesBgmEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.GamePath.Trim() == "" && !File.Exists(@"BgmTable.szs"))
            {
                MessageBox.Show("You must set the game Romfs path first !");
                return;
            }
            else if (!File.Exists(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs") && !File.Exists(@"BgmTable.szs"))
            {
                MessageBox.Show(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs not found !\r\nProbably your Romfs dump is incomplete or was modified.");
                return;
            }
            else if (File.Exists(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs") && !File.Exists(@"BgmTable.szs"))
            {
                File.Copy(Properties.Settings.Default.GamePath + "\\SoundData\\BgmTable.szs", @"BgmTable.szs");
            }
            BgmEditors.FrmBgmMain f = new BgmEditors.FrmBgmMain(LevelNameNum);
            f.Show();
        }

        private void From_Activated(object sender, EventArgs e) //Resume sorting
        {
            render.SetSortFrequency(0.5);
        }

        private void Form_Deactivate(object sender, EventArgs e) //Stop sorting
        {
            render.SetSortFrequency(0);
        }

        private void ListBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ObjectsListBox.ClearSelected();
                ObjectsListBox.SelectedIndex = -1;
                propertyGrid1.SelectedObject = null;
            }
        }

        private void modelImporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormEditors.FrmObjImport().ShowDialog();
        }

        private void objectsDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ObjectDatabase == null)
            {
                MessageBox.Show("The database was not loaded, this is normal if it's being downloaded, wait until it's done");
                return;
            }
            Form f = Application.OpenForms["ObjectDbEditor"];
            if (f != null)
            {
                f.Focus();
                return;
            }
            ObjectDbEditor d = new ObjectDbEditor(ObjectDatabase);
            d.Show();
            //LoadObjectDatabase();
        }

        private void generate2DSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!AllInfos.ContainsKey("ObjInfo"))
            {
                MessageBox.Show("This level doesn't include the type ObjInfo, add it to use this function");
                return;
            }
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("ObjInfo");
            Form f = Application.OpenForms["Frm2DSection"];
            if (f != null)
            {
                f.Focus();
                return;
            }
            Frm2DSection d = new Frm2DSection();
            d.Show();
        }

        private void lblDescription_Click(object sender, EventArgs e)
        {
            if (lblDescription.Tag.ToString() != "-1") new ObjectDB.ObjectDBView(ObjectDatabase.Entries[ObjectsListBox.SelectedItem.ToString()], ObjectsListBox.SelectedItem.ToString()).Show();
        }

        private void oggToBcstmConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BgmEditors.FrmMakeBcstm().ShowDialog();
        }

        private void downloadLatestObjectDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("objectdb.xml"))
            {
                if (MessageBox.Show("An object database file already exists, if you download a new one it will be replaced, if you edited it your changes will be lost, do you want to continue ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No) return;
                if (File.Exists("objectdb.xml.bak")) File.Delete("objectdb.xml.bak");
                File.Copy("objectdb.xml", "objectdb.xml.bak");
                File.Delete("objectdb.xml");
            }
            try
            {
                new WebClient().DownloadFile(Properties.Settings.Default.DownloadDbLink == "" ? ObjectDbLink : Properties.Settings.Default.DownloadDbLink, "objectdb.xml");
                LoadObjectDatabase();
                File.Delete("objectdb.xml.bak");
                MessageBox.Show("Done !");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error, the file wasn't downloaded: \r\n" + ex.Message);
                if (File.Exists("objectdb.xml.bak"))
                {
                    if (File.Exists("objectdb.xml")) File.Delete("objectdb.xml");
                    File.Copy("objectdb.xml.bak", "objectdb.xml");
                    File.Delete("objectdb.xml.bak");
                    MessageBox.Show("The backup was restored");
                }
                return;
            }
        }

        private void generatePreloadFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SzsFiles == null)
            {
                MessageBox.Show("Szs not loaded ?");
                return;
            }
            if (!SzsFiles.ContainsKey("PreLoadFileList1.byml"))
            {
                SzsFiles.Add("PreLoadFileList1.byml", new byte[] { 0x00 });
            }
            string PreLoadFileList = Properties.Resources.GenericPreloadList;
            string ObjsList = "";
            List<string> ProcessedItems = new List<string>();
            foreach (LevelObj o in AllInfos["ObjInfo"])
            {
                if (ProcessedItems.Contains(o.ToString())) continue;
                ProcessedItems.Add(o.ToString());
                if (ObjectDatabase.Entries.ContainsKey(o.ToString()) && ObjectDatabase.Entries[o.ToString()].files != "")
                {
                    foreach (string s in ObjectDatabase.Entries[o.ToString()].files.Split("\r\n"[0]))
                    {
                        if (s.StartsWith("/ObjectData") && s.EndsWith(".szs"))
                            ObjsList += "<C1>\r\n<A0 Name=\"Path\" StringValue=\"" + s + "\" />\r\n<A0 Name=\"Type\" StringValue=\"Archive\" />\r\n</C1>";
                    }
                }
            }
            PreLoadFileList = PreLoadFileList.Insert(252, ObjsList);
            if (SzsFiles.ContainsKey("PreLoadFileList2.byml"))
            {
                MessageBox.Show("This SZS contains more PreLoadFileList files, the result of the generation will be put in your clipboard as xml, open the file you want to replace and paste replacing everything, then click save (the layout of the text doesn't matter)");
                Clipboard.SetText(PreLoadFileList);
            }
            else
            {
                SzsFiles["PreLoadFileList1.byml"] = BymlConverter.GetByml(PreLoadFileList);
                MessageBox.Show("Done");
            }
        }
        #endregion

        #region LevelEditing
        bool IsEditingC0List = false;
        Stack<List<LevelObj>> C0ListEditingStack = new Stack<List<LevelObj>>();
        Stack<int> SelectionIndex = new Stack<int>();
        int InitialAllInfosSection = -1;
        List<LevelObj> dummyList = new List<LevelObj>();
        private List<LevelObj> CurrentAllInfosSection
        {
            get
            {
                if (IsEditingC0List) return C0ListEditingStack.Peek();
                else if (comboBox1.Text == "AllRailInfos") return dummyList;
                else return AllInfos[comboBox1.Text];
            }
        }

        private string CurrentAllInfosSectionName
        {
            get
            {
                if (IsEditingC0List) return "C0EditingListObjs";
                else return comboBox1.Text;
            }
        }

        public List<LevelObj> GetListByName(string name)
        {
                if (name == "C0EditingListObjs" || IsEditingC0List) return C0ListEditingStack.Peek();
                else return AllInfos[name];
        }

        void EditC0List(C0List list, bool addToStack = true) { if (list != null) EditC0List(list.List, addToStack); }
        public void EditC0List(List<LevelObj> list, bool addToStack = true)
        {
            if (addToStack && ObjectsListBox.SelectedItems.Count > 1)
            {
                MessageBox.Show("You can't edit more C0lists at once, edit only on one object then copy the children objects to the others");
                return;
            }
            fileToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            openToolStripMenuItem.Enabled = false;
            comboBox1.Text = "C0EditingListObjs";
            C0EditingPanel.Visible = true;
            IsEditingC0List = true;
            if (addToStack) {
                if (C0ListEditingStack.Count == 0) InitialAllInfosSection = comboBox1.SelectedIndex;
                C0ListEditingStack.Push(list);
                SelectionIndex.Push(ObjectsListBox.SelectedIndex);
            }
            ObjectsListBox.SelectedItems.Clear();
            render.ClearC0Objects();
            LoadModels(list, "C0EditingListObjs");
            ObjectsListBox.Items.Clear();
            if (!higestID.ContainsKey("C0EditingListObjs")) higestID.Add("C0EditingListObjs", 0); else higestID["C0EditingListObjs"] = 0;
            foreach (LevelObj o in list) if (o.Prop.ContainsKey("l_id")) if (Int32.Parse(((Node)o.Prop["l_id"]).StringValue) > higestID["C0EditingListObjs"]) higestID["C0EditingListObjs"] = Int32.Parse(((Node)o.Prop["l_id"]).StringValue);
            ObjectsListBox.Items.AddRange(list.ToArray());;
            propertyGrid1.SelectedObject = null;
        }

        private void C0ListEditorGoBack()
        {
            if (C0ListEditingStack.Count == 1)
            {
                render.ClearC0Objects();
                propertyGrid1.SelectedObject = null;
                IsEditingC0List = false;
                comboBox1.SelectedIndex = InitialAllInfosSection;
                comboBox1_SelectedIndexChanged(null,null);
                C0ListEditingStack.Pop();
                C0EditingPanel.Visible = false;
            }
            else
            {
                C0ListEditingStack.Pop();
                EditC0List(C0ListEditingStack.Peek(), false);
            }
            ObjectsListBox.SelectedIndex = SelectionIndex.Pop();
            fileToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            openToolStripMenuItem.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            C0ListEditorGoBack();
        }

        public void SetComboboxToLevObjList(List<LevelObj> list)
        {
            for (int i = 0; i < AllInfos.Keys.Count; i++) if (AllInfos.Values.ToArray()[i] == list) comboBox1.SelectedIndex = i;
        }

        private void CameraCode_click(object sender, EventArgs e)
        {
            if (CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString().Contains("Fog"))
            {
                int fogid = ((int)((int[])CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["Arg"])[0]);
                if (fogid < 0)
                {
                    MessageBox.Show("The FogArea Arg[0] can't be less than 0!");
                    return;
                }
                if (SzsFiles == null)
                {
                    MessageBox.Show("To use this function you must load a level from an szs file");
                    return;
                }
                string TmpFogParam = Properties.Resources.GenericFogParam;
                if (!SzsFiles.ContainsKey("FogParam.byml"))
                {
                    MessageBox.Show("This level doesn't contain the FogParam file, a generic FogParam will be generated");

                    SzsFiles.Add("FogParam.byml", BymlConverter.GetByml(TmpFogParam));
                    FormEditors.FrmAddFogSettings f = new FormEditors.FrmAddFogSettings(TmpFogParam, fogid, this);
                    f.ShowDialog();
                    OtherLevelDataMenu.DropDownItems.Clear();
                    for (int i = 0; i < SzsFiles.Keys.Count; i++)
                    {
                        ToolStripMenuItem btn = new ToolStripMenuItem();
                        btn.Name = "LoadFile" + i.ToString();
                        btn.Text = SzsFiles.Keys.ToArray()[i];
                        btn.Click += LoadFileList_click;
                        OtherLevelDataMenu.DropDownItems.Add(btn);
                    }
                }
                else 
                {
                    string FogParam = BymlConverter.GetXml(SzsFiles["FogParam.byml"]);
                    if (!FogParam.Contains("<C0 Name=\"FogAreas\">"))
                    {
                        DialogResult r = MessageBox.Show("The FogParam from this szs can't be used, do you want to generate a new FogParam ?", "FogParam.byml", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (r == DialogResult.Yes)
                        {
                            FormEditors.FrmAddFogSettings f = new FormEditors.FrmAddFogSettings(TmpFogParam, fogid, this);
                            f.ShowDialog();
                        }
                        else return;
                    }
                    else
                    {
                        if (FogParam.Contains("<D1 Name=\"Area Id\" StringValue=\"" + fogid.ToString() + "\" />"))
                        {
                            FormEditors.FrmXmlEditor frm = new FormEditors.FrmXmlEditor(BymlConverter.GetXml(SzsFiles["FogParam.byml"]), "FogParam.byml", false, FogParam.IndexOf("<D1 Name=\"Area Id\" StringValue=\"" + fogid.ToString() + "\" />"));
                            frm.ShowDialog();
                            if (frm.XmlRes != null) SzsFiles["FogParam.byml"] = BymlConverter.GetByml(frm.XmlRes);
                        }
                        else
                        {
                            FormEditors.FrmAddFogSettings f = new FormEditors.FrmAddFogSettings(FogParam, fogid, this);
                            f.ShowDialog();
                        }
                    }
                }
            }
            else if (CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString() == "CameraArea")
            {
                int cameraId = int.Parse(((Node)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["CameraId"]).StringValue);
                if (cameraId < 0)
                {
                    MessageBox.Show("CameraId can't be less than 0 !");
                    return;
                }
                if (SzsFiles == null)
                {
                    MessageBox.Show("To use this function you must load a level from an szs file");
                    return;
                }
                if (!SzsFiles.ContainsKey("CameraParam.byml"))
                {
                    MessageBox.Show("This level doesn't contain the CameraParam file, a generic CameraParam will be generated");
                    string TmpCameraParam = Properties.Resources.GenericCameraParam;
                    SzsFiles.Add("CameraParam.byml", BymlConverter.GetByml(TmpCameraParam));
                    FormEditors.FrmAddCameraSettings f = new FormEditors.FrmAddCameraSettings(TmpCameraParam, cameraId, this);
                    f.ShowDialog();
                }
                else
                {
                    string CameraParam = BymlConverter.GetXml(SzsFiles["CameraParam.byml"]);
                    if (!CameraParam.Contains("<C0 Name=\"CameraParams\">"))
                    {
                        DialogResult r = MessageBox.Show("The CameraParam.byml from this szs can't be used, do you want to generate a new CameraParam ?", "CameraParam.byml", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (r == DialogResult.Yes)
                        {
                            string TmpCameraParam = Properties.Resources.GenericCameraParam;
                            FormEditors.FrmAddCameraSettings f = new FormEditors.FrmAddCameraSettings(TmpCameraParam, cameraId, this);
                            f.ShowDialog();
                        }
                        else return;
                    }
                    else
                    {
                        if (CameraParam.Contains("<D1 Name=\"UserGroupId\" StringValue=\"" + cameraId.ToString() + "\" />"))
                        {
                            FormEditors.FrmXmlEditor frm = new FormEditors.FrmXmlEditor(BymlConverter.GetXml(SzsFiles["CameraParam.byml"]), "CameraParam.byml", false, CameraParam.IndexOf("<D1 Name=\"UserGroupId\" StringValue=\"" + cameraId.ToString() + "\" />"));
                            frm.ShowDialog();
                            if (frm.XmlRes != null) SzsFiles["CameraParam.byml"] = BymlConverter.GetByml(frm.XmlRes);
                        }
                        else
                        {
                            FormEditors.FrmAddCameraSettings f = new FormEditors.FrmAddCameraSettings(CameraParam, cameraId, this);
                            f.ShowDialog();
                        }
                    }
                }
            }
        }

        private void AddType_click(object sender, EventArgs e) //Addtype
        {
            FormEditors.FrmStringInput f = new FormEditors.FrmStringInput();
            f.ShowDialog();
            string[] internalNames = new string[] {"C0EditingListObjs", "TmpChildrenObjs", "SelectedRail", "TmpAreaChildrenObjs" };
            if (f.Result == null) return;
            else if (f.Result.Trim() == "") return;
            else if (AllInfos.ContainsKey(f.Result)) MessageBox.Show("This type is already in use");
            else if (internalNames.Contains(f.Result)) MessageBox.Show("This type name is reserved");
            else
            {
                comboBox1.Items.Add(f.Result);
                render.AddKey(f.Result);
                AllInfos.Add(f.Result, new AllInfoSection());
                higestID.Add(f.Result, 0);
            }
        }

        bool warningShow = false;
        private void propertyGridChange(object s, PropertyValueChangedEventArgs e)
        {
            if (ObjectsListBox.SelectedIndex < 0) { MessageBox.Show("No object selected in the list"); return; }
            if (comboBox1.Text == "AllRailInfos")
            {
                UpdateRailpos(ObjectsListBox.SelectedIndex, AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray());
                Action<object[]> act;
                act = (object[] args) =>
                {
                    int id = (int)args[0];
                    string propName = (string)args[1];
                    object value = args[2];
                    AllRailInfos[id][propName] = value;
                    propertyGrid1.Refresh();
                    UpdateRailpos(id, ((Rail)value).GetPointArray());
                };
                Undo.Push(new UndoAction("Changed value: " + e.ChangedItem.Label + " of rail: " + AllRailInfos[ObjectsListBox.SelectedIndex].ToString(), new object[] { ObjectsListBox.SelectedIndex, e.ChangedItem.Label, e.OldValue }, act));
                ObjectsListBox.Items[ObjectsListBox.SelectedIndex] = AllRailInfos[ObjectsListBox.SelectedIndex].ToString();
                return;
            }
            else
            {
                //string name = e.ChangedItem.Parent.Value is Node ? e.ChangedItem.Parent.Label : e.ChangedItem.Label;
                string name = null;
                string pname = null;
                if (e.ChangedItem.Parent.Value is Node)
                {
                    name = e.ChangedItem.Parent.Label;
                }
                else if(e.ChangedItem.Parent.Value is Array) { name = e.ChangedItem.Label; pname = e.ChangedItem.Parent.Label; } else {name = e.ChangedItem.Label; }
                if (name == "name" || name == "l_id")
                {
                    if (!warningShow)
                    {
                        MessageBox.Show("You shouldn't mess up with the name or the l_id property of the objects, you should add a new object instead and copy the position from this object to the new one.\r\nYou can undo this action from the undo button", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        warningShow = true;
                    }
                    string path = GetModelname(CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString());
                    if (!System.IO.File.Exists(path)) path = "models\\UnkBlue.obj";
                    if (name == "name") {
                      foreach(int i in ObjectsListBox.SelectedIndices)  render.ChangeModel(CurrentAllInfosSectionName, i, path);
                    }
                }
                
                UpdateOBJPos(ObjectsListBox.SelectedIndices,  CurrentAllInfosSection, CurrentAllInfosSectionName);
                if (ObjectsListBox.SelectedItems.Count == 1)
                {
                    Action<object[]> action;
                    action = (object[] args) =>
                    {
                        if (args[5] == null) 
                        { 
                        List<LevelObj> type = (List<LevelObj>)args[0];
                        render.ClearSelection();
                        string TypeName = (string)args[1];
                        int id = (int)args[2];
                        string propName = (string)args[3];
                        object value = args[4];
                        if (type[id].Prop[propName] is Node) ((Node)type[id].Prop[propName]).StringValue = value.ToString();
                        else
                            type[id].Prop[propName] = value;
                        propertyGrid1.Refresh();
                        if (type.GetHashCode() == CurrentAllInfosSection.GetHashCode()) UpdateOBJPos(id, type, TypeName,true);
                        if (propName == "name")
                        {
                            string path = GetModelname(type[id].ToString());
                            if (!System.IO.File.Exists(path)) path = "models\\UnkBlue.obj";
                            if (name == "name") render.ChangeModel(TypeName, id, path);
                            ObjectsListBox.Items[id] = type[id].ToString();
                        }
                        }
                        else
                        {
                            List<LevelObj> type = (List<LevelObj>)args[0];
                            render.ClearSelection();
                            string TypeName = (string)args[1];
                            int id = (int)args[2];
                            string propName = (string)args[5];
                            object value = args[4];
                            int axis = Int32.Parse(String.Join("", ((string)args[3]).Split('[', ']')));
                            if (propName == "pos" || propName == "scale" || propName == "dir")
                            ((Single[])(((type)[id].Prop[propName])))[axis] = (Single)value;
                            else
                            {
                                ((Int32[])(((type)[id].Prop[propName])))[axis] = (Int32)value;
                            }
                            propertyGrid1.Refresh();
                            if (type.GetHashCode() == CurrentAllInfosSection.GetHashCode()) UpdateOBJPos(id, type, TypeName, true);
                        }
                    };
                    if (pname == null)
                    {
                        Undo.Push(new UndoAction("Changed value: " + name + " of object: " + CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString(), new object[] { CurrentAllInfosSection, CurrentAllInfosSectionName, ObjectsListBox.SelectedIndex, name, e.OldValue, pname }, action));
                    }
                    else
                    {
                        Undo.Push(new UndoAction("Changed value: " + name + " of array "+ pname +" of object: " + CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString(), new object[] { CurrentAllInfosSection, CurrentAllInfosSectionName, ObjectsListBox.SelectedIndex, name, e.OldValue, pname }, action));
                    }
                }

                ObjectsListBox.Items[ObjectsListBox.SelectedIndex] = CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString();

            }
        }

        public void UpdateSelectedRailView()
        {
            UpdateRailpos(ObjectsListBox.SelectedIndex, AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray());
        }

        public void RailPointsChanged(List<Rail.Point> OldPoints)
        {
            UpdateRailpos(ObjectsListBox.SelectedIndex, AllRailInfos[ObjectsListBox.SelectedIndex].GetPointArray());
            Action<object[]> act;
            act = (object[] args ) =>
            {
                int id = (int)args[0];
                AllRailInfos[id].Points = (List<Rail.Point>)args[1];
                propertyGrid1.Refresh();
                UpdateRailpos(id, AllRailInfos[id].GetPointArray());
            };
            Undo.Push(new UndoAction("Changed points of rail: " + AllRailInfos[ObjectsListBox.SelectedIndex].ToString(), new object[] { ObjectsListBox.SelectedIndex, OldPoints }, act));
        }

        public int GetObjectGenChidCount(string type, int index)
        {
            if (!CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop.ContainsKey("GenerateChildren")) return 0;
            else return ((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"]).List.Count;
        }

        private void UpdateOBJPos(ListBox.SelectedIndexCollection selectedIndices, List<LevelObj> Source, string Type)
        {
            foreach (int i in selectedIndices) UpdateOBJPos(i, Source, Type);
        }

        public void UpdateOBJPos(int id, List<LevelObj> Source, string Type, bool isUndo = false)
        {
            string setlng;
            if (Properties.Settings.Default.DotComma != true) { setlng = "de-DE"; } else { setlng = "en-UK"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(setlng);
            if (Type == "TmpChildrenObjs")
            {
                Source = ((C0List)CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["GenerateChildren"]).List;
            }
           // xyz = ads[1]
            Single X, Y, Z, ScaleX, ScaleY, ScaleZ, RotX, RotY, RotZ, A, B, C;
            Single[] scale_XDD;
            Single[] rotation;
            Single[] position;
            if (Source[id].Prop.ContainsKey("pos") == true)
            { position = (Single[])Source[id].Prop["pos"]; }
            else
            {
                if (Source[id].Prop["pos_x"] != null)
                {
                    A = Single.Parse(((Node)Source[id].Prop["pos_x"]).StringValue);
                    B = Single.Parse(((Node)Source[id].Prop["pos_y"]).StringValue);
                    C = Single.Parse(((Node)Source[id].Prop["pos_z"]).StringValue);
                    position = new Single[3] { A, B, C };

                    Source[id].Prop.Add("pos", position);
                    Source[id].Prop.Remove("pos_x");
                    Source[id].Prop.Remove("pos_y");
                    Source[id].Prop.Remove("pos_z");
                }
                else
                {
                    position = new Single[3] { 1, 1, 1 };
                }
            }
            X = position[0];
            Y = position[1];
            Z = position[2];
            if (Source[id].Prop.ContainsKey("scale") == true)
            { 
                scale_XDD = (Single[])Source[id].Prop["scale"];

                if (Source[id].Prop.ContainsKey("scale_x") == true && Source[id].Prop["scale_x"] != null)
                {
                    Source[id].Prop.Remove("scale_x");
                    Source[id].Prop.Remove("scale_y");
                    Source[id].Prop.Remove("scale_z");
                }
            }
            else 
            {
                if (Source[id].Prop["scale_x"] != null) 
                {
                    A = Single.Parse(((Node)Source[id].Prop["scale_x"]).StringValue);
                    B = Single.Parse(((Node)Source[id].Prop["scale_y"]).StringValue);
                    C = Single.Parse(((Node)Source[id].Prop["scale_z"]).StringValue);
                    scale_XDD = new Single[3] { A,B,C };
                    Source[id].Prop.Add("scale", scale_XDD);
                    Source[id].Prop.Remove("scale_x");
                    Source[id].Prop.Remove("scale_y");
                    Source[id].Prop.Remove("scale_z");
                }
                else
                {
                    scale_XDD = new Single[3] { 1, 1, 1 };
                    Source[id].Prop.Add("scale", scale_XDD);
                }
            }
            ScaleX = scale_XDD[0];
            ScaleY = scale_XDD[1];
            ScaleZ = scale_XDD[2];//Single.Parse(((Node)Source[id].Prop["scale_z"]).StringValue);
            if (Source[id].Prop.ContainsKey("dir") == true)
            {
                rotation = (Single[])Source[id].Prop["dir"];

                if (Source[id].Prop.ContainsKey("dir_x") == true && Source[id].Prop["dir_x"] != null)
                {
                    Source[id].Prop.Remove("dir_x");
                    Source[id].Prop.Remove("dir_y");
                    Source[id].Prop.Remove("dir_z");
                }
            }
            else
            {
                if (Source[id].Prop["dir_x"] != null)
                {
                    A = Single.Parse(((Node)Source[id].Prop["dir_x"]).StringValue);
                    B = Single.Parse(((Node)Source[id].Prop["dir_y"]).StringValue);
                    C = Single.Parse(((Node)Source[id].Prop["dir_z"]).StringValue);
                    rotation = new Single[3] { A, B, C };
                    Source[id].Prop.Add("dir", rotation);
                    Source[id].Prop.Remove("dir_x");
                    Source[id].Prop.Remove("dir_y");
                    Source[id].Prop.Remove("dir_z");
                }
                else
                {
                    rotation = new Single[3] { 1, 1, 1 };
                    Source[id].Prop.Add("dir", rotation);
                }
            }
            RotX = rotation[0];
            RotY = rotation[1];
            RotZ = rotation[2];
            render.ChangeTransform(Type, id, new Vector3D(X, -Z, Y), new Vector3D(ScaleX, ScaleZ, ScaleY), RotX, -RotZ, RotY , (Type != "AreaObjInfo" && Type != "CameraAreaInfo" && !isUndo));
            //ScaleXd = Single.Parse(((Node)Source[id].array(prop)Prop(0).["X"]).StringValue);
        }

        private void button5_Click(object sender, EventArgs e) //Remove values
        {
            if (comboBox1.Text == "AllRailInfos") { MessageBox.Show("You can't remove properties from rails"); return; }
            if (propertyGrid1.SelectedObject == null) return;
            if (!CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop.ContainsKey(propertyGrid1.SelectedGridItem.Label)) return;
            if (comboBox1.Text == "CameraAreaInfo" && propertyGrid1.SelectedGridItem.Label == "CameraId")
            {
                MessageBox.Show("You can't remove this value from a camera");
                return;
            }
            if (propertyGrid1.SelectedGridItem.Label.Contains("dir") || propertyGrid1.SelectedGridItem.Label.Contains("pos") || propertyGrid1.SelectedGridItem.Label.Contains("scale") || propertyGrid1.SelectedGridItem.Label.Contains("id") || propertyGrid1.SelectedGridItem.Label.ToLower().Contains("name"))
            {
                MessageBox.Show("You can't remove this value");
                return;
            }
            foreach (int i in ObjectsListBox.SelectedIndices)
            {
                Action<object[]> action;
                action = (object[] args) =>
                {
                    List<LevelObj> type = (List<LevelObj>)args[0];
                    int at = (int)args[1];
                    string propName = (string)args[2];
                    type[at].Prop.Add(propName, args[3]);
                    propertyGrid1.Refresh();
                    propertyGrid1.Update();
                };
                object prop = CurrentAllInfosSection[i].Prop[propertyGrid1.SelectedGridItem.Label];
                Undo.Push(new UndoAction("Removed property: " + propertyGrid1.SelectedGridItem.Label, new object[] { CurrentAllInfosSection, i, propertyGrid1.SelectedGridItem.Label, prop }, action));
                CurrentAllInfosSection[i].Prop.Remove(propertyGrid1.SelectedGridItem.Label);
            }
            propertyGrid1.Refresh();
            propertyGrid1.Update();
        }

        private void button4_Click(object sender, EventArgs e)//Add Value
        {
            if (comboBox1.Text == "AllRailInfos") { MessageBox.Show("You can't remove properties from rails"); return; }
            if (propertyGrid1.SelectedObject == null) return;
            FrmAddValue v = new FrmAddValue(CurrentAllInfosSection[ObjectsListBox.SelectedIndex]);
            v.ShowDialog();
            if (v.resName != null && v.resName != "") CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop.Add(v.resName, v.result); else return;
            Action<object[]> action;
            action = (object[] args) =>
            {
                List<LevelObj> type = (List<LevelObj>)args[0];
                int at = (int)args[1];
                type[at].Prop.Remove((string)args[2]);
                propertyGrid1.Refresh();
                propertyGrid1.Update();
            };
            Undo.Push(new UndoAction("Added property: " + v.resName,new object[] { CurrentAllInfosSection, ObjectsListBox.SelectedIndex, v.resName }, action));
            propertyGrid1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e) //Remove objects
        {
            DelSelectedObj();
        }

        void DelSelectedObj(bool NoUndo = false)
        {
            if (ObjectsListBox.SelectedIndex == -1) return;
            if (comboBox1.Text == "AllRailInfos")
            {
                if (!NoUndo)
                {
                    Rail tmp = AllRailInfos[ObjectsListBox.SelectedIndex].Clone();
                    Action<object[]> action;
                    action = (object[] args) =>
                    {
                        int at = (int)args[0];
                        AddRail((Rail)args[1], at, true);
                    };
                    Undo.Push(new UndoAction("Removed rail: " + tmp.ToString(), new object[] { ObjectsListBox.SelectedIndex, tmp }, action));
                }
                render.RemoveModel(CurrentAllInfosSectionName, ObjectsListBox.SelectedIndex);
                AllRailInfos.RemoveAt(ObjectsListBox.SelectedIndex);
                ObjectsListBox.Items.RemoveAt(ObjectsListBox.SelectedIndex);
            }
            else
            {
                int[] indexes = GetSelectedIndexes();
                if (!NoUndo)
                {
                    List<LevelObj> tmp = new List<LevelObj>();
                    for (int i = 0; i < indexes.Length; i++) tmp.Add(CurrentAllInfosSection[indexes[i]]);
                    Action<object[]> action;
                    action = (object[] args) =>
                    {
                        List<LevelObj> type = (List<LevelObj>)args[0];
                        int[] at = (int[])args[1];
                        LevelObj[] t = ((LevelObj[])args[2]).Reverse().ToArray();
                        int[] index = at.Reverse().ToArray();
                        for (int i = 0; i < t.Length; i++) AddObj(t[i], type, (string)args[3], false, index[i], type.GetHashCode());
                        RefreshTmpChildrenObjects();
                    };
                    string name = (indexes.Length == 1) ? "Removed object: " + tmp[0].ToString() : "Removed " + indexes.Length.ToString() + " objects";
                    Undo.Push(new UndoAction(name, new object[] {CurrentAllInfosSection, indexes, tmp.ToArray(), CurrentAllInfosSectionName }, action));
                }
                foreach (int i in indexes)
                {
                    render.RemoveModel(CurrentAllInfosSectionName, i);
                    CurrentAllInfosSection.RemoveAt(i);
                    ObjectsListBox.Items.RemoveAt(i);
                }
            }
            propertyGrid1.SelectedObject = null;
            propertyGrid1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)//Duplicating objects
        {
            if (ObjectsListBox.SelectedIndices.Count != 1) return;
            if (comboBox1.Text == "AllRailInfos")
            {
                Rail tmp = new Rail();
                tmp = AllRailInfos[ObjectsListBox.SelectedIndex].Clone();
                AddRail(tmp);
            }
            else
            {
                AddObj(CurrentAllInfosSection[ObjectsListBox.SelectedIndex],  CurrentAllInfosSection, CurrentAllInfosSectionName);
            }
        }

        void AddRail(Rail r, int at = -1, bool IsUndo = false)
        {
            higestID["AllRailInfos"]++;
            r.l_id = higestID["AllRailInfos"];
            LoadRailsModels(r, at);
            if (at == -1) AllRailInfos.Add(r); else AllRailInfos.Insert(at, r);
            if (at == -1) ObjectsListBox.Items.Add(r.ToString()); else ObjectsListBox.Items.Insert(at, r.ToString());
            ObjectsListBox.SetSelected(at == -1 ? ObjectsListBox.Items.Count - 1 : at, true);
            if (!IsUndo)
            {
                Action<object[]> action;
                action = (object[] args) =>
                {
                    AllRailInfos.RemoveAt((int)args[0]);
                    ObjectsListBox.Items.RemoveAt((int)args[0]);
                };
                Undo.Push(new UndoAction("Added rail: " + r.ToString(), new object[] { ObjectsListBox.Items.Count - 1 }, action));
            }
        }

        void AddObj(LevelObj inobj, List<LevelObj> list, string name, bool clone = true, int at = -1, int UndoHash = -1)
        {
            if (!higestID.ContainsKey(name)) higestID.Add(name, 0);
            higestID[name]++;
            LevelObj obj = new LevelObj();
            if (clone) obj = inobj.Clone(); else obj = inobj;
            if (obj.Prop.ContainsKey("l_id")) obj.Prop["l_id"] = new Node(higestID[name].ToString(), "D1");
            if (inobj.ToString() == "CameraArea") obj.Prop["CameraId"] = new Node(higestID[name].ToString(), "D1");
            if (at == -1) list.Add(obj); else list.Insert(at, obj);
            if (UndoHash == -1 || CurrentAllInfosSection.GetHashCode() == UndoHash)
            {
                if (at == -1) ObjectsListBox.Items.Add(obj.ToString()); else ObjectsListBox.Items.Insert(at, obj.ToString());
                List<LevelObj> tmp = new List<LevelObj>();
                tmp.Add(obj);
                if (name == "AreaObjInfo") LoadModels(tmp, name, "models\\UnkYellow.obj", at);
                else if (name == "CameraAreaInfo") LoadModels(tmp, name, "models\\UnkGreen.obj", at);
                else LoadModels(tmp, name, "models\\UnkBlue.obj", at);
                ObjectsListBox.ClearSelected();
                ObjectsListBox.SetSelected(at == -1 ? ObjectsListBox.Items.Count - 1 : at, true);
            }
            if (UndoHash == -1)
            {
                Action<object[]> action;
                action = (object[] args) =>
                {
                    List<LevelObj> type = (List<LevelObj>)args[0];
                    int index = (int)args[1];
                    string typename = (string)args[2];
                    type.RemoveAt(index);
                    if (type.GetHashCode() == CurrentAllInfosSection.GetHashCode())
                    {
                        render.RemoveModel(typename, index);
                        ObjectsListBox.Items.RemoveAt(index);
                    }
                    render.ClearTmpObjects();
                };
                Undo.Push(new UndoAction("added object: " + obj.ToString(), new object[] { CurrentAllInfosSection, ObjectsListBox.Items.Count - 1, CurrentAllInfosSectionName }, action));
            }
        }  

        private void BtnAddObj_Click(object sender, EventArgs e)//Add new object
        {
            Vector3D pos = AddObjectOrigin ? new Vector3D(0,0,0) : render.GetPositionInView();
            if (comboBox1.Text == "AllRailInfos")
            {
                AddRail(new Rail(true, pos));
                if (AutoMoveCam) render.LookAt(pos);
            }
            else
            {
                FrmAddObj frm = new FrmAddObj(CreatorClassNameTable.Keys.ToArray(), ObjectDatabase, comboBox1.Text,pos );
                frm.ShowDialog();
                if (frm.Value == null) return;
                AddObj(frm.Value,  CurrentAllInfosSection, CurrentAllInfosSectionName);
                if (AutoMoveCam) render.LookAt(pos);
            }
        }

        private void pasteValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ObjectsListBox.SelectedIndex < 0)
            {
                if (clipboard[clipboard.Count - 1].Type != ClipBoardItem.ClipboardType.FullObject) return;
            }
            if (comboBox1.Text == "AllRailInfos" && !(clipboard[clipboard.Count - 1].Type == ClipBoardItem.ClipboardType.Rail || clipboard[clipboard.Count - 1].Type == ClipBoardItem.ClipboardType.IntArray)) return;
            PasteValue(ObjectsListBox.SelectedIndex, CurrentAllInfosSection,CurrentAllInfosSectionName, clipboard[clipboard.Count - 1]);
            ClipBoardMenu.Close();
        }

        int[] GetSelectedIndexes()
        {
            if (ObjectsListBox.SelectedItems.Count == 0) return new int[] { -1 };
            int[] res = new int[ObjectsListBox.SelectedItems.Count];
            for (int i = 0; i < ObjectsListBox.SelectedItems.Count; i++) res[i] = ObjectsListBox.SelectedIndices[i];
            return res.Reverse().ToArray(); //From the last to the first
        }

        private void copyPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "AllRailInfos") return;
            if (ObjectsListBox.SelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "pos");
        }

        private void copyRotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "AllRailInfos") return;
            if (ObjectsListBox.SelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "dir");
        }

        private void copyScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "AllRailInfos") return;
            if (ObjectsListBox.SelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "scale");
        }

        private void ClipBoardMenu_CopyArgs_Click(object sender, EventArgs e)
        {
            //if (comboBox1.Text == "AllRailInfos") return;
            if (ObjectsListBox.SelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "Arg");
        }

        private void ClipBoardMenu_CopyFull_Click(object sender, EventArgs e)
        {
            if (ObjectsListBox.SelectedIndex < 0) return;
            int[] indexes = GetSelectedIndexes();
            if (indexes.Length == 1) CopyValue(indexes, CurrentAllInfosSectionName, "Full");
            else CopyValue(indexes, CurrentAllInfosSectionName, "FullArray");
        }

        private void Btn_CopyObjs_Click(object sender, EventArgs e)
        {
            int[] indexes = GetSelectedIndexes();
            if (indexes.Length > 1) CopyValue(indexes, CurrentAllInfosSectionName, "FullArray");
        }


        private void ClipBoardMenu_CopyRail_Click(object sender, EventArgs e)
        {
            if (ObjectsListBox.SelectedIndex < 0) return;
            CopyValue(GetSelectedIndexes(), CurrentAllInfosSectionName, "Rail");
        }

        void CopyValue(int[] indexes, string type, string value)
        {
            ClipBoardItem cl = new ClipBoardItem();
            int index = indexes[0];
            /*if (value == "pos_" || value == "dir_")
            {
                if (value == "pos_") cl.Type = ClipBoardItem.ClipboardType.Position;
                else if (value == "dir_") cl.Type = ClipBoardItem.ClipboardType.Rotation;
                else cl.Type = ClipBoardItem.ClipboardType.Scale;
                if (GetListByName(type)[index].Prop.ContainsKey(value + "x") && GetListByName(type)[index].Prop.ContainsKey(value + "y") && GetListByName(type)[index].Prop.ContainsKey(value + "z"))
                {
                    cl.X = Single.Parse(((Node)GetListByName(type)[index].Prop[value + "x"]).StringValue);
                    cl.Y = Single.Parse(((Node)GetListByName(type)[index].Prop[value + "y"]).StringValue);
                    cl.Z = Single.Parse(((Node)GetListByName(type)[index].Prop[value + "z"]).StringValue);
                }
                else MessageBox.Show("You can't copy this value from this object");
            }*/
            if (value == "Arg")
            {
                if (type == "AllRailInfos")
                {
                    cl.Type = ClipBoardItem.ClipboardType.IntArray;
                    cl.Args = AllRailInfos[index].Arg.ToArray();
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.IntArray;
                    if (GetListByName(type)[index].Prop.ContainsKey("Arg"))
                    {
                        cl.Args = (int[])((int[])GetListByName(type)[index].Prop["Arg"]).Clone(); //This looks strange but (int[])GetListByName(type)[index].Prop["Arg"] doesn't work
                    }
                    else MessageBox.Show("You can't copy this value from this object");
                }
            }
            else if (value == "scale")
            {
                if (type == "AllRailInfos")
                {
                    return;
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.Scale;
                    if (GetListByName(type)[index].Prop.ContainsKey("scale"))
                    {
                        cl.Scale = (Single[])((Single[])GetListByName(type)[index].Prop["scale"]).Clone();
                    }
                    else MessageBox.Show("You can't copy this value from this object");
                }
            }
            else if (value == "dir")
            {
                if (type == "AllRailInfos")
                {
                    return;
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.Rotation;
                    if (GetListByName(type)[index].Prop.ContainsKey("dir"))
                    {
                        cl.Dir = (Single[])((Single[])GetListByName(type)[index].Prop["dir"]).Clone();
                    }
                    else MessageBox.Show("You can't copy this value from this object");
                }
            }
            else if (value == "pos")
            {
                if (type == "AllRailInfos")
                {
                    return;
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.Position;
                    if (GetListByName(type)[index].Prop.ContainsKey("pos"))
                    {
                        cl.Pos = (Single[])((Single[])GetListByName(type)[index].Prop["pos"]).Clone();
                    }
                    else MessageBox.Show("You can't copy this value from this object");
                }
            }
            else if (value == "Full")
            {
                if (type == "AllRailInfos")
                {
                    cl.Type = ClipBoardItem.ClipboardType.Rail;
                    cl.Rail = AllRailInfos[index].Clone();
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.FullObject;
                    cl.Objs = new LevelObj[] { GetListByName(type)[index].Clone() };
                }
            }
            else if (value == "FullArray")
            {
                if (type == "AllRailInfos")
                {
                    MessageBox.Show("Multi-Rail copy not implemented");
                }
                else
                {
                    cl.Type = ClipBoardItem.ClipboardType.ObjectArray;
                    List<LevelObj> l = new List<LevelObj>();
                    foreach (int i in indexes) l.Add(GetListByName(type)[i].Clone());
                    cl.Objs = l.ToArray();
                }
            }
            else if (value == "Rail")
            {
                cl.Type = ClipBoardItem.ClipboardType.Rail;
                cl.Rail = ((Rail)GetListByName(type)[index].Prop["Rail"]).Clone();
            }
            clipboard.Add(cl);
            if (clipboard.Count > 10) clipboard.RemoveAt(0);
            ClipBoardMenu_Paste.DropDownItems.Clear();
            List<ToolStripMenuItem> Items = new List<ToolStripMenuItem>();
            for (int i = 0; i < clipboard.Count; i++)
            {
                ToolStripMenuItem btn = new ToolStripMenuItem();
                btn.Name = "ClipboardN" + i.ToString();
                btn.Text = clipboard[i].ToString();
                btn.Click += QuickClipboardItem_Click;
                Items.Add(btn);
            }
            Items.Reverse();
            ClipBoardMenu_Paste.DropDownItems.AddRange(Items.ToArray());
        }


        private void ClipBoardMenu_Opening(object sender, CancelEventArgs e)
        {
            if (GetSelectedIndexes().Length > 1)
            {
                ClipBoardMenu_CopyPos.Enabled = false;
                ClipBoardMenu_CopyRot.Enabled = false;
                ClipBoardMenu_CopyScale.Enabled = false;
                ClipBoardMenu_CopyArgs.Enabled = false;
                ClipBoardMenu_CopyFull.Text = "Copy objects";
                ClipBoardMenu_Paste.Enabled = false;
                ClipBoardMenu_CopyRail.Visible = false;
            }
            else
            {
                ClipBoardMenu_CopyArgs.Enabled = true;
                ClipBoardMenu_Paste.Enabled = true;
                ClipBoardMenu_CopyRail.Visible = false;
                if (comboBox1.Text == "AllRailInfos")
                {
                    ClipBoardMenu_CopyPos.Enabled = false;
                    ClipBoardMenu_CopyRot.Enabled = false;
                    ClipBoardMenu_CopyScale.Enabled = false;
                    ClipBoardMenu_CopyFull.Text = "Copy rail";
                }
                else
                {
                    ClipBoardMenu_CopyFull.Text = "Copy full object";
                    ClipBoardMenu_CopyPos.Enabled = true;
                    ClipBoardMenu_CopyRot.Enabled = true;
                    ClipBoardMenu_CopyScale.Enabled = true;
                    if (ObjectsListBox.SelectedItems.Count == 1 && CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop.Keys.Contains("Rail") && CurrentAllInfosSection[ObjectsListBox.SelectedIndex].Prop["Rail"] is Rail)
                        ClipBoardMenu_CopyRail.Visible = true;
                }
                ClipBoardMenu_Paste.DropDownItems.Clear();
                List<ToolStripMenuItem> Items = new List<ToolStripMenuItem>();
                for (int i = 0; i < clipboard.Count; i++)
                {
                    ToolStripMenuItem btn = new ToolStripMenuItem();
                    btn.Name = "ClipboardN" + i.ToString();
                    btn.Text = clipboard[i].ToString(ObjectsListBox.SelectedIndex);
                    btn.Click += QuickClipboardItem_Click;
                    Items.Add(btn);
                }
                Items.Reverse();
                ClipBoardMenu_Paste.DropDownItems.AddRange(Items.ToArray());
            }
        }

        private void QuickClipboardItem_Click(object sender, EventArgs e)
        {
            string SenderName = ((ToolStripMenuItem)sender).Name;
            int index = int.Parse(SenderName.Substring("ClipboardN".Length));
            if (ObjectsListBox.SelectedIndex < 0)
            {
                if (clipboard[index].Type != ClipBoardItem.ClipboardType.FullObject && clipboard[index].Type != ClipBoardItem.ClipboardType.ObjectArray) return;
            }
            if (comboBox1.Text == "AllRailInfos" && !(clipboard[index].Type == ClipBoardItem.ClipboardType.Rail || clipboard[index].Type == ClipBoardItem.ClipboardType.IntArray)) return;
            PasteValue(ObjectsListBox.SelectedIndex, CurrentAllInfosSection,CurrentAllInfosSectionName, clipboard[index]);
        }

        public void PasteValue(int index, List<LevelObj> type, string TypeName, ClipBoardItem itm)
        {
            if (index >= 0)
            {
                Action<object[]> act;
                act = (object[] args) =>
                {
                    List<LevelObj> _type = (List<LevelObj>)args[0];
                    string _typeName = (string)args[1];
                    int id = (int)args[2];
                    object Inobj = args[3];
                    if (_typeName == "AllRailInfos")
                    {
                        AllRailInfos[id] = ((Rail)Inobj);
                    }
                    else
                    {
                        _type[id] = ((LevelObj)Inobj);
                        if (_type.GetHashCode() == CurrentAllInfosSection.GetHashCode()) UpdateOBJPos(id, _type, _typeName);
                    }
                };
                object obj = TypeName == "AllRailInfos" ? (object)AllRailInfos[index].Clone() : type[index].Clone();
                Undo.Push(new UndoAction("Pasted value to object " + obj.ToString(), new object[] { type, TypeName, index, obj }, act));
            }
            if (itm.Type == ClipBoardItem.ClipboardType.Position)
            {
                if (type[index].Prop.ContainsKey("pos")) type[index].Prop["pos"] = itm.Pos.Clone();
                else type[index].Prop.Add("pos", itm.Pos.Clone());
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.Rotation)
            {
                if (type[index].Prop.ContainsKey("dir")) type[index].Prop["dir"] = itm.Dir.Clone();
                else type[index].Prop.Add("dir", itm.Dir.Clone());
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.Scale)
            {
                if (type[index].Prop.ContainsKey("scale")) type[index].Prop["scale"] = itm.Scale.Clone();
                else type[index].Prop.Add("scale", itm.Scale.Clone());
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.IntArray)
            {
                if (TypeName == "AllRailInfos")
                {
                    AllRailInfos[index].Arg = itm.Args.ToList();
                    return;
                }
                else
                {
                    if (type[index].Prop.ContainsKey("Arg")) type[index].Prop["Arg"] = itm.Args.Clone();
                    else type[index].Prop.Add("Arg", itm.Args.Clone());
                }
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.Rail)
            {
                if (TypeName == "AllRailInfos")
                {
                    int id = AllRailInfos[index].l_id;
                    int no = AllRailInfos[index].no;
                    string name = AllRailInfos[index].Name;
                    AllRailInfos[index] = itm.Rail.Clone();
                    AllRailInfos[index].l_id = id;
                    AllRailInfos[index].no = no;
                    AllRailInfos[index].Name = name;
                    ObjectsListBox_SelectedIndexChanged(null, null);
                    return;
                }
                else
                {
                    if (type[index].Prop.ContainsKey("Rail")) type[index].Prop["Rail"] = itm.Rail.Clone();
                    else type[index].Prop.Add("Rail", itm.Rail.Clone());
                }
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.FullObject)
            {
                if (index < 0 || propertyGrid1.SelectedObject == null) AddObj(itm.Objs[0],  type, TypeName, true);
                else
                {
                    string name = itm.Objs[0].ToString();
                    if (name == "ObjectChildArea" || name == "SwitchKeepOnArea" || name == "SwitchOnArea")
                    {
                        if (!type[index].Prop.ContainsKey("ChildrenArea")) type[index].Prop.Add("ChildrenArea", new C0List());
                        ((C0List)type[index].Prop["ChildrenArea"]).List.Add(itm.Objs[0].Clone());
                    }
                    else 
                    {
                        if (!type[index].Prop.ContainsKey("GenerateChildren")) type[index].Prop.Add("GenerateChildren", new C0List());
                        ((C0List)type[index].Prop["GenerateChildren"]).List.Add(itm.Objs[0].Clone());
                    }
                }
            }
            else if (itm.Type == ClipBoardItem.ClipboardType.ObjectArray)
            {
                if (index < 0 || propertyGrid1.SelectedObject == null) foreach (LevelObj o in itm.Objs) AddObj(o,  type, TypeName, true);
                else
                {
                    if (index < 0 || propertyGrid1.SelectedObject == null) foreach (LevelObj o in itm.Objs) AddObj(o, type, TypeName, true);
                    foreach (LevelObj o in itm.Objs)
                    {
                        string name = o.ToString();
                        if (name == "ObjectChildArea" || name == "SwitchKeepOnArea" || name == "SwitchOnArea")
                        {
                            if (!type[index].Prop.ContainsKey("ChildrenArea")) type[index].Prop.Add("ChildrenArea", new C0List());
                            ((C0List)type[index].Prop["ChildrenArea"]).List.Add(o.Clone());
                        }
                        else
                        {
                            if (!type[index].Prop.ContainsKey("GenerateChildren")) type[index].Prop.Add("GenerateChildren", new C0List());
                            ((C0List)type[index].Prop["GenerateChildren"]).List.Add(o.Clone());
                        }
                    }
                }
            }
            propertyGrid1.Refresh();
            if (index >= 0) UpdateOBJPos(index,  type, CurrentAllInfosSectionName);
        }

        void FindIndex(string type, string PropertyName, string Value)
        {
            Form ToClose = Application.OpenForms["FrmSearchResults"];
            if (ToClose != null) ToClose.Close();
            List<string> HitsNames = new List<string>();
            List<int> HitsIndexes = new List<int>();
            if (type == "AllRailInfos")
            {
                MessageBox.Show("You can't search here");
                
            }
            else
            {
                for (int i = 0; i < GetListByName(type).Count; i++)
                {
                    if (GetListByName(type)[i].Prop.ContainsKey(PropertyName) && GetListByName(type)[i].Prop[PropertyName] is Rail)
                    {
                        if (((Rail)GetListByName(type)[i].Prop[PropertyName]).Name.ToLower() == Value.ToLower())
                        {
                            HitsNames.Add(GetListByName(type)[i].ToString()); HitsIndexes.Add(i);
                        }
                    }
                    else if (GetListByName(type)[i].Prop.ContainsKey(PropertyName))
                    {
                        if (((Node)GetListByName(type)[i].Prop[PropertyName]).StringValue.ToLower().Contains(Value.ToLower()))
                        {
                            HitsNames.Add(GetListByName(type)[i].ToString()); HitsIndexes.Add(i);
                        }
                    }
                    if (GetListByName(type)[i].Prop.ContainsKey("GenerateChildren"))
                    {
                        C0List children = (C0List)GetListByName(type)[i].Prop["GenerateChildren"];
                        for (int ii = 0; ii < children.List.Count; ii++)
                        {
                            if (children.List[ii].Prop.ContainsKey(PropertyName) && children.List[ii].Prop[PropertyName] is Rail)
                            {
                                if (((Rail)children.List[ii].Prop[PropertyName]).Name.ToLower() == Value.ToLower())
                                {
                                    HitsNames.Add(children.List[ii].ToString() + " In GenerateChildren[" + ii.ToString() + "]"); HitsIndexes.Add(i); 
                                }
                            }
                            else if (children.List[ii].Prop.ContainsKey(PropertyName))
                            {
                                if (((Node)children.List[ii].Prop[PropertyName]).StringValue.ToLower().Contains(Value.ToLower()))
                                {
                                    HitsNames.Add(children.List[ii].ToString() + " In GenerateChildren[" + ii.ToString() + "]"); HitsIndexes.Add(i);
                                }
                            }
                        }
                    }
                }
            }
            if (HitsIndexes.Count == 0) { MessageBox.Show("Not found"); return; }
            else
            {
                FormEditors.FrmSearchResults f = new FormEditors.FrmSearchResults(type, HitsNames, HitsIndexes, this);
                f.Text = "Search Results for: " + PropertyName + " = " + Value.ToString() + "  in " + type;
                f.Show();
            }
        }

        void FindIndex(string type, string PropertyName, int Value)
        {
            Form ToClose = Application.OpenForms["FrmSearchResults"];
            if (ToClose != null) ToClose.Close();
            List<string> HitsNames = new List<string>();
            List<int> HitsIndexes = new List<int>();
            if (type == "AllRailInfos")
            {
                if (PropertyName == "l_id")
                {
                    for (int i = 0; i < AllRailInfos.Count; i++)
                    {
                        if (AllRailInfos[i].l_id == Value) { HitsNames.Add(AllRailInfos[i].ToString()); HitsIndexes.Add(i); }
                    }
                }
            }
            else
            {
                for (int i = 0; i < GetListByName(type).Count; i++)
                {
                    if (GetListByName(type)[i].Prop.ContainsKey(PropertyName) && GetListByName(type)[i].Prop[PropertyName] is Node && ((Node)GetListByName(type)[i].Prop[PropertyName]).NodeType == Node.NodeTypes.Int)
                    {
                        if (((Node)GetListByName(type)[i].Prop[PropertyName]).StringValue == Value.ToString()) { HitsNames.Add(GetListByName(type)[i].ToString()); HitsIndexes.Add(i); }
                    }
                    if (GetListByName(type)[i].Prop.ContainsKey("GenerateChildren"))
                    {
                        C0List children = (C0List)GetListByName(type)[i].Prop["GenerateChildren"];
                        for (int ii = 0; ii < children.List.Count; ii++)
                        {
                            if (children.List[ii].Prop.ContainsKey(PropertyName) && children.List[ii].Prop[PropertyName] is Node && ((Node)children.List[ii].Prop[PropertyName]).NodeType == Node.NodeTypes.Int)
                            {
                                if (((Node)children.List[ii].Prop[PropertyName]).StringValue == Value.ToString()) { HitsNames.Add(children.List[ii].ToString() + " In GenerateChildren[" + ii.ToString() + "]"); HitsIndexes.Add(i); }
                            }
                        }
                    }
                }
            }
            if (HitsIndexes.Count == 0) { MessageBox.Show("Not found"); return; }
            else
            {
                FormEditors.FrmSearchResults f = new FormEditors.FrmSearchResults(type, HitsNames, HitsIndexes, this);
                f.Text = "Search Results for: " + PropertyName + " = " + Value.ToString() + "  in " +type;
                f.Show();
            }
        }

        public void SetSelectedObj(string Type, int Index)
        {
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(Type);
            ObjectsListBox.ClearSelected();
            ObjectsListBox.SelectedIndex = Index;
        }
        #endregion

        #region Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadedFile.EndsWith(".szs", StringComparison.InvariantCultureIgnoreCase) && SzsFiles != null) SzsSave(LoadedFile);
            else XmlSave(LoadedFile, LoadedFile.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase));
        }

        void SetupSZS()
        {
            if (SzsFiles == null)
            {
                SzsFiles = new Dictionary<string, byte[]>();
                SzsFiles.Add("CameraParam.byml", BymlConverter.GetByml(Properties.Resources.GenericCameraParam));
                SzsFiles.Add("PreLoadFileList1.byml", BymlConverter.GetByml(Properties.Resources.GenericPreloadList));
                SzsFiles.Add("StageInfo1.byml", BymlConverter.GetByml(Properties.Resources.GenericStageInfo));
                for (int i = 0; i < SzsFiles.Keys.Count; i++)
                {
                    ToolStripMenuItem btn = new ToolStripMenuItem();
                    btn.Name = "LoadFile" + i.ToString();
                    btn.Text = SzsFiles.Keys.ToArray()[i];
                    btn.Click += LoadFileList_click;
                    OtherLevelDataMenu.DropDownItems.Add(btn);
                }
            }
        }
        public Dictionary<string, AllInfoSection> DesignInfos = new Dictionary<string, AllInfoSection>();
        public Dictionary<string, AllInfoSection> SoundInfos = new Dictionary<string, AllInfoSection>();
        public Dictionary<string, AllInfoSection> MapInfos = new Dictionary<string, AllInfoSection>();
        void SzsSave(string filename)
        {
            DesignInfos.Clear();
            SoundInfos.Clear();
            MapInfos.Clear();
            foreach (string type in AllInfos.Keys)
            {
                MapInfos.Add(type, new AllInfoSection());
            }
            foreach (string section in AllInfos.Keys)
            {

                    MapInfos[section].AddRange(AllInfos[section]);
                
            }
            //MapInfos = new Dictionary<string, AllInfoSection>(AllInfos);
            if ((!(filename.Substring(filename.Count() - 15).Contains("Sound") || filename.Substring(filename.Count() - 15).Contains("Design")))&& Properties.Settings.Default.UseDesignSound )
            { 
            int Scenario = 1;
            if (Int32.TryParse(filename.Substring(filename.Count() - 5, 1), out _))
            {
                    Scenario = Int32.Parse(filename.Substring(filename.Count() - 5, 1));
            }
            string DesignFile = filename.Substring(0, filename.Count() - 8) + "Design" + Scenario.ToString() + ".szs";
            string SoundFile = filename.Substring(0, filename.Count() - 8) + "Sound" + Scenario.ToString() + ".szs";
                DesignInfos.Add("AreaObjInfo", new AllInfoSection());
                SoundInfos.Add("AreaObjInfo", new AllInfoSection());
                SoundInfos.Add("ObjInfo", new AllInfoSection());
                CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SFSDirectory dir = new SFSDirectory("", true);
            SFSDirectory dsgndir = new SFSDirectory("", true);
            SFSDirectory snddir = new SFSDirectory("", true);
            for (int i = 0; i < SzsFiles.Count; i++)
            {
                SFSFile file = new SFSFile(i, SzsFiles.Keys.ToArray()[i], dir);
                file.Data = SzsFiles.Values.ToArray()[i];
                if (SzsFiles.Keys.ToArray()[i].Contains("FogParam")|| SzsFiles.Keys.ToArray()[i].Contains("LightParam")|| SzsFiles.Keys.ToArray()[i].Contains("ModelTo")||SzsFiles.Keys.ToArray()[i].Contains("AreaIdTo")) 
                {
                    dsgndir.Files.Add(file);
                }
                else
                {
                    dir.Files.Add(file);
                }

            }
            SFSFile StgData = new SFSFile(SzsFiles.Count, "StageData.byml", dir);
            StgData.Data = BymlConverter.GetByml(MakeXML(true));
            dir.Files.Add(StgData);
            SzsArch.FromFileSystem(dir);
            File.WriteAllBytes(filename, y.Compress(SzsArch.Write()));
            if (DesignInfos["AreaObjInfo"].Count>0) 
                {
                    SzsArch = new NDS.NitroSystem.FND.NARC();
                    StgData = new SFSFile(SzsFiles.Count, "StageData.byml", dsgndir);
                    StgData.Data = BymlConverter.GetByml(MakeXML(false, 1));
                    dsgndir.Files.Add(StgData);
                    SzsArch.FromFileSystem(dsgndir);
                    File.WriteAllBytes(DesignFile, y.Compress(SzsArch.Write()));
                }
            if (SoundInfos["AreaObjInfo"].Count > 0||SoundInfos["ObjInfo"].Count>0)
                {
                    SzsArch = new NDS.NitroSystem.FND.NARC();
                    StgData = new SFSFile(SzsFiles.Count, "StageData.byml", snddir);
                    StgData.Data = BymlConverter.GetByml(MakeXML(false, 2));
                    snddir.Files.Add(StgData);
                    SzsArch.FromFileSystem(snddir);
                    File.WriteAllBytes(SoundFile, y.Compress(SzsArch.Write()));
                }
                MessageBox.Show("Done !");
            }
            else
            {
                CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
                NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
                SFSDirectory dir = new SFSDirectory("", true);
                for (int i = 0; i < SzsFiles.Count; i++)
                {
                    SFSFile file = new SFSFile(i, SzsFiles.Keys.ToArray()[i], dir);
                    file.Data = SzsFiles.Values.ToArray()[i];
                    dir.Files.Add(file);
                }
                SFSFile StgData = new SFSFile(SzsFiles.Count, "StageData.byml", dir);
                StgData.Data = BymlConverter.GetByml(MakeXML());
                dir.Files.Add(StgData);
                SzsArch.FromFileSystem(dir);
                File.WriteAllBytes(filename, y.Compress(SzsArch.Write()));
                MessageBox.Show("Done !");
            }
        }

        void XmlSave(string filename, bool BYML)
        {
            if (BYML) File.WriteAllBytes(filename, BymlConverter.GetByml(MakeXML()));
            else File.WriteAllText(filename, MakeXML(), DefEnc);
            MessageBox.Show("Done !");
        }

        private void saveAsSZSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.FileName = Path.GetFileNameWithoutExtension(LoadedFile);
            sav.Filter = "szs file|*.szs";
            if (sav.ShowDialog() == DialogResult.OK)
            {
                SzsSave(sav.FileName);
                LoadedFile = sav.FileName;
                this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + LoadedFile;
                saveToolStripMenuItem.Enabled = true;
            }
        }

        private void saveAsXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.FileName = Path.GetFileNameWithoutExtension(LoadedFile);
            sav.Filter = "Xml file|*.xml";
            if (sav.ShowDialog() == DialogResult.OK)
            {
                XmlSave(sav.FileName, false);
                LoadedFile = sav.FileName;
                this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + LoadedFile;
                saveToolStripMenuItem.Enabled = true;
            }
        }

        private void saveAsBymlToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.FileName = Path.GetFileNameWithoutExtension(LoadedFile);
            sav.Filter = "Byml file|*.byml";
            if (sav.ShowDialog() == DialogResult.OK)
            {
                XmlSave(sav.FileName, true);
                LoadedFile = sav.FileName;
                this.Text = LoadedFile == "" ? "The 4th Dimension - by Exelix11" : "The 4th Dimension - " + LoadedFile;
                saveToolStripMenuItem.Enabled = true;
            }
        }

        string MakeXML(bool getobjs = false, int snddsgn = 0)
        {
            using (var stream = new MemoryStream())
            {
                using (var xr = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true, Encoding = DefEnc }))
                {
                    xr.WriteStartDocument();
                    xr.WriteStartElement("Root");
                    xr.WriteStartElement("isBigEndian");
                    xr.WriteAttributeString("Value", "False");
                    xr.WriteEndElement();
                    xr.WriteStartElement("BymlFormatVersion");
                    xr.WriteAttributeString("Value", ((uint)1).ToString());
                    xr.WriteEndElement();
                    xr.WriteStartElement("C1"); //Byml Root
                    xr.WriteStartElement("C1");
                    xr.WriteAttributeString("Name", "AllInfos");
                    if (snddsgn == 0) { 
                        if (getobjs == true)
                        {
                            List<string> keys = AllInfos.Keys.ToList();
                            keys.Sort(StringComparer.Ordinal);
                            foreach (string k in keys)
                            {
                                if (AllInfos[k].Count != 0)
                                {
                                    WriteOBJInfoSection(xr, k, AllInfos[k], "C0", true);
                                }

                            }
                        }
                        else
                        {
                            List<string> keys = MapInfos.Keys.ToList();
                            keys.Sort(StringComparer.Ordinal);
                            foreach (string k in keys)
                            {
                                if (MapInfos[k].Count != 0)
                                {
                                    WriteOBJInfoSection(xr, k, MapInfos[k]);
                                }

                            }
                        }
                    }
                    else if(snddsgn == 1)
                    {
                        List<string> keys = DesignInfos.Keys.ToList();
                        keys.Sort(StringComparer.Ordinal);
                        foreach (string k in keys)
                        {
                            if (DesignInfos[k].Count != 0)
                            {
                                WriteOBJInfoSection(xr, k, DesignInfos[k]);
                            }

                        }
                    }
                    else
                    {
                        List<string> keys = SoundInfos.Keys.ToList();
                        keys.Sort(StringComparer.Ordinal);
                        foreach (string k in keys)
                        {
                            if (SoundInfos[k].Count != 0)
                            {
                                WriteOBJInfoSection(xr, k, SoundInfos[k]);
                            }

                        }
                    }
                    xr.WriteEndElement();
                    xr.WriteStartElement("C1");
                    xr.WriteAttributeString("Name", "AllRailInfos");
                    if (AllRailInfos.Count != 0 && snddsgn == 0)
                    {
                        xr.WriteStartElement("C0");
                        xr.WriteAttributeString("Name", "RailInfo");
                        foreach (Rail r in AllRailInfos)
                        {
                            xr.WriteStartElement("C1");
                            WriteRail(xr, r);
                            xr.WriteEndElement();
                        }
                        xr.WriteEndElement();
                    }
                    xr.WriteEndElement();
                    xr.WriteStartElement("C0");
                    xr.WriteAttributeString("Name", "LayerInfos");
                    WriteLayerInfos(xr, snddsgn);
                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.Close();
                }

                return (DefEnc.GetString(stream.ToArray()));
            }
        }

        void WriteLayerInfos(XmlWriter xr, int snddsgn = 0)
        {
            //string[] LayerNames = new string[5] { "共通", "共通サブ", "シナリオ1", "シナリオ1＆2", "シナリオ1＆3" }; //PlaceHolders
            List<string> LayerNames = new List<string>();
            Dictionary<string, Dictionary<string, List<LevelObj>>> _AllInfos = new Dictionary<string, Dictionary<string, List<LevelObj>>>();
            List<string> keys;
            if (snddsgn == 0) 
            {
                 keys = MapInfos.Keys.ToList();
            }
            else if (snddsgn == 1)
            {
                keys = DesignInfos.Keys.ToList();
            }
            else 
            { 
                keys = SoundInfos.Keys.ToList(); 
            }

            keys.Sort(StringComparer.Ordinal);
            if (snddsgn == 0)
            {
                foreach (string k in keys)
                {
                    if (MapInfos[k].Count != 0)
                    {
                        _AllInfos.Add(k, new Dictionary<string, List<LevelObj>>());
                        ProcessLayerNames(MapInfos[k], _AllInfos[k], LayerNames);
                    }
                }
            }else if (snddsgn == 1)
            {
                foreach (string k in keys)
                {
                    if (DesignInfos[k].Count != 0)
                    {
                        _AllInfos.Add(k, new Dictionary<string, List<LevelObj>>());
                        ProcessLayerNames(DesignInfos[k], _AllInfos[k], LayerNames);
                    }
                }
            }
            else
            {
                foreach (string k in keys)
                {
                    if (SoundInfos[k].Count != 0)
                    {
                        _AllInfos.Add(k, new Dictionary<string, List<LevelObj>>());
                        ProcessLayerNames(SoundInfos[k], _AllInfos[k], LayerNames);
                    }
                }
            }

            for (int i = 0; i < LayerNames.Count; i++)
            {
                xr.WriteStartElement("C1");
                xr.WriteStartElement("C1");
                xr.WriteAttributeString("Name", "Infos");
                foreach (string k in _AllInfos.Keys)
                {
                    if (_AllInfos[k].ContainsKey(LayerNames[i]))
                    {
                        WriteOBJInfoSection(xr, k, _AllInfos[k][LayerNames[i]]);
                    }
                }
                xr.WriteEndElement();
                xr.WriteStartElement("A0");
                xr.WriteAttributeString("Name", "LayerName");
                xr.WriteAttributeString("StringValue", GetEnglishName(LayerNames[i]));
                xr.WriteEndElement();
                xr.WriteEndElement();
            }
        }

        string GetEnglishName(string Name)
        {
            if (Name == "共通") return "Common";
            else if (Name == "共通サブ") return "CommonSub";
            else if (Name.StartsWith("シナリオ"))
            {
                if (Name.Length == "シナリオ1".Length) return "Scenario" + Name.Substring("シナリオ".Length, 1);
                else return "Scenario" + Name.Substring("シナリオ".Length, 1) + "And" + Name.Substring("シナリオ1＆".Length, 1);
            }
            else throw new Exception("Unsupported name !");
        }

        void ProcessLayerNames( List<LevelObj> list, Dictionary<string, List<LevelObj>> Dict,  List<string> AllLayerNames)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string layerName = ((Node)list[i].Prop["LayerName"]).StringValue;
                if (Dict.ContainsKey(layerName)) Dict[layerName].Add(list[i]);
                else { Dict.Add(layerName, new List<LevelObj>()); Dict[layerName].Add(list[i]); }
                if (!AllLayerNames.Contains(layerName)) AllLayerNames.Add(layerName);
            }
            if (AllLayerNames.Count > 5) throw new Exception("Too many layer names !");
        }

        void WriteOBJInfoSection(XmlWriter xr, string name, List<LevelObj> list, string startelement = "C0", bool getobjs = false)
        {

            Dictionary<string, LevelObj> delete = new Dictionary<string, LevelObj>();
            xr.WriteStartElement(startelement);
            xr.WriteAttributeString("Name", name);
            list = new List<LevelObj>(list);
            foreach (LevelObj obj in list)
            {
                if (getobjs == true)
                {
                    string o = obj.ObjectData["name"].ToString().Substring(9);
                    switch (o)
                    {//AreaObjInfo ObjInfo
                        case "FogArea":
                            DesignInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "FogAreaCameraPos":
                            DesignInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "LightArea":
                            DesignInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "AudioVolumeSettingArea":
                            SoundInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "AudioEffectChangeArea":
                            SoundInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "BgmChangeArea":
                            SoundInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        case "SoundEmitObj":
                            SoundInfos["ObjInfo"].Add(obj);
                            MapInfos["ObjInfo"].Remove(obj);
                            break;
                        case "SoundEmitArea":
                            SoundInfos["AreaObjInfo"].Add(obj);
                            MapInfos["AreaObjInfo"].Remove(obj);
                            break;
                        default:
                            WriteOBJ(xr, obj);
                            break;
                    }
                    
                }
                else
                {
                    WriteOBJ(xr, obj);
                }
            }
            xr.WriteEndElement();
        }

        void WriteOBJ(XmlWriter xr, LevelObj obj)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            if (obj.ToString().StartsWith("@")) return; //for @CameraPositionHelper
            xr.WriteStartElement("C1");
            List<string> Keys = obj.Prop.Keys.ToList();
            Keys.Sort(StringComparer.Ordinal);
            foreach (string Key in Keys)
            {
                Object node = obj.Prop[Key];
                if (node is int[]) //Args
                {
                    int[] tmp = ((int[])node);
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        xr.WriteStartElement("D1");
                        xr.WriteAttributeString("Name", Key + i.ToString());
                        xr.WriteAttributeString("StringValue", tmp[i].ToString());
                        xr.WriteEndElement();
                    }
                }
                else if (node is Single[]) //POS, ROT, SCA XYZ
                    
                    {
                        Single[] tmp = ((Single[])node);
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            string side = "";
                            switch (i) 
                            {
                            case 0:
                                side = "_x";
                                break;
                            case 1:
                                side = "_y";
                                break;
                            case 2:
                                side = "_z";
                                break;
                        }

                            xr.WriteStartElement("D2");
                            xr.WriteAttributeString("Name", Key + side/*.ToString()*/);
                            xr.WriteAttributeString("StringValue", tmp[i].ToString());
                            xr.WriteEndElement();
                        }
                    }
                    else if (node is C0List)
                {
                    C0List tmp = (C0List)node;
                    xr.WriteStartElement("C0");
                    xr.WriteAttributeString("Name", Key);
                    foreach (LevelObj o in tmp.List) WriteOBJ(xr, o);
                    xr.WriteEndElement();
                }
                else if (node is Rail)
                {
                    Rail tmp = (Rail)node;
                    xr.WriteStartElement("C1");
                    xr.WriteAttributeString("Name", Key);
                    WriteRail(xr, tmp);
                    xr.WriteEndElement();
                }
                else
                {
                    Node tmp = (Node)node;
                    string startelement = tmp._StringNodeType;
                    if (tmp.NodeType == Node.NodeTypes.Empty) startelement = "A1";
                    else if (tmp.NodeType == Node.NodeTypes.String) startelement = "A0";
                    else if (tmp.NodeType == Node.NodeTypes.Int) startelement = "D1";
                    else if (tmp.NodeType == Node.NodeTypes.Single) startelement = "D2";
                    xr.WriteStartElement(startelement);
                    xr.WriteAttributeString("Name", Key);
                    xr.WriteAttributeString("StringValue", tmp.StringValue);
                    xr.WriteEndElement();
                }
            }
            xr.WriteEndElement();
        }

        void WriteRail(XmlWriter xr, Rail r)
        {
            for (int i = 0; i < r.Arg.Count; i++)
            {
                xr.WriteStartElement("D1");
                xr.WriteAttributeString("Name", "Arg" + i.ToString());
                xr.WriteAttributeString("StringValue", r.Arg[i].ToString());
                xr.WriteEndElement();
            }
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "LayerName");
            xr.WriteAttributeString("StringValue", r.LayerName);
            xr.WriteEndElement();
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "MultiFileName");
            xr.WriteAttributeString("StringValue", "StageData_tool");
            xr.WriteEndElement();
            xr.WriteStartElement("C0");
            xr.WriteAttributeString("Name", "Points");
            foreach (Rail.Point p in r.Points) writePoint(xr, p);
            xr.WriteEndElement();
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "closed");
            xr.WriteAttributeString("StringValue", r._closed);
            xr.WriteEndElement();
            xr.WriteStartElement("D1");
            xr.WriteAttributeString("Name", "l_id");
            xr.WriteAttributeString("StringValue", r.l_id.ToString());
            xr.WriteEndElement();
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "name");
            xr.WriteAttributeString("StringValue", r.Name);
            xr.WriteEndElement();
            xr.WriteStartElement("D1");
            xr.WriteAttributeString("Name", "no");
            xr.WriteAttributeString("StringValue", r.no.ToString());
            xr.WriteEndElement();
            xr.WriteStartElement("D1");
            xr.WriteAttributeString("Name", "num_pnt");
            xr.WriteAttributeString("StringValue", r.Points.Count.ToString());
            xr.WriteEndElement();
            xr.WriteStartElement("A0");
            xr.WriteAttributeString("Name", "type");
            xr.WriteAttributeString("StringValue", r.Type);
            xr.WriteEndElement();
        }

        void writePoint(XmlWriter xr, Rail.Point p)
        {
            xr.WriteStartElement("C1");
            for (int i = 0; i < p.Args.Count; i++)
            {
                xr.WriteStartElement("D1");
                xr.WriteAttributeString("Name", "Arg" + i.ToString());
                xr.WriteAttributeString("StringValue", p.Args[i].ToString());
                xr.WriteEndElement();
            }
            xr.WriteStartElement("D1");
            xr.WriteAttributeString("Name", "id");
            xr.WriteAttributeString("StringValue", p.ID.ToString());
            xr.WriteEndElement();
            for (int i = 0; i < p._X.Count; i++)
            {
                xr.WriteStartElement("D2");
                xr.WriteAttributeString("Name", "pnt" + i.ToString() + "_x");
                xr.WriteAttributeString("StringValue", p._X[i].ToString());
                xr.WriteEndElement();
                xr.WriteStartElement("D2");
                xr.WriteAttributeString("Name", "pnt" + i.ToString() + "_y");
                xr.WriteAttributeString("StringValue", p._Y[i].ToString());
                xr.WriteEndElement();
                xr.WriteStartElement("D2");
                xr.WriteAttributeString("Name", "pnt" + i.ToString() + "_z");
                xr.WriteAttributeString("StringValue", p._Z[i].ToString());
                xr.WriteEndElement();
            }
            xr.WriteEndElement();
        }


        #endregion

        #region Settings

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormEditors.Settings(render).ShowDialog();
            /*
            SettingsPanel.Visible = true;
            CamInertiaUpDown.Value = (decimal)render.CameraInertiaFactor;
            ChbFps.Checked = render.ShowFps;
            ChbTriCount.Checked = render.ShowTriangleCount;
            ChbDebugInfo.Checked = render.ShowDebugInfo;
            cbCameraMode.SelectedIndex = render.CamMode == HelixToolkit.Wpf.CameraMode.Inspect ? 0 : 1;
            ZoomSenUpDown.Value = (decimal)render.ZoomSensitivity;
            RotSenUpDown.Value = (decimal)render.RotationSensitivity;
            ChbAddCameraMove.Checked = AutoMoveCam;
            ChbStartupUpdate.Checked = Properties.Settings.Default.CheckUpdates;
            ChbStartupDb.Checked = Properties.Settings.Default.DownloadDb;
            tbUrl.Text = Properties.Settings.Default.DownloadDbLink;
            chbAddObjectOrigin.Checked = AddObjectOrigin;
            SettingsPanel.Focus();*/
        }

        #endregion

        private async void StartupChecks_DoWork(object sender, DoWorkEventArgs e)
        {
            string state = "";
            try
            {               
                if (Properties.Settings.Default.DownloadDb)
                {
                    state = "downloading object database";
                    if (File.Exists("objectdb.xml.bak")) File.Delete("objectdb.xml.bak");
                    if (File.Exists("objectdb.xml")) File.Move("objectdb.xml", "objectdb.xml.bak");
                    new WebClient().DownloadFile(Properties.Settings.Default.DownloadDbLink == "" ? ObjectDbLink : Properties.Settings.Default.DownloadDbLink, "objectdb.xml");
                    if (!File.Exists("objectdb.xml"))
                    {
                        MessageBox.Show("There was an error downloading the object database");
                        if (File.Exists("objectdb.xml.bak")) File.Move("objectdb.xml.bak", "objectdb.xml");
                    }
                }
                if (Properties.Settings.Default.CheckUpdates)
                {
                    state = " checking updates";
                    
                    var githubClient = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("TheFourthDimension"));
                    var ver = await githubClient.Repository.Release.GetAll("exelix11", "TheFourthDimension");
                    if (ver.Count > ReleaseId)
                    {
                        if (MessageBox.Show("There's a new version of T4D available, do you want to open its release page in your web browser?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            System.Diagnostics.Process.Start("https://github.com/KirbysDarkNebula/t4d-qol/releases/latest");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error" + state + ": " + ex.Message);
                if (File.Exists("objectdb.xml.bak") && !File.Exists("objectdb.xml")) File.Move("objectdb.xml.bak", "objectdb.xml");
            }
        }

        private void StartupChecks_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusLbl.Text = "";
            StatusLbl.Visible = false;
            downloadLatestObjectDatabaseToolStripMenuItem.Enabled = true;
            if (ObjectDatabase == null) LoadObjectDatabase();
        }

        private void guideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/exelix11/TheFourthDimension/blob/master/guide.md");
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
        private void downloadLatestT4DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/KirbysDarkNebula/t4d-qol/releases/latest");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != ""&&(int.TryParse(textBox1.Text, out _)== false || int.Parse(textBox1.Text) > 9999)) { textBox1.Text = 10.ToString(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out _) == false|| textBox1.Text == "") { textBox1.Text = 10.ToString(); }
            Int32 add;
            int.TryParse(textBox1.Text, out add);
            string type = comboBox1.Text;
            int id = ObjectsListBox.SelectedIndex;
            string kind = "";
            switch (IncType.SelectedIndex)
            {
                case 0:
                    kind = "pos";
                    break;
                case 1:
                    kind = "dir";
                    break;
                case 2:
                    kind = "scale";
                    break;
            }
            int OldValue = (int)((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex];
            ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex] += (Single)add;
            Single current = ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex];
            if (IncType.SelectedIndex == 1 && (current > 360 || current < -360))
            {
                while (current > 360)
                {
                    ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex] -= 360;
                    current = ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex];
                }
                while (current < -360)
                {
                    ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex] += 360;
                    current = ((Single[])GetListByName(type)[id].Prop[kind])[IncAxis.SelectedIndex];
                }
            }
            string name = "["+IncAxis.SelectedIndex.ToString()+"]";
            string pname = kind;

            Action<object[]> action;
            action = (object[] args) =>
            {
                
                    List<LevelObj> typ = (List<LevelObj>)args[0];
                    render.ClearSelection();
                    string TypeName = (string)args[1];
                    int ido = (int)args[2];
                    string propName = (string)args[5];
                    object value = args[4];
                    int axis = Int32.Parse(String.Join("", ((string)args[3]).Split('[', ']')));
                    if (propName == "pos" || propName == "scale" || propName == "dir")
                        ((Single[])(((typ)[ido].Prop[propName])))[axis] = Single.Parse((value).ToString());
                    else
                    {
                        ((Int32[])(((typ)[ido].Prop[propName])))[axis] = (Int32)value;
                    }
                    propertyGrid1.Refresh();
                    if (typ.GetHashCode() == CurrentAllInfosSection.GetHashCode()) UpdateOBJPos(ido, typ, TypeName, true);
                
            };
            Undo.Push(new UndoAction("Changed value: " + name + " of array " + pname + " of object: " + CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString(), new object[] { CurrentAllInfosSection, CurrentAllInfosSectionName, ObjectsListBox.SelectedIndex, name, OldValue, pname }, action));
            UpdateOBJPos(id, GetListByName(type), type);
            propertyGrid1.Update();
            propertyGrid1.Refresh();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out _) == false || textBox1.Text == "") { textBox1.Text = 10.ToString(); }
            Int32 add;
            int.TryParse(textBox1.Text, out add);
            string typo = comboBox1.Text;
            int ido = ObjectsListBox.SelectedIndex;
            string kind = "";
            switch (IncType.SelectedIndex)
            {
                case 0:
                    kind = "pos";
                    break;
                case 1:
                    kind = "dir";
                    break;
                case 2:
                    kind = "scale";
                    break;
            }
            int OldValue = (int)((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex];
            ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex] -= (Single)add;
            Single current = ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex];
            if (IncType.SelectedIndex == 1 && (current > 360 || current < -360))
            {
                while (current > 360) 
                { 
                    ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex] -= 360;
                    current = ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex];
                }
                while (current < -360) 
                { 
                    ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex] += 360;
                    current = ((Single[])GetListByName(typo)[ido].Prop[kind])[IncAxis.SelectedIndex];
                }
            }

            


            string name = "["+IncAxis.SelectedIndex.ToString()+"]";
            string pname = kind;

            Action<object[]> action;
            action = (object[] args) =>
            {
                
                    List<LevelObj> type = (List<LevelObj>)args[0];
                    render.ClearSelection();
                    string TypeName = (string)args[1];
                    int id = (int)args[2];
                    string propName = (string)args[5];
                    object value = args[4];
                    int axis = Int32.Parse(String.Join("", ((string)args[3]).Split('[', ']')));
                    if (propName == "pos" || propName == "scale" || propName == "dir")
                        ((Single[])(((type)[id].Prop[propName])))[axis] = Single.Parse((value).ToString());
                    else
                    {
                        ((Int32[])(((type)[id].Prop[propName])))[axis] = (Int32)value;
                    }
                    propertyGrid1.Refresh();
                    if (type.GetHashCode() == CurrentAllInfosSection.GetHashCode()) UpdateOBJPos(id, type, TypeName, true);
                
            };
            Undo.Push(new UndoAction("Changed value: " + name + " of array " + pname + " of object: " + CurrentAllInfosSection[ObjectsListBox.SelectedIndex].ToString(), new object[] { CurrentAllInfosSection, CurrentAllInfosSectionName, ObjectsListBox.SelectedIndex, name, OldValue, pname }, action));
            UpdateOBJPos(ido, GetListByName(typo), typo);
            propertyGrid1.Update();
            propertyGrid1.Refresh();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                
                e.Effect = DragDropEffects.Copy;
                elementHost1.Hide();
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {

            string[] filename = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            LoadFile(filename[0]);
        }

        private void Form1_DragLeave(object sender, EventArgs e)
        {
            if (this.Text.Contains(@"\"))
            {
                elementHost1.Show();
            }
        }

        private void objectByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditors.FrmSearchValInput f = new FormEditors.FrmSearchValInput(true);
            f.ShowDialog();
            if (f.Res == null || (string)f.Res == "") return;
            FindIndex(comboBox1.Text, "name", (string)f.Res);
        }
    }
}