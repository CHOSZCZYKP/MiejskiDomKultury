using Microsoft.EntityFrameworkCore;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiejskiDomKultury
{
    /// <summary>
    /// Interaction logic for WypozyczenieDaty.xaml
    /// </summary>
    public partial class WypozyczenieDaty : Window
    {
        private Przedmiot _obiekt;
        private List<Wypozyczenie> _wypozyczeniaList;

        private Dictionary<int, Button> dayButtons = new();

        private int month;
        private int year;
        private int daysInMonth;

        private int startDate = 0;
        private int endDate = 0;

        public WypozyczenieDaty(Przedmiot obiekt, List<Wypozyczenie> wypozyczeniaList)
        {
            InitializeComponent();
            _obiekt = obiekt;
            _wypozyczeniaList = wypozyczeniaList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MonthComboBox.ItemsSource = Enumerable.Range(1, 12).Select(i => new DateTime(2000, i, 1).ToString("MMMM"));
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;

            YearComboBox.ItemsSource = Enumerable.Range(2020, 11);
            YearComboBox.SelectedItem = DateTime.Now.Year;

            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
            YearComboBox.SelectedItem = DateTime.Now.Year;
        }

        private void MonthOrYearChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedIndex == -1 || YearComboBox.SelectedItem == null)
                return;

            month = MonthComboBox.SelectedIndex + 1;
            year = (int)YearComboBox.SelectedItem;
            daysInMonth = DateTime.DaysInMonth(year, month);

            DaysPanel.Children.Clear();
            dayButtons.Clear();

            for (int day = 1; day <= daysInMonth; day++)
            {
                int selectedDay = day;
                DateTime currentDate = new DateTime(year, month, day);

                Button btn = new Button
                {
                    Content = day.ToString(),
                    Width = 40,
                    Height = 40,
                    Margin = new Thickness(5)
                };

                bool isOccupied = _wypozyczeniaList.Any(w =>
                    currentDate.Date >= w.DataWypozyczenia.Date &&
                    currentDate.Date <= w.DataZwrotu.Date);

                if (isOccupied)
                {
                    btn.Background = Brushes.Red;
                    btn.IsEnabled = false;
                }
                else
                {
                    btn.Click += (s, ev) =>
                    {
                        updateStartStop(selectedDay);
                    };
                }

                DaysPanel.Children.Add(btn);
                dayButtons[selectedDay] = btn;
            }
        }

        public void updateStartStop(int dayNum)
        {
            DateTime selectedDate = new DateTime(year, month, dayNum);

            bool taken = _wypozyczeniaList.Any(w =>
                selectedDate.Date >= w.DataWypozyczenia.Date &&
                selectedDate.Date <= w.DataZwrotu.Date);

            bool encircled = false;

            if (startDate != 0)
            {
                DateTime potDateStart = new DateTime(year, month, startDate);

                encircled = _wypozyczeniaList.Any(w =>
                    potDateStart.Date <= w.DataWypozyczenia.Date &&
                    selectedDate.Date >= w.DataZwrotu.Date);
            }

            if (!taken && !encircled)
            {
                if (startDate == 0)
                {
                    startDate = dayNum;
                }
                else if (endDate == 0)
                {
                    endDate = dayNum;
                }
            }

            foreach (var kv in dayButtons)
            {
                int day = kv.Key;
                var btn = kv.Value;

                if (day >= startDate && day <= endDate)
                {
                    btn.Background = Brushes.LightGreen;
                }
                else
                {
                    btn.ClearValue(Button.BackgroundProperty);
                }
            }
        }


        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (startDate > endDate || startDate == 0 || endDate == 0)
            {
                MessageBox.Show("Invalid Reservation.");
                return;
            }

            DateTime start = new DateTime(year, month, startDate);
            DateTime end = new DateTime(year, month, endDate);

            var newWypozyczenie = new Wypozyczenie
            {
                IdUzytkownika = 1,
                IdPrzedmiotu = _obiekt.Id,
                DataWypozyczenia = start,
                DataZwrotu = end
            };

            try
            {
                using (var db = new DbContextDomKultury())
                {
                    db.Wypozyczenia.Add(newWypozyczenie);
                    db.SaveChanges();
                }

                MessageBox.Show("Reservation saved.");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving reservation:\n{ex.Message}\n\n{ex.InnerException?.Message}");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            startDate = 0;
            endDate = 0;
        }
    }
}
