using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Helpers;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;
using MiejskiDomKultury.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MiejskiDomKultury.ViewModel
{
    public class RejestracjaUzytkownika : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new();

        private readonly IUserRepository _userRepository;

        private PrzekaznikCommand _zarejestrujUzytkownika;
        public ICommand ZarejestrujUzytkownika => _zarejestrujUzytkownika;

        private string _imie;
        public string Imie
        {
            get => _imie;
            set
            {
                _imie = value;
                OnPropertyChanged(nameof(Imie));
                ValidateProperty(nameof(Imie), value);
                _zarejestrujUzytkownika.RaiseCanExecuteChanged();
            }
        }

        private string _nazwisko;
        public string Nazwisko
        {
            get => _nazwisko;
            set
            {
                _nazwisko = value;
                OnPropertyChanged(nameof(Nazwisko));
                ValidateProperty(nameof(Nazwisko), value);
                _zarejestrujUzytkownika.RaiseCanExecuteChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                ValidateProperty(nameof(Email), value);
                _zarejestrujUzytkownika.RaiseCanExecuteChanged();
            }
        }

        private string _nazwaUzytkownika;
        public string NazwaUzytkownika
        {
            get => _nazwaUzytkownika;
            set
            {
                _nazwaUzytkownika = value;
                OnPropertyChanged(nameof(NazwaUzytkownika));
                ValidateProperty(nameof(NazwaUzytkownika), value);
                _zarejestrujUzytkownika.RaiseCanExecuteChanged();
            }
        }

        private string _haslo;
        public string Haslo
        {
            get => _haslo;
            set
            {
                _haslo = value;
                OnPropertyChanged(nameof(Haslo));
                ValidateProperty(nameof(Haslo), value);
                _zarejestrujUzytkownika.RaiseCanExecuteChanged();
            }
        }

        private string _powtorzHaslo;
        public string PowtorzHaslo
        {
            get => _powtorzHaslo;
            set
            {
                _powtorzHaslo = value;
                OnPropertyChanged(nameof(PowtorzHaslo));
                ValidateProperty(nameof(PowtorzHaslo), value);
                _zarejestrujUzytkownika.RaiseCanExecuteChanged();
            }
        }

        public RejestracjaUzytkownika()
        {
            _userRepository = new UserRepositoryService();
            _zarejestrujUzytkownika = new PrzekaznikCommand(DodajUzytkownika, CzyDodacUzytkownika);
        }

        private bool CzyDodacUzytkownika()
        {
            if (!_userRepository.DoesUserExist(Email)
                && !string.IsNullOrWhiteSpace(Imie) 
               && !string.IsNullOrWhiteSpace(Nazwisko)
               && !string.IsNullOrWhiteSpace(NazwaUzytkownika)
               && !string.IsNullOrWhiteSpace(Haslo)
               && !string.IsNullOrWhiteSpace(PowtorzHaslo)
               && !string.IsNullOrWhiteSpace(Email))
            {
                return true;
            }
            else
                return false;
        }

        private void DodajUzytkownika()
        {
            var nowyUzytkownik = new Uzytkownik
            {
                Imie = Imie,
                Nazwisko = Nazwisko,
                NazwaUzytkownika = NazwaUzytkownika,
                Email = Email,
                HasloHash = PasswordHasher.HashPassword(Haslo),
                Rola = "User"
            };
            _userRepository.AddNewUser(nowyUzytkownik);

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                Session.Login(nowyUzytkownik);
                mainWindow.WidocznoscPrzyciskow();
                mainWindow.Main.Content = App.ServiceProvider.GetRequiredService<Home>();
            }
        }

        private void ValidateProperty(string propertyName, string value)
        {
            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(value))
            {
                var wyswietl = propertyName switch
                {
                    nameof(Imie) => "Imie",
                    nameof(Nazwisko) => "Nazwisko",
                    nameof(NazwaUzytkownika) => "Nazwa użytkownika",
                    nameof(Email) => "Email",
                    nameof(Haslo) => "Hasło",
                    nameof(PowtorzHaslo) => "Powtórz hasło",
                    _ => propertyName
                };

                errors.Add($"Pole {wyswietl} jest wymagane.");
            }
            else if (nameof(PowtorzHaslo) == propertyName && value != Haslo)
            {
                errors.Add("Hasła nie zgadzają się");
            }

            if (errors.Any())
                _errors[propertyName] = errors;

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            return null;
        }
    }
}
