using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.DbContexts;
using WpfApp1.Models;
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
        private readonly Hotel hotel;
        private readonly NavigationStore navigationStore;
        private readonly ReservoomDbContextFactory dbContextFactory;
        private readonly HotelStore hotelStore;

        public App()
        {

            dbContextFactory = new ReservoomDbContextFactory(ConnectionString);
            Services.ReservationConflictValidators.IReservationConflicValidator reservationConflicValidator = new DatabaseReservationConflictValidator(dbContextFactory);
            Services.ReservationCreaters.IReservationCreator reservationCreator = new DatabaseReservationCreator(dbContextFactory);
            Services.ReservationProvides.IReservationProvider reservationProvider = new DatabaseReservationProvider(dbContextFactory);
            ReservationBook reservationBook = new ReservationBook(reservationProvider, reservationCreator, reservationConflicValidator);
            hotel = new Hotel("Comseung Suite", reservationBook);
            hotelStore = new HotelStore(hotel);
            navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            using (ReservoomDbContext dbContext = dbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }

                
            navigationStore.CurrentViewModel = CreateReservationListingViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private MakeReservationViewModel CreateMakeReservationViewModel()
        {
            return new MakeReservationViewModel(hotelStore, new Services.NavigationService(navigationStore, CreateReservationListingViewModel));
        }

        private ReservationListingViewModel CreateReservationListingViewModel()
        {
            return ReservationListingViewModel.LoadViewModel(hotelStore, new Services.NavigationService(navigationStore, CreateMakeReservationViewModel));
        }
    }
}
