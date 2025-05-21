using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Dto
{
    public class BanZUzytkownikiemDto
    {
        public string Imie { get; set; } = default!;
        public string Nazwisko { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string NazwaUzytkownika { get; set; } = default!;
        public DateTime Data { get; set; } = default!;
        public int Dlugosc { get; set; } = default!;
        public string Przyczyna { get; set; } = default!;
    }
}
