using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class ErrorException : Exception
    {
        public IEnumerable<string> Errors { get; } = Enumerable.Empty<string>();
        public ErrorException(string message) : base(message) { }
        public ErrorException(string message, IEnumerable<string> errors) : base(message) 
        {
            Errors = errors;
        }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base() { }
    }
}
