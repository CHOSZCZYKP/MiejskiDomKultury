using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using MiejskiDomKultury.Services;
using MiejskiDomKultury.Views;
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
    /// Logika interakcji dla klasy Home.xaml
    /// </summary>
    public partial class Home : Page
    {

        public INewsRepository newsService;
        public Home( )
        {
            newsService = new NewsService();
            InitializeComponent();
            WyswietlLosoweWiadomosci();

        }

        private void Rezeracja_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Rezerwacje());
        }

        private void Wypozyczalnia_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WypozyczaniePrzedmiotow());
        }
        private void Chat_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChatBot());
        }


        private void Film_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AvailableMovies());
        }


        private void WyswietlLosoweWiadomosci()
        {
            var losowe = newsService.GetLastNews();
            NewsList.ItemsSource = losowe;
        }
    }
}
