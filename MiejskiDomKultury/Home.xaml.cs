using Microsoft.Extensions.DependencyInjection;
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
        public Home()
        {
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

        public class Wiadomosc
        {
            public string Text { get; set; }
            public string ImagePath { get; set; }
        }

        private readonly List<Wiadomosc> WszystkieWiadomosci = new List<Wiadomosc>
        {
            new Wiadomosc { Text = "Astrolodzy przewidują opad meteorytów dzisiaj o 21:00!", ImagePath = "Assets/news1.webp" },
            new Wiadomosc { Text = "Kobieta poszukiwana za zdemolowanie sali tanecznej!", ImagePath = "Assets/news2.webp" },
            new Wiadomosc { Text = "Zapisy na zajęcia z planowania strategii wojennej ruszyły!", ImagePath = "Assets/news3.webp" },
            new Wiadomosc { Text = "Ostrołęka pogrążona w żałobie. Zmarł Bóbr Bartek", ImagePath = "Assets/news4.webp" },
            new Wiadomosc { Text = "Parada Imperatora w Ostrołęce - zdjęcia", ImagePath = "Assets/news5.webp" }
        };



        private void WyswietlLosoweWiadomosci()
        {
            var losowe = WszystkieWiadomosci.OrderBy(x => Guid.NewGuid()).Take(3).ToList();
            NewsList.ItemsSource = losowe;
        }
    }
}
