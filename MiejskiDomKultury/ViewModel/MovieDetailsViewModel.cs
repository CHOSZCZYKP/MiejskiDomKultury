using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury.ViewModel
{
    public class MovieDetailsViewModel : INotifyPropertyChanged
    {
        public Film Film { get; set; }
        public ObservableCollection<string> ShowDates { get; set; }
        public ICommand NavigateToSeatsCommand { get; }

        public MovieDetailsViewModel(Film film)
        {
            if (!Settings.Default.CzyLangAngielski)
            {
                film.Opis = film.OpisPL;
            }
            Film = film;
            ShowDates = new ObservableCollection<string>();

            LoadShowDates(film.Id);
            NavigateToSeatsCommand = new RelayCommand<string>(OnNavigateToSeats);
        }


        private void LoadShowDates(int movieId)
        {
            MovieRepositoryService mr = new MovieRepositoryService();
            var dates = mr.GetMovieShowDates(movieId)
                          .Select(date => date.ToString("dd-MM-yyyy HH:mm"))
                          .ToList();

            ShowDates.Clear();
            foreach (var date in dates)
            {
                ShowDates.Add(date);
            }

            OnPropertyChanged(nameof(ShowDates));
        }

        private void OnNavigateToSeats(string showDate)
        {
           
            var seatsPage = new SeatsReservation(showDate, Film);
            var mainWindow = Application.Current.MainWindow as MainWindow;
        

            if (Session.User != null)
            {
                mainWindow?.Main.Navigate(seatsPage);
            }
            else
            {
                MessageBox.Show((string)Application.Current.FindResource("logowanieRezerwacjaMiejsca"), (string)Application.Current.FindResource("zalogujSieTytul"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



}
