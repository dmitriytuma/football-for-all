using System;
using System.Linq;
using FootballForAll.Data.Models;
using FootballForAll.Data.Models.Common;
using FootballForAll.Data.Models.People;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballForAll.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region DbSets

        public DbSet<Country> Countries { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Season> Seasons { get; set; }

        public DbSet<Championship> Championships { get; set; }

        public DbSet<SeasonTable> SeasonTables { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<Stadium> Stadiums { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Referee> Referees { get; set; }

        #endregion DbSets

        /// <summary>
        /// Add audit info (extra info about CreatedOn and ModifiedOn dates) to the changed entities and save the changes to DB.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        /// <summary>
        /// Adds audit info (extra info about CreatedOn and ModifiedOn dates) to the changed entities.
        /// </summary>
        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
