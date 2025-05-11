using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;

namespace MiejskiDomKultury.Services
{
    internal class MovieRepositoryService : IMovieRepository
    {

        private readonly DbContextDomKultury _context;

        public MovieRepositoryService()
        {
            _context = new DbContextDomKultury();
        }
        public List<Movie> GetAvailableMovies()
        {
            Movie film = new Movie
            {
                ActorsNames = new List<string> { "Malcolm McDowell", "Patrick Magee", "Michael Bates" },
                Description = "Alex DeLarge wraz ze swoim gangiem sieje spustoszenie na ulicach. Kiedy trafia do więzienia, otrzymuje propozycję odmiany swojego życia.",
                Kinds = new List<string> { "Crime", "Drama", "Sci-Fi" },
                Title = "Mechaniczna pomarańcza",
                Year = 1971
            };

            Movie film2 = new Movie
            {
                ActorsNames = new List<string> { "Keir Dullea", "Gary Lockwood", "Douglas Rain" },
                Description = "\"2001: Odyseja kosmiczna\" to wyprawa w krainę jutra, mapa ludzkiego przeznaczenia, droga do nieskończoności i fascynująca opowieść o starciu człowieka z maszyną - arcydzieło sztuki filmowej, które wyróżniono Oscarem za efekty specjalne.",
                Kinds = new List<string> { "Adventure", "Drama", "Sci-Fi" },
                Title = "2001: Odyseja kosmiczna",
                Year = 1968
            };

            return new List<Movie> { film, film2 };
        }

        public List<int> GetTakenSeats(int seansId)
        {
            return _context.Bilety.Where(p=>p.SeansId==seansId).Select(s=>s.SeatNumber).ToList();
        }
    }
}
