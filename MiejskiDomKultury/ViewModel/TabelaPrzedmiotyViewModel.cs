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

        public ObservableCollection<Przedmiot> PrzemiotyCollection
        {
            get => _przedmiotCollection;
            set
            {
                _przedmiotCollection = value;
                OnPropertyChanged(nameof(PrzemiotyCollection));
            }
        }


        public TabelaPrzedmiotyViewModel()
        {
            var context = new DbContextDomKultury();
            _przedmiotRepository = new PrzedmiotRepository(context);
            DodajPrzedmiotCommand = new PrzekaznikCommand(async () => await DodajPrzedmiot());
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) 
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
