using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using proj.Models;

namespace proj.Data
{
    public class ProjContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public ProjContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("ProjConnection"));
        }

        public DbSet<Detail> Details { get; set; }
    }
}