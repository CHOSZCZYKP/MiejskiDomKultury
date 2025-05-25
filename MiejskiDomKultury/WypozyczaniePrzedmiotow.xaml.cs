using Microsoft.EntityFrameworkCore;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiejskiDomKultury
{
    /// <summary>
    /// Logika interakcji dla klasy WypozyczaniePrzedmiotow.xaml
    /// </summary>
    public partial class WypozyczaniePrzedmiotow : Page
    {
        private TabelaPrzedmiotyViewModel _viewModel;
        public WypozyczaniePrzedmiotow()
        {
            InitializeComponent();
            _viewModel = new TabelaPrzedmiotyViewModel();
            DataContext = _viewModel;
        }


        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        private void DataGridPrzedmioty_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = DataGridPrzedmioty.SelectedItem as Przedmiot;
            if (selectedItem == null) return;

            using (var db = new DbContextDomKultury())
            {
                var wypozyczenia = db.Wypozyczenia
                    .Where(w => w.IdPrzedmiotu == selectedItem.Id)
                    .ToList();

                var wypDat = new WypozyczenieDaty(selectedItem, wypozyczenia);
                wypDat.ShowDialog();
            }
        }
    }
}
