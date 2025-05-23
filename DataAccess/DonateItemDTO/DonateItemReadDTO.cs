﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DonateItemDTO
{
    public class DonateItemReadDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string ItemImage { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Cpu { get; set; } = null!;
        public string Ram { get; set; } = null!;
        public string Storage { get; set; } = null!;
        public string ScreenSize { get; set; } = null!;
        public string ConditionItem { get; set; } = null!;
        public int TotalBorrowedCount { get; set; }
        public string Status { get; set; } = null!;
        public int UserId { get; set; }
        public int DonateFormId { get; set; }
        public string SerialNumber { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string GraphicsCard { get; set; } = null!;
        public string Battery { get; set; } = null!;
        public string Ports { get; set; } = null!;
        public int ProductionYear { get; set; }
        public string OperatingSystem { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
    }
}
