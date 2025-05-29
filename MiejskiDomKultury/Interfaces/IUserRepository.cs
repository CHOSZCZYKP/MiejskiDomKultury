using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiejskiDomKultury.Model;

namespace MiejskiDomKultury.Interfaces
{
     public interface IUserRepository
    {
        public bool DoesUserExist(string email);

        public Uzytkownik GetUserByEmail(string email);

        public void AddNewUser(Uzytkownik user);

        IEnumerable<Uzytkownik> GetAllUsers();
        void UpdateUser(Uzytkownik uzytkownik);
    }
}
