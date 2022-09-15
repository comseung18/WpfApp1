using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.DbContexts;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.Services.ReservationConflictValidators;
using WpfApp1.Services.ReservationCreaters;
using WpfApp1.Services.ReservationProvides;
using WpfApp1.Stores;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private const string ConnectionString = "Data Source=reservoom.db";
        private readonly IHost host;

        public App()
        {
            host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton(new ReservoomDbContextFactory(ConnectionString));
                services.AddSingleton<IReservationProvider, DatabaseReservationProvider>();
                services.AddSingleton<IReservationCreator, DatabaseReservationCreator>();
                services.AddSingleton<IReservationConflicValidator, DatabaseReservationConflictValidator>();

                services.AddTransient<ReservationBook>();
                services.AddSingleton<Hotel>((s) => new Hotel("Comseung Suite", s.GetService<ReservationBook>()));

                
                services.AddSingleton<NavigationService<MakeReservationViewModel>>();
                services.AddSingleton<NavigationService<ReservationListingViewModel>>();

                services.AddTransient<MakeReservationViewModel>();
                services.AddSingleton<Func<MakeReservationViewModel>>((s) => () => s.GetRequiredService<MakeReservationViewModel>());

                services.AddTransient<ReservationListingViewModel>((s)=> CreateReservationListingViewModel(s));
                services.AddSingleton<Func<ReservationListingViewModel>>((s) => () => s.GetRequiredService<ReservationListingViewModel>());

                services.AddSingleton<HotelStore>();
                services.AddSingleton<NavigationStore>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
            }).Build();
        }

        private ReservationListingViewModel CreateReservationListingViewModel(IServiceProvider s)
        {
            return ReservationListingViewModel.LoadViewModel(
                s.GetRequiredService<HotelStore>(),
                s.GetRequiredService<NavigationService<MakeReservationViewModel>>()
                );
        }

        protected override void OnExit(ExitEventArgs e)
        {
            host.Dispose();
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            host.Start();

            ReservoomDbContextFactory reservoomDbContextFactory = host.Services.GetService<ReservoomDbContextFactory>();
            using (ReservoomDbContext dbContext = reservoomDbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }

            NavigationService<ReservationListingViewModel> navigationService = host.Services.GetRequiredService<NavigationService<ReservationListingViewModel>>();
            navigationService.Navigate();

            MainWindow = host.Services.GetService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        
    }
}
