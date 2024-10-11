using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Data;
using Data.Entities;
using System.Data.Entity;
using System.Security.Claims;
using Utilities.Exceptions;

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
            return await _context.Questions.
                Where(x => x.TestId.Equals(testId))
                .ToListAsync();
        }

        public async Task<IEnumerable<TestResult>> GetResultsByUserId(string userId)
        {
            return await _context.TestResults
                .Where(x => x.UserId.Equals(userId)).AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Test>> GetSavedTestsOfUser(string userId)
        {
            var rawData = from test in _context.Tests
                          join st in _context.SavedTest on test.Id equals st.TestId
                          select test;
            return await rawData.ToListAsync();
        }

        public Task<bool> IsSaved(string userId, string testId)
        {
            return _context.SavedTest.AnyAsync(x => x.UserId.Equals(userId) && x.UserId.Equals(testId));
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
            _context.SavedTest.Add(savedTest);
        }

        public async Task UnSave(string userId, string testId)
        {
            var uit = await _context.SavedTest
                .Where(x => x.UserId.ToString() == userId && x.TestId.ToString() == testId)
                .FirstOrDefaultAsync() ?? throw new ErrorException("Không tìm thấy test cần bỏ lưu");
            _context.SavedTest.Remove(uit);
        }

        public void UpdateQuestion(Question question)
        {
            _context.Questions.Update(question);
        }
    }
}
