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
using System.Xml;
using LibEveryFileExplorer.Files.SimpleFileSystem;

namespace The4Dimension.FormEditors
{
    public partial class FrmWorldEditor : Form
    {
        Dictionary<string, byte[]> SzsFiles = new Dictionary<string, byte[]>();
        Dictionary<Dictionary<int, string>, Dictionary<int, Dictionary<string, string>>> Worldmap = new Dictionary<Dictionary<int, string>, Dictionary<int, Dictionary<string, string>>>(); //(World id, world type), (node id,(level property, value))
        string lvltype = "";
        string miniature = "";
        string lvlname = "";
        bool strcoin = false;
        int starcoins = 0;
        int scenario = 1;
        int selectednode = -1;
        private PictureBox[] nodes = new PictureBox[8];
        private PictureBox[] pictures = new PictureBox[8];
        private List<World> wlist = new List<World>();
        public FrmWorldEditor()
        { 
            InitializeComponent();
            //load GamesystemDataTable.szs/CourseList.byml
            nodes = new PictureBox[] { node1, node2, node3, node4, node5, node6, node7, node8 };
            pictures = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8 };
        }

        private void FrmWorldEditor_Load(object sender, EventArgs e)
        {
            
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SzsArch = new NDS.NitroSystem.FND.NARC(y.Decompress(File.ReadAllBytes(@"GameSystemDataTable.szs")));
            foreach (SFSFile file in SzsArch.ToFileSystem().Files) SzsFiles.Add(file.FileName, file.Data);
            string ConvertedXml = BymlConverter.GetXml(SzsFiles["CourseList.byml"]);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(ConvertedXml);
            XmlNode nodes = xml.SelectSingleNode("/Root/C1/C0");//C0 "Worlds"
            int worldid = 0;
            //string worldtype = "";
            Dictionary<int, Dictionary<string, string>> level = new Dictionary<int, Dictionary<string, string>>();
            foreach (XmlNode node in nodes.ChildNodes)//C1
            {
                List<Level> lvllist = new List<Level>();
                foreach (XmlNode worldnode in node.ChildNodes)// C0(includes the level nodes) and A0(world type)
                {

                    int levelid = 0;
                    if (worldnode.Name == "C0")
                    {
                        lvllist = new List<Level>();
                        foreach (XmlNode n in worldnode.ChildNodes)//C1(per level(A B C...)), each c1 is a level
                        {
                            Level tst = new Level();
                            
                            tst.lvlnodeid = levelid;
                            Dictionary<string, string> levelparams = new Dictionary<string, string>();
                            foreach (XmlNode levelnode in n.ChildNodes)//level properties
                            {
                                levelparams.Add(levelnode.Attributes["Name"].Value, levelnode.Attributes["StringValue"].Value);
                                
                            }
                            tst.lvlparamslist = new Dictionary<string, string>(levelparams);
                            lvllist.Add(tst);
                            levelid += 1;
                            //level.Add(levelid, new Dictionary<string, string>(levelparams));
                        }
                    }

                    if (worldnode.Name == "A0")
                    {
                        World test = new World();
                        
                        test.wid = worldid;

                        test.wtype = worldnode.Attributes["StringValue"].Value;

                        test.wlvls = new Level[] { lvllist[0], lvllist[1], lvllist[2], lvllist[3], lvllist[4], lvllist[5], lvllist[6], lvllist[7]};
                        wlist.Add(test);
                        worldid += 1;

                        //Dictionary<int, string> world = new Dictionary<int, string>();
                        //world.Add(worldid, worldtype);
                        //Worldmap.Add(new Dictionary<int, string>(world), new Dictionary<int, Dictionary<string, string>>(level));
                        //level.Clear();

                    }
                }
            }
            comboBox1.SelectedIndex = 0;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var stream = new MemoryStream())
            {
                using (var xr = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true, Encoding = Form1.DefEnc }))
                {
                    xr.WriteStartDocument();
                    xr.WriteStartElement("Root");
                    xr.WriteStartElement("isBigEndian");
                    xr.WriteAttributeString("Value", "False");
                    xr.WriteEndElement();
                    xr.WriteStartElement("BymlFormatVersion");
                    xr.WriteAttributeString("Value", ((uint)1).ToString());
                    xr.WriteEndElement();
                    xr.WriteStartElement("C1");
                    xr.WriteStartElement("C0");
                    xr.WriteAttributeString("Name", "Worlds");
                    foreach (World n in wlist)
                    {
                        xr.WriteStartElement("C1");
                        xr.WriteStartElement("C0");
                        xr.WriteAttributeString("Name", "Course");
                        foreach (Level k in n.wlvls)
                        {
                            xr.WriteStartElement("C1");
                            foreach (KeyValuePair<string, string> d in k.lvlparamslist)
                            {
                                if (d.Key == "CollectCoinNum" || d.Key == "Scenario") 
                                {
                                    xr.WriteStartElement("D1");
                                }
                                else
                                {
                                    xr.WriteStartElement("A0");
                                }
                                xr.WriteAttributeString("Name", d.Key);
                                xr.WriteAttributeString("StringValue", d.Value);
                                xr.WriteEndElement();

                            }
                            xr.WriteEndElement();
                        }
                        xr.WriteEndElement();
                        xr.WriteStartElement("A0");
                        xr.WriteAttributeString("Name", "Type");
                        xr.WriteAttributeString("StringValue", n.wtype);
                        xr.WriteEndElement();
                        xr.WriteEndElement();
                    }
                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.WriteEndElement();
                    xr.Close();
                }
                Clipboard.SetText(Form1.DefEnc.GetString(stream.ToArray()));
                SzsFiles["CourseList.byml"] = BymlConverter.GetByml(Form1.DefEnc.GetString(stream.ToArray()));
                if (checkBox2.Checked)
                {
                    File.WriteAllBytes(@"CourseList.byml", BymlConverter.GetByml(Form1.DefEnc.GetString(stream.ToArray())));
                }
            }
            CommonCompressors.YAZ0 y = new CommonCompressors.YAZ0();
            NDS.NitroSystem.FND.NARC SzsArch = new NDS.NitroSystem.FND.NARC();
            SFSDirectory dir = new SFSDirectory("", true);
            for (int i = 0; i < SzsFiles.Count; i++)
            {
                SFSFile file = new SFSFile(i, SzsFiles.Keys.ToArray()[i], dir);
                file.Data = SzsFiles.Values.ToArray()[i];
                dir.Files.Add(file);
            }
            SzsArch.FromFileSystem(dir);
            File.WriteAllBytes(@"GameSystemDataTable.szs", y.Compress(SzsArch.Write()));
            MessageBox.Show("Done, file was saved as GameSystemDataTable.szs in this program folder");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void nodeinfo(int nodeid)
        {
            selectednode = nodeid; 
            wlist[comboBox1.SelectedIndex].wlvls[nodeid].lvlparamslist.TryGetValue("Type", out lvltype);
            comboBox2.SelectedIndex = comboBox2.Items.IndexOf(lvltype); comboBox2.Enabled = true;

            switch (lvltype.ToLower())
            {
                case "empty":
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox1.Checked = false;
                    numericUpDown1.Value = 0;
                    numericUpDown2.Value = 1;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Enabled = false;
                    break;
                case "dokan":
                    wlist[comboBox1.SelectedIndex].wlvls[nodeid].lvlparamslist.TryGetValue("Miniature", out miniature);
                    if (miniature != null)
                    {
                        textBox1.Text = miniature;
                        textBox1.Enabled = true;
                    }
                    textBox2.Text = "";
                    textBox2.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox1.Checked = false;
                    numericUpDown1.Value = 0;
                    numericUpDown2.Value = 1;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Enabled = false;
                    break;
                default:
                    wlist[comboBox1.SelectedIndex].wlvls[nodeid].lvlparamslist.TryGetValue("Miniature", out miniature);
                    if (miniature != null)
                    {
                        textBox1.Text = miniature;
                        textBox1.Enabled = true;
                    }
                    else
                    {
                        textBox1.Text = "";
                    }
                    wlist[comboBox1.SelectedIndex].wlvls[nodeid].lvlparamslist.TryGetValue("Stage", out lvlname);
                    if (lvlname != null)
                    {
                        textBox2.Text = lvlname;
                        textBox2.Enabled = true;
                    }
                    else
                    {
                        textBox2.Text = "";
                    }
                    wlist[comboBox1.SelectedIndex].wlvls[nodeid].lvlparamslist.TryGetValue("Scenario", out miniature);
                    if (miniature != null)
                    {
                        numericUpDown2.Value = Int32.Parse(miniature);
                        numericUpDown2.Enabled = true;
                    }
                    else
                    {
                        numericUpDown2.Value = 1;

                    }
                    wlist[comboBox1.SelectedIndex].wlvls[nodeid].lvlparamslist.TryGetValue("CollectCoinNum", out miniature);
                    if (miniature != null)
                    {
                        if (miniature != "0")
                        {
                            checkBox1.Checked = true;
                            numericUpDown1.Value = Int32.Parse(miniature);
                            numericUpDown1.Enabled = true;
                        }
                        else
                        {
                            wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Remove("CollectCoinNum");
                            checkBox1.Checked = false;
                            numericUpDown1.Value = 0;
                            numericUpDown1.Enabled = false;
                        }
                    }
                    else
                    {
                        checkBox1.Checked = false;
                        numericUpDown1.Value = 0;
                        numericUpDown1.Enabled = false;
                    }
                    checkBox1.Enabled = true;
                    break;
            }
            return;
        }
        private void node1_Click(object sender, EventArgs e)
        {
            nodeinfo(0);
        }

        private void node2_Click(object sender, EventArgs e)
        {
            nodeinfo(1);
        }

        private void node3_Click(object sender, EventArgs e)
        {
            nodeinfo(2);
        }

        private void node4_Click(object sender, EventArgs e)
        {
            nodeinfo(3);
        }

        private void node5_Click(object sender, EventArgs e)
        {
            nodeinfo(4);
        }

        private void node6_Click(object sender, EventArgs e)
        {
            nodeinfo(5);
        }

        private void node7_Click(object sender, EventArgs e)
        {
            nodeinfo(6);
        }

        private void node8_Click(object sender, EventArgs e)
        {
            nodeinfo(7);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                numericUpDown1.Enabled = true;
            }
            else { numericUpDown1.Enabled = false; }
        }

        private void nodecolour()
        {
            string level = "";
            List<Level> normlvls = new List<Level>();
            for (int i = 0; i < 8; i++)
            {
                wlist[comboBox1.SelectedIndex].wlvls[i].lvlparamslist.TryGetValue("Type", out level);
                switch (level.ToLower())
                {
                    case "empty":
                        nodes[i].BackColor = Color.FromArgb(200, 200, 200);
                        pictures[i].Hide();
                        break;
                    case "dokan":
                        pictures[i].Show();
                        nodes[i].BackColor = Color.FromArgb(0, 255, 0);
                        pictures[i].Image = Image.FromFile(@"icons\pipe.png");
                        break;
                    case "championship":
                        pictures[i].Show();
                        nodes[i].BackColor = Color.FromArgb(255, 255, 126);
                        pictures[i].Image = Image.FromFile(@"icons\championship.png");
                        break;
                    case "kinopiohousepresent":
                        pictures[i].Show();
                        nodes[i].BackColor = Color.FromArgb(255, 60, 102);
                        pictures[i].Image = Image.FromFile(@"icons\thpresent.png");
                        break;
                    case "kinopiohousealbum":
                        pictures[i].Show();
                        nodes[i].BackColor = Color.FromArgb(0, 166, 255);
                        pictures[i].Image = Image.FromFile(@"icons\thalbum.png");
                        break;
                    case "mysterybox":
                        pictures[i].Show();
                        nodes[i].BackColor = Color.FromArgb(212, 85, 255);
                        pictures[i].Image = Image.FromFile(@"icons\mysterybox.png");
                        break;
                    case "normal":
                        pictures[i].Show();
                        normlvls.Add(wlist[comboBox1.SelectedIndex].wlvls[i]);
                        nodes[i].BackColor = Color.FromArgb(57, 80, 255);
                        pictures[i].Image = Image.FromFile(@"icons\"+(normlvls.IndexOf(wlist[comboBox1.SelectedIndex].wlvls[i])+1).ToString() +".png");
                        break;
                    case "koopabattleship":
                        pictures[i].Show();
                        nodes[i].BackColor = Color.FromArgb(156, 42, 42);
                        pictures[i].Image = Image.FromFile(@"icons\airship.png");
                        break;
                    case "koopafortress":
                        pictures[i].Show();
                        nodes[i].BackColor = Color.FromArgb(156, 42, 42);
                        pictures[i].Image = Image.FromFile(@"icons\fortress.png");
                        break;
                    default:
                        pictures[i].Show();
                        nodes[i].BackColor = Color.FromArgb(156, 42, 42);
                        pictures[i].Image = Image.FromFile(@"icons\castle.png");
                        break;
                }
                wlist[comboBox1.SelectedIndex].wlvls[i].lvlparamslist.TryGetValue("CollectCoinNum", out level);
                if (level != null)
                {
                    nodes[i].BackColor = Color.FromArgb(32, 32, 32);
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            nodecolour();
            selectednode = -1;
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox2.SelectedIndex = -1;
            comboBox2.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            checkBox1.Enabled = false;
            checkBox1.Checked = false;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 1;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            bgcolour(comboBox1.SelectedIndex);
        }
        private void bgcolour(int index) 
        {
            switch (index)
            {
                case 0:
                    panel1.BackColor = Color.FromArgb(74, 247, 74);
                    break;
                case 1:
                    panel1.BackColor = Color.FromArgb(255, 219, 82);
                    break;
                case 2:
                    panel1.BackColor = Color.FromArgb(123, 255, 255);
                    break;
                case 3:
                    panel1.BackColor = Color.FromArgb(197, 255, 33);
                    break;
                case 4:
                    panel1.BackColor = Color.FromArgb(255, 162, 82);
                    break;
                case 5:
                    panel1.BackColor = Color.FromArgb(222, 239, 255);
                    break;
                case 6:
                    panel1.BackColor = Color.FromArgb(181, 61, 181);
                    break;
                case 7:
                    panel1.BackColor = Color.FromArgb(181, 20, 0);
                    break;
                case 8:
                    panel1.BackColor = Color.FromArgb(181, 20, 0);
                    break;
                case 9://special worlds
                    panel1.BackColor = Color.FromArgb(74, 231, 132);
                    break;
                case 10:
                    panel1.BackColor = Color.FromArgb(214, 170, 58);
                    break;
                case 11:
                    panel1.BackColor = Color.FromArgb(8, 81, 132);
                    break;
                case 12:
                    panel1.BackColor = Color.FromArgb(74, 129, 0);
                    break;
                case 13:
                    panel1.BackColor = Color.FromArgb(99, 61, 8);
                    break;
                case 14:
                    panel1.BackColor = Color.FromArgb(189, 178, 181);
                    break;
                case 15:
                    panel1.BackColor = Color.FromArgb(255, 198, 197);
                    break;
                case 16:
                    panel1.BackColor = Color.FromArgb(255, 251, 132);
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectednode == -1) { return; }
            string index = comboBox2.SelectedItem.ToString();
            switch (index.ToLower())
            {
                case "empty":
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox1.Checked = false;
                    numericUpDown1.Value = 0;
                    numericUpDown2.Value = 1;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Enabled = false;
                    break;
                case "dokan":
                    //wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("Miniature", out miniature);
                    textBox1.Text = "MiniatureDokan";
                    textBox1.Enabled = true;
                    textBox2.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox1.Checked = false;
                    numericUpDown1.Value = 0;
                    numericUpDown2.Value = 1;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Enabled = false;
                    break;
                case "kinopiohousealbum":
                    textBox1.Text = "MiniatureKinopioHouse";
                    textBox2.Text = "KinopioHouseAlbumStage";
                    checkBox1.Enabled = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    numericUpDown2.Enabled = true;
                    break;
                case "kinopiohousepresent":
                    textBox1.Text = "MiniatureKinopioHouse";
                    checkBox1.Enabled = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    numericUpDown2.Enabled = true;
                    break;
                case "mysterybox":
                    textBox1.Text = "MiniatureMysteryBox";
                    textBox2.Text = "MysteryBoxNormalStage";
                    checkBox1.Enabled = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    numericUpDown2.Enabled = true;
                    break;
                case "koopabattleship":
                    textBox1.Text = "MiniatureBattleship";
                    checkBox1.Enabled = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    numericUpDown2.Enabled = true;
                    break;
                case "koopafortress":
                    textBox1.Text = "MiniatureKoopaFortress";
                    checkBox1.Enabled = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    numericUpDown2.Enabled = true;
                    break;
                case "koopacastle":
                    textBox1.Text = "MiniatureKoopaCastle";
                    checkBox1.Enabled = true;
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    numericUpDown2.Enabled = true;
                    break;
                
                default:
                    wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("Miniature", out miniature);
                    if (miniature != null)
                    {
                        textBox1.Text = miniature;
                        textBox1.Enabled = true;
                    }
                    else
                    {
                        textBox1.Text = "";
                        textBox1.Enabled = false;
                    }
                    wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("Stage", out lvlname);
                    if (lvlname != null)
                    {
                        textBox2.Text = lvlname;
                        textBox2.Enabled = true;
                    }
                    else
                    {
                        textBox2.Text = "";
                        textBox2.Enabled = false;
                    }
                    wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("Scenario", out miniature);
                    if (miniature != null)
                    {
                        numericUpDown2.Value = Int32.Parse(miniature);
                        numericUpDown2.Enabled = true;
                    }
                    else
                    {
                        numericUpDown2.Value = 1;

                    }
                    wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("CollectCoinNum", out miniature);
                    if (miniature != null)
                    {
                        checkBox1.Checked = true;
                        numericUpDown1.Value = Int32.Parse(miniature);
                        numericUpDown1.Enabled = true;
                    }
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    checkBox1.Checked = false;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Enabled = true;
                    checkBox1.Enabled = true;
                    break;
            }

            if (wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("Type", out _))
            {
                wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Remove("Type");
                wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Add("Type",index);
            }
            nodecolour();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (selectednode == -1) { return; }
            if (wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("Miniature", out _))
            {
                wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Remove("Miniature");
            }
            wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Add("Miniature", textBox1.Text);

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (selectednode == -1) { return; }
            if (wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("Stage", out _))
            {
                wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Remove("Stage");
            }
            wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Add("Stage", textBox2.Text);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (selectednode == -1) { return; }
            if (checkBox1.Checked == true)
            {
                if (wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("CollectCoinNum", out _))
                {
                    wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Remove("CollectCoinNum");
                }
                wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Add("CollectCoinNum", numericUpDown1.Value.ToString());
                Dictionary<string, string> order = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> k in wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.OrderBy(y => y.Key))
                {
                    order.Add(k.Key, k.Value);
                }
                wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Clear();
                wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist = order;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (selectednode == -1) { return; }
            if (wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.TryGetValue("Scenario", out _))
            {
                wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Remove("Scenario");
            }
            wlist[comboBox1.SelectedIndex].wlvls[selectednode].lvlparamslist.Add("Scenario", numericUpDown2.Value.ToString());
        }
    }
}
