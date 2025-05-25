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
    public class RezerwacjeRepository : IRezerwacjaRepository
    {
        private readonly DbContextDomKultury _dbContextDomKultury;
        public RezerwacjeRepository(DbContextDomKultury dbContextDomKultury)
        {
            this._dbContextDomKultury = dbContextDomKultury;
        }

        public IEnumerable<Rezerwacja> GetAllRezerwacje()
            => _dbContextDomKultury.Rezerwacje.ToList();

        public IEnumerable<RezerwacjaSaliPrzezUzytkownikaDto> GetAllRezerwacjeWithUserAndRoom()
            => _dbContextDomKultury.Rezerwacje
            .Include(r => r.Sala)
            .Include(r => r.Uzytkownik)
            .Select(r => new RezerwacjaSaliPrzezUzytkownikaDto
            {
                Imie = r.Uzytkownik.Imie,
                Nazwisko = r.Uzytkownik.Nazwisko,
                Email = r.Uzytkownik.Email,
                Data = r.Data,
                GodzinaKoncowa = r.GodzinaKoncowa,
                GodzinaPoczatkowa = r.GodzinaPoczatkowa,
                NazwaSali = r.Sala.Nazwa
            }).ToList();

        public Dictionary<string, int> GetHowManyTimesRoomBooked()
            => _dbContextDomKultury.Rezerwacje
            .Include(r => r.Sala)
            .GroupBy(r => r.IdSali)
            .Select(g => new
            {
                Nazwa = g.First().Sala.Nazwa,
                Liczba = g.Count()
            }).ToDictionary(x => x.Nazwa, x => x.Liczba);
    }
}
