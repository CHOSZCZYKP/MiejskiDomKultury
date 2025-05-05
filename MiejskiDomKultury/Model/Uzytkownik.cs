using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Uzytkownik
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Imie { get; set; } = default!;
        [Required]
        [MaxLength(70)]
        [MinLength(2)]
        public string Nazwisko { get; set; } = default!;
        [Required]
        [MaxLength(256)]
        [MinLength(8)]
        public string Email { get; set; } = default!;
        [Required]
        public string HasloHash { get; set; } = default!;
        [Required]
        [MaxLength(256)]
        public string NazwaUzytkownika { get; set; } = default!;
        [Required]
        [MaxLength(5)]
        public string Rola { get; set; } = default!;
        [Required]
        public bool CzyMaBana { get; set; }

        public ICollection<Rezerwacja> Rezerwacje { get; set; } = new List<Rezerwacja>();
        public ICollection<Ban> Bany { get; set; } = new List<Ban>();
        public ICollection<Transakcja> Transakcje { get; set; } = new List<Transakcja>();
        public ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();
    }
}
