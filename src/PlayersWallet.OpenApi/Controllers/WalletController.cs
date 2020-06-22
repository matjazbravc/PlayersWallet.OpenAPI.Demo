using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlayersWallet.Contracts.Dto.Requests;
using PlayersWallet.Contracts.Dto.Responses;
using PlayersWallet.Contracts.Entities;
using PlayersWallet.Core.Errors;
using PlayersWallet.Persistence.Repositories;
using PlayersWallet.OpenApi.Controllers.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayersWallet.OpenApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [EnableCors("AllowAll")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WalletController : BaseController<WalletController>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITransactionRepository _transactionRepository;

        public WalletController(IPlayerRepository playerRepository,
            ITransactionRepository transactionRepository)
        {
            _playerRepository = playerRepository;
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Return current amount of money available to the player
        /// </summary>
        /// <param name="playerId">Player Id</param>
        /// <returns>Return 200 with player Balance when everything is ok</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("balance/{playerId}", Name = nameof(GetBalanceAsync))]
        [SwaggerOperation(description: "Return current amount of money available to the player")]
        [SwaggerResponse(200, description: "Return 200 with player's Balance when everything is ok", Type = typeof(BalanceResponse))]
        [SwaggerResponse(404, description: "Return when player was not found")]
        [SwaggerResponse(500, description: "Return whenever something unexpected went wrong")]
        public async Task<IActionResult> GetBalanceAsync(int playerId)
        {
            Logger.LogDebug(nameof(GetBalanceAsync));

            // Get Player if exists
            var player = await _playerRepository.GetSingleAsync(pl => pl.PlayerId == playerId).ConfigureAwait(false);
            if (player == null)
            {
                return NotFound(new NotFoundError($"The player with Id {playerId} was not found"));
            }

            var result = new BalanceResponse
            {
                PlayerId = playerId,
                Name = player.Name,
                Balance = player.Balance
            };
            return Ok(result);
        }

        /// <summary>
        /// Return player's transactions
        /// </summary>
        /// <param name="playerId">Player Id</param>
        /// <returns>Return 200 with player's transactions when everything is ok</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("transactions/{playerId}", Name = nameof(GetTransactionsAsync))]
        [SwaggerOperation(description: "Return player's transactions")]
        [SwaggerResponse(200, description: "Return 200 with player's transactions when everything is ok", Type = typeof(IEnumerable<Transaction>))]
        [SwaggerResponse(404, description: "Return when player was not found")]
        [SwaggerResponse(500, description: "Return whenever something unexpected went wrong")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsAsync(int playerId)
        {
            Logger.LogDebug(nameof(GetTransactionsAsync));

            // Get Player if exists
            var player = await _playerRepository.GetSingleAsync(pl => pl.PlayerId == playerId).ConfigureAwait(false);
            if (player == null)
            {
                return NotFound(new NotFoundError($"The player with Id {playerId} was not found"));
            }

            return Ok(player.Transactions.OrderByDescending(x => x.CreatedDate));
        }

        /// <summary>
        /// Post game transaction that takes a certain amount of money from player’s Balance
        /// </summary>
        /// <param name="request">Bet request</param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        [HttpPost("bet", Name = nameof(BetAsync))]
        [SwaggerOperation(description: "Post game transaction that takes a certain amount of money from player’s Balance")]
        [SwaggerResponse(200, description: "Return 200 with player info when everything is ok", Type = typeof(Player))]
        [SwaggerResponse(404, description: "Return when player was not found")]
        [SwaggerResponse(500, description: "Return whenever something unexpected went wrong")]
        public async Task<IActionResult> BetAsync([FromBody] BetRequest request)
        {
            Logger.LogDebug(nameof(BetAsync));

            // Validate request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get Player if exists
            var player = await _playerRepository.GetSingleAsync(pl => pl.PlayerId == request.PlayerId).ConfigureAwait(false);
            if (player == null)
            {
                return NotFound(new NotFoundError($"The Player with Id {request.PlayerId} was not found"));
            }

            // Check Player's Balance
            if (player.Balance - request.Bet < 0)
            {
                return BadRequest(new BadRequestError($"The Player MAX. Bet Amout is {player.Balance} EUR"));
            }

            // Create new transaction
            var transaction = new Transaction
            {
                PlayerId = player.PlayerId,
                Bet = request.Bet
            };

            // Persist Transaction
            await _transactionRepository.AddAsync(transaction).ConfigureAwait(false);

            // Re-read Player
            player = await _playerRepository.GetSingleAsync(pl => pl.PlayerId == request.PlayerId).ConfigureAwait(false);
            return Ok(player);
        }

        /// <summary>
        /// Post transaction that adds a certain amount of money to player’s Balance
        /// </summary>
        /// <param name="request">PayIn request</param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("payIn", Name = nameof(PayInAsync))]
        [SwaggerOperation(description: "Post transaction that adds a certain amount of money to player’s Balance")]
        [SwaggerResponse(200, description: "Return 200 with player info when everything is ok", Type = typeof(Player))]
        [SwaggerResponse(404, description: "Return when player was not found")]
        [SwaggerResponse(500, description: "Return whenever something unexpected went wrong")]
        public async Task<IActionResult> PayInAsync([FromBody] PayInRequest request)
        {
            Logger.LogDebug(nameof(PayInAsync));

            // Validate request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get Player if exists
            var player = await _playerRepository.GetSingleAsync(pl => pl.PlayerId == request.PlayerId).ConfigureAwait(false);
            if (player == null)
            {
                return NotFound(new NotFoundError($"The player with Id {request.PlayerId} was not found"));
            }

            // Create new transaction
            var transaction = new Transaction
            {
                PlayerId = player.PlayerId,
                PayIn = request.PayIn
            };

            // Persist Transaction
            await _transactionRepository.AddAsync(transaction).ConfigureAwait(false);

            // Re-read Player
            player = await _playerRepository.GetSingleAsync(pl => pl.PlayerId == request.PlayerId).ConfigureAwait(false);
            return Ok(player);
        }

        /// <summary>
        /// Post game transaction that awards/adds certain amount of money to player’s Balance
        /// </summary>
        /// <param name="request">Win request</param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        [HttpPost("win", Name = nameof(WinAsync))]
        [SwaggerOperation(description: "Post game transaction that awards/adds certain amount of money to player’s Balance")]
        [SwaggerResponse(200, description: "Return 200 with player info when everything is ok", Type = typeof(Player))]
        [SwaggerResponse(404, description: "Return when player was not found")]
        [SwaggerResponse(500, description: "Return whenever something unexpected went wrong")]
        public async Task<IActionResult> WinAsync([FromBody] WinRequest request)
        {
            Logger.LogDebug(nameof(WinAsync));

            // Validate request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get Player if exists
            var player = await _playerRepository.GetSingleAsync(pl => pl.PlayerId == request.PlayerId).ConfigureAwait(false);
            if (player == null)
            {
                return NotFound(new NotFoundError($"The player with Id {request.PlayerId} was not found"));
            }

            // Create new transaction
            var transaction = new Transaction
            {
                PlayerId = player.PlayerId,
                Win = request.Win
            };

            // Persist Transaction
            await _transactionRepository.AddAsync(transaction).ConfigureAwait(false);

            // Re-read Player
            player = await _playerRepository.GetSingleAsync(pl => pl.PlayerId == request.PlayerId).ConfigureAwait(false);
            return Ok(player);
        }

        /// <summary>
        /// Return list of players
        /// </summary>
        /// <returns>Return 200 with list of players when everything is ok</returns>
        [MapToApiVersion("1.0")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("players", Name = nameof(GetPlayersAsync))]
        [SwaggerOperation(description: "Return list of players")]
        [SwaggerResponse(200, description: "Return 200 with list of players when everything is ok", Type = typeof(IEnumerable<Player>))]
        [SwaggerResponse(500, description: "Return whenever something unexpected went wrong")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersAsync()
        {
            Logger.LogDebug(nameof(GetPlayersAsync));

            var result = await _playerRepository.GetAllAsync().ConfigureAwait(false);
            return Ok(result);
        }
    }
}
