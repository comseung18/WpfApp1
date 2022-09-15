using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Commands;
using WpfApp1.Models;
using WpfApp1.Stores;

namespace WpfApp1.ViewModels
{
    internal class MakeReservationViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private int floorNumber;
        public int FloorNumber
        {
            get
            {
                return floorNumber;
            }
            set
            {
                floorNumber = value;
                OnPropertyChanged(nameof(FloorNumber));
            }
        }

        private int roomNumber;
        public int RoomNumber
        {
            get
            {
                return roomNumber;
            }
            set
            {
                roomNumber = value;
                OnPropertyChanged(nameof(RoomNumber));
            }
        }

        private DateTime startDate = DateTime.Now;
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));

                ClearErrors(nameof(StartDate));
                ClearErrors(nameof(EndDate));
                if (EndDate < StartDate)
                {
                    AddError("The start date cannot be after the end date.", nameof(StartDate));
                }
            }
        }

        private DateTime endDate = DateTime.Now;

        
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
                ClearErrors(nameof(StartDate));
                ClearErrors(nameof(EndDate));
                if (EndDate < StartDate)
                {
                    AddError("The end date cannot be before the start date.", nameof(EndDate));
                }

            }
        }

        private void AddError(string errorMessage, string propertyName)
        {
            if (!propertyNameToErrorDictionary.ContainsKey(propertyName))
            {
                propertyNameToErrorDictionary.Add(propertyName, new List<string>());
            }

            propertyNameToErrorDictionary[propertyName].Add(errorMessage);
            OnErrosChanged(nameof(propertyName));
        }

        private void OnErrosChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(propertyName)));
        }

        private void ClearErrors(string propertyName)
        {
            propertyNameToErrorDictionary.Remove(propertyName);
            OnErrosChanged(nameof(propertyName));
        }

        public ICommand SubmitCommand { get; }

        public ICommand CancelCommand { get; }

        private readonly Dictionary<string, List<string>> propertyNameToErrorDictionary;


        public MakeReservationViewModel(HotelStore hotelStore, Services.NavigationService<ReservationListingViewModel> reservaionViewNavigationService)
        {
            SubmitCommand = new MakeReservationCommand(this, hotelStore, reservaionViewNavigationService);
            CancelCommand = new NavigateCommand<ReservationListingViewModel>(reservaionViewNavigationService);
            this.propertyNameToErrorDictionary = new Dictionary<string, List<string>>();
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => propertyNameToErrorDictionary.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            return propertyNameToErrorDictionary.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<string>();
        }
    }
}
