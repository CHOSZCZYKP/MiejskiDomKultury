using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury
{
    public partial class AvailableMovies : Page
    {
        private MovieRepositoryService _moviesRepositoryService;
        public ObservableCollection<Film> FilteredMovies { get; set; }

        public AvailableMovies()
        {
            InitializeComponent();
            _moviesRepositoryService = new MovieRepositoryService();
            FilteredMovies = new ObservableCollection<Film>(_moviesRepositoryService.GetAvailableMovies());
           
            DataContext = this;
        }

        public void FilterMovies(string searchText)
        {
            var allMovies = _moviesRepositoryService.GetAvailableMovies();
            var filtered = allMovies.Where(f => f.Tytul.Contains(searchText, System.StringComparison.OrdinalIgnoreCase)).ToList();
            FilteredMovies.Clear();
            foreach (var movie in filtered)
            {
                FilteredMovies.Add(movie);
            }
        }

      
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            FilterMovies(textBox.Text);
            
        }

        private void MovieDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedMovie = button.Tag as Film;

            if (selectedMovie != null)
            {
                
                NavigationService.Navigate(new MovieDetails(selectedMovie));
            }
        }
    }
}
