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
using System.Windows.Shapes;

namespace MiejskiDomKultury
{
    public partial class VolumeWindow : Window
    {
        public VolumeWindow(Action<int> updateVolumeCallback)
        {
            InitializeComponent();
            SliderControl.SetVolumeCallback(updateVolumeCallback);
        }
    }
}
