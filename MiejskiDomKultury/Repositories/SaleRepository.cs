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

    }
}
