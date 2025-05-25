using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Interfaces
{
    public interface ISaleRepository
    {
        IEnumerable<Sala> GetAllSale();


        Dictionary<Sala, string> GetAvailableAtDay(DateOnly date);

        bool IsSalaFreeByHourToHour(DateTime start, DateTime end, string name);
    }
}
