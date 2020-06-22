using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlayersWallet.Contracts.Entities;

namespace PlayersWallet.Persistence.Configurations
{
	public class PlayerConfiguration
	{
		public PlayerConfiguration(EntityTypeBuilder<Player> entity)
		{
			// Table
			entity.ToTable("Players");

			// Properties
			entity.HasKey(e => e.PlayerId);

			entity.Property(e => e.PlayerId)
				.ValueGeneratedOnAdd();

			entity.Property(e => e.Name)
				.IsRequired()
				.HasMaxLength(128);

			// Relationships
			entity.HasMany(a => a.Transactions)
				.WithOne(b => b.Player)
				.HasForeignKey(c => c.PlayerId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
