using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Common;
using Core.Interfaces.IServices.Features;
using Core.Interfaces.IServices.System;
using Core.Mappers;
using Data.Entities.TestEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Core.Services.Features
{
    public class TestService : ITestService
    {
        private const string IMAGE_REQUEST_PATH = "/api/images/test/";
        private readonly string _imageFolderPath;

        private readonly ITestRepository _testRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileService _fileService;
        private readonly ISummaryService _summaryService;
        private readonly ILogger<TestService> _logger;
        private readonly ILogService _logService;
        private readonly INotificationService _notificationService;

        public TestService(ITestRepository testRepository, IFileService fileService, ISummaryService summaryService, ICategoryRepository categoryRepository, ILogger<TestService> logger, ILogService logHubService, INotificationService notificationService)
        {
            _imageFolderPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Images", "Test");
            _testRepository = testRepository;
            _fileService = fileService;
            _summaryService = summaryService;
            _categoryRepository = categoryRepository;
            _logger = logger;
            _logService = logHubService;
            _notificationService = notificationService;
        }

        public async Task<APIResult> CreateTest(ClaimsPrincipal claimsPrincipal, CreateTestDTO dto)
        {
            //check if test name existed
            if (await _testRepository.IsExist(x => x.Name.Equals(dto.Name)))
                throw new ErrorException("Tên đề trắc nghiệm bị trùng");

            //check if questions list is empty
            if (dto.Questions == null)
                throw new ErrorException("Danh sách câu hỏi trống");

            //create new question set
            var test = TestMapper.MapFromCreate(dto);
            test.AuthorName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)!.Value;
            test.AuthorId = Guid.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            test.CategoryId = (await _categoryRepository.Get(e => e.Slug.Equals(dto.CategorySlug)))!.Id;
            if(dto.Image != null)
            {
                test.ImageUrl = IMAGE_REQUEST_PATH + (await _fileService.Save(dto.Image, _imageFolderPath));
            }
            else
            {
                test.ImageUrl = null;
            }

            //create questions
            var questions = new List<Question>();
            int order = 1;
            foreach (var question in dto.Questions)
            {
                var newQuestion = new Question
                {
                    Id = Guid.NewGuid(),
                    Content = question.Content,
                    Order = order++,
                    AnswerA = question.AnswerA,
                    AnswerB = question.AnswerB,
                    AnswerC = question.AnswerC,
                    AnswerD = question.AnswerD,
                    CorrectAnswer = question.CorrectAnswer,
                    TestId = test.Id,
                };
                if (question.Image != null)
                {
                    newQuestion.ImageUrl = IMAGE_REQUEST_PATH + (await _fileService.Save(question.Image, _imageFolderPath));
                }
                else
                {
                    newQuestion.ImageUrl = null;
                } 
                    
                questions.Add(newQuestion);
            }
            test.Questions = questions;

            _testRepository.Create(test);
            if(await _testRepository.SaveChanges())
            {
                // Log
                _logger.LogInformation($"Đề trắc nghiệm mới được tạo bởi {test.AuthorName}");
                await _logService.SendInfoLog($"Đề trắc nghiệm mới được tạo bởi {test.AuthorName}");

                return new APISuccessResult("Tạo đề thành công");
            } 
            else 
            {
                return new APIErrorResult("Tạo đề thất bại");
            }
        }

        public async Task<APIResult> DeleteFromSaved(ClaimsPrincipal claimsPrincipal, string testId)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _testRepository.UnSave(userId, testId);
            if (await _testRepository.SaveChanges())
            {
                return new APISuccessResult("Bỏ lưu thành công");
            }
            else
            {
                return new APIErrorResult("Bỏ lưu thất bại");
            }
        }

        public async Task<APIResult> DeleteTest(ClaimsPrincipal claimsPrincipal, string testId)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var test = await _testRepository.GetById(Guid.Parse(testId))
                ?? throw new ErrorException("Đề trắc nghiệm không tồn tại");

            if (!test.AuthorId.ToString().Equals(userId))
            {
                if(!claimsPrincipal.IsInRole(Consts.ADMIN_ROLE))
                {
                    throw new ForbiddenException();
                }
            }  

            // remove all image
            if(test.ImageUrl != null)
            {
                await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(test.ImageUrl)));
            }
            foreach (var q in await _testRepository.GetQuestions(testId))
            {
                if(q.ImageUrl != null)
                {
                    await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(q.ImageUrl)));
                }
            }

            _testRepository.Delete(test);
            if (await _testRepository.SaveChanges())
            {
                return new APISuccessResult("Xóa đề thành công");
            }
            else
            {
                return new APIErrorResult("Xóa đề thất bại");
            }
        }

        public async Task<APIResult<PaginatedResult<TestResult>>> GetAllResults(TestResultSearchDTO data)
        {
            var query = _testRepository.GetResultQuery().AsNoTracking();

            //filter
            if(!string.IsNullOrEmpty(data.UserName))
            {
                query = query.Where(e => e.UserName.Equals(data.UserName, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(data.TestName))
            {
                query = query.Where(e => e.TestName.Equals(data.TestName, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(data.StartTime))
            {
                query = query.Where(e => DateTime.Parse(e.StartTime).CompareTo(DateTime.Parse(data.StartTime)) >= 0);
            }
            if (!string.IsNullOrEmpty(data.EndTime))
            {
                query = query.Where(e => DateTime.Parse(e.EndTime).CompareTo(DateTime.Parse(data.EndTime)) <= 0);
            }

            //paging
            query = query
                .Skip((data.PageIndex - 1) * data.PageSize)
                .Take(data.PageSize)
                .OrderByDescending(e => e.StartTime);

            return new APISuccessResult<PaginatedResult<TestResult>>(new PaginatedResult<TestResult>
            {
                Total = await query.CountAsync(),
                Data = await query.ToListAsync()
            });
        }

        public async Task<APIResult<List<TestItemDTO>>> GetSavedTestsOfUser(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var rawData = await _testRepository.GetSavedTestsOfUser(userId);
            return new APISuccessResult<List<TestItemDTO>> (rawData.Select(e => TestMapper.MapToItem(e)).ToList());
        }

        public async Task<APIResult<TestDTO>> GetTestContent(ClaimsPrincipal claimsPrincipal, string testId)
        {
            var test = await _testRepository.GetById(Guid.Parse(testId))
                ?? throw new ErrorException("Đề không tồn tại");

            if (test.IsPrivate)
            {
                var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(userId == null || !userId.Equals(test.AuthorId.ToString()))
                {
                    throw new ForbiddenException();
                }
            }

            var questions = await _testRepository.GetQuestions(testId);
            questions.Sort((a, b) => a.Order.CompareTo(b.Order));
            if(questions.Count == 0)
            {
                throw new ErrorException("Đề trống (không có câu hỏi)");
            }

            return new APISuccessResult<TestDTO>(TestMapper.MapToContent(test, questions));
        }

        public async Task<APIResult<TestInfoDTO>> GetTestInfo(string testId)
        {
            var test = await _testRepository.GetById(Guid.Parse(testId))
                ?? throw new ErrorException("Đề không tồn tại");
            var info = TestMapper.MapToInfo(test);
            info.CategoryName = (await _categoryRepository.Get(e => e.Slug.Equals(test.CategorySlug)))!.Name;
            return new APISuccessResult<TestInfoDTO>(info);
        }

        public async Task<APIResult<List<TestInfoDTO>>> GetTestInfosOfUser(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var tests = await _testRepository.GetAll(x => x.AuthorId.ToString().Equals(userId));
            return new APISuccessResult<List<TestInfoDTO>>(tests.Select(x => TestMapper.MapToInfo(x)).ToList());
        }

        public async Task<APIResult<List<TestResult>>> GetTestResultsOfUser(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var results = await _testRepository.GetResultsByUserId(userId);
            return new APISuccessResult<List<TestResult>>(results);
        }

        public async Task<APIResult<UpdateTestDTO>> GetTestUpdateContent(ClaimsPrincipal claimsPrincipal, string testId)
        {
            var test = await _testRepository.GetById(Guid.Parse(testId))
                ?? throw new ErrorException("Đề không tồn tại");
            var questions = await _testRepository.GetQuestions(testId);
            questions.Sort((a, b) => a.Order.CompareTo(b.Order));
            return new APISuccessResult<UpdateTestDTO>(TestMapper.MapToUpdate(test, questions));
        }

        public async Task<APIResult<bool>> IsSaved(ClaimsPrincipal claimsPrincipal, string testId)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _testRepository.IsSaved(userId, testId);
            return new APISuccessResult<bool>(result);
        }

        public async Task<APIResult<TestResultDTO>> MarkTest(ClaimsPrincipal claimsPrincipal, MarkTestDTO dto, string ip)
        {
            var test = await _testRepository.GetById(Guid.Parse(dto.TestId))
                ?? throw new ErrorException("Đề không tồn tại");

            test.NumberOfAttempts++;

            var questions = await _testRepository.GetQuestions(test.Id.ToString());

            //mark correct ans
            Dictionary<string, int> map = new();
            foreach (var e in questions)
            {
                map.Add(e.Id.ToString(), e.CorrectAnswer);
            }

            //count number of correct and unselected ans
            int correct = 0;
            int unselected = 0;
            foreach (var e in dto.Answers)
            {
                if (e.Selected == 0)
                {
                    unselected++;
                    continue;
                }

                if (e.Selected == map[e.Id.ToLower()])
                {
                    correct++;
                }

            }

            //calculate used time
            var start = DateTime.Parse(dto.StartTime).AddHours(-7);
            var end = DateTime.Parse(dto.EndTime).AddHours(-7); ;
            var duration = end - start;

            //calculate score and save result
            double score = (double)correct / questions.Count * 10;

            var result = new TestResult
            {
                Id = Guid.NewGuid(),
                Score = (decimal)score,
                Correct = correct,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                UsedTime = (int)duration.TotalSeconds,
                TestId = Guid.Parse(dto.TestId),
                TestName = dto.TestName,
                UserId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? ip,
                UserName = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "undefined"
            };
            _testRepository.SaveResult(result);

            //summary
            await _summaryService.IncreaseTestCompletionCount();

            if (await _testRepository.SaveChanges())
            {
                return new APISuccessResult<TestResultDTO>("Lưu kết quả thành công", new TestResultDTO
                {
                    Total = questions.Count,
                    Score = score,
                    Correct = correct,
                    Unselected = unselected,
                    UsedTime = (int)duration.TotalSeconds,
                    Detail = dto.Answers.Select(x => map[x.Id.ToLower()]).ToList()
                });
            }
            else
            {
                throw new ErrorException("Lưu kết quả thất bại");
            }
        }

        public async Task<APIResult> SaveTest(ClaimsPrincipal claimsPrincipal, string testId)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var savedTest = new SavedTest
            {
                UserId = Guid.Parse(userId),
                TestId = Guid.Parse(testId),
                MarkedAt = DateOnly.FromDateTime(DateTime.Today).ToString(),
            };
            _testRepository.SaveTest(savedTest);

            if (await _testRepository.SaveChanges())
            {
                return new APISuccessResult("Lưu đề thành công");
            }
            else
            {
                return new APIErrorResult("Lưu đề thất bại");
            }
        }

        public async Task<APIResult<PaginatedResult<TestItemDTO>>> GetAsItems(TestSearchDTO data)
        {
            var query = _testRepository.GetQuery().AsNoTracking();

            //filter
            if (!string.IsNullOrEmpty(data.CategorySlug))
            {
                query = query.Where(e => e.CategorySlug.Equals(data.CategorySlug));
            }
            if (!string.IsNullOrEmpty(data.Name))
            {
                query = query.Where(e => e.Name.Equals(data.Name, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(data.Description))
            {
                query = query.Where(e => e.Description.Equals(data.Description, StringComparison.OrdinalIgnoreCase));
            }

            //paging
            query = query
                .Skip((data.PageIndex - 1) * data.PageSize)
                .Take(data.PageSize)
                .OrderByDescending(e => e.NumberOfAttempts);

            return new APISuccessResult<PaginatedResult<TestItemDTO>>(new PaginatedResult<TestItemDTO>
            {
                Total = await query.CountAsync(),
                Data = await query.Select(e => TestMapper.MapToItem(e)).ToListAsync()
            });
        }

        public async Task<APIResult> UpdateTest(ClaimsPrincipal claimsPrincipal, string testId, UpdateTestDTO dto)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var test = await _testRepository.GetById(Guid.Parse(testId))
                ?? throw new ErrorException("Đề không tồn tại");

            if (!test.AuthorId.ToString().Equals(userId))
                throw new ForbiddenException();

            //update test
            test = TestMapper.MapFromUpdate(test, dto);
            test.CategoryId = (await _categoryRepository.Get(e => e.Slug.Equals(dto.CategorySlug)))!.Id;
            if (dto.Image != null)
            {
                if (test.ImageUrl != null)
                {
                    await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(test.ImageUrl)));
                }
                test.ImageUrl = IMAGE_REQUEST_PATH + await _fileService.Save(dto.Image, _imageFolderPath);
            }

            //update question
            Dictionary<string, UpdateQuestionDTO> map = new();
            foreach (var e in dto.Questions)
            {
                // if id is null, it is new question
                if (e.Id == null)
                {
                    var newQ = new Question
                    {
                        Id = Guid.NewGuid(),
                        Content = e.Content,
                        AnswerA = e.AnswerA,
                        AnswerB = e.AnswerB,
                        AnswerC = e.AnswerC,
                        AnswerD = e.AnswerD,
                        CorrectAnswer = e.CorrectAnswer,
                        TestId = test.Id,
                    };
                    if (e.Image != null)
                    {
                        newQ.ImageUrl = IMAGE_REQUEST_PATH + (await _fileService.Save(e.Image, _imageFolderPath));
                    }
                    else
                    {
                        newQ.ImageUrl = null;
                    }
                    _testRepository.AddQuestion(newQ);
                }
                else
                {
                    map.Add(e.Id, e);
                }
            }
            var oldQuestions = await _testRepository.GetQuestions(testId);
            foreach (var e in oldQuestions)
            {
                // update old question or delete if not exists
                if (map.TryGetValue(e.Id.ToString(), out var updated))
                {
                    if(updated.Image != null)
                    {
                        if (e.ImageUrl != null)
                        {
                            await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(e.ImageUrl)));
                        }
                        e.ImageUrl = IMAGE_REQUEST_PATH + await _fileService.Save(updated.Image, _imageFolderPath);
                    } else if(updated.ImageUrl == null && e.ImageUrl != null)
                    {
                        await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(e.ImageUrl)));
                        e.ImageUrl = null;
                    }
                    e.Content = updated.Content;
                    e.AnswerA = updated.AnswerA;
                    e.AnswerB = updated.AnswerB;
                    e.AnswerC = updated.AnswerC;
                    e.AnswerD = updated.AnswerD;
                    e.CorrectAnswer = updated.CorrectAnswer;
                    _testRepository.UpdateQuestion(e);
                }
                else
                {
                    if (e.ImageUrl != null)
                    {
                        await _fileService.Delete(Path.Combine(_imageFolderPath, Path.GetFileName(e.ImageUrl)));
                    }
                    _testRepository.RemoveQuestion(e);
                }
            }

            if (await _testRepository.SaveChanges())
            {
                return new APISuccessResult("Cập nhật đề thành công");
            }
            else
            {
                return new APIErrorResult("Cập nhật đề thất bại");
            }
        }

        public async Task<APIResult<PaginatedResult<TestInfoDTO>>> GetAsInfos(TestSearchDTO data)
        {
            var query = _testRepository.GetQuery().AsNoTracking();

            //filter
            if (!string.IsNullOrEmpty(data.CategorySlug))
            {
                query = query.Where(e => e.CategorySlug.Equals(data.CategorySlug));
            }
            if (!string.IsNullOrEmpty(data.Name))
            {
                query = query.Where(e => e.Name.Equals(data.Name, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(data.Description))
            {
                query = query.Where(e => e.Description.Equals(data.Description, StringComparison.OrdinalIgnoreCase));
            }

            //paging
            query = query
                .Skip((data.PageIndex - 1) * data.PageSize)
                .Take(data.PageSize)
                .OrderByDescending(e => e.NumberOfAttempts);

            return new APISuccessResult<PaginatedResult<TestInfoDTO>>(new PaginatedResult<TestInfoDTO>
            {
                Total = await query.CountAsync(),
                Data = await query.Select(e => TestMapper.MapToInfo(e)).ToListAsync()
            });
        }
    }
}
