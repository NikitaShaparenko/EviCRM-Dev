using Microsoft.EntityFrameworkCore;

namespace EviCRM.Server.Models.Auth
{
    public class UsersContext2: DbContext
    {
        public DbSet<User> Users { get; set; }
        public UsersContext2(DbContextOptions<UsersContext2> options)
                : base(options)
            {
                //Database.EnsureCreated();
            }
    }
}