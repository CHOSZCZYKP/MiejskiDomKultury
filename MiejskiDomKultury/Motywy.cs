using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiejskiDomKultury
{
    internal class Motywy
    {
        public static void ZmianaMotywu(Uri motywUri)
        {
            ResourceDictionary Motyw = new ResourceDictionary() { Source = motywUri };
            ResourceDictionary MotywShared = new ResourceDictionary() { Source = new Uri("/Themes/Shared.xaml", UriKind.Relative) };

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Motyw);
            App.Current.Resources.MergedDictionaries.Add(MotywShared);
        }
    }
}
