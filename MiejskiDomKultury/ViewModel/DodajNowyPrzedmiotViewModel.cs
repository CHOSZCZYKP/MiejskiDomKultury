using MiejskiDomKultury.Data;
using MiejskiDomKultury.Helpers;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MiejskiDomKultury.ViewModel
{
    public class DodajNowyPrzedmiotViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private PrzedmiotRepository _przedmiotRepository;
        private Window _window;
        private PrzekaznikCommand _anulujCommand;
        public ICommand AnulujCommand => _anulujCommand;
        private PrzekaznikCommand _dodajCommand;
        public ICommand DodajCommand => _dodajCommand;

        private readonly Dictionary<string, List<string>> _errors = new();

        #region Wlasciwosci
        private string _nazwa;
        public string Nazwa
        {
            get => _nazwa; 
            set 
            {  
                _nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
                _dodajCommand.RaiseCanExecuteChanged();
                ValidateProperty(nameof(Nazwa), value);
            }
        }
        private string _stan;
        public string Stan
        {
            get => _stan;
            set
            {
                _stan = value;
                OnPropertyChanged(nameof(Stan));
                _dodajCommand.RaiseCanExecuteChanged();
                ValidateProperty(nameof(Stan), value);
            }
        }
        private string _typ;
        public string Typ
        { 
            get => _typ;
            set
            {
                _typ = value;
                OnPropertyChanged(nameof(Typ));
                _dodajCommand.RaiseCanExecuteChanged();
                ValidateProperty(nameof(Typ), value);
            }
        }
        private string _cena_wartosc;
        public string Cena_Wartosc
        {
            get => _cena_wartosc;
            set
            {
                _cena_wartosc = value;
                OnPropertyChanged(nameof(Cena_Wartosc));
                _dodajCommand.RaiseCanExecuteChanged();
                ValidateProperty(nameof(Cena_Wartosc), value);
            }
        }
        private string _wybranaWaluta;
        public string WybranaWaluta
        {
            get => _wybranaWaluta;
            set
            {
                _wybranaWaluta = value;
                OnPropertyChanged(nameof(WybranaWaluta));
                _dodajCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public List<string> WalutaComboBox { get; set; } = new List<string>()
        {
            "PLN",
            "EUR",
            "USD"
        };

        public DodajNowyPrzedmiotViewModel(Window window)
        {
            var context = new DbContextDomKultury();
            _przedmiotRepository = new PrzedmiotRepository(context);
            _window = window;
            _dodajCommand = new PrzekaznikCommand(async () => { await DodajPrzedmiot();}, CzyDodac);
            _anulujCommand = new PrzekaznikCommand(() => { ZamknijOkno(); });
            WybranaWaluta = "PLN";
        }

        private void ZamknijOkno()
        {
            _window.Close();
        }

        private bool CzyDodac()
        {
            if (!string.IsNullOrEmpty(Nazwa)
                && !string.IsNullOrEmpty(Stan)
                && !string.IsNullOrEmpty(Typ)
                && !string.IsNullOrEmpty(WybranaWaluta)
                && !string.IsNullOrEmpty(Cena_Wartosc)
                && decimal.TryParse(Cena_Wartosc, out decimal wynik)
                && wynik > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task DodajPrzedmiot()
        {
            var nowyPrzedmiot = new Przedmiot
            {
                Nazwa = Nazwa,
                Typ = Typ,
                Stan = Stan,
                CenaZaDobe_Wartosc = decimal.Parse(Cena_Wartosc),
                CenaZaDobe_Waluta = WybranaWaluta,
                Dostepnosc = true
            };
            await _przedmiotRepository.AddNewPrzedmiot(nowyPrzedmiot);
            _window.DialogResult = true;
            _window.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ValidateProperty(string propertyName, string value)
        {
            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(value))
            {
                var wyswietl = propertyName switch
                {
                    nameof(Nazwa) => "Nazwa",
                    nameof(Stan) => "Stan",
                    nameof(Typ) => "Typ",
                    nameof(Cena_Wartosc) => "Cena za dobę",
                    _ => propertyName
                };

                errors.Add($"Pole {wyswietl} jest wymagane.");
            }
            else if (!decimal.TryParse(Cena_Wartosc, out decimal wynik) && propertyName == nameof(Cena_Wartosc))
            {
                errors.Add("Wprowadź liczbę");
            }
            else if (wynik <= 0 && propertyName == nameof(Cena_Wartosc))    
            {
                errors.Add("Wprowadź liczbę większą od 0");
            }

            if (errors.Any())
                _errors[propertyName] = errors;

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            return null;
        }

    }
}
