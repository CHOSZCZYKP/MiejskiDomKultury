using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Sala
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Nazwa { get; set; } = default!;
        [Required]
        public int IloscMiejsc { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Typ { get; set; } = default!;
        [Required]
        public decimal CenaZaGodz_Wartosc { get; set; }
        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string CenaZaGodz_Waluta { get; set; } = default!;

        public ICollection<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
    }
}
