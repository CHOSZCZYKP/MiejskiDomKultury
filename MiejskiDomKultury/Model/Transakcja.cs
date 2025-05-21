using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Transakcja
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal Kwota_Wartosc { get; set; }
        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string Kwota_Waluta { get; set; } = default!;
        [Required]
        public DateTime Data { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Typ { get; set; } = default!;
        [Required]
        public int IdUzytkownika { get; set; }
        public Uzytkownik Uzytkownik { get; set; } = default!;

        public string Kwota_Pelna => $"{Kwota_Wartosc} {Kwota_Waluta}";
    }
}
