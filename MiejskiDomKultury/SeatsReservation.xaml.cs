using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury
{
    public partial class SeatsReservation : Page
    {
        private HashSet<int> selectedSeats = new HashSet<int>();
        private List<int> freeSeats;

        public SeatsReservation(string date, Film movie)
        {
            InitializeComponent();

            var repo = new MovieRepositoryService();
            var seans = repo.GetSeans(DateTime.Parse(date), movie.Id);
            
            freeSeats = repo.GetFreeSeats(seans.Id);

            GenerateSeatGrid();
        }

        private void GenerateSeatGrid()
        {
            if (SeatsGrid == null)
                return;

            int totalSeats = 40; 
            for (int seatNumber = 1; seatNumber <= totalSeats; seatNumber++)
            {
                int currentSeatNumber = seatNumber;  

                Button seatButton = new Button
                {
                    Content = currentSeatNumber,
                    Margin = new Thickness(5),
                    Style = freeSeats.Contains(currentSeatNumber) ?
                            (Style)FindResource("FreeSeatButtonStyle") :
                            (Style)FindResource("TakenSeatButtonStyle"),
                    Foreground = Brushes.Black,
                    IsEnabled = freeSeats.Contains(currentSeatNumber),
                };

                seatButton.Click += (sender, e) => ToggleSeatSelection(seatButton, currentSeatNumber);
                SeatsGrid.Children.Add(seatButton);
            }
        }


        private void ToggleSeatSelection(Button seatButton, int seatNumber)
        {
            if (selectedSeats.Contains(seatNumber))
            {
                selectedSeats.Remove(seatNumber);
                seatButton.Background = Brushes.CornflowerBlue;
            }
            else
            {
                selectedSeats.Add(seatNumber);
                seatButton.Background = Brushes.YellowGreen;
            }
        }

        public void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Musisz wybrać miejsce");
                return;
            }
            NavigationService.Navigate(new Payment(selectedSeats));
        }
    }
}
