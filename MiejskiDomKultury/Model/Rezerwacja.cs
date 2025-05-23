using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Rezerwacja
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdSali { get; set; }
        public Sala Sala { get; set; } = default!;
        [Required]
        public int IdUzytkownika { get; set; }
        public Uzytkownik Uzytkownik { get; set; } = default!;
        [Required]
        public DateTime Data { get; set; }
        [Required]
        [Column(TypeName ="time")]
        public TimeSpan GodzinaPoczatkowa { get; set; }
        [Required]
        [Column(TypeName = "time")]
        public TimeSpan GodzinaKoncowa { get; set; }
    }
}
