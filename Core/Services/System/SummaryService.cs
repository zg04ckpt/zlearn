using Core.Common;
using Core.DTOs;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.System;
using Core.Repositories;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.System
{
    public class SummaryService : ISummaryService
    {
        private readonly IServiceProvider _serviceProvider;
        private int _userCount;
        private int _testCompletionCount;
        private int _commentCount;
        private ConcurrentDictionary<string, int> _ipCounter;
        private Timer _timer;

        public SummaryService(IServiceProvider serviceProvider)
        {
            _ipCounter = new ConcurrentDictionary<string, int>();
            var timeline = DateTime.Now.Date;
            if (timeline < DateTime.Now)
            {
                timeline = timeline.AddDays(1);
            }
            _timer = new Timer(Reset, null, timeline - DateTime.Now, TimeSpan.FromDays(1));
            _serviceProvider = serviceProvider;
        }

        public async void Reset(object state)
        {
            await SaveToDatabase();
            _userCount = 0;
            _testCompletionCount = 0;
            _commentCount = 0;
            _ipCounter.Clear();
        }

        public async Task<APIResult<SummaryDTO>> GetByRange(string start, string end)
        {
            using var _scope = _serviceProvider.CreateScope();
            var summaryRepository = _scope.ServiceProvider.GetRequiredService<ISummaryRepository>();

            var startDate = DateTime.Parse(start).Date;
            var endDate = DateTime.Parse(end).Date;
            var summaries = await summaryRepository.GetByRange(startDate, endDate);
            var data = new SummaryDTO();
            foreach (var item in summaries)
            {
                data.AccessCount += item.AccessCount;
                data.TestCompletionCount += item.TestCompletionCount;
                data.UserCount += item.UserCount;
                data.CommentCount += item.CommentCount;
            }
            if (endDate.Equals(DateTime.Today))
            {
                data.AccessCount += _ipCounter.Count;
                data.TestCompletionCount += _testCompletionCount;
                data.UserCount += _userCount;
                data.CommentCount += _commentCount;
            }
            return new APISuccessResult<SummaryDTO>(data);
        }

        public APIResult<SummaryDTO> GetToday()
        {
            var data = new SummaryDTO
            {
                AccessCount = _ipCounter.Count,
                TestCompletionCount = _testCompletionCount,
                UserCount = _userCount,
                CommentCount = _commentCount
            };
            return new APISuccessResult<SummaryDTO>(data);
        }

        public Task IncreaseAccessCount(string ip)
        {
            _ipCounter.AddOrUpdate(ip, 1, (key, count) => count + 1);
            return Task.CompletedTask;
        }

        public Task IncreaseCommentCount()
        {
            Interlocked.Increment(ref _commentCount);
            return Task.CompletedTask;
        }

        public Task IncreaseRequestCount()
        {
            return Task.CompletedTask;
        }

        public Task IncreaseTestCompletionCount()
        {
            Interlocked.Increment(ref _testCompletionCount);
            return Task.CompletedTask;
        }

        public Task IncreaseUserCount()
        {
            Interlocked.Increment(ref _userCount);
            return Task.CompletedTask;
        }

        private async Task SaveToDatabase()
        {
            using var _scope = _serviceProvider.CreateScope();
            var summaryRepository = _scope.ServiceProvider.GetRequiredService<ISummaryRepository>();
            await summaryRepository.SaveToDatabase(new Summary
            {
                Date = DateTime.Today.AddDays(-1),
                AccessCount = _ipCounter.Count,
                CommentCount = _commentCount,
                UserCount = _userCount,
                TestCompletionCount = _testCompletionCount
            });
        }
    }
}
