using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
    public enum ContractStatus
    {
        Pending,   // Chờ xác nhận
        Active,    // Đang có hiệu lực
        Completed, // Đã hoàn thành
        Canceled   // Bị hủy
    }

}
