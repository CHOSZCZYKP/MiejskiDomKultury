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


        // idk czy to zadziala
        public Dictionary<Sala, string> GetAvailableAtDay(DateOnly date)
        {

            TimeSpan start = TimeSpan.Parse("8:00");
            TimeSpan end = TimeSpan.Parse("22:00");
            var  reserverations=_dbContextDom.Rezerwacje.Include(a=>a.Sala).Where(p=>DateOnly.FromDateTime(p.Data) ==date).ToList();
            
            var sale = reserverations.Select(p=>p.Sala).Distinct().ToList();
           
            Dictionary<Sala,string> result = new Dictionary<Sala, string>();

            foreach(var sala in sale)
            {
                string freeHours="";
                TimeSpan lastHour =start;
                int i = 0;
                var reservationRoom = sala.Rezerwacje.OrderBy(a => a.GodzinaPoczatkowa);
                foreach (var res in reservationRoom) {
                    i++;
                    if (res.GodzinaPoczatkowa > start)
                    {
                        freeHours =$"{start} do {res.GodzinaPoczatkowa}";
                        lastHour=res.GodzinaPoczatkowa;
                    }

                    if (lastHour < res.GodzinaPoczatkowa) {
                        freeHours +=$"{lastHour} do {res.GodzinaPoczatkowa}";
                        lastHour = res.GodzinaPoczatkowa;
                    }
                    
                    if(res.GodzinaKoncowa < end && i==reservationRoom.Count()-1)
                    {
                        freeHours += $"{res.GodzinaKoncowa} do {end}";
                    }
                
                    result.Add(sala,freeHours);
                }

            }
            return result;
        }

        //tu tez nie wiem
        public bool IsSalaFreeByHourToHour(DateTime start, DateTime end, string name)
        {
            var reservation = _dbContextDom.Sale
                .Where(s => s.Nazwa == name)
                .SelectMany(s => s.Rezerwacje)
                .ToList();

            foreach (var rez in reservation)
            {
                var startRes = rez.Data.Date + rez.GodzinaPoczatkowa;
                var endRes = rez.Data.Date + rez.GodzinaKoncowa;

                
                bool colision =
                    (start < endRes && end > startRes);

                if (colision)
                    return false;
            }

            return true;
        }

    }
}

