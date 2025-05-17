using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
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
            App.Current.MainWindow.Content = seatsPage;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
