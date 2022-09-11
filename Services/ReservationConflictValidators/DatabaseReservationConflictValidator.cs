using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.DbContexts;
using WpfApp1.DTOs;
using WpfApp1.Models;

namespace WpfApp1.Services.ReservationConflictValidators
{
    internal class DatabaseReservationConflictValidator : IReservationConflicValidator
    {
        private readonly ReservoomDbContextFactory dbContextFactory;

        public DatabaseReservationConflictValidator(ReservoomDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<Reservation> GetConflictingReservation(Reservation reservation)
        {
            using(ReservoomDbContext context = dbContextFactory.CreateDbContext())
            {
                ReservationDTO reservationDTO = await context.Reservations.
                                    Where(r => r.FloorNumber == reservation.RoomID.FloorNumber).
                                    Where(r => r.RoomNumber == reservation.RoomID.RoomNumber).
                                    Where(r => r.EndTime < reservation.StartTime).
                                    Where(r => r.StartTime < reservation.EndTime).
                                    FirstOrDefaultAsync();
                if(reservationDTO == null)
                {
                    return null;
                }
                return ToReservation(reservationDTO);
            }
        }

        private static Reservation ToReservation(ReservationDTO r)
        {
            return new Reservation(new RoomID(r.FloorNumber, r.RoomNumber), r.Username, r.StartTime, r.EndTime);
        }

    }
}
