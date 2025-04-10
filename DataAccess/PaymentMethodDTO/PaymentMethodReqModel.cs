using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PaymentMethodDTO
{
    public class PaymentMethodReqModel
    {
        [Required(ErrorMessage = "MethodName is required")]
        public string MethodName { get; set; } = null!;
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
