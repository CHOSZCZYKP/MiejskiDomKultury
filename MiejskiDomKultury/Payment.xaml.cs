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


          //  MessageBox.Show("Platnosc sie  powiodla");

            Dispatcher.Invoke(() => { 

             List < SeansBilet > tickets = new List<SeansBilet>();
                var sum = 0;
            foreach (var x in seats)
            {
                    var seansBilet = new SeansBilet
                    {
                        Date = DateTime.Now,
                        SeansId = seans.Id, 
                     
                        SeatNumber = x,
                        Cena = 32
                    };
                    sum += x;
                movieRepositoryService.AddSeansBilet(seansBilet);
                    seansBilet.Seans = seans;
                     tickets.Add(seansBilet);
                }
            PdfService pdfService = new PdfService();
            var att= pdfService.GenerateTickets(tickets);
                EmailService emailService = new EmailService();

                //Trzeba dać prawdziwy adres email!!!
                emailService.sendTickets(att, Session.User.Email);
                Transakcja t = new Transakcja { IdUzytkownika = Session.User.Id, Kwota_Wartosc = sum, Kwota_Waluta = "PLN", Typ = "Płatność elektroniczna", Data=DateTime.Now };
                transakcjaService.AddTransakcja(t);
            NavigationService.Navigate(new Home());
        });
        }

        private void HandlePaymentCancel(string sessionId)
        {
            MessageBox.Show("Platnosc sie nie powiodla");
            _paymentListener.Stop();
        }

    }
}
