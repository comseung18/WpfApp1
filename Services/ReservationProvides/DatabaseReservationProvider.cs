using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.DbContexts;
using WpfApp1.DTOs;
using WpfApp1.Models;

namespace WpfApp1.Services.ReservationProvides
{
    internal class DatabaseReservationProvider : IReservationProvider
    {
        private readonly ReservoomDbContextFactory dbContextFactory;

        public DatabaseReservationProvider(ReservoomDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            using(ReservoomDbContext context = dbContextFactory.CreateDbContext())
            {
                IEnumerable<ReservationDTO> reservationDTOs =  await context.Reservations.ToListAsync();

                return reservationDTOs.Select(r => ToReservation(r));

            }
        }

        private static Reservation ToReservation(ReservationDTO r)
        {
            return new Reservation(new RoomID(r.FloorNumber, r.RoomNumber), r.Username, r.StartTime, r.EndTime);
        }
    }
}
