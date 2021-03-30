using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using HelixToolkit;
using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media.Media3D;

namespace ModelViewer
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UserControlNEW : UserControl
    {
        Dictionary<string, Model3D> ImportedModels = new Dictionary<string, Model3D>();
        Dictionary<string, List<ModelVisual3D>> Models = new Dictionary<string, List<ModelVisual3D>>();
        public Dictionary<string, List<Vector3D>> Positions = new Dictionary<string, List<Vector3D>>();
        ModelImporter Importer = new ModelImporter();
        SortingVisual3D ModelViewer = new SortingVisual3D();

        Vector3D CameraTarget = new Vector3D(0, 0, 0);
        public UserControlNEW()
        {
            InitializeComponent();
            ModelViewer.SortingFrequency = 0.5;
            //NEW..Add(ModelViewer);
        }
    }
}
