using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Services.ReservationConflictValidators;
using WpfApp1.Services.ReservationCreaters;
using WpfApp1.Services.ReservationProvides;

namespace WpfApp1.Models
{
    internal class ReservationBook
    {
        private readonly IReservationProvider reservationProvider;
        private readonly IReservationCreator reservationCreator;
        private readonly IReservationConflicValidator reservationConflicValidator;


        public ReservationBook(IReservationProvider reservationProvider, IReservationCreator reservationCreator, IReservationConflicValidator reservationConflicValidator)
        {
            this.reservationProvider = reservationProvider;
            this.reservationCreator = reservationCreator;
            this.reservationConflicValidator = reservationConflicValidator;
        }

        public async Task<IEnumerable<Reservation>> GetReservations()
        {
            return await reservationProvider.GetAllReservations();
        }

        public async Task AddReservation(Reservation reservation)
        {
            Reservation conflictingReservation = await reservationConflicValidator.GetConflictingReservation(reservation);
            if(conflictingReservation != null)
            {
                throw new ReservationConflictException(conflictingReservation, reservation);
            }
            
            await reservationCreator.CreateReservation(reservation);
        }
    }
}
