using Microsoft.EntityFrameworkCore;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Dto;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Repositories
{
    public class BanRepository : IBanRepository
    {
        private readonly DbContextDomKultury _dbContextDomKultury;
        public BanRepository(DbContextDomKultury dbContextDomKultury)
        {
            this._dbContextDomKultury = dbContextDomKultury;
        }

        public void AddNewBan(Ban ban)
        {
            _dbContextDomKultury.Bany.Add(ban);
            _dbContextDomKultury.SaveChanges();
        }

        public IEnumerable<BanZUzytkownikiemDto> GetAllBansWithUsers()
            => _dbContextDomKultury.Bany
            .Include(b => b.Uzytkownik)
            .Select(b => new BanZUzytkownikiemDto
            {
                Imie = b.Uzytkownik.Imie,
                Nazwisko = b.Uzytkownik.Nazwisko,
                NazwaUzytkownika = b.Uzytkownik.NazwaUzytkownika,
                Data = b.Data,
                Dlugosc = b.Dlugosc,
                Email = b.Uzytkownik.Email,
                Przyczyna = b.Przyczyna
            }).ToList();
    }
}
