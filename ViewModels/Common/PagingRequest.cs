using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Common
{
    public class PagingRequest
    {
        public string? Key { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
