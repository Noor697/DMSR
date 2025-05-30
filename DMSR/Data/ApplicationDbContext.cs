using DMSR.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DMSR.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User_Management> user_managements { get; set; }
        public DbSet<Doc_Management> doc_managements { get; set; }
        public DbSet<DocActivityLogs> activity_logs { get; set; }

        public DbSet<UserActivityLog> user_logs { get; set; }


    }
}
