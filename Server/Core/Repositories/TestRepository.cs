using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Services.Common;
using Data;
using Data.Entities.TestEntities;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class TestRepository :  BaseRepository<Test, Guid>, ITestRepository
    {
        public TestRepository(AppDbContext context) : base(context)
        {
        }

        public void AddQuestion(Question question)
        {
            _context.Questions.Add(question);
        }

        public async Task<PaginatedResult<TestResult>> GetAllResults(int pageIndex, int pageSize, List<ExpressionFilter> filters)
        {
             var query = _context.TestResults.AsQueryable().AsNoTracking();

            if (filters != null && filters.Any())
            {
                var lambda = LambdaBuilder.GetAndLambdaExpression<TestResult>(filters);
                query = query.Where(lambda);
            }

            var data = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<TestResult>
            {
                Total = await query.CountAsync(),
                Data = data
            };
        }

        public async Task<List<Question>> GetQuestions(string testId)
        {
            return await _context.Questions
                .Where(x => x.TestId.ToString().Equals(testId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<TestResult>> GetResultsByUserId(string userId)
        {
            return await _context.TestResults
                .Where(x => x.UserId.Equals(userId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Test>> GetSavedTestsOfUser(string userId)
        {
            return await(from test in _context.Tests
                        join st in _context.SavedTests on test.Id equals st.TestId
                        where st.UserId.ToString().Equals(userId)
                        select test).ToListAsync();
        }

        public async Task<List<Test>> GetTopByAttempt(int amount)
        {
            return await _context.Tests.AsNoTracking()
                .OrderByDescending(x => x.NumberOfAttempts)
                .Take(amount)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<bool> IsSaved(string userId, string testId)
        {
            return _context.SavedTests
                .AnyAsync(x => x.UserId.ToString().Equals(userId) && x.TestId.ToString().Equals(testId));
        }

        public void RemoveQuestion(Question question)
        {
            _context.Questions.Remove(question);
        }

        public void SaveResult(TestResult testResult)
        {
            _context.TestResults.Add(testResult);
        }

        public void SaveTest(SavedTest savedTest)
        {
            _context.SavedTests.Add(savedTest);
        }

        public async Task UnSave(string userId, string testId)
        {
            var uit = await _context.SavedTests
                .Where(x => x.UserId.ToString().Equals(userId) && x.TestId.ToString().Equals(testId))
                .FirstOrDefaultAsync() ?? throw new ErrorException("Không tìm thấy test cần bỏ lưu");
            _context.SavedTests.Remove(uit);
        }

        public void UpdateQuestion(Question question)
        {
            _context.Questions.Update(question);
        }
    }
}
