﻿using System;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Model;


namespace MiejskiDomKultury.Services
{
    internal class UserRepositoryService : IUserRepository
    {
        private readonly DbContextDomKultury _context;

        public UserRepositoryService()
        {
            _context = new DbContextDomKultury();
        }

        public void AddNewUser(Uzytkownik user)
        {
            _context.Uzytkownicy.Add(user);
            _context.SaveChanges();
        }

        public bool DoesUserExist(string email)
        {
           return _context.Uzytkownicy.Any(x=>x.Email == email);
        }

        public Uzytkownik GetUserByEmail(string email)
        {
            return _context.Uzytkownicy.FirstOrDefault(x=>x.Email == email);
        }
    }
}
