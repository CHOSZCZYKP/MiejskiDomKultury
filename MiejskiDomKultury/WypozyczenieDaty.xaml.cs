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
        public WypozyczenieDaty(Przedmiot obiekt)
        {
            InitializeComponent();
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

            int month = MonthComboBox.SelectedIndex + 1;
            int year = (int)YearComboBox.SelectedItem;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            DaysPanel.Children.Clear();

            for (int day = 1; day <= daysInMonth; day++)
            {
                Button btn = new Button
                {
                    Content = day.ToString(),
                    Width = 40,
                    Height = 40,
                    Margin = new Thickness(5)
                };
                btn.Click += (s, ev) =>
                {
                    updateStartStop();
                };
                DaysPanel.Children.Add(btn);
            }
        }

        public void updateStartStop(int dayNum)
        {

        }
    }
}
