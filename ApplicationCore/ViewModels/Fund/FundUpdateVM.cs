﻿namespace ApplicationCore.ViewModels.Fund
{
    public class FundUpdateVM
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TotalFund { get; set; }

        public string Note { get; set; } = null!;

        public Guid UserUpdateId { get; set; }

        public Guid UserAssignId { get; set; }
    }
}
