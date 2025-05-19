using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Views.Administrator;

namespace MiejskiDomKultury.Repositories
{
    public interface INewsRepository
    {
        List<Ogloszenie> GetLastNews();
        void AddNews(Ogloszenie ogloszenie);
    }

    
}
