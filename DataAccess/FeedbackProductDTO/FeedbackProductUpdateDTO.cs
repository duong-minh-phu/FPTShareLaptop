using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.FeedbackProductDTO
{
    public class FeedbackProductUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public string Comments { get; set; } = string.Empty;

        public bool IsAnonymous { get; set; }
    }
}
