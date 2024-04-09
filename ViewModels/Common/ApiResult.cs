using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Common
{
    public class ApiResult
    {
        public object Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public ApiResult()
        {
            IsSuccess = true;
        }

        public ApiResult(object data)
        {
            IsSuccess = true;
            Data = data;
        }

        public ApiResult(string message)
        {
            IsSuccess = false;
            Message = message;
        }
    }
}
