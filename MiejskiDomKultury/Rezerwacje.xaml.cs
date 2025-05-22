using MiejskiDomKultury.ViewModel;
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

namespace MiejskiDomKultury
{
    /// <summary>
    /// Interaction logic for Rezerwacje.xaml
    /// </summary>
    public partial class Rezerwacje : Page
    {
        public Rezerwacje()
        {
      
            InitializeComponent();
            DataContext = new RezerwacjaViewModel();
        }
    }
}
