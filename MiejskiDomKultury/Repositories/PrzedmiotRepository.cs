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

        public Task EditPrzedmiot(Przedmiot przedmiot)
        {
            throw new NotImplementedException();
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
