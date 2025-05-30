using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Services;
using System.Windows.Controls;
using ControlzEx.Standard;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiejskiDomKultury
{
    public partial class MainWindow : Window
    {
        public VoiceCommandBot voiceBot;
        public bool Play = true;
       private CancellationTokenSource cts;
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        private VolumeWindow volumeWindow;
        bool IsVoiceOn = true;
        public MainWindow()
        {
            InitializeComponent();

            (VoiceToggleButton.Content as Image).Source = new BitmapImage(new Uri("Assets/bot.png", UriKind.RelativeOrAbsolute));
            //proszem nie usuwac
            try
            {
                voiceBot = new VoiceCommandBot(Main);
                cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    voiceBot.StartListening(cts.Token);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd z voice botem "+ex.Message);
            }
            Main.Content = App.ServiceProvider.GetRequiredService<Home>();
            PlayBackgroundMusic();
            
        }

        private void PlayBackgroundMusic()
        {
            try
            {
                //w pliku ustawić nie jako zasób tylko jako zawartość
                _mediaPlayer.Open(new Uri("Assets/song2.mp3", UriKind.Relative));
                _mediaPlayer.Play();
                _mediaPlayer.Volume = 0.05;
              
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
            /*if (Play)
            {
                _mediaPlayer.Pause();
                Play = false;
            }
            else
            {
                _mediaPlayer.Play();
                Play = true;
            }*/
            if (volumeWindow == null || !volumeWindow.IsVisible)
            {
                volumeWindow = new VolumeWindow(SetVolumeFromSlider);
                volumeWindow.Show();
            }
            else
            {
                volumeWindow.Focus();
            }
        }


        private void VoiceToggleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var iconUri = IsVoiceOn
                    ? new Uri("Assets/disabled.png", UriKind.RelativeOrAbsolute)
                    : new Uri("Assets/bot.png", UriKind.RelativeOrAbsolute);

                (VoiceToggleButton.Content as Image).Source = new BitmapImage(iconUri);

                if (IsVoiceOn)
                {
                    cts.Cancel();
                    IsVoiceOn = false;
                }
                else
                {
                    cts = new CancellationTokenSource();
                    Task.Run(() => voiceBot.StartListening(cts.Token));
                    IsVoiceOn = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy przełączaniu VoiceBota: " + ex.Message);
            }
        }



        private void SetVolumeFromSlider(int totalDiceValue)
        {
            double normalized = Math.Clamp(totalDiceValue / 90.0, 0.0, 1.0);
            _mediaPlayer.Volume = normalized;
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
