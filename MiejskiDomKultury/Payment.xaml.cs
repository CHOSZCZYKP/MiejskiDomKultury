using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MiejskiDomKultury
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Page
    {
        private const int SeatPrice = 32;

        public Payment(HashSet<int> seats)
        {
            InitializeComponent();

            // Calculate total price
            int totalPrice = seats.Count * SeatPrice;

            // Create summary text
            string seatsList = string.Join(", ", seats);
            string summaryText = $"Wybrałeś miejsca: {seatsList}\n" +
                                 $"Liczba miejsc: {seats.Count}\n" +
                                 $"Łączna kwota do zapłaty: {totalPrice} zł";

            // Set the text in the summary panel
            SummaryTextBlock.Text = summaryText;
        }

        private void OnPayNowClicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Płatność została pomyślnie zakończona!");
        }

    }
}
