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
    public class PlayerRepositoryTests : IClassFixture<WebApiTestFactory>
    {
        private readonly ILogger _logger;
        private readonly WebApiTestFactory _factory;
        private readonly IPlayerRepository _playerRepository;

        public PlayerRepositoryTests(WebApiTestFactory factory)
        {
            _factory = factory;
            _logger = _factory.Services.GetRequiredService<ILogger<WebApiTestFactory>>();
            _playerRepository = factory.Services.GetRequiredService<IPlayerRepository>();
        }

        [Fact]
        public async Task CanAdd()
        {
            _logger.LogInformation("CanAdd");
            var player = new Player
            {
                Name = "New Player",
                Email = "new.player@ows.com"
            };
            var newPlayer = await _playerRepository.AddAsync(player).ConfigureAwait(false);
            Assert.Equal("New Player", newPlayer.Name);
        }

        [Fact]
        public async Task CanCount()
        {
            var result = await _playerRepository.CountAsync().ConfigureAwait(false);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task CanDelete()
        {
            var player = new Player
            {
                Name = "Delete Test Player",
                Email = "delete.test.player@demo.com"
            };
            var newPlayer = await _playerRepository.AddAsync(player).ConfigureAwait(false);
            var result = await _playerRepository.DeleteAsync(newPlayer).ConfigureAwait(false);
            Assert.True(result > 0);
        }

        [Fact]
        public async Task CanGetAllByPredicate()
        {
            var result = await _playerRepository.GetAllAsync(cmp => cmp.Name.Equals("Player 1")).ConfigureAwait(false);
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task CanGetSingle()
        {
            var result = await _playerRepository.GetSingleAsync(cmp => cmp.Name.Equals("Player 2")).ConfigureAwait(false);
            Assert.Equal("Player 2", result.Name);
        }

        [Fact]
        public async Task CanGetAll()
        {
            var result = await _playerRepository.GetAllAsync().ConfigureAwait(false);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task CanUpdate()
        {
            var player = new Player
            {
                PlayerId = 1,
                Name = "Updated Player 1"
            };
            var updatedPlayer = await _playerRepository.UpdateAsync(player).ConfigureAwait(false);
            Assert.Equal("Updated Player 1", updatedPlayer.Name);
        }
    }
}
