using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury
{
    /// <summary>
    /// Logika interakcji dla klasy Logowanie.xaml
    /// </summary>
    public partial class Logowanie : Page
    {
        private readonly IUserRepository _userRepository;


            //tu jest DI
        public Logowanie(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var email = TextBoxLogin.Text;
            var password = PasswordBox.Password;
            var user = _userRepository.GetUserByEmail(email);

            if (user == null || user.HasloHash!= PasswordHasher.HashPassword(password))
            {
                MessageBox.Show("Błędne dane logowania", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Session.Login(user);
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.WidocznoscPrzyciskow();
            }
            NavigationService.Navigate(new Home());
        }
    }

}
