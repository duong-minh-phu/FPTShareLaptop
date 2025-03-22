using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CategoryDTO
{
    public class CategoryUpdateDTO
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(255)]
        public string CategoryName { get; set; } = string.Empty;
    }
}
