using System;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class UserHistoryVm
    {
        public string UserId { get; set; }

        public string FullName { get; set; }
        public string Image { get; set; }
        public decimal Handicap { get; set; }
    }

    public class HandicapVm
    {
        public DateTime Date { get; set; }
        public decimal Handicap { get; set; }
    }

}
