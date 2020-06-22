using Newtonsoft.Json;
using System;

namespace PlayersWallet.Contracts.Dto.Requests
{
    [Serializable]
    [JsonObject(IsReference = false)]
    public class WinRequest
    {
        [JsonProperty("playerId")]
        public int PlayerId { get; set; }

        [JsonProperty("win")]
        public int Win { get; set; } = 0;
    }
}
