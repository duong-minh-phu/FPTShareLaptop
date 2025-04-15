using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MajorDTO
{
    public class MajorUpdateDTO
    {
        [Required]
        public string Status { get; set; } = null!;
    }
}
