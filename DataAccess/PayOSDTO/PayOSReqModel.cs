    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace DataAccess.PayOSDTO
    {
        public class PayOSReqModel
        {
            public long OrderId { get; set; }
            public string ProductName { get; set; }
            public decimal Amount { get; set; }
            public string RedirectUrl { get; set; }
            public string CancelUrl { get; set; }
        }
    }
