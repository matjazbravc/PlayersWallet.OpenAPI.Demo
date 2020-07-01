using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayersWallet.Contracts.Entities;
using PlayersWallet.Persistence.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using PlayersWallet.Persistence.Exceptions;

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

        public override async Task<IList<Transaction>> GetAllAsync(bool disableTracking = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<Transaction> query = DatabaseSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            var result = await query
                .Include(tr => tr.Player)
                .AsNoTracking()
                .ToListAsync(cancellationToken).ConfigureAwait(false);
            if (result == null)
            {
                throw new TransactionsListEmptyException("Transactions list is empty");
            }
            return result;
        }
    }
}
