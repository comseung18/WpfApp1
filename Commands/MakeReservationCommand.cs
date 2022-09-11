using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.Stores;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    internal class MakeReservationCommand : AsyncCommandBase
    {
        private readonly NavigationService reservaionViewNavigationService;
        private readonly MakeReservationViewModel makeReservationViewModel;
        private readonly HotelStore hotelStore;

        public override bool CanExecute(object parameter)
        {

            return !string.IsNullOrEmpty( makeReservationViewModel.Username) &&  
               makeReservationViewModel.FloorNumber > 0 &&
               base.CanExecute(parameter);
        }

        public MakeReservationCommand(MakeReservationViewModel makeReservationViewModel,
            HotelStore hotelStore,
            NavigationService reservaionViewNavigationService)
        {
            this.makeReservationViewModel = makeReservationViewModel;
            this.hotelStore = hotelStore;
            this.reservaionViewNavigationService = reservaionViewNavigationService;
            makeReservationViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override async Task ExcuteAsync(object parameter)
        {
            Reservation reservation = new Reservation(
                new RoomID(makeReservationViewModel.FloorNumber, makeReservationViewModel.RoomNumber),
                makeReservationViewModel.Username,
                makeReservationViewModel.StartDate,
                makeReservationViewModel.EndDate
                );

            try
            {
                await hotelStore.MakeReservation(reservation);

                MessageBox.Show("Successfully reserved room.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                reservaionViewNavigationService.Navigate();
            }
            catch(ReservationConflictException)
            {
                MessageBox.Show("This room is already taken.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception)
            {
                MessageBox.Show("Failed to make reservation.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(MakeReservationViewModel.Username) ||
                e.PropertyName == nameof(MakeReservationViewModel.FloorNumber))
            {
                OnCanExcutedChanged();
            }
        }
    }
}
