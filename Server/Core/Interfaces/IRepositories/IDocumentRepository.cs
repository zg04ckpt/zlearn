﻿using Data.Entities.DocumentEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IRepositories
{
    public interface IDocumentRepository : IBaseRepository<Document, Guid>
    { 
    }
}
