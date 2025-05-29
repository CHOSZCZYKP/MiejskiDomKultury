using Microsoft.EntityFrameworkCore;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Dto;
using MiejskiDomKultury.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Repositories
{
    public class WypozyczalniaRepository : IWypozyczeniaRepository
    {
        private readonly DbContextDomKultury _dbContextDomKultury;

        public WypozyczalniaRepository(DbContextDomKultury dbContextDomKultury)
        {
            this._dbContextDomKultury = dbContextDomKultury;
        }

        public IEnumerable<WypozyczeniePrzedmiotuPrzezUzytkownika> GetAllWyozyczeniaWithUsersAndItems()
            => _dbContextDomKultury.Wypozyczenia
            .Include(w => w.Uzytkownik)
            .Include(w => w.Przedmiot)
            .Select(w => new WypozyczeniePrzedmiotuPrzezUzytkownika
            {
                Imie = w.Uzytkownik.Imie,
                Nazwisko = w.Uzytkownik.Nazwisko,
                Email = w.Uzytkownik.Email,
                DataWypozyczenia = w.DataWypozyczenia,
                DataZwrotu = w.DataZwrotu,
                NazwaPrzedmiotu = w.Przedmiot.Nazwa,
                Cena = w.Przedmiot.CenaZaDobe_Pelna
            }).ToList();
    }
}
