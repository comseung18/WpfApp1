using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Stores
{
    internal class HotelStore
    {
        private readonly List<Reservation> reservations;
        private readonly Hotel hotel;
        private readonly Lazy<Task> initializeLazy;

        public HotelStore(Hotel hotel)
        {
            this.reservations = new List<Reservation>();
            this.hotel = hotel;
            initializeLazy = new Lazy<Task>(Initialize);
        }

        public IEnumerable<Reservation> Reservations => reservations;

        public async Task Load()
        {
            await initializeLazy.Value;


        }

        public async Task MakeReservation(Reservation reservation)
        {
            await hotel.MakeReservation(reservation);

            reservations.Add(reservation);
        }

        private async Task Initialize()
        {
            IEnumerable<Reservation> reservations = await hotel.GetReservations();

            this.reservations.Clear();
            this.reservations.AddRange(reservations);
        }
    }
}
