using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class PowerRanking
    {
        [Key] public int EventId { get; set; }
        public Event Event { get; set; }
        [MaxLength(10)] public string Year { get; set; }
        public string Disclaimer { get; set; }
        [Column(TypeName = "jsonb")] public List<Ranking> Rankings { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class Ranking
    {
        public Guid RankingId { get; set; }
        public string UserId { get; set; }
        [Column(TypeName = "decimal(10,1)")] public decimal Handicap { get; set; }
        public int Rank { get; set; }
        public string Trending { get; set; }
        public string Writeup { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
