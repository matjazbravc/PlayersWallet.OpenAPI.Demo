using System;

namespace PlayersWallet.Contracts.Entities.Base
{
	public interface IBaseAuditEntity
	{
		DateTime CreatedDate { get; set; }
		
		DateTime ModifiedDate { get; set; }
	}
}
