using MiejskiDomKultury.Dto;
using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Interfaces
{
    public interface ITranskacjaRepository
    {
        IEnumerable<Transakcja> GetAllTransakcja();
        IEnumerable<TransakcjaZUzytkownikiemDto> GetAllTransakcjeZUzytkownikami();

        List<Transakcja> GetTranskacjeByUzytkownik(int id);
        Dictionary<string, int> GetAllTransakcjeGroupTyp();

        void AddTransakcja(Transakcja t);
    }
}
