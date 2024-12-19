using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Features;
using Data;
using Data.Entities.DocumentEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class DocumentRepository : BaseRepository<Document, Guid>, IDocumentRepository
    {
        public DocumentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
