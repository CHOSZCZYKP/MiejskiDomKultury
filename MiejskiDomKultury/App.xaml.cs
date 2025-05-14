using System.Configuration;
using System.Data;
using System.Windows;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Data;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Seeders;
using MiejskiDomKultury.Services;
using MiejskiDomKultury.Views.Administrator;

namespace MiejskiDomKultury
{
    
    public partial class App : Application
    {
        public App()
        {
            Env.Load("SecretFile.env");
        }
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
            

            base.OnStartup(e);

            using var context = ServiceProvider.GetRequiredService<DbContextDomKultury>();
            var seeder = new MiejskiDomKulturySeeder(context);

            await seeder.Seed();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepositoryService>();

            // tu trzeba wrzucić widoki, które wykorzystują DI, jeśli nie wykorzystują to też można xdd
            services.AddTransient<Logowanie>();
            services.AddTransient<Home>();
            services.AddTransient<Rejestracja>();
            services.AddTransient<PanelAdmina>();
            services.AddTransient<TabelaUzytkownicyAdmin>();
            services.AddTransient<TabelaBanyAdmin>();
            services.AddTransient<TabelaSaleAdmin>();
            services.AddTransient<TabelaWypozyczeniaAdmin>();
            services.AddTransient<TabelaRezerwacjeAdmin>();
            services.AddTransient<TabelaTransakcjeAdmin>();
            services.AddTransient<TabelaPrzedmiotyAdmin>();
            services.AddTransient<WykresyStatystyk>();
            services.AddTransient<WypozyczaniePrzedmiotow>();
            services.AddTransient<Logo>();
            services.AddTransient<AvailableMovies>();
            services.AddTransient<AddMovie>();

            services.AddDbContext<DbContextDomKultury>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("Database_KEY"));
            });
        }
    }

}
