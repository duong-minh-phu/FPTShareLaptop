using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DonateItemDTO
{
    public class DonateItemUpdateDTO
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? Cpu { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
        public string? ScreenSize { get; set; }
        public string? ConditionItem { get; set; }
        public string? Status { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public string? GraphicsCard { get; set; }
        public string? Battery { get; set; }
        public string? Ports { get; set; }
        public int? ProductionYear { get; set; }
        public string? OperatingSystem { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
    }
}
