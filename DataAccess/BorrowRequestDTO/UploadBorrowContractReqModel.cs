using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DataAccess.BorrowRequestDTO
{
    public class UploadBorrowContractReqModel
    {
        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageBorrowConract { get; set; }
    }
}
