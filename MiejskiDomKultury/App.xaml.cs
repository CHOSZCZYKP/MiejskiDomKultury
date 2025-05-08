using System.Configuration;
using System.Data;
using System.Windows;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using MiejskiDomKultury.Interfaces;
using MiejskiDomKultury.Services;

namespace MiejskiDomKultury
{
    
    public partial class App : Application
    {
        public App()
        {
            Env.Load("SecretFile.env");
        }
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();


            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepositoryService>();

            // tu trzeba wrzucić widoki, które wykorzystują DI, jeśli nie wykorzystują to też można xdd
            services.AddTransient<Logowanie>();
            services.AddTransient<Home>();
            services.AddTransient<Rejestracja>();
            services.AddTransient<Logo>();

        }
    }

}
