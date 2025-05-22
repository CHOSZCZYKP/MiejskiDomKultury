using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Dto
{
    public class RezewacjeSaleDto
    {
        public string Nazwa { get; set; } = default!;
        public int IloscMiejsc { get; set; }
        public string Typ { get; set; } = default!;
        public decimal CenaZaGodz_Wartosc { get; set; }
        public DateTime OdKiedy { get; set; }
        public int Id { get; set; }
    }
}
