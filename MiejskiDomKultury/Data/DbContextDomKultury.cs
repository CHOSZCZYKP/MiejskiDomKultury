using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiejskiDomKultury.Data
{
    public class DbContextDomKultury : DbContext
    {
        static DbContextDomKultury()
        {
            Env.Load(Path.Combine(AppContext.BaseDirectory, "SecretFile.env"));
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MiejskiDomKultury;Trusted_Connection=True;");
            //optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("Database_KEY"));
        }

        public DbSet<Ban> Bany { get; set; }
        public DbSet<Przedmiot> Przedmioty { get; set; }
        public DbSet<Rezerwacja> Rezerwacje { get; set; }
        public DbSet<Sala> Sale { get; set; }
        public DbSet<Transakcja> Transakcje { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Wypozyczenie> Wypozyczenia { get; set; }

        

        public DbSet<Ogloszenie> Ogloszenia { get; set; }
        public DbSet<Film> Filmy { get; set; }
        public DbSet<Seans> Seanse { get; set; }
        public DbSet<SeansBilet> Bilety { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ban>(mb =>
            {
                mb.HasKey(b => b.Id);
                mb.HasOne(b => b.Uzytkownik)
                    .WithMany(u => u.Bany)
                    .HasForeignKey(b => b.IdUzytkownika);
            });

            modelBuilder.Entity<Przedmiot>(mb =>
            {
                mb.HasKey(p => p.Id);
                mb.HasMany(p => p.Wypozyczenia)
                    .WithOne(w => w.Przedmiot)
                    .HasForeignKey(p => p.IdPrzedmiotu);
            });

            modelBuilder.Entity<Wypozyczenie>(mb =>
            {
                mb.HasKey(w => w.Id);
                mb.HasOne(w => w.Uzytkownik)
                    .WithMany(u => u.Wypozyczenia)
                    .HasForeignKey(w => w.IdUzytkownika);
            });

            modelBuilder.Entity<Transakcja>(mb =>
            {
                mb.HasKey(t => t.Id);
                mb.HasOne(t => t.Uzytkownik)
                    .WithMany(u => u.Transakcje)
                    .HasForeignKey(t => t.IdUzytkownika);
            });

            modelBuilder.Entity<Rezerwacja>(mb =>
            {
                mb.HasKey(r => r.Id);
                mb.HasOne(r => r.Uzytkownik)
                    .WithMany(u => u.Rezerwacje)
                    .HasForeignKey(r => r.IdUzytkownika);

                mb.HasOne(r => r.Sala)
                    .WithMany(s => s.Rezerwacje)
                    .HasForeignKey(r => r.IdSali);
            });
        }
    }
}
