using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using MiejskiDomKultury.Views.Administrator;

namespace MiejskiDomKultury.Services
{
    public class NewsService : INewsRepository
    {
        DbContextDomKultury _context;

        public NewsService()
        {
            _context = new DbContextDomKultury();
        }

        public void AddNews(Ogloszenie ogloszenie)
        {
            _context.Ogloszenia.Add(ogloszenie);
            _context.SaveChanges();
        }

        public List<Ogloszenie> GetLastNews()
        {
            return _context.Ogloszenia
        .OrderByDescending(o => o.CreatedAt)
        .Take(5)
        .AsNoTracking()
        .ToList();
        }
    }
}
