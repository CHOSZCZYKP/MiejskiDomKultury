using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class MovieShow
    {

        public int Id { get; set; }
        public int FilmId { get; set; }

        public Movie Film { get; set; }

        public DateTime DateStart { get; set; }

        public int Time { get; set; }
    }
}
