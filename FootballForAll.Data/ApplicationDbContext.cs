using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FootballForAll.Data.Models;
using FootballForAll.Data.Models.Common;
using FootballForAll.Data.Models.People;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballForAll.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, ApplicationRole, string>
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

        public DbSet<TeamPosition> TeamPositions { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<Stadium> Stadiums { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Referee> Referees { get; set; }

        #endregion DbSets

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            // Disable cascade delete
            var entityTypes = builder.Model.GetEntityTypes().ToList();
            var foreignKeys = entityTypes.SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        #region SaveChanges

        public override int SaveChanges()
        {
            return SaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(true, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
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

        #endregion SaveChanges
    }
}
