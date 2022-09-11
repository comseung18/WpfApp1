using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DbContexts
{
    internal class ReservoomDbContextFactory
    {
        private string connectionString;

        public ReservoomDbContextFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ReservoomDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder().UseSqlite(connectionString)
                .Options;

            return new ReservoomDbContext(options);
        }
    }
}
