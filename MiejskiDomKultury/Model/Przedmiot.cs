using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Przedmiot
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string Nazwa { get; set; } = default!;
        [Required]
        [MaxLength(25)]
        [MinLength(2)]
        public string Stan { get; set; } = default!;
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Typ { get; set; } = default!;
        [Required]
        public decimal CenaZaDobe_Wartosc { get; set; }
        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string CenaZaDobe_Waluta { get; set; } = default!;
        [Required]
        public bool Dostepnosc { get; set; }

        public ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();

        public string CenaZaDobe_Pelna => $"{CenaZaDobe_Wartosc} {CenaZaDobe_Waluta}";
    }
}
