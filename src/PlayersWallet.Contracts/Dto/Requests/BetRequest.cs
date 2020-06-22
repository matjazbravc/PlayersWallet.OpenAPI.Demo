using Newtonsoft.Json;
using System;

namespace PlayersWallet.Contracts.Dto.Requests
{
    [Serializable]
    [JsonObject(IsReference = false)]
    public class BetRequest
    {
        [JsonProperty("playerId")]
        public int PlayerId { get; set; }

        [JsonProperty("bet")]
        public int Bet { get; set; } = 0;
    }
}
