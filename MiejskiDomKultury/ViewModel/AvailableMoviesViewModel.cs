using System.Collections.ObjectModel;
using System.Linq;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;
using System.ComponentModel;

namespace MiejskiDomKultury.ViewModels
{
    public class AvailableMoviesViewModel : INotifyPropertyChanged
    {
        private MovieRepositoryService _moviesRepositoryService;
        private string _searchText;

        public ObservableCollection<Film> FilteredMovies { get; set; }

        public AvailableMoviesViewModel()
        {
            _moviesRepositoryService = new MovieRepositoryService();
            FilteredMovies = new ObservableCollection<Film>(_moviesRepositoryService.GetAvailableMovies());
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterMovies();
                }
            }
        }

        private void FilterMovies()
        {
            var allMovies = _moviesRepositoryService.GetAvailableMovies();
            var filtered = allMovies.Where(f => f.Tytul.Contains(_searchText ?? string.Empty, System.StringComparison.OrdinalIgnoreCase)).ToList();
            FilteredMovies.Clear();
            foreach (var movie in filtered)
            {
                FilteredMovies.Add(movie);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
