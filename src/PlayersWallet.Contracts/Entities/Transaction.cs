using Newtonsoft.Json;
using PlayersWallet.Contracts.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PlayersWallet.Contracts.Entities
{
    [Serializable]
    [JsonObject(IsReference = false)]
    public class Transaction : BaseAuditEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        public int PayIn { get; set; } = 0;

        public int Bet { get; set; } = 0;

        public int Win { get; set; } = 0;

        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

        [JsonIgnore]
        public Player Player { get; set; }
    }
}