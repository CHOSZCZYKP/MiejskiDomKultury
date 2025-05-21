using Microsoft.EntityFrameworkCore;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DbContextDomKultury _dbContextDom;

        public SaleRepository(DbContextDomKultury dbContextDom)
        {
            this._dbContextDom = dbContextDom;
        }

        public IEnumerable<Sala> GetAllSale()
            => _dbContextDom.Sale.ToList();

        //zwraca sale ktore nie są zarezerwowane w danym dniu
        public IEnumerable<Sala> GetAvailableAtDay(DateOnly date)
        {
          return  _dbContextDom.Rezerwacje
       .Where(a => DateOnly.FromDateTime(a.OdKiedy) != date || DateOnly.FromDateTime(a.DoKiedy) != date)
       .Select(s => s.Sala)
       .ToList();
        }

        public bool IsSalaFreeByHourToHour(DateTime start, DateTime end,string name)
        {
            var rezerwacje = _dbContextDom.Sale
            .Where(s => s.Nazwa == name)
            .Select(s => s.Rezerwacje)
            .FirstOrDefault();

            foreach(var rez in rezerwacje)
            {
               if(rez.DoKiedy>start && rez.DoKiedy<end || (rez.OdKiedy >= start && rez.DoKiedy <=end) || (rez.OdKiedy>start && rez.OdKiedy<end))
                {
                    return false;
                }
            }



            return true;
        }
    }
}
