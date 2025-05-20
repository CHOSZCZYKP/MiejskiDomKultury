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


        IEnumerable<Sala> GetAvailableAtDay(DateOnly date);
    }
}
