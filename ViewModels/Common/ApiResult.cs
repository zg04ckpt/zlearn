using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace  ViewModels.Common
{
    public class ApiResult
    {
        public object Data { get; set; }
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }

        public ApiResult()
        {
            Code = HttpStatusCode.OK;
        }

        public ApiResult(object data)
        {
            Code = HttpStatusCode.OK;
            Data = data;
        }

        public ApiResult(string message, HttpStatusCode code)
        {
            Code = code;
            Message = message;
        }
    }
}
