using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Repositories
{
    public class RezerwacjeRepository : IRezerwacjaRepository
    {
        private readonly DbContextDomKultury _dbContextDomKultury;
        public RezerwacjeRepository(DbContextDomKultury dbContextDomKultury)
        {
            this._dbContextDomKultury = dbContextDomKultury;
        }

        public IEnumerable<Rezerwacja> GetAllRezerwacje()
            => _dbContextDomKultury.Rezerwacje.ToList();
    }
}
