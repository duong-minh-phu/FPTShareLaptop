using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DonateItemDTO
{
    public class LaptopSuggestionResultDTO
    {
        public string SuggestionMessage { get; set; } = null!;
        public List<DonateItemReadDTO> SuitableLaptops { get; set; } = new();
    }
}
