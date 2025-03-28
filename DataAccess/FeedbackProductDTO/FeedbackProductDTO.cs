using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.FeedbackProductDTO
{
    public class FeedbackProductDTO
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; } = string.Empty;
        public bool IsAnonymous { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
