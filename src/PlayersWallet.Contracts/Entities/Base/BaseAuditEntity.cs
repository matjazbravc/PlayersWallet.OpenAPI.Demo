using System;

namespace PlayersWallet.Contracts.Entities.Base
{
	public abstract class BaseAuditEntity : IBaseAuditEntity    
	{
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		
		public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
	}
}
