using Newtonsoft.Json;
using System;

namespace PlayersWallet.Contracts.Dto.Requests
{
    [Serializable]
    [JsonObject(IsReference = false)]
    public class PayInRequest
    {
        [JsonProperty("playerId")]
        public int PlayerId { get; set; }

        [JsonProperty("payIn")]
        public int PayIn { get; set; } = 0;
    }
}
