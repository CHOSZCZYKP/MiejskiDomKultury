using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MiejskiDomKultury.Model;

namespace MiejskiDomKultury
{
    public partial class AddMovie : Page
    {
        private Film _selectedFilm;
        private List<DateTime> _screeningTimes = new List<DateTime>();

        public AddMovie()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string title = SearchBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(title))
            {
                var movies = GetMoviesByTitle(title);
                MovieList.ItemsSource = movies;
            }
        }

        private void MovieList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovieList.SelectedItem is Film selectedFilm)
            {
                _selectedFilm = selectedFilm;
                DisplayMovieDetails(selectedFilm);
                ScreeningForm.Visibility = Visibility.Visible;
                ScreeningTimesList.ItemsSource = _screeningTimes;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is Film selectedFilm)
            {
                _selectedFilm = selectedFilm;
                DisplayMovieDetails(selectedFilm);
                MessageBox.Show($"Film '{selectedFilm.Tytul}' został wybrany!");
            }
        }

        private void AddScreeningButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScreeningDate.SelectedDate.HasValue && !string.IsNullOrWhiteSpace(ScreeningTime.Text))
            {
                DateTime fullDateTime = ScreeningDate.SelectedDate.Value.Add(TimeSpan.Parse(ScreeningTime.Text));
                _screeningTimes.Add(fullDateTime);
                ScreeningTimesList.ItemsSource = null;
                ScreeningTimesList.ItemsSource = _screeningTimes;
            }
        }

        private void RemoveScreeningButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScreeningTimesList.SelectedItem is DateTime selectedTime)
            {
                _screeningTimes.Remove(selectedTime);
                ScreeningTimesList.ItemsSource = null;
                ScreeningTimesList.ItemsSource = _screeningTimes;
            }
        }

        private void DisplayMovieDetails(Film film)
        {
            TitleText.Text = film.Tytul;
            YearText.Text = $"Rok: {film.Rok}";
            DescriptionText.Text = film.Opis;
        }

        public List<Film> GetMoviesByTitle(string title)
        {
            Film film = new Film
            {
                Aktorzy = new List<string> { "Malcolm McDowell", "Patrick Magee", "Michael Bates" },
                Opis = "Alex DeLarge wraz ze swoim gangiem sieje spustoszenie na ulicach. Kiedy trafia do więzienia, otrzymuje propozycję odmiany swojego życia.",
                Gatunki = new List<string> { "Crime", "Drama", "Sci-Fi" },
                Tytul = "Mechaniczna pomarańcza",
                PlakatURL = "https://i.ebayimg.com/00/s/MTYwMFgxMDYw/z/arkAAOSwNsdXR516/$_57.JPG?set_id=8800005007",
                Rok = 1971
            };

            Film film2 = new Film
            {
                Aktorzy = new List<string> { "Keir Dullea", "Gary Lockwood", "Douglas Rain" },
                Opis = "\"2001: Odyseja kosmiczna\" to wyprawa w krainę jutra, mapa ludzkiego przeznaczenia, droga do nieskończoności i fascynująca opowieść o starciu człowieka z maszyną - arcydzieło sztuki filmowej, które wyróżniono Oscarem za efekty specjalne.",
                Gatunki = new List<string> { "Adventure", "Drama", "Sci-Fi" },
                PlakatURL = "https://m.media-amazon.com/images/M/MV5BNjU0NDFkMTQtZWY5OS00MmZhLTg3Y2QtZmJhMzMzMWYyYjc2XkEyXkFqcGc@._V1_SX300.jpg",
                Tytul = "2001: Odyseja kosmiczna",
                Rok = 1968
            };

            return new List<Film> { film, film2 };
        }
    }
}
