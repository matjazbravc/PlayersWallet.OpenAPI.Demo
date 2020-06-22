using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayersWallet.Contracts.Entities;

namespace PlayersWallet.Persistence.Configurations
{
	public class TransactionConfiguration
	{
		public TransactionConfiguration(EntityTypeBuilder<Transaction> entity)
		{
            // Table
            entity.ToTable("Transactions");
			
			// Properties
			entity.HasKey(e => e.TransactionId);

			entity.Property(e => e.TransactionId)
				.ValueGeneratedOnAdd();
		}
	}
}
