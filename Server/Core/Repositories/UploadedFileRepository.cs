using Core.Interfaces.IRepositories;
using Data;
using Data.Entities.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class UploadedFileRepository : BaseRepository<UploadedFile, Guid>, IUploadedFileRepository
    {
        public UploadedFileRepository(AppDbContext context) : base(context)
        {
        }
    }
}
