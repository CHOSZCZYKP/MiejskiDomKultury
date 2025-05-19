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
    /// <summary>
    /// Logika interakcji dla klasy Ustawienia.xaml
    /// </summary>
    public partial class Ustawienia : Window
    {
        public Ustawienia()
        {
            InitializeComponent();
            MotywToggleButton.Content = "Motyw Jasny";
        }

        private void MotywToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Motywy.ZmianaMotywu(new Uri("/Themes/MotywCiemny.xaml", UriKind.Relative));
            MotywToggleButton.Content = "Motyw Ciemny";
        }

        private void MotywToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Motywy.ZmianaMotywu(new Uri("/Themes/MotywJasny.xaml", UriKind.Relative));
            MotywToggleButton.Content = "Motyw Jasny";
        }
    }
}
