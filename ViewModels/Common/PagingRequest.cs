using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Common
{
    public class PagingRequest
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
