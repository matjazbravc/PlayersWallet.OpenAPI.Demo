using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayersWallet.Contracts.Entities;
using PlayersWallet.Persistence.DbContexts;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace PlayersWallet.Persistence.Repositories
{
	public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
	{
		private readonly ILogger<TransactionRepository> _logger;

		public TransactionRepository(ApplicationDbContext appDbContext, ILogger<TransactionRepository> logger) 
			: base(appDbContext)
		{
			_logger = logger;
		}

		public override async Task<Transaction> GetSingleAsync(Expression<Func<Transaction, bool>> predicate, bool disableTracking = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			Transaction result = null;
			try
			{
				result = await DatabaseSet
                    .Include(tr => tr.Player)
					.AsNoTracking()
					.SingleOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
				return result;
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.Error, ex, "Error when trying to get single Transaction.");
				return result;
			}
		}

		public override async Task<IList<Transaction>> GetAllAsync(bool disableTracking = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			var result = new List<Transaction>();
			IQueryable<Transaction> query = DatabaseSet;
			try
			{
				if (disableTracking)
				{
					query = query.AsNoTracking();
				}
				result = await query
					.Include(tr => tr.Player)
					.AsNoTracking()
					.ToListAsync(cancellationToken).ConfigureAwait(false);
				return result;
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.Error, ex, "Error when trying to get list of all Transactions.");
				return result;
			}
		}
	}
}
