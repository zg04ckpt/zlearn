using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public class APIResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class APIResult<T> : APIResult
    {
        public T Data { get; set; }
    }

    public class APISuccessResult : APIResult
    {
        public APISuccessResult()
        {
            Success = true;
        }

        public APISuccessResult(string message)
        {
            Success = true;
            Message = message;
        }

    }

    public class APISuccessResult<T> : APIResult<T>
    {

        public APISuccessResult(string message, T data)
        {
            Success = true;
            Message = message; 
            Data = data;
        }

        public APISuccessResult(T data)
        {
            Success = true;
            Data = data;
        }
    }

    public class APIErrorResult : APIResult
    {
        public IEnumerable<string> Details { get; set; }
        public APIErrorResult(string message)
        {
            Success = false;
            Message = message;
            Details = Array.Empty<string>();
        }

        public APIErrorResult(string message, IEnumerable<string> errors)
        {
            Success = false;
            Message = message;
            Details = errors;
        }
    }
}
