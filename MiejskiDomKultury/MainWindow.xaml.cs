using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Services;
using System.Windows.Controls;
using ControlzEx.Standard;
using System.Windows.Media;

namespace MiejskiDomKultury
{
    public partial class MainWindow : Window
    {
        public VoiceCommandBot voiceBot;
        public bool Play = true;
       private CancellationTokenSource cts;
        private MediaPlayer _mediaPlayer = new MediaPlayer();
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
            PlayBackgroundMusic();

        }

        private void PlayBackgroundMusic()
        {
            try
            {
                //w pliku ustawić nie jako zasób tylko jako zawartość
                _mediaPlayer.Open(new Uri("Assets/song.mp3", UriKind.Relative));
                _mediaPlayer.Play();
                _mediaPlayer.Volume = 0.1;
              
                _mediaPlayer.MediaEnded += (s, e) =>
                {
                    _mediaPlayer.Position = TimeSpan.Zero;
                    _mediaPlayer.Play();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odtwarzania: " + ex.Message);
            }
        }

        private void VoiceOnOff(object sender, RoutedEventArgs e)
        {
            if (Play)
            {
                _mediaPlayer.Pause();
                Play = false;
            }
            else
            {
                _mediaPlayer.Play();
                Play = true;
            }
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
