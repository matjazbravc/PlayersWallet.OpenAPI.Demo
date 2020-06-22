using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlayersWallet.Persistence.Repositories;
using PlayersWallet.Tests.Services;
using PlayersWallet.Contracts.Entities;
using Xunit;

namespace PlayersWallet.Tests.UnitTests
{
    [Collection("Sequential")]
    public class TransactionRepositorsTests : IClassFixture<WebApiTestFactory>
    {
        private readonly ILogger _logger;
        private readonly WebApiTestFactory _factory;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionRepositorsTests(WebApiTestFactory factory)
        {
            _factory = factory;
            _logger = _factory.Services.GetRequiredService<ILogger<WebApiTestFactory>>();
            _transactionRepository = factory.Services.GetRequiredService<ITransactionRepository>();
        }

        [Fact]
        public async Task CanAdd()
        {
            _logger.LogInformation("CanAdd");
            var transaction = new Transaction
            {
                Bet = 100
            };
            var newTransaction = await _transactionRepository.AddAsync(transaction).ConfigureAwait(false);
            Assert.True(newTransaction.Bet == 100);
        }

        [Fact]
        public async Task CanCount()
        {
            var result = await _transactionRepository.CountAsync().ConfigureAwait(false);
            Assert.True(result >= 0);
        }

        [Fact]
        public async Task CanDelete()
        {
            var transaction = new Transaction
            {
                Bet = 500
            };
            var newTransaction = await _transactionRepository.AddAsync(transaction).ConfigureAwait(false);
            var result = await _transactionRepository.DeleteAsync(newTransaction).ConfigureAwait(false);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task CanGetAll()
        {
            var result = await _transactionRepository.GetAllAsync().ConfigureAwait(false);
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task CanUpdate()
        {
            var transaction = new Transaction
            {
                TransactionId = 1,
                Bet = 999
            };
            var updatedTransaction = await _transactionRepository.UpdateAsync(transaction).ConfigureAwait(false);
            Assert.True(updatedTransaction.Bet == 999);
        }
    }
}
