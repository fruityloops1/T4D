using _3DS.NintendoWare.GFX;
using CommonFiles;
using LibEveryFileExplorer.Files.SimpleFileSystem;
using Microsoft.WindowsAPICodePack.Dialogs;
using ModelViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static The4Dimension.Ohana.RenderBase;

namespace The4Dimension.FormEditors
{
    public partial class FrmObjImport : Form
    {
        string filename = "";
        string modelPath = "";
        string ObjModelPath = "";
        bool IsObj;
        public UserControl1 render = new UserControl1();
        string tmpPath = "";
        string KclPath = "";
        string PaPath = "";
        private Dictionary<string, string> strings;
        public FrmObjImport()
        {
            strings = new Dictionary<string, string>();
            InitializeComponent();
            comboBox1.SelectedIndex = 5;
            elementHost1.Child = render;
            render.AddKey("Model");
        }

        private void FrmObjImport_Load(object sender, EventArgs e)
        {
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
                            case "ModelImport":
                                CForm = "FrmObjImport";
                                break;
                            default:
                                break;
                        }
                    }
                    if (LANG.NodeType == XmlNodeType.EndElement)
                    {
                        switch (LANG.Name)
                        {
                            case "ModelImport":
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
                }
            }
            else 
            {
                //Do not edit these unless you find translation errors, change the ones in your preferred language xml instead
                strings.Add("label4","KCL: ");
                strings.Add("label5","PA: ");
                strings.Add("notset", "not set");
                strings.Add("objconvY", "The obj will be converted to bcmdl with Every File Explorer's method, this is known to have problems, especially with models made in sketchup.\r\nDo you want to continue ?");
                strings.Add("objconvN", "You can convert the model to bcmdl with the leaked tools");
                strings.Add("nosup", "File not supported");
                strings.Add("selectmodel", "You must select your model file to use this function");
                strings.Add("enotfound", "not found!");
                strings.Add("notfound", "Files not found, something went wrong!");
                strings.Add("panf", "Pa file not found!");
                strings.Add("kclnf", "Kcl file not found!");
                strings.Add("modelnf", "Model file not found!");
                strings.Add("done", "Done!");
                strings.Add("CC", "Remember you need to add the object to the CreatorClassNameTable to use the object in-game (Other modding -> CreatorClassNameTable editor)");
                strings.Add("view1", "To view the model in the editor you must copy it in the models folder with the name ");
                strings.Add("view2", ".obj or else you will see a blue box");
                strings.Add("kclpa", "The kcl and pa files were saved in :");
                strings.Add("rename", "\r\n Don't forget to rename them to your model name!");
                strings.Add("unsure", "If unsure leave the default option (collision terrain/option no. 6)");
            }
            #endregion
            if (Directory.Exists(Path.GetTempPath() + "TmpT4D")) { try { Directory.Delete(Path.GetTempPath() + "TmpT4D", true); } catch { } }
            OpenFileDialog opn = new OpenFileDialog();
            opn.Title = "Open a model file";
            opn.Filter = "Supported formats (.bcmdl, .obj)|*.bcmdl; *.obj";
            bool ok = true;
            if (opn.ShowDialog() != DialogResult.OK) ok = false;
            if (Path.GetExtension(opn.FileName).ToLower() == ".obj")
            {
                if (MessageBox.Show(strings["objconvY"], "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    modelPath = opn.FileName;
                    ObjModelPath = modelPath;
                    IsObj = true;
                    render.addModel(modelPath, "Model", new System.Windows.Media.Media3D.Vector3D(0, 0, 0), new System.Windows.Media.Media3D.Vector3D(1, 1, 1), 0, 0, 0);
                }
                else
                {
                    MessageBox.Show(strings["objconvN"]);
                    this.Close();
                }
            }
            else if (Path.GetExtension(opn.FileName).ToLower() == ".bcmdl")
            {
                tmpPath = Path.GetTempPath() + "TmpT4D";
                filename = Path.GetFileNameWithoutExtension(opn.FileName);
                string Name = tmpPath + "\\"+Path.GetFileNameWithoutExtension(opn.FileName)+".obj";
                Directory.CreateDirectory(tmpPath);
                CGFX mod = null;
                mod = new _3DS.NintendoWare.GFX.CGFX(File.ReadAllBytes(opn.FileName));
                CommonFiles.OBJ o = mod.Data.Models[0].ToOBJ();
                o.MTLPath = Path.GetFileNameWithoutExtension(Name) + ".mtl";
                MTL m = mod.Data.Models[0].ToMTL("Tex");
                byte[] d = o.Write();
                byte[] d2 = m.Write();
                File.Create(Name).Close();
                File.WriteAllBytes(Name, d);
                File.Create(Path.ChangeExtension(Name, "mtl")).Close();
                File.WriteAllBytes(Path.ChangeExtension(Name, "mtl"), d2);
                Directory.CreateDirectory(tmpPath + "\\Tex");
                foreach (var v in mod.Data.Textures)
                {
                    if (!(v is ImageTextureCtr)) continue;
                    if (File.Exists(tmpPath + "\\Tex\\" + v.Name + ".png")) continue;
                    ((ImageTextureCtr)v).GetBitmap().Save(tmpPath + "\\Tex\\" + v.Name + ".png");
                }
                modelPath = opn.FileName;
                ObjModelPath = Name;
                IsObj = false;
                render.addModel(Name, "Model", new System.Windows.Media.Media3D.Vector3D(0, 0, 0), new System.Windows.Media.Media3D.Vector3D(1, 1, 1), 0, 0, 0);
                textBox1.Text = mod.Data.Models[0].Name;
                textBox1.Enabled = false;
                render.SetSortFrequency(0);
            }
            else
            {
                if (ok) MessageBox.Show(strings["nosup"]);
                else MessageBox.Show(strings["selectmodel"]);
                this.Close();
            }
        }

        private void f_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ObjModelPath == "") return;
            render.RemoveModel("Model", 0);
            render.Clear();
            elementHost1.Dispose();
            try { Directory.Delete(tmpPath, true); } catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "Kcl file|*.kcl";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = false;
                KclPath = opn.FileName;
                label4.Text = Path.GetFileName(KclPath);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            opn.Filter = "Pa file|*.pa";
            if (opn.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = false;
                PaPath = opn.FileName;
                label5.Text = Path.GetFileName(PaPath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists("CollisionsMng.exe"))
            {
                MessageBox.Show("CollisionsMng.exe"+strings["enotfound"]);
                return;
            }
            button2.Enabled = false;
            Process p = new Process();
            ProcessStartInfo s = new ProcessStartInfo();
            s.FileName = "CollisionsMng.exe";
            s.Arguments = (!ObjModelPath.StartsWith("\"")) ? "\"" + ObjModelPath + "\"" : ObjModelPath;
            p.StartInfo = s;
            p.Start();
            p.WaitForExit();
            if (File.Exists(ObjModelPath + ".kcl") && File.Exists(ObjModelPath + ".pa"))
            {
                PaPath = ObjModelPath + ".pa";
                label5.Text = strings["label5"] + filename + ".pa";
                KclPath = ObjModelPath + ".kcl";
                label4.Text = strings["label4"] + filename + ".kcl";
                button7.Enabled = true;
            }
            else MessageBox.Show(strings["notfound"]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(PaPath)) { MessageBox.Show(strings["panf"]); return; }
            if (!File.Exists(KclPath)) { MessageBox.Show(strings["kclnf"]); return; }
            if (!File.Exists(modelPath)) { MessageBox.Show(strings["modelnf"]); return; }
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "Szs files|*.szs";
            s.FileName = textBox1.Text;
            if (s.ShowDialog() != DialogResult.OK) return;
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SFSDirectory dir = new SFSDirectory("", true);

            //Model
            SFSFile Model = new SFSFile(0, textBox1.Text + ".bcmdl", dir);
            if (IsObj)
            {
                CGFX mod = null;
                mod = new CGFX();
                CGFXGenerator.FromOBJ(mod, modelPath, textBox1.Text);
                Model.Data = mod.Write();
            }
            else Model.Data = File.ReadAllBytes(modelPath);
            dir.Files.Add(Model);
            //Collisions
            SFSFile KclFile = new SFSFile(1, textBox1.Text + ".kcl", dir);
            KclFile.Data = File.ReadAllBytes(KclPath);
            dir.Files.Add(KclFile);
            SFSFile PaFile = new SFSFile(2, textBox1.Text + ".pa", dir);
            PaFile.Data = File.ReadAllBytes(PaPath);
            dir.Files.Add(PaFile);
            //InitSensor
            SFSFile Sensor = new SFSFile(3, "InitSensor.byml", dir);
            Sensor.Data = BymlConverter.GetByml(Properties.Resources.Sensor);
            dir.Files.Add(Sensor);
            //InitActor
            string actor = "<?xml version =\"1.0\" encoding=\"shift_jis\"?>\r\n<Root>\r\n  <isBigEndian Value=\"False\" />\r\n  <BymlFormatVersion Value=\"1\" />\r\n  <C1>\r\n    <C1 Name=\"Collision\">\r\n      <A0 Name=\"Sensor\" StringValue=\"Collision\" />\r\n    </C1>\r\n    <C1 Name=\"Debug\">\r\n      <A0 Name=\"NodeName\" StringValue=\"地形\" />\r\n    </C1>\r\n    <C1 Name=\"Executor\">\r\n      <A0 Name=\"CategoryName\" StringValue=\"" + comboBox1.SelectedItem + "\" />\r\n    </C1>\r\n    <C1 Name=\"GroupClipping\">\r\n      <D1 Name=\"MaxCount\" StringValue=\"64\" />\r\n    </C1>\r\n    <FF Name=\"Model\" StringValue=\"00000000\" />\r\n  </C1>\r\n</Root>";
            SFSFile Actor = new SFSFile(4, "InitActor.byml", dir);
            Actor.Data = BymlConverter.GetByml(actor);//Properties.Resources.Actor);
            dir.Files.Add(Actor);
            //InitClipping
            string clip = "<?xml version=\"1.0\" encoding=\"shift_jis\"?>\r\n<Root>\r\n  <isBigEndian Value=\"False\" />\r\n  <BymlFormatVersion Value=\"1\" />\r\n  <C1>\r\n    <D2 Name=\"Radius\" StringValue=\"" + numericUpDown1.Value.ToString() + "\" />\r\n  </C1>\r\n</Root>";
            SFSFile Clipping = new SFSFile(5, "InitClipping.byml", dir);
            Clipping.Data = BymlConverter.GetByml(clip);
            dir.Files.Add(Clipping);

            SzsArch.FromFileSystem(dir);
            File.WriteAllBytes(s.FileName, y.Compress(SzsArch.Write()));
            MessageBox.Show(strings["done"]);
            MessageBox.Show(strings["CC"]);
            MessageBox.Show(strings["view2"] + textBox1.Text + strings["view2"]);
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Properties.Resources.ExecName);
            MessageBox.Show(strings["unsure"]);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "Szs files|*.szs";
            s.FileName = textBox1.Text;
            if (s.ShowDialog() != DialogResult.OK) return;
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SFSDirectory dir = new SFSDirectory("", true);

            //Model
            SFSFile Model = new SFSFile(0, textBox1.Text + ".bcmdl", dir);
            if (IsObj)
            {
                CGFX mod = null;
                mod = new CGFX();
                CGFXGenerator.FromOBJ(mod, modelPath, textBox1.Text);
                Model.Data = mod.Write();
            }
            else Model.Data = File.ReadAllBytes(modelPath);
            dir.Files.Add(Model);
            //Collisions
            /*SFSFile KclFile = new SFSFile(1, textBox1.Text + ".kcl", dir);
            KclFile.Data = File.ReadAllBytes(KclPath);
            dir.Files.Add(KclFile);
            SFSFile PaFile = new SFSFile(2, textBox1.Text + ".pa", dir);
            PaFile.Data = File.ReadAllBytes(PaPath);
            dir.Files.Add(PaFile);*/
            //InitSensor
            SFSFile Sensor = new SFSFile(3, "InitSensor.byml", dir);
            Sensor.Data = BymlConverter.GetByml(Properties.Resources.SensorNPC);
            dir.Files.Add(Sensor);
            //InitActor
            string actor = "<?xml version =\"1.0\" encoding=\"shift_jis\"?>\r\n<Root>\r\n  <isBigEndian Value=\"False\" />\r\n  <BymlFormatVersion Value=\"1\" />\r\n  <C1>\r\n        <C1 Name=\"Debug\">\r\n      <A0 Name=\"NodeName\" StringValue=\"地形\" />\r\n    </C1>\r\n    <C1 Name=\"Executor\">\r\n      <A0 Name=\"CategoryName\" StringValue=\"" + comboBox1.SelectedItem + "\" />\r\n    </C1>\r\n    <C1 Name=\"GroupClipping\">\r\n      <D1 Name=\"MaxCount\" StringValue=\"64\" />\r\n    </C1>\r\n    <FF Name=\"Model\" StringValue=\"00000000\" />\r\n  </C1>\r\n</Root>";
            SFSFile Actor = new SFSFile(4, "InitActor.byml", dir);
            Actor.Data = BymlConverter.GetByml(actor);//Properties.Resources.Actor);
            dir.Files.Add(Actor);
            //InitClipping
            string clip = "<?xml version=\"1.0\" encoding=\"shift_jis\"?>\r\n<Root>\r\n  <isBigEndian Value=\"False\" />\r\n  <BymlFormatVersion Value=\"1\" />\r\n  <C1>\r\n    <D2 Name=\"Radius\" StringValue=\"" + numericUpDown1.Value.ToString() + "\" />\r\n  </C1>\r\n</Root>";
            SFSFile Clipping = new SFSFile(5, "InitClipping.byml", dir);
            Clipping.Data = BymlConverter.GetByml(clip);
            dir.Files.Add(Clipping);

            SzsArch.FromFileSystem(dir);
            File.WriteAllBytes(s.FileName, y.Compress(SzsArch.Write()));
            MessageBox.Show(strings["done"]);
            MessageBox.Show(strings["CC"]);
            MessageBox.Show(strings["view2"] + textBox1.Text + strings["view2"]);
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog fld = new CommonOpenFileDialog { IsFolderPicker = true };
            fld.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (fld.ShowDialog() != CommonFileDialogResult.Ok) return;
            if (File.Exists(fld.FileName + "\\pa.pa")!=true)
            { 
            File.Copy(PaPath, fld.FileName+"\\pa.pa");
            File.Copy(KclPath, fld.FileName+"\\kcl.kcl");
            }
            else 
            {
                File.Delete(fld.FileName + "\\pa.pa");
                File.Delete(fld.FileName + "\\kcl.kcl");
                File.Copy(PaPath, fld.FileName + "\\pa.pa");
                File.Copy(KclPath, fld.FileName + "\\kcl.kcl");
            }
            MessageBox.Show(strings["kclpa"]+ fld.FileName+strings["rename"]);
        }
    }
}