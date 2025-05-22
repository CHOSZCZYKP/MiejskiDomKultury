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

namespace MiejskiDomKultury.Services
{
    public class RezerwacjaService : IRezerwacja
    {
        private readonly DbContextDomKultury _context;



        public RezerwacjaService(DbContextDomKultury context)
        {
            _context = context;
        }

        public bool CzySalaDostepna(int idSali, DateTime od, DateTime doKiedy)
        {
            return !_context.Rezerwacje.Any(r => r.IdSali == idSali &&
                ((od >= r.OdKiedy && od < r.DoKiedy) ||
                 (doKiedy > r.OdKiedy && doKiedy <= r.DoKiedy) ||
                 (od <= r.OdKiedy && doKiedy >= r.DoKiedy)));
        }

        public bool ZarezerwujSale(int idSali, int idUzytkownika, DateTime od, DateTime doKiedy, int okres, int cykle, out (DateTime start, DateTime end)? kolidujacyTermin)
        {
            kolidujacyTermin = null;
            var rezerwacje = new List<Rezerwacja>();

            for (int i = 0; i < cykle; i++)
            {
                var start = od.AddDays(i * okres);
                var end = doKiedy.AddDays(i * okres);

                if (!CzySalaDostepna(idSali, start, end))
                {
                    
                    var kolidujaca = _context.Rezerwacje.First(r => r.IdSali == idSali &&
                        ((start >= r.OdKiedy && start < r.DoKiedy) ||
                         (end > r.OdKiedy && end <= r.DoKiedy) ||
                         (start <= r.OdKiedy && end >= r.DoKiedy)));

                    kolidujacyTermin = (kolidujaca.OdKiedy, kolidujaca.DoKiedy);
                    return false;
                }

                rezerwacje.Add(new Rezerwacja
                {
                    IdSali = idSali,
                    IdUzytkownika = idUzytkownika,
                    OdKiedy = start,
                    DoKiedy = end
                });
            }

            _context.Rezerwacje.AddRange(rezerwacje);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Sala> PobierzDostepneSale(DateTime od, DateTime doKiedy)
        {
            var zajeteSaleIds = _context.Rezerwacje
                .Where(r => (od < r.DoKiedy && doKiedy > r.OdKiedy))
                .Select(r => r.IdSali)
                .Distinct()
                .ToList();

            return _context.Sale.Where(s => !zajeteSaleIds.Contains(s.Id)).ToList();
        }

        public IEnumerable<RezewacjeSaleDto> PobierzSaleRezerwacje()
        => _context.Rezerwacje.Include(r => r.Sala).Select(r => new RezewacjeSaleDto
        {
            CenaZaGodz_Wartosc = r.Sala.CenaZaGodz_Wartosc,
            Id = r.Id,
            IloscMiejsc = r.Sala.IloscMiejsc,
            Nazwa = r.Sala.Nazwa,
            OdKiedy = r.OdKiedy,
            Typ = r.Sala.Typ
        }).ToList();
    }
}

