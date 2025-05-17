using Microsoft.EntityFrameworkCore;
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
    public class PrzedmiotRepository : IPrzedmiotRepository
    {
        private readonly DbContextDomKultury _dbContextDomKultury;
        public PrzedmiotRepository(DbContextDomKultury dbContextDomKultury)
        {
            this._dbContextDomKultury = dbContextDomKultury;
        }

        public async Task AddNewPrzedmiot(Przedmiot przedmiot)
        {
            _dbContextDomKultury.Add(przedmiot);
            await _dbContextDomKultury.SaveChangesAsync();
        }

        public async Task Commit()
            => await _dbContextDomKultury.SaveChangesAsync();

        public async Task EditPrzedmiot(Przedmiot przedmiot)
        {
            var istniejacy = await _dbContextDomKultury.Przedmioty.FirstOrDefaultAsync(p => p.Id == przedmiot.Id);

            if (istniejacy != null)
            {
                istniejacy.Nazwa = przedmiot.Nazwa;
                istniejacy.Stan = przedmiot.Stan;
                istniejacy.Typ = przedmiot.Typ;
                istniejacy.CenaZaDobe_Waluta = przedmiot.CenaZaDobe_Waluta;
                istniejacy.CenaZaDobe_Wartosc = przedmiot.CenaZaDobe_Wartosc;
                istniejacy.Dostepnosc = przedmiot.Dostepnosc;

                await _dbContextDomKultury.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Przedmiot>> GetAllPrzedmioty()
            => await _dbContextDomKultury.Przedmioty.ToListAsync();

        public async Task RemovePrzedmiot(Przedmiot przedmiot)
        {
            _dbContextDomKultury.Przedmioty.Remove(przedmiot);
            await _dbContextDomKultury.SaveChangesAsync();
        }
    }
}
