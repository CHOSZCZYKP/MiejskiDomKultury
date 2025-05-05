using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Ban
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdUzytkownika { get; set; }
        public Uzytkownik Uzytkownik { get; set; } = default!;
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public int Dlugosc { get; set; }
        [Required]
        [MaxLength(500)]
        [MinLength(3)]
        public string Przyczyna { get; set; } = default!;
    }
}
