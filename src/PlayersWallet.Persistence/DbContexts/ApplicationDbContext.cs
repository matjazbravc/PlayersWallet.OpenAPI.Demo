using Microsoft.EntityFrameworkCore;
using PlayersWallet.Contracts.Entities.Base;
using PlayersWallet.Contracts.Entities;
using PlayersWallet.Persistence.Configurations;
using System.Linq;
using System;

namespace PlayersWallet.Persistence.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
	    public ApplicationDbContext(DbContextOptions options) 
		    : base(options)
        {
        }

		public DbSet<Player> Players { get; set; }

	    public DbSet<Transaction> Transactions { get; set; }

		public override int SaveChanges()
		{
			TrackChanges();
			return base.SaveChanges();
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new PlayerConfiguration(modelBuilder.Entity<Player>());
	        new TransactionConfiguration(modelBuilder.Entity<Transaction>());
        }

        private void TrackChanges()
        {
            var entries = ChangeTracker.Entries()
	            .Where(x => x.Entity is IBaseAuditEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
	            if (entry.State == EntityState.Added)
                {
                    ((IBaseAuditEntity)entry.Entity).CreatedDate = DateTime.UtcNow;
                }
                ((IBaseAuditEntity)entry.Entity).ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}
