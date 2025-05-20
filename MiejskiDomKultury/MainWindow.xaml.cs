using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Services;
using System.Windows.Controls;
using ControlzEx.Standard;

namespace MiejskiDomKultury
{
    public partial class MainWindow : Window
    {
        public VoiceCommandBot voiceBot;
       private CancellationTokenSource cts;

        public MainWindow()
        {
            InitializeComponent();
            //proszem nie usuwac
           /* voiceBot = new VoiceCommandBot(Main);
            cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                voiceBot.StartListening(cts.Token);
            });*/
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
