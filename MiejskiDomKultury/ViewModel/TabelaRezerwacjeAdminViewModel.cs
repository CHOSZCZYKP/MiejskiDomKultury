using MiejskiDomKultury.Data;
using MiejskiDomKultury.Dto;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MiejskiDomKultury.ViewModel
{
    public class TabelaRezerwacjeAdminViewModel : INotifyPropertyChanged
    {
        private readonly IRezerwacjaRepository _rezerwacjaRepository;

        private ObservableCollection<RezerwacjaSaliPrzezUzytkownikaDto> _rezerwacjaSaliCollection;
        public ObservableCollection<RezerwacjaSaliPrzezUzytkownikaDto> RezerwacjaSaliCollection
        {
            get => _rezerwacjaSaliCollection;
            set
            {
                _rezerwacjaSaliCollection = value;
                OnPropertyChanged(nameof(RezerwacjaSaliCollection));
            }
        }

        private ICollectionView _rezerwacjeSaliCollectionView;
        public ICollectionView RezerwacjaSaliCollectionView
        {
            get => _rezerwacjeSaliCollectionView;
            set
            {
                _rezerwacjeSaliCollectionView = value;
                OnPropertyChanged(nameof(RezerwacjaSaliCollectionView));
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
                RezerwacjaSaliCollectionView.Refresh();
            }
        }

        public TabelaRezerwacjeAdminViewModel()
        {
            var context = new DbContextDomKultury();
            _rezerwacjaRepository = new RezerwacjeRepository(context);

            RezerwacjaSaliCollection = new ObservableCollection<RezerwacjaSaliPrzezUzytkownikaDto>(_rezerwacjaRepository.GetAllRezerwacjeWithUserAndRoom());
            RezerwacjaSaliCollectionView = CollectionViewSource.GetDefaultView(RezerwacjaSaliCollection);
            RezerwacjaSaliCollectionView.Filter = FilterRezerwacjaSali;
        }

        private bool FilterRezerwacjaSali(object obj)
        {
            if (obj is RezerwacjaSaliPrzezUzytkownikaDto dto)
            {
                if (string.IsNullOrWhiteSpace(WyszukiwanaFraza))
                {
                    return true;
                }
                else
                {
                    return dto.Imie.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Nazwisko.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Email.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.NazwaSali.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Data.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.GodzinaPoczatkowa.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.GodzinaKoncowa.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);
                }
            }
            else
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
