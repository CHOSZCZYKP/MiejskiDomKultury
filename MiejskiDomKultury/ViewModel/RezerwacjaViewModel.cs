using LiveChartsCore.Kernel;
using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Helpers;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using MiejskiDomKultury.Services;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Printing;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiejskiDomKultury.ViewModel
{
    public class RezerwacjaViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly IRezerwacjaRepository _rezerwacjaRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly ITranskacjaRepository _transkacjaRepository;
        private Sala PrzechowanieSali;
        private PrzekaznikCommand _zarezerwujSaleCommand;
        public ICommand ZarezerwujSaleCommand => _zarezerwujSaleCommand;

        private readonly Dictionary<string, List<string>> _errors = new();

        private PaymentListener _paymentListener;
        private string _currentSessionId;
        private int price;

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
                ValidateProperty(nameof(GodzinaOd), value);
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
                ValidateProperty(nameof(GodzinaDo), value);
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

        private string _iloscCykli;
        public string IloscCykli
        {
            get => _iloscCykli;
            set
            {
                _iloscCykli = value;
                OnPropertyChanged(nameof(IloscCykli));
                ValidateProperty(nameof(IloscCykli), value);
                SaleCollectionView.Refresh();
            }
        }

        private string _wybranyOkres;
        public string WybranyOkres
        {
            get => _wybranyOkres;
            set
            {
                _wybranyOkres = value;
                OnPropertyChanged(nameof(WybranyOkres));
                OnPropertyChanged(nameof(CzyJednorazowo));
                SaleCollectionView.Refresh();
                if (_wybranyOkres == "Jednorazowo")
                {
                    IloscCykli = string.Empty;
                }
            }
        }

        private Sala _wybranaSala;
        public Sala WybranaSala
        {
            get => _wybranaSala;
            set
            {
                _wybranaSala = value;
                OnPropertyChanged(nameof(WybranaSala));
                _zarezerwujSaleCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        public List<string> OkresComboBox { get; set; } = new List<string>()
        {
            "Jednorazowo",
            "1 raz w tygodniu",
            "1 raz w miesiącu",
            "1 raz w roku"
        };

        public RezerwacjaViewModel()
        {
            DbContextDomKultury context = new DbContextDomKultury();
            _saleRepository = new SaleRepository(context);
            _rezerwacjaRepository = new RezerwacjeRepository(context);
            _transkacjaRepository = new TransakcjaRepository(context);

            LadowanieDanychDoWidoku();
            _zarezerwujSaleCommand = new PrzekaznikCommand(Zarezerwuj, CzyMoznaZarezerwoac);
            Data = DateTime.Now;
            WybranyOkres = "Jednorazowo";
        }

        private void LadowanieDanychDoWidoku()
        {
            SaleCollection = new ObservableCollection<Sala>(_saleRepository.GetAllSale());
            RezerwacjeCollection = new ObservableCollection<Rezerwacja>(_rezerwacjaRepository.GetAllRezerwacjeOdDzis());
            SaleCollectionView = CollectionViewSource.GetDefaultView(SaleCollection);
            SaleCollectionView.Filter = FilterSale;
        }

        public bool CzyJednorazowo => WybranyOkres.Equals("Jednorazowo") ? false : true;


        private bool CzyMoznaZarezerwoac()
        {
            return WybranaSala != null && TimeSpan.TryParse(GodzinaOd, out _) && TimeSpan.TryParse(GodzinaDo, out _);
        }

        private void Zarezerwuj()
        {
            var wszystkieDatyRezerwacji = new List<DateTime>();
 
            if (WybranyOkres == "Jednorazowo")
            {
                wszystkieDatyRezerwacji.Add(Data);
            }
            else if (int.TryParse(IloscCykli, out int iloscCykliInt))
            {
                var aktualnaData = Data;
                for (int i = 0; i < iloscCykliInt; i++)
                {
                    wszystkieDatyRezerwacji.Add(aktualnaData);
                    aktualnaData = WybranyOkres switch
                    {
                        "1 raz w tygodniu" => aktualnaData.AddDays(7),
                        "1 raz w miesiącu" => aktualnaData.AddMonths(1),
                        "1 raz w roku" => aktualnaData.AddYears(1),
                        _ => aktualnaData
                    };
                }
            }
            foreach (var dataRezerwacji in wszystkieDatyRezerwacji)
            {
                var nowaRezerwacja = new Rezerwacja
                {
                    IdSali = WybranaSala.Id,
                    IdUzytkownika = Session.User!.Id,
                    Data = dataRezerwacji,
                    GodzinaPoczatkowa = TimeSpan.Parse(GodzinaOd),
                    GodzinaKoncowa = TimeSpan.Parse(GodzinaDo)
                };
                _rezerwacjaRepository.AddNewRezerwacja(nowaRezerwacja);
                PrzechowanieSali = WybranaSala;
            }

            /* MessageBox.Show("Pomyślnie dodano rezerwacje.", "Dodatnie rezerwacji", MessageBoxButton.OK, MessageBoxImage.Information);*/
            StartLocalServer();
            var stripeService = new StripeService();
            if (int.TryParse(IloscCykli, out int ic) && WybranyOkres != "Jednorazowo")
            {
                price = (int)WybranaSala.CenaZaGodz_Wartosc * ic;
            }
            else
            {
                price = (int)WybranaSala.CenaZaGodz_Wartosc;
            }
            _currentSessionId = stripeService.CreateCheckoutSession(price);
            LadowanieDanychDoWidoku();

        }

        private void StartLocalServer()
        {

            _paymentListener = new PaymentListener(58741);
            Task.Run(() => _paymentListener.StartListeningAsync(
                sessionId => HandlePaymentSuccess(sessionId),
                sessionId => HandlePaymentCancel(sessionId)
            ));

        }

        private void HandlePaymentSuccess(string sessionId)
        {
            if (sessionId != _currentSessionId) return;
            Application.Current.Dispatcher.Invoke(async () => {
                var stripe = new Stripe.StripeClient(Environment.GetEnvironmentVariable("STRIPE_KEY"));
                var service = new Stripe.Checkout.SessionService(stripe);
                var session = await service.GetAsync(sessionId);

                var paymentIntentId = session.PaymentIntentId;
                var paymentIntentService = new Stripe.PaymentIntentService(stripe);
                var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);

                var paymentMethodType = paymentIntent.PaymentMethodTypes.Count > 0
                    ? paymentIntent.PaymentMethodTypes[0]
                    : "unknown";


                string typPlatnosci = ConvertPaymentType(paymentMethodType);
                if (int.TryParse(IloscCykli, out int iloscInt) && WybranyOkres != "Jednorazowo")
                {
                    Transakcja t = new Transakcja
                    {
                        IdUzytkownika = Session.User.Id,
                        Kwota_Wartosc = PrzechowanieSali.CenaZaGodz_Wartosc * iloscInt,
                        Kwota_Waluta = PrzechowanieSali.CenaZaGodz_Waluta,
                        Typ = typPlatnosci,
                        Data = DateTime.Now
                    };
                    _transkacjaRepository.AddTransakcja(t);
                }
                else
                {
                    Transakcja t = new Transakcja
                    {
                        IdUzytkownika = Session.User.Id,
                        Kwota_Wartosc = PrzechowanieSali.CenaZaGodz_Wartosc,
                        Kwota_Waluta = PrzechowanieSali.CenaZaGodz_Waluta,
                        Typ = typPlatnosci,
                        Data = DateTime.Now
                    };
                    _transkacjaRepository.AddTransakcja(t);
                }

                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.Main.Content = App.ServiceProvider.GetRequiredService<Success>();
                }
            });
        }

        private void HandlePaymentCancel(string sessionId)
        {
            MessageBox.Show((string)Application.Current.FindResource("niepowodzeniePlatnosci"));
            _paymentListener.Stop();
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

            var wszystkieDatyRezerwacji = new List<DateTime>();
            var aktualnaData = Data;

            if (WybranyOkres.Equals("Jednorazowo") && string.IsNullOrWhiteSpace(IloscCykli))
            {
                wszystkieDatyRezerwacji.Add(aktualnaData);
            }
            else
            {
                if (int.TryParse(IloscCykli, out int iloscCykilInt))
                {
                    for (int i = 0; i < iloscCykilInt; i++)
                    {
                        wszystkieDatyRezerwacji.Add(aktualnaData);

                        aktualnaData = WybranyOkres switch
                        {
                            "1 raz w tygodniu" => aktualnaData.AddDays(7),
                            "1 raz w miesiącu" => aktualnaData.AddMonths(1),
                            "1 raz w roku" => aktualnaData.AddYears(1),
                            _ => aktualnaData
                        };

                    }
                }
            }

            bool czyZajeta = wszystkieDatyRezerwacji.Any(wszystkieDatyRezerwacji =>
                RezerwacjeCollection.Any(r =>
                    r.IdSali == sala.Id &&
                    r.Data.Date == wszystkieDatyRezerwacji.Date &&
                    godzinaOd < r.GodzinaKoncowa &&
                    godzinaDo > r.GodzinaPoczatkowa));

            return !czyZajeta && spelniaFraze;
        }
        private string ConvertPaymentType(string stripeType)
        {
            return stripeType switch
            {
                "card" => "Karta",
                "blik" => "BLIK",
                "p24" => "Przelewy24",
                "paypal" => "PayPal",
                _ => "Inna metoda"
            };
        }
        private void ValidateProperty(string propertyName, string value)
        {
            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(value) && !WybranyOkres.Equals("Jednorazowo"))
            {
                var wyswietl = propertyName switch
                {
                    nameof(GodzinaOd) => "Godzina od",
                    nameof(GodzinaDo) => "Godzina do",
                    _ => propertyName
                };

                errors.Add($"Pole {wyswietl} jest wymagane.");
            }
            else if (propertyName == nameof(GodzinaOd) || propertyName == nameof(GodzinaDo))
            {
                if (!TimeSpan.TryParseExact(value, "hh\\:mm", null, out TimeSpan parsedTime))
                {
                    errors.Add("Wprowadź godzinę w formacie HH:mm");
                }
                else if (parsedTime.Hours < 0 || parsedTime.Hours > 23
                    || parsedTime.Minutes < 0 || parsedTime.Minutes > 59)
                {
                    errors.Add("Godzina musi być między 00:00 a 23:59");
                }
                else if (TimeSpan.TryParse(GodzinaOd, out TimeSpan godzOd) && TimeSpan.TryParse(GodzinaDo, out TimeSpan godzDo)
                    && godzOd > godzDo)
                {
                    errors.Add("Godzina początkowa musi być mniejsz od końcowej");
                }

            }
            else if (!int.TryParse(value, out int wynik) && propertyName == nameof(IloscCykli) && !WybranyOkres.Equals("Jednorazowo"))
            {
                errors.Add("Wprowadź liczbę");
            }
            else if (wynik <= 0 && propertyName == nameof(IloscCykli) && !WybranyOkres.Equals("Jednorazowo"))
            {
                errors.Add("Wprowadź liczbę większą od 0");
            }

            if (errors.Any())
                _errors[propertyName] = errors;

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public bool HasErrors => _errors.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public IEnumerable GetErrors(string? propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            return null;
        }
    }
}
