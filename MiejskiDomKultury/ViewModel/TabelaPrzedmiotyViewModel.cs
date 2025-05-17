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
        #endregion

        public TabelaPrzedmiotyViewModel()
        {
            var context = new DbContextDomKultury();
            _przedmiotRepository = new PrzedmiotRepository(context);
            DodajPrzedmiotCommand = new PrzekaznikCommand(async () => await DodajPrzedmiot());
            _usunPrzdmiotCommand = new PrzekaznikCommand(async () => await UsunPrzedmiot(),() => WybranyPrzedmiot != null);
            ZapiszZmianyCommand = new PrzekaznikCommand(async () => await EdytowanieKomorek(), () => WybranyPrzedmiot != null);
            _ = Init();

        }

        private async Task DodajPrzedmiot()
        {
            if (WywolajOknoDodajNowyPrzedmiot != null)
            {
                bool wynik = await WywolajOknoDodajNowyPrzedmiot.Invoke();
                if (wynik)
                {
                    await Refresh();
                }
            }
        }

        private async Task Init()
        {
            var przedmioty = await _przedmiotRepository.GetAllPrzedmioty();
            PrzemiotyCollection = new ObservableCollection<Przedmiot>(przedmioty);
        }

        public async Task Refresh()
        {
            var przedmioty = await _przedmiotRepository.GetAllPrzedmioty();
            PrzemiotyCollection = new ObservableCollection<Przedmiot>(przedmioty);
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) 
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
