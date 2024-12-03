﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public class PaginatedResult<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
