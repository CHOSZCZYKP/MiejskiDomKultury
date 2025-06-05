using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiejskiDomKultury
{
    /// <summary>
    /// Logika interakcji dla klasy Ustawienia.xaml
    /// </summary>
    public partial class Ustawienia : Window
    {
        public Ustawienia()
        {
            InitializeComponent();
            UstawStanPrzycisku();
            PasswordBoxEnabled();
        }
        private void UstawStanPrzycisku()
        {
            if (Motywy.CzyCiemny)
            {
                MotywToggleButton.IsChecked = true;
                MotywToggleButton.Content = "Motyw Ciemny";
            }
            else
            {
                MotywToggleButton.IsChecked = false;
                MotywToggleButton.Content = "Motyw Jasny";
            }

            if (Motywy.CzyAngielski)
            {
                LangToggleButton.IsChecked = true;
                LangToggleButton.Content = "English";
            }
            else
            {
                LangToggleButton.IsChecked = false;
                LangToggleButton.Content = "Polski";
            }
        }


        private void MotywToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Motywy.ZmianaMotywu(new Uri("/Themes/MotywCiemny.xaml", UriKind.Relative));
            MotywToggleButton.Content = "Motyw Ciemny";
        }

        private void MotywToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Motywy.ZmianaMotywu(new Uri("/Themes/MotywJasny.xaml", UriKind.Relative));
            MotywToggleButton.Content = "Motyw Jasny";
        }

        private void LangToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Motywy.ZmianaLang(new Uri("/Lang/en.xaml", UriKind.Relative));
            LangToggleButton.Content = "English";
        }

        private void LangToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Motywy.ZmianaLang(new Uri("/Lang/pl.xaml", UriKind.Relative));
            LangToggleButton.Content = "Polski";
        }

        private void PasswordBoxEnabled()
        {
            if (Session.CzyZalogowany)
            {
                StareHasloUstawienia.IsEnabled = true;
                NoweHasloUstawienia.IsEnabled = true;
                PowtorzNoweHasloUstawienia.IsEnabled = true;
            }
            else
            {
                StareHasloUstawienia.IsEnabled = false;
                NoweHasloUstawienia.IsEnabled = false;
                PowtorzNoweHasloUstawienia.IsEnabled = false;
            }
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            if (Session.CzyZalogowany)
            {
                var stareHaslo = StareHasloUstawienia.Password;
                var noweHaslo = NoweHasloUstawienia.Password;
                var powtNoweHaslo = PowtorzNoweHasloUstawienia.Password;

                if (noweHaslo == powtNoweHaslo && PasswordHasher.HashPassword(stareHaslo) == Session.User.HasloHash)
                {
                    using (var context = new DbContextDomKultury())
                    {
                        var userToUpdate = context.Uzytkownicy.FirstOrDefault(u => u.Id == Session.User.Id);
                        if (userToUpdate != null)
                        {
                            userToUpdate.HasloHash = PasswordHasher.HashPassword(noweHaslo);
                            context.SaveChanges();

                            MessageBox.Show("Hasło zostało zmienione.\nZmiany zostaną zastosowane po zamknięciu aplikacji.");
                        }
                    }
                } else {
                    MessageBox.Show("Błędne hasło.");
                }
            }
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
