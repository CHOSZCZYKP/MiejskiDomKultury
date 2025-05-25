using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Dto
{
    public class RezerwacjaSaliPrzezUzytkownikaDto
    {
        public string Imie { get; set; } = default!;
        public string Nazwisko { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string NazwaSali { get; set; } = default!;
        public DateTime Data { get; set; }
        public TimeSpan GodzinaPoczatkowa { get; set; }
        public TimeSpan GodzinaKoncowa { get; set; }
    }
}
