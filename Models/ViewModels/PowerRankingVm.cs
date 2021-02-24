using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.ViewModels
{
    public class PowerRankingVm
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        [MaxLength(10)] public string Year { get; set; }
        public string Disclaimer { get; set; }
        public List<RankingVm> Rankings { get; set; }
        public List<RegisteredUserVm> RegisteredUsers { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class RankingVm
    {
        public Guid RankingId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        [Column(TypeName = "decimal(10,1)")]
        public decimal Handicap { get; set; }
        public int Rank { get; set; }
        public string Trending { get; set; }
        public string Writeup { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
