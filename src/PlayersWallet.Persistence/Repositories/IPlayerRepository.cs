using PlayersWallet.Contracts.Entities;
using System.Threading.Tasks;

namespace PlayersWallet.Persistence.Repositories
{
	public interface IPlayerRepository : IBaseRepository<Player>
    {
        Task<Player> GetAsync(int playerId);
    }
}
