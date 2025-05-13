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
        private MovieService _service;
        public MovieRepositoryService()
        {
            _service = new MovieService();
            _context = new DbContextDomKultury();
        }

        public void AddFilm(Film film)
        {
          
           
            _context.Filmy.Add(film);
            
        }

        public void AddSeans(Seans seans)
        {
            _context.Seanse.Add(seans);
            _context.SaveChanges();
        }

        public List<Film> GetAvailableMovies()
        {
           

        
            return _context.Seanse.Where(p => p.DataStart > DateTime.UtcNow).Select(i => i.Film).Distinct().ToList();
           
        }



        public List<int> GetFreeSeats(int seansId)
        {
            var taken=_context.Bilety.Where(s=> s.SeansId == seansId).Select(x=>x.SeatNumber).ToList();
            List<int> freeSeats = new List<int>();
            for(int i = 1; i < 41; i++)
            {
                if (!taken.Contains(i))
                {
                    freeSeats.Add(i);
                }
            }
            return freeSeats;
        }

        public async Task<Film> GetMovieDetailsFromApi(string title, int year)
        {
           return await _service.GetMovieDetails(title, year);
        }

        public async Task<List<Film>> GetMoviesByTitleFromApi(string title)
        {
           return await _service.GetMoviesFromApi(title);
        }

        public List<DateTime> GetMovieShowDates(int movieId)
        {
           // return new List<DateTime> {DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), DateTime.Now.AddDays(3) };
            return _context.Seanse.Where(p => p.DataStart > DateTime.Now && p.FilmId==movieId).Select(p => p.DataStart).ToList();
        }

        public Seans GetSeans(DateTime date, int movieId)
        {
             return _context.Seanse.FirstOrDefault(p=>p.DataStart == date && p.FilmId==movieId);
           
        }

        public List<int> GetTakenSeats(int seansId)
        {
            return _context.Bilety.Where(p=>p.SeansId==seansId).Select(s=>s.SeatNumber).ToList();
        }
    }
}
