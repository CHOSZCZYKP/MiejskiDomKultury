using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy Rejestracja.xaml
    /// </summary>
    public partial class Rejestracja : Page
    {

        private readonly IUserRepository _userRepository;


        public Rejestracja(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            InitializeComponent();
        }


        public void Zarejestruj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ImieTextBox.Text) ||
               string.IsNullOrWhiteSpace(NazwiskoTextBox.Text) ||
               string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
               string.IsNullOrWhiteSpace(NazwaUzytkownikaTextBox.Text) ||
               string.IsNullOrWhiteSpace(HasloPasswordBox.Password) ||
               string.IsNullOrWhiteSpace(SecondHasloPasswordBox.Password)
               == true)
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (_userRepository.DoesUserExist(EmailTextBox.Text))
            {
                MessageBox.Show("Użytkownik o danym email już istnieje", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SecondHasloPasswordBox.Password != HasloPasswordBox.Password)
            {
                MessageBox.Show("Podane hasła są różne", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            var uzytkownik = new Uzytkownik
            {
                Imie = ImieTextBox.Text,
                Nazwisko = NazwiskoTextBox.Text,
                Email = EmailTextBox.Text,
                NazwaUzytkownika = NazwaUzytkownikaTextBox.Text,
                HasloHash = PasswordHasher.HashPassword(HasloPasswordBox.Password), 
                Rola = "User",
                
                
            };

            _userRepository.AddNewUser(uzytkownik);

            MessageBox.Show("Udana rejestracja", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
    }
}
