using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Dto
{
    public class TransakcjaZUzytkownikiemDto
    {
        public string Imie { get; set; } = default!;
        public string Nazwisko { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime Data { get; set; }
        public string Typ { get; set; } = default!;
        public string Kwota_Pelna { get; set; } = default!;
    }
}
