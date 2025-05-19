using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.ViewModel;

namespace MiejskiDomKultury
{
    public partial class MovieDetails : Page
    {
        public MovieDetails(Film film)
        {
            InitializeComponent();
            DataContext = new MovieDetailsViewModel(film);
            try
            {
                PosterImage.Source = new BitmapImage(new Uri(film.PlakatURL));
            }
            catch
            {
                PosterImage.Source = new BitmapImage(new Uri("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRoWcWg0E8pSjBNi0TtiZsqu8uD2PAr_K11DA&s"));
            }
           
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            PosterImage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

    }
}
