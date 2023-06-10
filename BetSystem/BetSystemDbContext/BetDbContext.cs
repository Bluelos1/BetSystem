using BetSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BetSystem.BetSystemDbContext
{
    public class BetDbContext : DbContext
    {

        public BetDbContext(DbContextOptions<BetDbContext> options) : base(options) { }


        public DbSet<BetOnEvent> Bets { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<EventResult> Results { get; set; }
        public DbSet<SportEvent> Events { get; set; }
    }
}
