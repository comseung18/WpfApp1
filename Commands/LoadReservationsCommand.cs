﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Stores;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    internal class LoadReservationsCommand : AsyncCommandBase
    {
        private readonly HotelStore hotelStore;
        private readonly ReservationListingViewModel viewModel;

        public LoadReservationsCommand(HotelStore hotelStore, ReservationListingViewModel viewModel)
        {
            this.hotelStore = hotelStore;
            this.viewModel = viewModel;
        }

        public override async Task ExcuteAsync(object parameter)
        {
            try
            {
                await hotelStore.Load();
                viewModel.UpdateReservations(hotelStore.Reservations);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load reservations.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
