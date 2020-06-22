using Newtonsoft.Json;
using PlayersWallet.Contracts.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace PlayersWallet.Contracts.Entities
{
    [Serializable]
    [JsonObject(IsReference = false)]
    public class Player : BaseAuditEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerId { get; set; }

        [Display(Name = "Player's Name")]
        [StringLength(128)]
        public string Name { get; set; }

        [Display(Name = "Player's Email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Player's Wallet Balance")]
        public int Balance
        {
            get
            {
                var totalPayIn = Transactions.Sum(x => x.PayIn);
                var totalWin = Transactions.Sum(x => x.Win);
                var totalBet = Transactions.Sum(x => x.Bet) * -1;
                return totalPayIn + totalWin + totalBet;
            }
        }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public override string ToString() => $"{PlayerId}, {Name}";
    }
}