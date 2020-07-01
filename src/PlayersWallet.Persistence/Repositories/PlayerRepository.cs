using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlayersWallet.Contracts.Entities;
using PlayersWallet.Persistence.DbContexts;
using PlayersWallet.Persistence.Exceptions;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

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

        public async Task<Player> GetAsync(int playerId)
        {
            var result = await GetSingleAsync(pl => pl.PlayerId == playerId).ConfigureAwait(false);
            if (result == null)
            {
                throw new RepositoryNotFoundException($"The player with Id {playerId} was not found");
            }
            return result;
        }

        public override async Task<Player> GetSingleAsync(Expression<Func<Player, bool>> predicate, bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<Player> query = DatabaseSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            var result = await query
                .Include(pl => pl.Transactions)
                .SingleOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
            return result;
        }

        public override async Task<IList<Player>> GetAllAsync(bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<Player> query = DatabaseSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            var result = await query
                .Include(pl => pl.Transactions)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
    }
}
