using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using MiejskiDomKultury.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Printing;
using System.Windows.Data;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiejskiDomKultury.ViewModel
{
    public class RezerwacjaViewModel : INotifyPropertyChanged
    {
        private readonly IRezerwacjaRepository _rezerwacjaRepository;
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

        public ObservableCollection<Rezerwacja> RezerwacjeCollection { get; set; }

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
        #region Wlasciowsci
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
        private string _godzinaOd;
        public string GodzinaOd
        {
            get => _godzinaOd;
            set
            {
                _godzinaOd = value;
                OnPropertyChanged(nameof(GodzinaOd));
                SaleCollectionView.Refresh();
            }
        }

        private string _godzinaDo;
        public string GodzinaDo
        {
            get => _godzinaDo;
            set
            {
                _godzinaDo = value;
                OnPropertyChanged(nameof(GodzinaDo));
                SaleCollectionView.Refresh();
            }
        }

        private DateTime _data;
        public DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
                SaleCollectionView.Refresh();
            }
        }
        #endregion

        public RezerwacjaViewModel()
        {
            DbContextDomKultury context = new DbContextDomKultury();
            _saleRepository = new SaleRepository(context);
            _rezerwacjaRepository = new RezerwacjeRepository(context);
            SaleCollection = new ObservableCollection<Sala>(_saleRepository.GetAllSale());
            RezerwacjeCollection = new ObservableCollection<Rezerwacja>(_rezerwacjaRepository.GetAllRezerwacje());
            SaleCollectionView = CollectionViewSource.GetDefaultView(SaleCollection);
            SaleCollectionView.Filter = FilterSale;
            Data = DateTime.Now;
        }


        private bool FilterSale(object obj)
        {
            if (obj is not Sala sala)
                return false;

            bool godzinaOdZParsowana = TimeSpan.TryParse(GodzinaOd, out TimeSpan godzinaOd);
            bool godzinaDoZParsowana = TimeSpan.TryParse(GodzinaDo, out TimeSpan godzinaDo);
            bool jestData = Data != default;

            bool spelniaFraze = string.IsNullOrWhiteSpace(WyszukiwanaFraza)
                || sala.Nazwa.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                || sala.Typ.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                || sala.IloscMiejsc.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                || sala.CenaZaGodz_Wartosc.ToString().Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase)
                || sala.CenaZaGodz_Waluta.Contains(WyszukiwanaFraza, StringComparison.OrdinalIgnoreCase);

            if (!jestData || !godzinaOdZParsowana || !godzinaDoZParsowana)
                return spelniaFraze;

            bool czyZajeta = RezerwacjeCollection.Any(r =>
                r.IdSali == sala.Id
                && r.Data.Date == Data.Date
                && (godzinaOd < r.GodzinaKoncowa && godzinaDo > r.GodzinaPoczatkowa)
            );

            return !czyZajeta && spelniaFraze;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
