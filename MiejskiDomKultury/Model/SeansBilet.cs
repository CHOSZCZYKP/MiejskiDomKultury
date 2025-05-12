using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class SeansBilet
    {
        public int Id { get; set; }
        public int SeansId {get;set;}
        public Seans Seans { get; set; }

        public int SeatNumber { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

    }
}
