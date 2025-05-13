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

         List<Film> GetAvailableMovies();

        List<DateTime> GetMovieShowDates(int movieId);

        List<int> GetFreeSeats(int seansId);

        Seans GetSeans(DateTime date, int movieId);

        void AddFilm(Film film);

        Task<Film> GetMovieDetailsFromApi(string title, int year);

        Task<List<Film>> GetMoviesByTitleFromApi(string title);

        void AddSeans(Seans seans);

    }
}
