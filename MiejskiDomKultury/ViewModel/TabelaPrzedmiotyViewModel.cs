using LiveChartsCore.Defaults;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Helpers;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using MiejskiDomKultury.Views.Administrator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MiejskiDomKultury.ViewModel
{
    public class TabelaPrzedmiotyViewModel : INotifyPropertyChanged
    {
        private PrzedmiotRepository _przedmiotRepository;
        private ObservableCollection<Przedmiot> _przedmiotCollection;

        public ICommand DodajPrzedmiotCommand { get; }
        public event Func<Task<bool>>? WywolajOknoDodajNowyPrzedmiot;

        private PrzekaznikCommand _usunPrzdmiotCommand;
        public ICommand UsunPrzedmiotCommand => _usunPrzdmiotCommand;
        public ICommand ZapiszZmianyCommand { get; }

        public ObservableCollection<Przedmiot> PrzemiotyCollection
        {
            get => _przedmiotCollection;
            set
            {
                _przedmiotCollection = value;
                OnPropertyChanged(nameof(PrzemiotyCollection));
            }
        }

        private Przedmiot _wybranyPrzedmiot;
        public Przedmiot WybranyPrzedmiot
        {
            get => _wybranyPrzedmiot;
            set
            {
                _wybranyPrzedmiot = value;
                OnPropertyChanged(nameof(WybranyPrzedmiot));
                _usunPrzdmiotCommand.RaiseCanExecuteChanged();
            }
        }

        private ICollectionView _przedmiotCollectionView;
        public ICollectionView PrzedmiotCollectionView
        {
            get => _przedmiotCollectionView;
            set
            {
                _przedmiotCollectionView = value;
                OnPropertyChanged(nameof(PrzedmiotCollectionView));
            }
        }

        private string _wyszukiwanaFraza;
        public string WyszukiwanaFraza
        {
            get => _wyszukiwanaFraza;
            set
            {
                _wyszukiwanaFraza = value;
                OnPropertyChanged(nameof(WyszukiwanaFraza));
                PrzedmiotCollectionView.Refresh();
            }
        }

        #region Wlasciwosci
        private string _nazwa;
        public string Nazwa
        {
            get => _nazwa;
            set
            {
                if (_nazwa != value)
                {
                    _nazwa = value;
                    OnPropertyChanged(nameof(Nazwa));
                }
            }
        }
        private string _stan;
        public string Stan
        {
            get => _stan;
            set
            {
                if (_stan != value)
                {
                    _stan = value;
                    OnPropertyChanged(nameof(Stan));
                }
            }
        }
        private string _typ;
        public string Typ
        {
            get => _typ;
            set
            {
                if (_typ != value)
                {
                    _typ = value;
                    OnPropertyChanged(nameof(Typ));
                }
            }
        }
        private decimal _cena_wartosc;
        public decimal CenaZaDobe_Wartosc
        {
            get => _cena_wartosc;
            set
            {
                if (_cena_wartosc != value)
                {
                    _cena_wartosc = value;
                    OnPropertyChanged(nameof(CenaZaDobe_Wartosc));
                }
            }
        }
        private string _waluta;
        public string CenaZaDobe_Waluta
        {
            get => _waluta;
            set
            {
                if (_waluta != value)
                {
                    _waluta = value;
                    OnPropertyChanged(nameof(CenaZaDobe_Waluta));
                }
            }
        }
        private bool _dostepnosc;
        public bool Dostepnosc
        {
            get => _dostepnosc;
            set
            {
                if (_dostepnosc != value)
                {
                    _dostepnosc = value;
                    OnPropertyChanged(nameof(Dostepnosc));
                }
            }
        }
        #endregion

        public TabelaPrzedmiotyViewModel()
        {
            var context = new DbContextDomKultury();
            _przedmiotRepository = new PrzedmiotRepository(context);
            DodajPrzedmiotCommand = new PrzekaznikCommand(async () => await DodajPrzedmiot());
            _usunPrzdmiotCommand = new PrzekaznikCommand(async () => await UsunPrzedmiot(),() => WybranyPrzedmiot != null);
            ZapiszZmianyCommand = new PrzekaznikCommand(async () => await EdytowanieKomorek(), () => WybranyPrzedmiot != null);
           
            var przedmioty = _przedmiotRepository.GetAllPrzedmioty();
            PrzemiotyCollection = new ObservableCollection<Przedmiot>(przedmioty);
            PrzedmiotCollectionView = CollectionViewSource.GetDefaultView(PrzemiotyCollection);
            PrzedmiotCollectionView.Filter = FilterPrzedmiot;
        }

        private async Task DodajPrzedmiot()
        {
            if (WywolajOknoDodajNowyPrzedmiot != null)
            {
                bool wynik = await WywolajOknoDodajNowyPrzedmiot.Invoke();
                if (wynik)
                {
                    var przedmioty = _przedmiotRepository.GetAllPrzedmioty();
                    PrzemiotyCollection = new ObservableCollection<Przedmiot>(przedmioty);
                    PrzedmiotCollectionView = CollectionViewSource.GetDefaultView(PrzemiotyCollection);
                }
            }
        }

        private async Task UsunPrzedmiot()
        {
            if (WybranyPrzedmiot != null)
            {
                await _przedmiotRepository.RemovePrzedmiot(WybranyPrzedmiot);
                PrzemiotyCollection.Remove(WybranyPrzedmiot);
            }
        }

        private async Task EdytowanieKomorek()
        {
            if (WybranyPrzedmiot != null)
            {
                await _przedmiotRepository.EditPrzedmiot(WybranyPrzedmiot);
            }
        }

        private bool FilterPrzedmiot(object obj)
        {
            if (obj is Przedmiot przedmiot)
            {
                if (string.IsNullOrWhiteSpace(WyszukiwanaFraza))
                {
                    return true;
                }
                else
                {
                    return przedmiot.Nazwa.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || przedmiot.Stan.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || przedmiot.Typ.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || przedmiot.CenaZaDobe_Waluta.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || przedmiot.CenaZaDobe_Wartosc.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        ||przedmiot.Dostepnosc.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);
                }
            }
            else
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) 
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
