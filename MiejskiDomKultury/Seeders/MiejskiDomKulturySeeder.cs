using MiejskiDomKultury.Data;
using MiejskiDomKultury.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiejskiDomKultury.Seeders
{
    public class MiejskiDomKulturySeeder
    {
        private readonly DbContextDomKultury _dbContextDomKultury;

        public MiejskiDomKulturySeeder(DbContextDomKultury dbContextDomKultury)
        {
            this._dbContextDomKultury = dbContextDomKultury;
        }

        public async Task Seed()
        {
            if (await _dbContextDomKultury.Database.CanConnectAsync())
            {
                if (!_dbContextDomKultury.Uzytkownicy.Any())
                {
                    _dbContextDomKultury.Uzytkownicy.AddRange(
                        new Uzytkownik
                        {
                            Imie = "Admin",
                            Nazwisko = "Admin",
                            Email = "Admin@gmail.com",
                            NazwaUzytkownika = "Admin",
                            Rola = "Admin",
                            HasloHash = PasswordHasher.HashPassword("Qwerty12#"),
                            CzyMaBana = false
                        },
                        new Uzytkownik
                        {
                            Imie = "ImieTest1",
                            Nazwisko = "NazwiskoTest1",
                            Email = "EmailTest1",
                            NazwaUzytkownika = "NazwaUzytkownikaTest1",
                            Rola = "User",
                            HasloHash = PasswordHasher.HashPassword("Qwerty12#"),
                            CzyMaBana = false
                        }
                    );
                    await _dbContextDomKultury.SaveChangesAsync();
                }

                if (!_dbContextDomKultury.Sale.Any())
                {
                    _dbContextDomKultury.Sale.AddRange(
                        new Sala
                        {
                            Nazwa = "Sala widowiskowa",
                            IloscMiejsc = 300,
                            Typ = "Widowiskowa",
                            CenaZaGodz_Wartosc = 1000,
                            CenaZaGodz_Waluta = "PLN",
                        },
                        new Sala
                        {
                            Nazwa = "Sala muzyczna",
                            IloscMiejsc = 200,
                            Typ = "Muzyczna",
                            CenaZaGodz_Wartosc = 750,
                            CenaZaGodz_Waluta = "PLN",
                        },
                        new Sala
                        {
                            Nazwa = "Sala taneczna",
                            IloscMiejsc = 150,
                            Typ = "Taneczna",
                            CenaZaGodz_Wartosc = 500,
                            CenaZaGodz_Waluta = "PLN",
                        },
                        new Sala
                        {
                            Nazwa = "Sala plastyczna",
                            IloscMiejsc = 20,
                            Typ = "Plastyczna",
                            CenaZaGodz_Wartosc = 200,
                            CenaZaGodz_Waluta = "PLN",
                        },
                        new Sala
                        {
                            Nazwa = "Sala multimedialna",
                            IloscMiejsc = 30,
                            Typ = "Multimedialna",
                            CenaZaGodz_Wartosc = 1000,
                            CenaZaGodz_Waluta = "PLN",
                        },
                        new Sala
                        {
                            Nazwa = "Sala szkoleniowa",
                            IloscMiejsc = 100,
                            Typ = "Szkoleniowa",
                            CenaZaGodz_Wartosc = 600,
                            CenaZaGodz_Waluta = "PLN",
                        },
                        new Sala
                        {
                            Nazwa = "Sala zajęć indywidualnych",
                            IloscMiejsc = 2,
                            Typ = "Zajęcia indywidualne",
                            CenaZaGodz_Wartosc = 100,
                            CenaZaGodz_Waluta = "PLN",
                        }
                    );
                    await _dbContextDomKultury.SaveChangesAsync();
                }

                if (!_dbContextDomKultury.Przedmioty.Any())
                {
                    _dbContextDomKultury.Przedmioty.AddRange(
                        new Przedmiot
                        {
                            Nazwa = "Laptop Dell",
                            Stan = "Bardzo dobry",
                            Typ = "Laptop",
                            CenaZaDobe_Waluta = "PLN",
                            CenaZaDobe_Wartosc = 50,
                            Dostepnosc = true
                        },
                        new Przedmiot
                        {
                            Nazwa = "Prrojektor SONY",
                            Stan = "Dobry",
                            Typ = "Projektor",
                            CenaZaDobe_Waluta = "PLN",
                            CenaZaDobe_Wartosc = 30,
                            Dostepnosc = true
                        }
                    );
                    await _dbContextDomKultury.SaveChangesAsync();
                }

            }
        }
    }
}
