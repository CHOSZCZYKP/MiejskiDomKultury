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
    public class TabelaWypozyczeniaAdminViewModel : INotifyPropertyChanged
    {
        private readonly IWypozyczeniaRepository _wypozyczeniaRepository;

        private ObservableCollection<WypozyczeniePrzedmiotuPrzezUzytkownika> _wypozyczeniaCollection = new();
        public ObservableCollection<WypozyczeniePrzedmiotuPrzezUzytkownika> WypozyczeniaCollection
        {
            get => _wypozyczeniaCollection;
            set
            {
                _wypozyczeniaCollection = value;
                OnPropertyChanged(nameof(WypozyczeniaCollection));
            }
        }

        private ICollectionView _wypozyczeniaCollectionView;
        public ICollectionView WypozyczeniaCollectionView
        {
            get => _wypozyczeniaCollectionView;
            set
            {
                _wypozyczeniaCollectionView = value;
                OnPropertyChanged(nameof(WypozyczeniaCollectionView));
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
                WypozyczeniaCollectionView.Refresh();
            }
        }

        public TabelaWypozyczeniaAdminViewModel()
        {
            DbContextDomKultury context = new DbContextDomKultury();
            _wypozyczeniaRepository = new WypozyczalniaRepository(context);

            WypozyczeniaCollection = new ObservableCollection<WypozyczeniePrzedmiotuPrzezUzytkownika>(_wypozyczeniaRepository.GetAllWyozyczeniaWithUsersAndItems());
            WypozyczeniaCollectionView = CollectionViewSource.GetDefaultView(WypozyczeniaCollection);
            WypozyczeniaCollectionView.Filter = FilterWypozyczenia;
        }

        private bool FilterWypozyczenia(object obj)
        {
            if (obj is WypozyczeniePrzedmiotuPrzezUzytkownika dto)
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
                        || dto.DataWypozyczenia.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.DataZwrotu.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.NazwaPrzedmiotu.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Cena.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);
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
