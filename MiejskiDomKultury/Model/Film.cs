using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Model
{
    public class Film
    {
        public int Id { get; set; }
        public string Tytul { get; set; }
        public List<string> Aktorzy { get; set; }
        public int Rok { get; set; }
        public string Opis { get; set; }

        public List<string> Gatunki { get; set; }

        public string PlakatURL { get; set; }

        
    }
}
