using MiejskiDomKultury.Dto;
using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Interfaces
{
    public interface IRezerwacja
    {
        bool CzySalaDostepna(int idSali, DateTime od, DateTime doKiedy);
        bool ZarezerwujSale(int idSali, int idUzytkownika, DateTime od, DateTime doKiedy, int okres, int cykle, out (DateTime start, DateTime end)? kolidujacyTermin);
        IEnumerable<Sala> PobierzDostepneSale(DateTime od, DateTime doKiedy);
        IEnumerable<RezewacjeSaleDto> PobierzSaleRezerwacje();

    }
}
