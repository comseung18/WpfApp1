using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Hotel hotel = new Hotel("Comseung Suites");

            hotel.MakeReservation(new Reservation(
                new RoomID(1, 3),
                "comseung",
                new DateTime(2000, 1, 1),
                new DateTime(2000, 1, 2)
                ));

            hotel.MakeReservation(new Reservation(
                new RoomID(1, 3),
                "comseung",
                new DateTime(2000, 1, 1),
                new DateTime(2000, 1, 4)
                ));

            IEnumerable<Reservation> reservations = hotel.GetReservationsForUser("comseung");


            base.OnStartup(e);
        }
    }
}
