using ModelViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace The4Dimension.FormEditors
{
    public partial class FrmCameraViewer : Form
    {
        public UserControl1 render = new UserControl1();
        public FrmCameraViewer(UserControl1 newrend)
        {
            InitializeComponent();
            //render = new UserControl1(newrend);
            //render = newrend.Clone();
            render.Camspeed = 100;
            render.ShowFps = false;
            
            render.ShowTriangleCount = false;
            render.HasAA = Properties.Settings.Default.HasAA;
            render.TextureFilter = Properties.Settings.Default.TextFilter;
            
            render.UnloadLevel();


            render = new UserControl1();
            render.ShowFps = false;
            render.ShowDebugInfo = false;
            render.ShowTriangleCount = false;
            render.Clone(newrend);

            elementHost1.Child = render;


            //render.UpdateLayout();
        }


        public void GameCamToViewport(Camera3DL camera)
        {
            elementHost1.Visible = false;
            render.CameraFrom3DLCamera(camera.GetAsValues());
            elementHost1.Size = new Size(400, 240);//add button for size 1x and 2x, and for camera buttons and to test them 
            elementHost1.Visible = true;
        }
    }
}
