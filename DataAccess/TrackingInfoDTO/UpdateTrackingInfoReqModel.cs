using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.TrackingInfoDTO
{
    public class UpdateTrackingInfoReqModel
    {
        public string Status { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
