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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace MiejskiDomKultury.ViewModel
{
    public class EdytujPrzedmiotViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private PrzedmiotRepository _przedmiotRepository;
        private Przedmiot _przedmiot;
        public Przedmiot Przedmiot
        {
            get => _przedmiot;
            set
            {
                _przedmiot = value;
                OnPropertyChanged(nameof(Przedmiot));
            }
        }

        
        public string Nazwa
        {
            get => _przedmiot.Nazwa;
            set
            {
                _przedmiot.Nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
                ValidateProperty(nameof(Nazwa), value);
            }
        }
        
        public string Stan
        {
            get => _przedmiot.Stan;
            set
            {
                _przedmiot.Stan = value;
                OnPropertyChanged(nameof(Stan));
                ValidateProperty(nameof(Stan), value);
            }
        }
        
        public string Typ
        {
            get => _przedmiot.Typ;
            set
            {
                _przedmiot.Typ = value;
                OnPropertyChanged(nameof(Typ));
                ValidateProperty(nameof(Typ), value);
            }
        }
        private string _cenaWartosc;
        public string Cena_Wartosc
        {
            get => _cenaWartosc;
            set
            {
                _cenaWartosc = value;
                if (decimal.TryParse(value, out decimal wynik))
                {
                    _przedmiot.CenaZaDobe_Wartosc = wynik;
                }
                OnPropertyChanged(nameof(Cena_Wartosc));
                ValidateProperty(nameof(Cena_Wartosc), value);
            }
        }

        private readonly Dictionary<string, List<string>> _errors = new();

        public ICommand ZapiszCommand { get; }
        public ICommand AnulujCommand { get; }

        public List<string> WalutaComboBox { get; set; } = new List<string>()
        {
            "PLN",
            "EUR",
            "USD"
        };

        public EdytujPrzedmiotViewModel(Przedmiot przedmiot)
        {
            this._przedmiot = przedmiot;
            Cena_Wartosc = _przedmiot.CenaZaDobe_Wartosc.ToString();
            var context = new DbContextDomKultury();
            _przedmiotRepository = new PrzedmiotRepository(context);
            ZapiszCommand = new PrzekaznikCommand(async () => await Zapisz(), CzyEdytowac);
            AnulujCommand = new PrzekaznikCommand(Anuluj);
        }

        private async Task Zapisz()
        {
            await _przedmiotRepository.EditPrzedmiot(Przedmiot);
            CloseDialogWithResult?.Invoke(true);
        }

        private bool CzyEdytowac()
        {
            if (!string.IsNullOrEmpty(Nazwa)
                && !string.IsNullOrEmpty(Stan)
                && !string.IsNullOrEmpty(Typ)
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

        private void Anuluj()
        {
            CloseDialogWithResult?.Invoke(true);
        }

        public event Action<bool>? CloseDialogWithResult;

        public event PropertyChangedEventHandler? PropertyChanged;
        

        protected void OnPropertyChanged(string propertyName) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            return null;
        }
    }
}
