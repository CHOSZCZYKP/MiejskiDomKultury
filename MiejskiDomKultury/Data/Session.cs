using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using MiejskiDomKultury.Model;

namespace MiejskiDomKultury.Data
{
    public static class Session
    {
        public static Uzytkownik? User {  get; private set; }

        public static event Action? ZmianaUzytkownika;

        public static void Login(Uzytkownik uzytkownik)
        {
            User = uzytkownik;
            ZmianaUzytkownika?.Invoke();
        }

        public static void Logout()
        {
            User = null;
            ZmianaUzytkownika?.Invoke();
        }

        public static bool CzyAdmin => User?.Rola == "Admin";
        public static bool CzyZalogowany => User != null;
    }
}
