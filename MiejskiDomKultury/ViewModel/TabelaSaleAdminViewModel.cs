using MiejskiDomKultury.Data;
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
    public class TabelaSaleAdminViewModel : INotifyPropertyChanged
    {
        private readonly ISaleRepository _saleRepository;
        private ObservableCollection<Sala> _saleCollection = new();
        public ObservableCollection<Sala> SaleCollection 
        {
            get => _saleCollection; 
            set
            {
                _saleCollection = value;
                OnPropertyChanged(nameof(SaleCollection));
            }
        }

        private ICollectionView _saleCollectionView;
        public ICollectionView SaleCollectionView
        {
            get => _saleCollectionView;
            set
            {
                _saleCollectionView = value;
                OnPropertyChanged(nameof(SaleCollectionView));
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
                SaleCollectionView.Refresh();
            }
        }

        public TabelaSaleAdminViewModel()
        {
            DbContextDomKultury context = new DbContextDomKultury();
            _saleRepository = new SaleRepository(context);
            
            SaleCollection = new ObservableCollection<Sala>(_saleRepository.GetAllSale());
            SaleCollectionView = CollectionViewSource.GetDefaultView(SaleCollection);
            SaleCollectionView.Filter = FilterSale;
        }

        private bool FilterSale(object obj)
        {
            if (obj is Sala sala)
            {
                if (string.IsNullOrWhiteSpace(WyszukiwanaFraza))
                {
                    return true;
                }
                else
                {
                    return sala.Nazwa.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || sala.Typ.Contains(WyszukiwanaFraza, StringComparison.CurrentCulture)
                        || sala.IloscMiejsc.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || sala.CenaZaGodz_Wartosc.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                        || sala.CenaZaGodz_Waluta.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);
                }
            }
            else
            {
                return false;
            }    
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
