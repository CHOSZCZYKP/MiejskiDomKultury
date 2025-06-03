using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Page
    {
        private const int SeatPrice = 32;
        private PaymentListener _paymentListener;
        private string _currentSessionId;
        int price;
        Seans seans;
        HashSet<int> seats;
        MovieRepositoryService movieRepositoryService;
        TransakcjaRepository transakcjaService;
        public Payment(HashSet<int> seats, Seans seans)
        {
            InitializeComponent();
            this.seans = seans;
            this.seats = seats;
            int totalPrice = seats.Count * SeatPrice;
            price = totalPrice;
            
            string seatsList = string.Join(", ", seats);
            string summaryText = "Error";
            var db = new DbContextDomKultury();
            transakcjaService = new TransakcjaRepository(db);
            if (!Settings.Default.CzyLangAngielski)
            {
                 summaryText = $"Wybrałeś miejsca: {seatsList}\n" +
                                 $"Liczba miejsc: {seats.Count}\n" +
                                 $"Łączna kwota do zapłaty: {totalPrice} zł";
            }
            else
            {
                 summaryText = $"Your seats: {seatsList}\n" +
                                $"All reserved seats: {seats.Count}\n" +
                                $"Amount: {totalPrice} zł";
            }

            movieRepositoryService = new MovieRepositoryService();
     
            SummaryTextBlock.Text = summaryText;
        }

        private void OnPayNowClicked(object sender, RoutedEventArgs e)
        {
            StartLocalServer();
            var stripeService = new StripeService();
            _currentSessionId = stripeService.CreateCheckoutSession(price);
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

            Dispatcher.Invoke(async () =>
            {
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

              
                List<SeansBilet> tickets = new List<SeansBilet>();
                int sum = 0;

                foreach (var seat in seats)
                {
                    var seansBilet = new SeansBilet
                    {
                        Date = DateTime.Now,
                        SeansId = seans.Id,
                        UserId = Session.User.Id,
                        SeatNumber = seat,
                        Cena = SeatPrice
                    };
                    sum += SeatPrice;
                    movieRepositoryService.AddSeansBilet(seansBilet);
                    seansBilet.Seans = seans;
                    tickets.Add(seansBilet);
                }

                PdfService pdfService = new PdfService();
                var attachment = pdfService.GenerateTickets(tickets);

                EmailService emailService = new EmailService();
                emailService.sendTickets(attachment, Session.User.Email);

                Transakcja t = new Transakcja
                {
                    IdUzytkownika = Session.User.Id,
                    Kwota_Wartosc = sum,
                    Kwota_Waluta = "PLN",
                    Typ = typPlatnosci, 
                    Data = DateTime.Now
                };
                transakcjaService.AddTransakcja(t);

                NavigationService.Navigate(new Success());
            });
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


        private void HandlePaymentCancel(string sessionId)
        {
            MessageBox.Show("Platnosc sie nie powiodla");
            _paymentListener.Stop();
        }

    }
}
