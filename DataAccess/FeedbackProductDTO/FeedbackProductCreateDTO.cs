using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.FeedbackProductDTO
{
    public class FeedbackProductCreateDTO
    {
        [Required]
        public int OrderDetailId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public string Comments { get; set; } = string.Empty;

        public bool IsAnonymous { get; set; }
    }
}
