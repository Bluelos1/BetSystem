using BetSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace BetSystem.BetSystemDbContext
{
    public class BetDbContext : DbContext
    {
        public BetDbContext() { }
        

        public BetDbContext(DbContextOptions<BetDbContext> options) : base(options) { }


        public virtual DbSet<BetOnEvent> Bets { get; set; }
        public virtual DbSet<Team>? Teams { get; set; }
        public virtual DbSet<EventResult> Results { get; set; }
        public virtual DbSet<SportEvent> Events { get; set; }
    }
}
