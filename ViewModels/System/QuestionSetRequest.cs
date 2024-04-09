﻿using ZG04WEB.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZG04WEB.ViewModels.System
{
    public class QuestionSetRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<QuestionCreateRequest> Questions { get; set; }
    }
}
