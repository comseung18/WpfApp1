using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    internal class Hotel
    {
        private readonly ReservationBook reservationBook;

        public string Name { get; }

        public Hotel(string name, ReservationBook reservationBook)
        {
            Name = name;
            this.reservationBook = reservationBook;
        }

        public async Task<IEnumerable<Reservation>> GetReservations()
        {
            return await reservationBook.GetReservations();
        }

        public async Task MakeReservation(Reservation reservation)
        {
            await reservationBook.AddReservation(reservation);
        }
    }
}
