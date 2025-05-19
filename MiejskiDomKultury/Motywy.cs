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

            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(Motyw);
        }
    }
}
