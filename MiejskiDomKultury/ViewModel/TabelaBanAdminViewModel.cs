using MiejskiDomKultury.Data;
using MiejskiDomKultury.Dto;
using MiejskiDomKultury.Interfaces;
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
    public class TabelaBanAdminViewModel : INotifyPropertyChanged
    {
        private readonly IBanRepository _banRepository;

        private ObservableCollection<BanZUzytkownikiemDto> _banCollection;
        public ObservableCollection<BanZUzytkownikiemDto> BanCollection
        {
            get => _banCollection;
            set
            {
                _banCollection = value;
                OnPropertyChanged(nameof(BanCollection));
            }
        }

        private ICollectionView _banCollectionView;
        public ICollectionView BanCollectionView
        {
            get => _banCollectionView;
            set
            {
                _banCollectionView = value;
                OnPropertyChanged(nameof(BanCollectionView));

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
                BanCollectionView.Refresh();
            }
        }

        public TabelaBanAdminViewModel()
        {
            var context = new DbContextDomKultury();
            _banRepository = new BanRepository(context);

            BanCollection = new ObservableCollection<BanZUzytkownikiemDto>(_banRepository.GetAllBansWithUsers());
            BanCollectionView = CollectionViewSource.GetDefaultView(BanCollection);
            BanCollectionView.Filter = FilterBan;
        }

        private bool FilterBan(object obj)
        {
            if (obj is BanZUzytkownikiemDto dto)
            {
                if (string.IsNullOrWhiteSpace(WyszukiwanaFraza))
                {
                    return true;
                }
                else
                {
                    return dto.Nazwisko.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Imie.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.NazwaUzytkownika.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Email.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Data.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Przyczyna.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Dlugosc.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);

                }
            }
            else
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
