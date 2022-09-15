using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Commands;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.Stores;

namespace WpfApp1.ViewModels
{
    internal class ReservationListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ReservationViewModel> _reservations;
        private readonly HotelStore hotelStore;

        private bool isLoading;

        public bool IsLoading
        {
            get 
            { 
                return isLoading;
            }
            set 
            { 
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set 
            { 
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);



        public ICommand MakeReservationCommand { get; }
        public ICommand LoadReservationsCommand { get; }

        public IEnumerable<ReservationViewModel> Reservations => _reservations;


        public ReservationListingViewModel(HotelStore hotelStore, Services.NavigationService<MakeReservationViewModel> makeReservationNavigationService)
        {
            _reservations = new ObservableCollection<ReservationViewModel>();

            MakeReservationCommand = new NavigateCommand<MakeReservationViewModel>(makeReservationNavigationService);
            LoadReservationsCommand = new LoadReservationsCommand(hotelStore, this);
            

            this.hotelStore = hotelStore;
            this.hotelStore.ReservationMade += OnReservationMade;
        }

        private void OnReservationMade(Reservation reservation)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
            _reservations.Add(reservationViewModel);

        }

        public static ReservationListingViewModel LoadViewModel(HotelStore hotelStore, Services.NavigationService<MakeReservationViewModel> makeReservationNavigationService )
        {
            ReservationListingViewModel viewModel = new ReservationListingViewModel(hotelStore, makeReservationNavigationService);

            viewModel.LoadReservationsCommand.Execute(null);

            return viewModel;
        }

        public void UpdateReservations(IEnumerable<Reservation> reservations)
        {
            _reservations.Clear();

            foreach(Reservation reservation in reservations)
            {
                ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
                _reservations.Add(reservationViewModel);
            }
        }

        public override void Dispose()
        {
            this.hotelStore.ReservationMade -= OnReservationMade;
            base.Dispose();
        }
    }
}
