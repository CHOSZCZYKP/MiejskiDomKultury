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
        public List<Film> GetAvailableMovies()
        {
            Film film1 = new Film
            {
                Aktorzy = new List<string> { "Malcolm McDowell", "Patrick Magee", "Michael Bates" },
                Opis = "Alex DeLarge wraz ze swoim gangiem sieje spustoszenie na ulicach. Kiedy trafia do więzienia, otrzymuje propozycję odmiany swojego życia.",
                Gatunki = new List<string> { "Crime", "Drama", "Sci-Fi" },
                Tytul = "Mechaniczna pomarańcza",
                PlakatURL = "https://i.ebayimg.com/00/s/MTYwMFgxMDYw/z/arkAAOSwNsdXR516/$_57.JPG",
                Rok = 1971
            };

            Film film2 = new Film
            {
                Aktorzy = new List<string> { "Keir Dullea", "Gary Lockwood", "Douglas Rain" },
                Opis = "\"2001: Odyseja kosmiczna\" to wyprawa w krainę jutra, mapa ludzkiego przeznaczenia, droga do nieskończoności i fascynująca opowieść o starciu człowieka z maszyną - arcydzieło sztuki filmowej, które wyróżniono Oscarem za efekty specjalne.",
                Gatunki = new List<string> { "Adventure", "Drama", "Sci-Fi" },
                PlakatURL = "https://m.media-amazon.com/images/M/MV5BNjU0NDFkMTQtZWY5OS00MmZhLTg3Y2QtZmJhMzMzMWYyYjc2XkEyXkFqcGc@._V1_SX300.jpg",
                Tytul = "2001: Odyseja kosmiczna",
                Rok = 1968
            };

            Film film3 = new Film
            {
                Aktorzy = new List<string> { "Jack Nicholson", "Shelley Duvall", "Danny Lloyd" },
                Opis = "Pisarka, jego żona i ich syn wprowadzają się do odciętego od świata hotelu, w którym zaczynają dziać się niepokojące rzeczy.",
                Gatunki = new List<string> { "Horror", "Drama", "Mystery" },
                PlakatURL = "https://m.media-amazon.com/images/M/MV5BZTQ1ODJkY2ItYzNlZS00ZDM1LThkY2MtOTU2ZjdmZTlmZjk0XkEyXkFqcGc@._V1_SX300.jpg",
                Tytul = "Lśnienie",
                Rok = 1980
            };

            Film film4 = new Film
            {
                Aktorzy = new List<string> { "Matthew Modine", "R. Lee Ermey", "Vincent D'Onofrio" },
                Opis = "Historia rekrutów szkolących się do wojny w Wietnamie oraz ich brutalnej rzeczywistości na froncie.",
                Gatunki = new List<string> { "Drama", "War" },
                PlakatURL = "https://m.media-amazon.com/images/M/MV5BYWQ2YjA3MTMtZTg1MC00Y2M5LThmMzAtOGYwYmNlMjg5YjA2XkEyXkFqcGc@._V1_SX300.jpg",
                Tytul = "Full Metal Jacket",
                Rok = 1987
            };

        

            return new List<Film> { film1, film2, film3, film4};
        }



        public List<int> GetFreeSeats(int seansId)
        {
            return new List<int> { 1, 5, 12, 20, 21, 23, 30 };
        }

        public List<DateTime> GetMovieShowDates(int movieId)
        {
            return new List<DateTime> {DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), DateTime.Now.AddDays(3) };
           // return _context.Seanse.Where(p => p.DateStart < DateTime.Now).Select(p => p.DateStart).ToList();
        }

        public Seans GetSeans(DateTime date, int movieId)
        {
            // return _context.Seanse.FirstOrDefault(p=>p.DataStart == date && p.FilmId==movieId);
            return new Seans
            {
                DataStart = DateTime.Now,
                Czas = 120,
                Film = new Film
                {
                    Aktorzy = new List<string> { "Keir Dullea", "Gary Lockwood", "Douglas Rain" },
                    Opis = "\"2001: Odyseja kosmiczna\" to wyprawa w krainę jutra, mapa ludzkiego przeznaczenia, droga do nieskończoności i fascynująca opowieść o starciu człowieka z maszyną - arcydzieło sztuki filmowej, które wyróżniono Oscarem za efekty specjalne.",
                    Gatunki = new List<string> { "Adventure", "Drama", "Sci-Fi" },
                    PlakatURL = "https://m.media-amazon.com/images/M/MV5BNjU0NDFkMTQtZWY5OS00MmZhLTg3Y2QtZmJhMzMzMWYyYjc2XkEyXkFqcGc@._V1_SX300.jpg",
                    Tytul = "2001: Odyseja kosmiczna",
                    Rok = 1968
                },
                Id = 21
            };
        }

        public List<int> GetTakenSeats(int seansId)
        {
            return _context.Bilety.Where(p=>p.SeansId==seansId).Select(s=>s.SeatNumber).ToList();
        }
    }
}
