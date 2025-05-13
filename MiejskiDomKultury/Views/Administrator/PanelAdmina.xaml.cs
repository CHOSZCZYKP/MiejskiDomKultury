using Microsoft.Extensions.DependencyInjection;
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

namespace MiejskiDomKultury.Views.Administrator
{
    /// <summary>
    /// Logika interakcji dla klasy PanelAdmina.xaml
    /// </summary>
    public partial class PanelAdmina : Page
    {
        public PanelAdmina()
        {
            InitializeComponent();
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<TabelaUzytkownicyAdmin>();
        }

        private void TabelaUzytkownicy_Click(object sender, RoutedEventArgs e)
        {
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<TabelaUzytkownicyAdmin>();
        }

        private void Wykresy_Click(object sender, RoutedEventArgs e)
        {
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<WykresyStatystyk>();
        }

        private void TabelaWypozyczenia_Click(object sender, RoutedEventArgs e)
        {
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<TabelaWypozyczeniaAdmin>();
        }

        private void TabelaSale_Click(object sender, RoutedEventArgs e)
        {
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<TabelaSaleAdmin>();
        }

        private void TabelaBany_Click(object sender, RoutedEventArgs e)
        {
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<TabelaBanyAdmin>();
        }

        private void TabelaTransakcje_Click(object sender, RoutedEventArgs e)
        {
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<TabelaTransakcjeAdmin>();
        }

        private void TabelaRezerwacje_Click(object sender, RoutedEventArgs e)
        {
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<TabelaRezerwacjeAdmin>();
        }

        private void TabelaPrzedmioty_Click(object sender, RoutedEventArgs e)
        {
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<TabelaPrzedmiotyAdmin>();
        }

        private void AddFilm_Click(object sender, RoutedEventArgs e)
        {
            ContentAdmin.Content = App.ServiceProvider.GetRequiredService<AddMovie>();
        }
    }
}
