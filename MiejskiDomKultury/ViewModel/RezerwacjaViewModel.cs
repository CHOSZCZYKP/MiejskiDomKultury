using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using MiejskiDomKultury.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace MiejskiDomKultury.ViewModel
{
    public class RezerwacjaViewModel : INotifyPropertyChanged
    {
        private readonly IRezerwacja _rezerwacjaService;

        private readonly ISaleRepository _saleRepository;



        public RezerwacjaViewModel()
        {
            DbContextDomKultury context = new DbContextDomKultury();
            _saleRepository = new SaleRepository(context);

            SaleCollection = new ObservableCollection<Sala>(_saleRepository.GetAllSale());
            SaleCollectionView = CollectionViewSource.GetDefaultView(SaleCollection);
            SaleCollectionView.Filter = FilterSale;
        }

        private string _GodzinaOd;
        public string GodzinaOd
        {
            get => _GodzinaOd;
            set
            {
                _GodzinaOd = value;
                OnPropertyChanged(nameof(GodzinaOd));
                SaleCollectionView.Refresh();
            }
        }

        private string _GodzinaDo;
        public string GodzinaDo
        {
            get => _GodzinaDo;
            set
            {
                _GodzinaDo = value;
                OnPropertyChanged(nameof(GodzinaDo));
                SaleCollectionView.Refresh();
            }
        }

        private DateTime _Data;
        public DateTime Data
        {
            get => _Data;
            set
            {
                _Data = value;
                OnPropertyChanged(nameof(Data));
                SaleCollectionView.Refresh();
            }
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
