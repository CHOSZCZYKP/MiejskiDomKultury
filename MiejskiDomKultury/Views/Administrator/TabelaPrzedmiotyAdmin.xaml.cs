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

namespace MiejskiDomKultury.Views.Administrator
{
    /// <summary>
    /// Logika interakcji dla klasy TabelaPrzedmiotyAdmin.xaml
    /// </summary>
    public partial class TabelaPrzedmiotyAdmin : Page
    {
        private TabelaPrzedmiotyViewModel _viewModel;
        public TabelaPrzedmiotyAdmin()
        {
            InitializeComponent();
           // DataContext = new TabelaPrzedmiotyViewModel();
           _viewModel = new TabelaPrzedmiotyViewModel();
            _viewModel.WywolajOknoDodajNowyPrzedmiot += WyswietlDodajNowyPrzedmiot;
            DataContext = _viewModel;
        }

        
        private Task<bool> WyswietlDodajNowyPrzedmiot()
        {
            DodajNowyPrzedmiot dodajNowyPrzedmiot = new DodajNowyPrzedmiot();
            bool? wynik = dodajNowyPrzedmiot.ShowDialog();
            return Task.FromResult(wynik == true);
        }

        private void DataGridPrzedmiotyAdmin_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var edytowanyPrzedmiot = e.Row.Item as Przedmiot;

                if (_viewModel.ZapiszZmianyCommand.CanExecute(edytowanyPrzedmiot))
                {
                    _viewModel.ZapiszZmianyCommand.Execute(edytowanyPrzedmiot);
                }
            }
        }
    }
}
