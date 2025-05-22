using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static MiejskiDomKultury.Slider;

namespace MiejskiDomKultury
{
    public partial class Slider : UserControl
    {
        public ObservableCollection<Die> Dice { get; set; } = new();
        private Random rand = new();
        private Action<int> _onVolumeChange;
        private int rollEscapes = 0;
        private bool imageRevealed = false;


        public Slider()
        {
            InitializeComponent();
            InitializeDice();
            DiceDisplay.ItemsSource = Dice;
        }

        public void SetVolumeCallback(Action<int> callback)
        {
            _onVolumeChange = callback;
        }

        private void InitializeDice()
        {
            for (int i = 0; i < 15; i++)
            {
                Dice.Add(new Die { Number = 1 }); // default to 1
            }

            UpdateVolume();
        }

        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var die in Dice)
            {
                if (!die.IsHeld)
                    die.Number = rand.Next(1, 7);
            }

            UpdateVolume();
        }

        private void UpdateVolume()
        {
            int total = Dice.Sum(d => d.Number);
            VolumeText.Text = $"Volume: {total}";
            _onVolumeChange?.Invoke(total);
        }
        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            VolumeText.Text = "Volume: 0 psuju dobrej zabawy";
            _onVolumeChange?.Invoke(0);
        }
        private void RollButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Dice.Sum(d => d.Number) <= 50) return;

            double canvasWidth = RollButtonCanvas.ActualWidth;
            double canvasHeight = RollButtonCanvas.ActualHeight;

            double maxX = canvasWidth - RollButton.ActualWidth;
            double maxY = canvasHeight - RollButton.ActualHeight;

            if (maxX <= 0 || maxY <= 0) return;

            var rand = new Random();
            double newLeft = rand.NextDouble() * maxX;
            double newTop = rand.NextDouble() * maxY;

            Canvas.SetLeft(RollButton, newLeft);
            Canvas.SetTop(RollButton, newTop);
            rollEscapes++;

            if (rollEscapes >= 4 && !imageRevealed)
            {
                RevealBackgroundImage();
            }
        }
        private void RevealBackgroundImage()
        {
            imageRevealed = true;

            var fadeIn = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(4)
            };

            BackgroundRevealImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }



        public class Die : INotifyPropertyChanged
        {
            private int _number;
            private bool _isHeld;

            public int Number
            {
                get => _number;
                set
                {
                    _number = value;
                    OnPropertyChanged(nameof(Number));
                    OnPropertyChanged(nameof(ImagePath));
                }
            }

            public bool IsHeld
            {
                get => _isHeld;
                set
                {
                    _isHeld = value;
                    OnPropertyChanged(nameof(IsHeld));
                }
            }

            public string ImagePath => $"/Assets/die{Number}.png";

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
