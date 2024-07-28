using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Services.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }

        public DbSet<User> Users {  get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Restuarant> Restuarants { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<ReservationTimeSlot> ReservationTimeSlots { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
