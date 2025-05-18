using System.Windows;
using System.Windows.Controls;
using MiejskiDomKultury.ViewModels;

namespace MiejskiDomKultury
{
    public partial class AvailableMovies : Page
    {
        private AvailableMoviesViewModel _viewModel;

        public AvailableMovies()
        {
            InitializeComponent();
            _viewModel = new AvailableMoviesViewModel();
            DataContext = _viewModel;
        }

        
    }
}
