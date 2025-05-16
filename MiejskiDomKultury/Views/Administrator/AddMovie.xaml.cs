using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury
{
    public partial class AddMovie : Page
    {
        private Film _selectedFilm;
        private List<DateTime> _screeningTimes = new List<DateTime>();
        private MovieRepositoryService _repositoryService;

        public AddMovie()
        {
            _repositoryService = new MovieRepositoryService();
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string title = SearchBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(title))
            {
                var movies = GetMoviesByTitle(title);
                MovieList.ItemsSource =await movies;
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



        private void AddScreeningButton_Click(object sender, RoutedEventArgs e)
        {
            string timeText = ScreeningTime.Text.Trim();
            Regex timeFormatRegex = new Regex(@"^(?:[01]\d|2[0-3]):[0-5]\d$"); // Sprawdza format HH:mm

            if (ScreeningDate.SelectedDate.HasValue &&
                !string.IsNullOrWhiteSpace(timeText) &&
                timeFormatRegex.IsMatch(timeText) &&
                TimeSpan.TryParse(timeText, out TimeSpan screeningTime))
            {
                DateTime fullDateTime = ScreeningDate.SelectedDate.Value.Add(screeningTime);
                if(fullDateTime< DateTime.UtcNow)
                {
                    MessageBox.Show("Termin musi być z przyszłości");
                    return;
                }
                _screeningTimes.Add(fullDateTime);
                ScreeningTimesList.ItemsSource = null;
                ScreeningTimesList.ItemsSource = _screeningTimes;
            }
            else
            {
                MessageBox.Show("Proszę wprowadzić godzinę w poprawnym formacie (HH:mm).");
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


        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;
            string timeText = ScreeningTime.Text.Trim();

            // Sprawdzenie czy wprowadzono datę
            if (!ScreeningDate.SelectedDate.HasValue)
            {
                ErrorMessage.Text = "Proszę wybrać datę.";
                ErrorMessage.Visibility = Visibility.Visible;
                return;
            }


            foreach (DateTime date in _screeningTimes)
            {
                Seans s = new Seans { DataStart=date, Film = _selectedFilm, Czas=180, };
                _repositoryService.AddSeans(s);
            }
          
            MessageBox.Show("Film został dodany pomyślnie!");
        }

        private async void DisplayMovieDetails(Film film)
        {
            
            film = await _repositoryService.GetMovieDetailsFromApi(film.Tytul, film.Rok);
            _selectedFilm = film;
            TitleText.Text = film.Tytul;
            YearText.Text = $"Rok: {film.Rok}";
            DescriptionText.Text = film.Opis;
        }

        public async Task<List<Film>> GetMoviesByTitle(string title)
        {
           
            return await _repositoryService.GetMoviesByTitleFromApi(title);
        }
    }
}
