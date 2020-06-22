using Newtonsoft.Json;
using System;

namespace PlayersWallet.Contracts.Dto.Responses
{
    [Serializable]
    [JsonObject(IsReference = false)]
    public class BalanceResponse
    {
        [JsonProperty("playerId")]
        public int PlayerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("balance")]
        public int Balance { get; set; } = 0;
    }
}
