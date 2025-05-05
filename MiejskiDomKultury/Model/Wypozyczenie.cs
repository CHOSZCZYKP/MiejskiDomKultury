using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Wypozyczenie
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdUzytkownika { get; set; }
        public Uzytkownik Uzytkownik { get; set; } = default!;
        [Required]
        public DateTime DataWypozyczenia { get; set; }
        [Required]
        public DateTime DataZwrotu { get; set; }
        [Required]
        public int IdPrzedmiotu { get; set; }
        public Przedmiot Przedmiot { get; set; } = default!;
    }
}
