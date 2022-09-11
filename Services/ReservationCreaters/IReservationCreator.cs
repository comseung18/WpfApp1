using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services.ReservationCreaters
{
    internal interface IReservationCreator
    {
        Task CreateReservation(Reservation reservation);
    }
}
