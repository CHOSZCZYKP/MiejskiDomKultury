using System.Windows;
using System.Windows.Controls;

namespace MiejskiDomKultury
{
    public partial class Success : Page
    {
        public Success()
        {
            InitializeComponent();
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Home());
        }
    }
}
