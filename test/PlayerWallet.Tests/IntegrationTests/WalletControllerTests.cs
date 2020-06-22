using PlayersWallet.Contracts.Dto.Requests;
using PlayersWallet.Contracts.Dto.Responses;
using PlayersWallet.Contracts.Entities;
using PlayersWallet.Tests.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PlayersWallet.Tests.IntegrationTests
{
    public class WalletControllerTests : ControllerTestsBase
    {
        private const string BASE_URL = "/api/v1.0/wallet/";
        private readonly HttpClientHelper _httpClientHelper;

        public WalletControllerTests(WebApiTestFactory factory)
            : base(factory)
        {
            _httpClientHelper = new HttpClientHelper(Client);
        }

        [Fact]
        public async Task PlayerBetTest()
        {
            var request = new BetRequest
            {
                PlayerId = 1,
                Bet = 100
            };
            var result = await _httpClientHelper.PostAsync<BetRequest, Player>(BASE_URL + "bet", request).ConfigureAwait(false);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PlayerPayInTest()
        {
            var request = new PayInRequest
            {
                PlayerId = 1,
                PayIn = 1500
            };
            var result = await _httpClientHelper.PostAsync<PayInRequest, Player>(BASE_URL + "payIn", request).ConfigureAwait(false);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PlayerWinTest()
        {
            var request = new WinRequest
            {
                PlayerId = 1,
                Win = 500
            };
            var result = await _httpClientHelper.PostAsync<WinRequest, Player>(BASE_URL + "win", request).ConfigureAwait(false);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PlayerBalanceTest()
        {
            var playerId = 1;
            var result = await _httpClientHelper.GetAsync<BalanceResponse>(BASE_URL + $"balance/{playerId}").ConfigureAwait(false);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PlayerTransactionsTest()
        {
            var playerId = 2;
            var result = await _httpClientHelper.GetAsync<IEnumerable<Transaction>>(BASE_URL + $"transactions/{playerId}").ConfigureAwait(false);
            Assert.NotNull(result);
        }
    }
}
