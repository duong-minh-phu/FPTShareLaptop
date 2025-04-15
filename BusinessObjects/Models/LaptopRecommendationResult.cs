using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class LaptopRecommendationResult
    {
        public string Cpu { get; set; } = string.Empty;
        public string Ram { get; set; } = string.Empty;
        public string GraphicsCard { get; set; } = string.Empty;
    }
}
