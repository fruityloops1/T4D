using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace The4Dimension
{
    public partial class FrmLevNameOpen : Form
    {
        public string res = null;
        public Dictionary<string, string> LevelNameNum = new Dictionary<string, string>();
        private Dictionary<int, List<string>> WorldLevelDict = new Dictionary<int, List<string>>();//tab id, levels of the world as a list

        public FrmLevNameOpen(Dictionary<string, string> s )
        {
            InitializeComponent();
            LevelNameNum = s;
            string[] FullLevelList = s.Keys.ToArray();
            List<string> vs = new List<string>();

            for (int i = 0; i < FullLevelList.Length; i++) 
            {
                if (FullLevelList[i].StartsWith("W")) 
                {
                    string newstring = FullLevelList[i].Substring(1,1);
                    string nextstring = FullLevelList[i + 1].Substring(1, 1);
                    switch (newstring)
                    {
                        case "1":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W1 = new List<string>(vs);
                                
                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W1);
                                vs.Clear();
                            }
                            break;
                        case "2":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W2 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W2);
                                vs.Clear();
                            }
                            break;
                        case "3":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W3 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W3);
                                vs.Clear();
                            }
                            break;
                        case "4":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W4 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W4);
                                vs.Clear();
                            }
                            break;
                        case "5":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W5 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W5);
                                vs.Clear();
                            }
                            break;
                        case "6":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W6 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W6);
                                vs.Clear();
                            }
                            break;
                        case "7":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W7 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W7);
                                vs.Clear();
                            }
                            break;
                        case "8":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> W8 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) - 1, W8);
                                vs.Clear();
                            }
                            break;
                    }

                }
                else if (FullLevelList[i].StartsWith("S"))
                {
                    string nextstring = "";
                    string newstring = FullLevelList[i].Substring(1, 1);
                    if (i != FullLevelList.Length - 1)
                    {
                        nextstring = FullLevelList[i + 1].Substring(1, 1);
                    }
                    switch (newstring)
                    {
                        case "1":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S1 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S1);
                                vs.Clear();
                            }
                            break;
                        case "2":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S2 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) +7, S2);
                                vs.Clear();
                            }
                            break;
                        case "3":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S3 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S3);
                                vs.Clear();
                            }
                            break;
                        case "4":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S4 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S4);
                                vs.Clear();
                            }
                            break;
                        case "5":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S5 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S5);
                                vs.Clear();
                            }
                            break;
                        case "6":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S6 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S6);
                                vs.Clear();
                            }
                            break;
                        case "7":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S7 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S7);
                                vs.Clear();
                            }
                            break;
                        case "8":
                            vs.Add(FullLevelList[i]);
                            if (newstring != nextstring)
                            {
                                List<string> S8 = new List<string>(vs);

                                WorldLevelDict.Add(Int32.Parse(newstring) + 7, S8);
                                vs.Clear();
                            }
                            break;
                    }

                }
                else if (FullLevelList[i].StartsWith("C")) 
                {
                    string nextstring = "";
                    string newstring = FullLevelList[i].Substring(0, 1);
                    if (i != FullLevelList.Length - 1)
                    {
                        nextstring = FullLevelList[i + 1].Substring(0, 1);
                    }
                    vs.Add(FullLevelList[i]);
                    if (newstring != nextstring)
                    {
                        List<string> Demo = new List<string>(vs);

                        WorldLevelDict.Add(16, Demo);
                        vs.Clear();
                    }
                }
                else if (FullLevelList[i].StartsWith("T")|| FullLevelList[i].StartsWith("G"))
                {
                    string nextstring = "";
                    string newstring = FullLevelList[i].Substring(0, 1);
                    if (i != FullLevelList.Length - 1)
                    {
                        nextstring = FullLevelList[i + 1].Substring(0, 1);
                    }
                    vs.Add(FullLevelList[i]);
                    if (newstring != nextstring && nextstring != "G" && newstring != "T")
                    {
                        List<string> Kinopio = new List<string>(vs);

                        WorldLevelDict.Add(17, Kinopio);
                        vs.Clear();
                    }
                }
                else if (FullLevelList[i].StartsWith("M"))
                {
                    string nextstring = "";
                    string newstring = FullLevelList[i].Substring(0, 1);
                    if (i != FullLevelList.Length - 1)
                    {
                        nextstring = FullLevelList[i + 1].Substring(0, 1);
                    }
                    vs.Add(FullLevelList[i]);
                    if (newstring != nextstring)
                    {
                        List<string> Mystery = new List<string>(vs);

                        WorldLevelDict.Add(18, Mystery);
                        vs.Clear();
                    }
                }
                //listBox1.Items.Add(FullLevelList[i]);
            }
            listBox1.Items.AddRange(WorldLevelDict[0].ToArray());
            comboBox1.SelectedIndex = 0;
            //listBox1.Items.Clear();

            //listBox1.Items.AddRange(s.Keys.ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            res = null;
            this.Close();
        }

        private void ListBox_DoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            res = listBox1.SelectedItem.ToString();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            res = listBox1.SelectedItem.ToString();
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(WorldLevelDict[comboBox1.SelectedIndex].ToArray());
        }
    }
}
