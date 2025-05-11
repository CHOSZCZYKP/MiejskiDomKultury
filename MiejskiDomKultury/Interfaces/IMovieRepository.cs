using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiejskiDomKultury.Model;

namespace MiejskiDomKultury.Interfaces
{
     interface IMovieRepository
    {
         List<int> GetTakenSeats(int seansId);

         List<Movie> GetAvailableMovies();

    }
}
