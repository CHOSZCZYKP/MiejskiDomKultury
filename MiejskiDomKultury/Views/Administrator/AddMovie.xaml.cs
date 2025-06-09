using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    MessageBox.Show(Settings.Default.CzyLangAngielski
   ? "There is no movie with this title"
   : "Film o takim tytule nie istnieje");
                    
                }
            }
        }

        private async void MovieList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovieList.SelectedItem is Film selectedFilm)
            {
                _selectedFilm = selectedFilm;
                
                var ok =await DisplayMovieDetails(selectedFilm);
                if (ok)
                {
                    ScreeningForm.Visibility = Visibility.Visible;
                    ScreeningTimesList.ItemsSource = _screeningTimes;
                }
                else
                {
                    MessageBox.Show("Error 404");
                   
                }
            }
        }



        private void AddScreeningButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;
            string timeText = ScreeningTime.Text.Trim();
            Regex timeFormatRegex = new Regex(@"^(?:[01]\d|2[0-3]):[0-5]\d$"); 

            if (ScreeningDate.SelectedDate.HasValue &&
                !string.IsNullOrWhiteSpace(timeText) &&
                timeFormatRegex.IsMatch(timeText) &&
                TimeSpan.TryParse(timeText, out TimeSpan screeningTime))
            {
                DateTime fullDateTime = ScreeningDate.SelectedDate.Value.Add(screeningTime);
                if (fullDateTime < DateTime.UtcNow)
                {
                    if (!Settings.Default.CzyLangAngielski)
                    {
                        ErrorMessage.Text = Settings.Default.CzyLangAngielski
    ? "The term must be from the future"
    : "Termin musi być z przyszłości";
                    }
                    else
                    {
                      
                    }
                    ErrorMessage.Visibility = Visibility.Visible;
                    return;
                }

                if (!_movieService.CanBeFilmAdd(_selectedFilm.Czas, fullDateTime))
                {
                    ErrorMessage.Text = Settings.Default.CzyLangAngielski
   ? "The screening date clashes with another screening"
   : "Data seansu koliduje z innym seansem";
                    ErrorMessage.Visibility = Visibility.Visible;
                    
                    return;
                }
                _screeningTimes.Add(fullDateTime);
                ScreeningTimesList.ItemsSource = null;
                ScreeningTimesList.ItemsSource = _screeningTimes;
            }
            else
            {
                ErrorMessage.Text = Settings.Default.CzyLangAngielski
    ? "Please enter a valid time in HH:mm format."
    : "Proszę wprowadzić godzinę w poprawnym formacie (HH:mm).";

                ErrorMessage.Visibility = Visibility.Visible;
                return;
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


        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;
            SuccessMessage.Visibility = Visibility.Collapsed;

            if (_screeningTimes.Count == 0)
            {
                ErrorMessage.Text = Settings.Default.CzyLangAngielski
                    ? "Please add at least one screening time."
                    : "Dodaj przynajmniej jeden termin seansu.";
                ErrorMessage.Visibility = Visibility.Visible;
                return;
            }


            if (!ScreeningDate.SelectedDate.HasValue)
            {
                ErrorMessage.Text = Settings.Default.CzyLangAngielski
     ? "Please select a date."
     : "Proszę wybrać datę.";

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

                SuccessMessage.Text = Settings.Default.CzyLangAngielski
     ? "The movie was added successfully!"
     : "Film został dodany pomyślnie!";
                SuccessMessage.Visibility = Visibility.Visible;

                ClearForm(leaveSuccessMessage: true);
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = $"Wystąpił błąd: {ex.Message}";
                ErrorMessage.Visibility = Visibility.Visible;
            }
        }




        private async Task<bool> DisplayMovieDetails(Film film)
        {
            try
            {
                film = await _movieService.GetMovieDetailsFromApi(film.Tytul, film.Rok);
            }
            catch
            {
                return false;
            }
            _selectedFilm = film;
            TitleText.Text = film.Tytul;
            if (!Settings.Default.CzyLangAngielski)
            {
                YearText.Text = $"Rok: {film.Rok}";
            }
            else
            {
                YearText.Text = $"Year: {film.Rok}";
            }
          



            if (Settings.Default.CzyLangAngielski)
            {
                DescriptionText.Text = film.Opis;
            }
            else
            {
                film.OpisPL = await _ai.Translate(film.Opis);
                DescriptionText.Text =film.OpisPL;
            }
            return true;
        }
        public async Task<List<Film>> GetMoviesByTitle(string title)
        {
           
            return await _movieService.GetMoviesByTitleFromApi(title);
        }

        private void ClearForm(bool leaveSuccessMessage = false)
        {
            SearchBox.Text = string.Empty;
            MovieList.ItemsSource = null;
            MovieList.SelectedItem = null;

            _selectedFilm = null;

            TitleText.Text = string.Empty;
            YearText.Text = string.Empty;
            DescriptionText.Text = string.Empty;

            _screeningTimes.Clear();
            ScreeningTimesList.ItemsSource = null;
            ScreeningTimesList.ItemsSource = _screeningTimes;

            ScreeningDate.SelectedDate = null;
            ScreeningTime.Text = string.Empty;

            ScreeningForm.Visibility = Visibility.Collapsed;

            ErrorMessage.Visibility = Visibility.Collapsed;
            if (!leaveSuccessMessage)
                SuccessMessage.Visibility = Visibility.Collapsed;
        }

    }
}
