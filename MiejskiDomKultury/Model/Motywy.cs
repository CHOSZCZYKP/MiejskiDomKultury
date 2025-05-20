using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiejskiDomKultury.Model
{
    public static class Motywy
    {
        public static bool CzyCiemny { get; private set; }
        public static bool CzyAngielski { get; private set; }

        public static void ZmianaMotywu(Uri motywUri)
        {
            ResourceDictionary Motyw = new ResourceDictionary() { Source = motywUri };
            ResourceDictionary MotywShared = new ResourceDictionary() { Source = new Uri("/Themes/Shared.xaml", UriKind.Relative) };

            bool zapisanyAngielski = Settings.Default.CzyLangAngielski;

            var langUri = zapisanyAngielski
                ? new Uri("/Lang/en.xaml", UriKind.Relative)
                : new Uri("/Lang/pl.xaml", UriKind.Relative);

            if (zapisanyAngielski)
                CzyAngielski = true;
            else
                CzyAngielski = false;

            ResourceDictionary Lang = new ResourceDictionary() { Source = langUri };

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Lang);
            App.Current.Resources.MergedDictionaries.Add(Motyw);
            App.Current.Resources.MergedDictionaries.Add(MotywShared);

            CzyCiemny = motywUri.OriginalString.Contains("Ciemny");

            Settings.Default.CzyMotywCiemny = CzyCiemny;
            Settings.Default.Save();
        }

        public static void ZaladujMotywStartowy()
        {
            bool zapisanyCiemny = Settings.Default.CzyMotywCiemny;

            var uri = zapisanyCiemny
                ? new Uri("/Themes/MotywCiemny.xaml", UriKind.Relative)
                : new Uri("/Themes/MotywJasny.xaml", UriKind.Relative);

            ZmianaMotywu(uri);
        }



        public static void ZmianaLang(Uri langUri)
        {
            ResourceDictionary Lang = new ResourceDictionary() { Source = langUri };
            ResourceDictionary MotywShared = new ResourceDictionary() { Source = new Uri("/Themes/Shared.xaml", UriKind.Relative) };

            bool zapisanyCiemny = Settings.Default.CzyMotywCiemny;

            var motywUri = zapisanyCiemny
                ? new Uri("/Themes/MotywCiemny.xaml", UriKind.Relative)
                : new Uri("/Themes/MotywJasny.xaml", UriKind.Relative);

            ResourceDictionary Motyw = new ResourceDictionary() { Source = motywUri };

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Lang);
            App.Current.Resources.MergedDictionaries.Add(Motyw);
            App.Current.Resources.MergedDictionaries.Add(MotywShared);

            CzyAngielski = langUri.OriginalString.Contains("en");

            Settings.Default.CzyLangAngielski = CzyAngielski;
            Settings.Default.Save();
        }
    }

}
