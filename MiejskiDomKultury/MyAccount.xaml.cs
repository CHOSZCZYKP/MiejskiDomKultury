using System.Collections.Generic;
using System.Windows.Controls;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Model;
using MiejskiDomKultury.Repositories;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury
{
    public partial class MyAccount : Page
    {
        private readonly TransakcjaRepository transakcjaRepository;
        private readonly MovieRepositoryService movieRepositoryService;

        public MyAccount()
        {
            InitializeComponent();
            transakcjaRepository = new TransakcjaRepository(new Data.DbContextDomKultury());
            movieRepositoryService = new MovieRepositoryService();
            LoadData();
            SetUserInfo();
        }

        private void LoadData()
        {
            List<SeansBilet> tickets = movieRepositoryService.GetBiletyByUzytkownik(Session.User.Id);
            List<Transakcja> transakcje = transakcjaRepository.GetTranskacjeByUzytkownik(Session.User.Id);

            TicketsList.ItemsSource = tickets;
            TransakcjeList.ItemsSource = transakcje;
        }

        private void SetUserInfo()
        {
            this.DataContext = new
            {
                UserName = Session.User.Imie + " " + Session.User.Nazwisko,
                Email = Session.User.Email
            };
        }
    }
}
