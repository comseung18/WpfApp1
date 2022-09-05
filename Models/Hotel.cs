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

        public Hotel(string name)
        {
            Name = name;
            reservationBook = new ReservationBook();
        }

        public IEnumerable<Reservation> GetReservationsForUser(string username)
        {
            return reservationBook.GetReservationsForUser(username);
        }

        public void MakeReservation(Reservation reservation)
        {
            reservationBook.AddReservation(reservation);
        }
    }
}
