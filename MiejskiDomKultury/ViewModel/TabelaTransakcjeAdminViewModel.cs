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
    public class TabelaTransakcjeAdminViewModel : INotifyPropertyChanged
    {
        private readonly ITranskacjaRepository _transakcjaRepository;

        private ObservableCollection<TransakcjaZUzytkownikiemDto> _transakcjaCollection;
        public ObservableCollection<TransakcjaZUzytkownikiemDto> TransakcjaCollection
        {
            get => _transakcjaCollection;
            set
            {
                _transakcjaCollection = value;
                OnPropertyChanged(nameof(TransakcjaCollection));
            }
        }

        private ICollectionView _transakcjaCollectionView;
        public ICollectionView TransakcjaCollectionView
        {
            get => _transakcjaCollectionView;
            set
            {
                _transakcjaCollectionView = value;
                OnPropertyChanged(nameof(TransakcjaCollectionView));
                
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
                TransakcjaCollectionView.Refresh();
            }
        }

        public TabelaTransakcjeAdminViewModel()
        {
            var context = new DbContextDomKultury();
            _transakcjaRepository = new TransakcjaRepository(context);


            TransakcjaCollection = new ObservableCollection<TransakcjaZUzytkownikiemDto>(_transakcjaRepository.GetAllTransakcjeZUzytkownikami());
            TransakcjaCollectionView = CollectionViewSource.GetDefaultView(TransakcjaCollection);
            TransakcjaCollectionView.Filter = FilterTransakcje;
            
        }

        private bool FilterTransakcje(object obj)
        {
            if (obj is TransakcjaZUzytkownikiemDto dto)
            {
                if (string.IsNullOrWhiteSpace(WyszukiwanaFraza))
                {
                    return true;
                }
                else
                {
                    return dto.Nazwisko.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Imie.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Email.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Typ.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Data.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || dto.Kwota_Pelna.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);
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
