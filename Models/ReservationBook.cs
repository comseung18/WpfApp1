using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    internal class ReservationBook
    {
        private readonly List<Reservation> reservations;

        public ReservationBook()
        {
            this.reservations = new List<Reservation>();
        }

        public IEnumerable<Reservation> GetReservationsForUser(string username)
        {
            return reservations.Where(x => x.Username == username);
        }

        public void AddReservation(Reservation reservation)
        {
            foreach(Reservation existingReservation in reservations)
            {
                if (existingReservation.Conflicts(reservation))
                {
                    throw new ReservationConflictException(existingReservation, reservation);
                }
            }
            reservations.Add(reservation);
        }
    }
}
