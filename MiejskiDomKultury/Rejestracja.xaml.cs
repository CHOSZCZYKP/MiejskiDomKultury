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
using MahApps.Metro.Controls;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Helpers;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;
using MiejskiDomKultury.ViewModel;

namespace MiejskiDomKultury
{
    /// <summary>
    /// Logika interakcji dla klasy Rejestracja.xaml
    /// </summary>
    public partial class Rejestracja : Page
    {
        public Rejestracja()
        {
            InitializeComponent();
            DataContext = new RejestracjaUzytkownika();
        }

        private void haslo_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RejestracjaUzytkownika vm)
            {
                vm.Haslo = haslo.Password;
                ValidationHelpers.UpdateValidation(haslo, nameof(vm.Haslo), vm);
            }
        }

        private void PowtorzHaslo_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RejestracjaUzytkownika vm)
            {
                vm.PowtorzHaslo = PowtorzHaslo.Password;
                ValidationHelpers.UpdateValidation(PowtorzHaslo, nameof(vm.PowtorzHaslo), vm);
            }
        }
    }
}
