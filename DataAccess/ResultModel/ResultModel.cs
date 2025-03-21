using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ResultModel
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; }

        public static ResultModel Success(object? data, string? message = "Success")
        {
            return new ResultModel { IsSuccess = true, Code = 200, Data = data, Message = message };
        }

        public static ResultModel Created(object? data, string? message = "Created successfully")
        {
            return new ResultModel { IsSuccess = true, Code = 201, Data = data, Message = message };
        }

        public static ResultModel NotFound(string? message = "Not found")
        {
            return new ResultModel { IsSuccess = false, Code = 404, Data = null, Message = message };
        }

        public static ResultModel BadRequest(string? message = "Bad request")
        {
            return new ResultModel { IsSuccess = false, Code = 400, Data = null, Message = message };
        }

    }
}
