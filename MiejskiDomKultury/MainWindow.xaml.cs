using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Services;
using System.Windows.Controls;

namespace MiejskiDomKultury
{
    public partial class MainWindow : Window
    {
        
        
        public MainWindow()
        {
            InitializeComponent();
           

            Main.Content = App.ServiceProvider.GetRequiredService<Home>();
        }

     

        private void Logowanie_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = App.ServiceProvider.GetRequiredService<Logowanie>();
        }

        private void Rejestracja_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = App.ServiceProvider.GetRequiredService<Rejestracja>();
        }

        private void PanelAdmina_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = App.ServiceProvider.GetRequiredService<Views.Administrator.PanelAdmina>();
        }

        private void PanelMenu_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = App.ServiceProvider.GetRequiredService<Home>();
        }

        private void Ustawiania_Click(object sender, RoutedEventArgs e)
        {
            Ustawienia ustawienia = new Ustawienia();
            ustawienia.Show();
        }
    }
}
