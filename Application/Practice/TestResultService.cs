using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.QuestionSet;
using ViewModels.Test;

namespace Application.Practice
{
    public class TestResultService : ITestResultService
    {
        private readonly AppDbContext _context;

        public TestResultService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult> GetAll()
        {
            try
            {
                var testResults = await _context.TestResults.Select(x => new TestResultResponse
                {
                    Id = x.Id,
                    Score = x.Score,
                    CorrectsCount = x.CorrectsCount,
                    UsedTime = new TestTime
                    {
                        Minutes = x.UsedTime.Minutes,
                        Seconds = x.UsedTime.Seconds
                    },
                    StartTime = x.StartTime,
                    UserInfo = x.UserInfo,
                    QuestionSetId = x.QuestionSetId
                }).ToListAsync();
                return new ApiResult(testResults);
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResult> Create(TestResultCreateRequest request)
        {
            try
            {
                //chỉnh múi giờ
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                request.StartTime = TimeZoneInfo.ConvertTimeFromUtc(request.StartTime, cstZone);

                var testResult = new TestResult
                {
                    Id = Guid.NewGuid(),
                    Score = request.Score,
                    CorrectsCount = request.CorrectsCount,
                    UsedTime = new TimeSpan(0, request.UsedTime.Minutes, request.UsedTime.Seconds),
                    StartTime = request.StartTime,
                    UserInfo = request.UserInfo,
                    QuestionSetId = request.QuestionSetId
                };
                await _context.TestResults.AddAsync(testResult);
                await _context.SaveChangesAsync();
                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApiResult> RemoveAll()
        {
            try
            {
                _context.TestResults.RemoveRange(_context.TestResults);
                await _context.SaveChangesAsync();
                return new ApiResult();
            }
            catch (Exception e)
            {
                return new ApiResult(e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
