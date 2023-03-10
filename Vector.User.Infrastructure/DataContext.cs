using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Vector.User.Domain.Models;
using VECTOR.CRUDLibrary.Context;

namespace Vector.User.Infrastructure
{
    public class DataContext : BaseContext
    {
        public DataContext(DbContextOptions options)
                : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer();
        }
        public DbSet<UserModel> User { get; set; }
        public DbSet<UserOtpNotificationModel> UserOtpNotification { get; set; }
        public DbSet<CustomerManagementModel>CustomerManagement { get; set; }
    }
}
