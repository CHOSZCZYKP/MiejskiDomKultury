using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
    public class TransakcjaRepository : ITranskacjaRepository
    {
        private readonly DbContextDomKultury _dbContextDomKultury;
        public TransakcjaRepository(DbContextDomKultury dbContextDomKultury)
        {
            this._dbContextDomKultury = dbContextDomKultury;
        }
        public IEnumerable<Transakcja> GetAllTransakcja()
            => _dbContextDomKultury.Transakcje.ToList();

        public IEnumerable<TransakcjaZUzytkownikiemDto> GetAllTransakcjeZUzytkownikami()
            => _dbContextDomKultury.Transakcje
            .Include(t => t.Uzytkownik)
            .Select(t => new TransakcjaZUzytkownikiemDto
            {
                Imie = t.Uzytkownik.Imie,
                Nazwisko = t.Uzytkownik.Nazwisko,
                Email = t.Uzytkownik.Email,
                Data = t.Data,
                Typ = t.Typ,
                Kwota_Pelna = t.Kwota_Pelna
            }).ToList();

        public Dictionary<string, int> GetAllTransakcjeGroupTyp()
            => _dbContextDomKultury.Transakcje
            .GroupBy(t => t.Typ)
            .Select(g => new
            {
                Typ = g.Key,
                Liczba = g.Count()
            }).ToDictionary(x => x.Typ, x => x.Liczba);
    }
}
