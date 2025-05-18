using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Interfaces
{
    public interface IPrzedmiotRepository
    {
        Task AddNewPrzedmiot(Przedmiot przedmiot);
        Task RemovePrzedmiot(Przedmiot przedmiot);
        IEnumerable<Przedmiot> GetAllPrzedmioty();
        Task EditPrzedmiot(Przedmiot przedmiot);
        Task Commit();
    }
}
