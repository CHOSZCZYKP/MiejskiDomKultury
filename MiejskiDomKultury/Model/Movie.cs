using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<string> ActorsNames { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }

        public List<string> Kinds { get; set; }

        public ICollection<Uzytkownik> Widzowie { get; set; } = new List<Uzytkownik>();
    }
}
