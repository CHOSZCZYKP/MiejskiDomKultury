using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
           var lista =  _context.Ogloszenia
        .OrderByDescending(o => o.CreatedAt)
        .Take(2)
        .AsNoTracking()
        .ToList();






            Ogloszenie o1 = new Ogloszenie
            {
                Title = "Koniec świata w Ostrołęce",
                Content = "Astrolodzy przewidują opad meteorytów dzisiaj o 21:00!",
                CreatedAt = DateTime.Now,
                ImageData = LoadImageBytes("Assets/news1.webp")
            };

            Ogloszenie o2 = new Ogloszenie
            {
                Title = "Dramat w Ostrołęce",
                Content = "Kobieta poszukiwana za zdemolowanie sali tanecznej!",
                CreatedAt = DateTime.Now,
                ImageData = LoadImageBytes("Assets/news2.webp")
            };

            Ogloszenie o3 = new Ogloszenie
            {
                Title = "Bober zdech",
                Content = "Ostrołęka pogrążona w żałobie. Zmarł Bóbr Bartek",
                CreatedAt = DateTime.Now,
                ImageData = LoadImageBytes("Assets/news3.webp")
            };

            lista.Add(o1);
            lista.Add(o2);
            lista.Add(o3);
            return lista;
        }
        private byte[] LoadImageBytes(string relativePath)
        {
            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                return File.ReadAllBytes(fullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania obrazka: {ex.Message}");
                return new byte[0];
            }
        }
    }

}
