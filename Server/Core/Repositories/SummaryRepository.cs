using Core.Interfaces.IRepositories;
using Data;
using Data.Entities.CommonEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class SummaryRepository : ISummaryRepository
    {
        private readonly AppDbContext _context;
        public SummaryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Summary>> GetByRange(DateTime start, DateTime end)
        {
            var summaties = await _context.Summaries.AsNoTracking()
                .Where(e => e.Date.CompareTo(start) >= 0 && e.Date.CompareTo(end) <= 0)
                .ToListAsync();
            return summaties;
        }

        public async Task SaveToDatabase(Summary summary)
        {
            _context.Summaries.Add(summary);
            await _context.SaveChangesAsync();
        }
    }
}
