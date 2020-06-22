using PlayersWallet.Contracts.Entities;
using PlayersWallet.Persistence.DbContexts;
using System.Linq;

namespace PlayersWallet.OpenApi.Data
{
    public class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Players.Any())
            {
                context.Players.AddRange(
                    new Player
                    {
                        PlayerId = 1,
                        Name = "Player 1",
                        Email = "player1@demo.com"
                    },
                    new Player
                    {
                        PlayerId = 2,
                        Name = "Player 2",
                        Email = "player2@demo.com"
                    },
                    new Player
                    {
                        PlayerId = 3,
                        Name = "Player 3",
                        Email = "player3@demo.com"
                    }
                );

                context.Transactions.AddRange(
                    new Transaction
                    {
                        PlayerId = 1,
                        PayIn = 1000
                    },
                    new Transaction
                    {
                        PlayerId = 2,
                        PayIn = 2000
                    },
                    new Transaction
                    {
                        PlayerId = 3,
                        PayIn = 3000
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
