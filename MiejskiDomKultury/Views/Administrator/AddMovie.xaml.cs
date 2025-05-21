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
        private AIService _ai;
        private MovieService _movieService;
        public AddMovie()
        {
            _movieService = new MovieService();
            _ai = new AIService();
            _repositoryService = new MovieRepositoryService();
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string title = SearchBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(title))
            {
                try
                {
                    var movies = GetMoviesByTitle(title);
                    MovieList.ItemsSource = await movies;
                }
                catch
                {
                    MessageBox.Show("Film o takim tytule nie istnieje!");
                }
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
                if (!_movieService.CanBeFilmAdd(_selectedFilm.Czas, fullDateTime))
                {
                    MessageBox.Show("Data filmu koliduje z innym!");
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


        private async void Confirm_Click(object sender, RoutedEventArgs e) // Changed from Task to void
        {
            ErrorMessage.Visibility = Visibility.Collapsed;
            string timeText = ScreeningTime.Text.Trim();

            if (!ScreeningDate.SelectedDate.HasValue)
            {
                ErrorMessage.Text = "Proszę wybrać datę.";
                ErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                _selectedFilm.OpisPL = await _ai.Translate(_selectedFilm.Opis);
                foreach (DateTime date in _screeningTimes)
                {
                    Seans s = new Seans { DataStart = date, Film = _selectedFilm };
                    _repositoryService.AddSeans(s);
                }

                MessageBox.Show("Film został dodany pomyślnie!");
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}");
            }
        }



        private async void DisplayMovieDetails(Film film)
        {

            film = await _movieService.GetMovieDetailsFromApi(film.Tytul, film.Rok);
            _selectedFilm = film;
            TitleText.Text = film.Tytul;
            YearText.Text = $"Rok: {film.Rok}";
            
           

            if (Settings.Default.CzyLangAngielski)
            {
                DescriptionText.Text = film.Opis;
            }
            else
            {
                film.OpisPL = await _ai.Translate(film.Opis);
                DescriptionText.Text =film.OpisPL;
            }
            
        }
        public async Task<List<Film>> GetMoviesByTitle(string title)
        {
           
            return await _movieService.GetMoviesByTitleFromApi(title);
        }

        private void ClearForm()
        {
            // Wyczyszczenie wyszukiwania i listy filmów
            SearchBox.Text = string.Empty;
            MovieList.ItemsSource = null;
            MovieList.SelectedItem = null;

            // Resetowanie wybranego filmu
            _selectedFilm = null;

            // Wyczyszczenie szczegółów filmu
            TitleText.Text = string.Empty;
            YearText.Text = string.Empty;
            DescriptionText.Text = string.Empty;

            // Wyczyszczenie seansów
            _screeningTimes.Clear();
            ScreeningTimesList.ItemsSource = null; // Przypisz ponownie, jeśli potrzebne
            ScreeningTimesList.ItemsSource = _screeningTimes;

            // Resetowanie kontrolek formularza seansów
            ScreeningDate.SelectedDate = null;
            ScreeningTime.Text = string.Empty;

            // Ukryj formularz seansów
            ScreeningForm.Visibility = Visibility.Collapsed;

            // Ukryj ewentualne komunikaty błędów
            ErrorMessage.Visibility = Visibility.Collapsed;
        }
    }
}
