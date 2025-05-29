using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Dto
{
    public class WypozyczeniePrzedmiotuPrzezUzytkownika
    {
        public string Imie { get; set; } = default!;
        public string Nazwisko { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime DataWypozyczenia { get; set; }
        public DateTime DataZwrotu { get; set; }
        public string NazwaPrzedmiotu { get; set; } = default!;
        public string Cena { get; set; } = default!;
    }
}
