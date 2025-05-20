using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Seans
    {

        public int Id { get; set; }
        public int FilmId { get; set; }

        public Film Film { get; set; }

        public DateTime DataStart { get; set; }

        

        public ICollection<Uzytkownik> Widzowie { get; set; } = new List<Uzytkownik>();
    }
}
