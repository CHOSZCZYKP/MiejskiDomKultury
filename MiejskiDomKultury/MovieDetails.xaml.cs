using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury
{
    /// <summary>
    /// Logika interakcji dla klasy MovieDetails.xaml
    /// </summary>
    public partial class MovieDetails : Page
    {

        Film Film { get; set; }
        public MovieDetails(Film film)
        {
            InitializeComponent();
            PosterImage.Source = new BitmapImage(new Uri(film.PlakatURL));
            DataContext = film;
            this.Film = film;
            LoadShowDates(film.Id);
        }

        private  void LoadShowDates(int movieId)
        {
            MovieRepositoryService mr = new MovieRepositoryService();
            List<DateTime> showDates =  mr.GetMovieShowDates(movieId);

            DataContext = new
            {
                ((Film)DataContext).Tytul,
                ((Film)DataContext).Opis,
                ((Film)DataContext).Rok,
                ((Film)DataContext).Aktorzy,
                ((Film)DataContext).Gatunki,
                ShowDates = showDates.Select(date => date.ToString("dd-MM-yyyy HH:mm")).ToList()
            };
        }

        private void ShowDateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string showDate)
            {
                MessageBox.Show($"Wybrano seans: {showDate}");
                NavigationService.Navigate(new SeatsReservation(showDate, Film));
            }
        }
    }
}
