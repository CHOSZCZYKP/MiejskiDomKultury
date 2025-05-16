using MiejskiDomKultury.Data;
using MiejskiDomKultury.Helpers;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MiejskiDomKultury.ViewModel
{
    public class DodajNowyPrzedmiotViewModel : INotifyPropertyChanged
    {
        private PrzedmiotRepository _przedmiotRepository;
        private Window _window;
        private PrzekaznikCommand _anulujCommand;
        public ICommand AnulujCommand => _anulujCommand;
        private PrzekaznikCommand _dodajCommand;
        public ICommand DodajCommand => _dodajCommand;

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
            }
        }
        private decimal _cena_wartosc;
        public decimal Cena_Wartosc
        {
            get => _cena_wartosc;
            set
            {
                _cena_wartosc = value;
                OnPropertyChanged(nameof(Cena_Wartosc));
                _dodajCommand.RaiseCanExecuteChanged();
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
        }

        private void ZamknijOkno()
        {
            _window.Close();
        }

        private bool CzyDodac()
        {
            if (!string.IsNullOrEmpty(Nazwa)
                && !string.IsNullOrEmpty(Typ)
                && !string.IsNullOrEmpty(WybranaWaluta)
                && !string.IsNullOrEmpty(Stan))
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
                CenaZaDobe_Wartosc = Cena_Wartosc,
                CenaZaDobe_Waluta = WybranaWaluta,
                Dostepnosc = true
            };
            await _przedmiotRepository.AddNewPrzedmiot(nowyPrzedmiot);
            _window.DialogResult = true;
            _window.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
