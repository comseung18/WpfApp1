using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.Models
{
    internal class Reservation
    {
        public Reservation(RoomID roomID, string username, DateTime startTime, DateTime endTime)
        {
            RoomID = roomID;
            StartTime = startTime;
            EndTime = endTime;
            Username = username;
        }

        public RoomID RoomID { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public string Username { get; }

        public TimeSpan Length => EndTime.Subtract(StartTime);

        public bool Conflicts(Reservation reservation)
        {
            if (RoomID != reservation.RoomID) return false;

            return (StartTime <= reservation.StartTime && reservation.StartTime <= EndTime) ||
                (StartTime <= reservation.EndTime && reservation.EndTime <= EndTime);
        }
    }
}
