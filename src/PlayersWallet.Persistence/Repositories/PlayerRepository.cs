using Microsoft.EntityFrameworkCore;
using PlayersWallet.Contracts.Entities;
using PlayersWallet.Persistence.DbContexts;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;

namespace PlayersWallet.Persistence.Repositories
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        private readonly ILogger<PlayerRepository> _logger;

        public PlayerRepository(ApplicationDbContext appDbContext, ILogger<PlayerRepository> logger)
            : base(appDbContext)
        {
            _logger = logger;
        }

        public override async Task<Player> GetSingleAsync(Expression<Func<Player, bool>> predicate, bool disableTracking = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            Player result = null;
            try
            {
                result = await DatabaseSet
                    .Include(pl => pl.Transactions)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error when trying to get single Player.");
                return result;
            }
        }

        public override async Task<IList<Player>> GetAllAsync(bool disableTracking = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new List<Player>();
            IQueryable<Player> query = DatabaseSet;
            try
            {
                if (disableTracking)
                {
                    query = query.AsNoTracking();
                }
                result = await query
                    .Include(pl => pl.Transactions)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error when trying to get list of all Players.");
                return result;
            }
        }
    }
}
