using Core.Common;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Common;
using Core.Interfaces.IServices.Features;
using Core.Mappers;
using Data.Entities;
using System.Security.Claims;

namespace Core.Services.Features
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        private readonly IFileService _fileService;

        public TestService(ITestRepository testRepository, IFileService fileService)
        {
            _testRepository = testRepository;
            _fileService = fileService;
        }

        public async Task<APIResult> CreateTest(ClaimsPrincipal claimsPrincipal, CreateTestDTO dto)
        {
            //check if test name existed
            if (await _testRepository.IsExist(x => x.Name.Equals(dto.Name)))
                throw new ErrorException("Tên đề trắc nghiệm bị trùng");

            //create new question set
            var test = TestMapper.MapFromCreate(dto);
            test.AuthorName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)!.Value;
            test.AuthorId = Guid.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            //create questions
            var questions = new List<Question>();
            if (dto.Questions == null)
                throw new ErrorException("Danh sách câu hỏi trống");

            foreach (var question in dto.Questions)
            {
                var newQuestion = new Question
                {
                    Id = Guid.NewGuid(),
                    Content = question.Content,
                    ImageUrl = question.ImageUrl,
                    AnswerA = question.AnswerA,
                    AnswerB = question.AnswerB,
                    AnswerC = question.AnswerC,
                    AnswerD = question.AnswerD,
                    CorrectAnswer = question.CorrectAnswer,
                    TestId = test.Id,
                };
                questions.Add(newQuestion);
            }
            test.Questions = questions;

            _testRepository.Create(test);
            if(await _testRepository.SaveChanges())
            {
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
                throw new ForbiddenException();

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

        public async Task<APIResult<PaginatedResult<TestResult>>> GetAllResults(int pageSize, int pageIndex, List<ExpressionFilter> filters)
        {
            var data = await _testRepository.GetAllResults(pageSize, pageIndex, filters);
            return new APISuccessResult<PaginatedResult<TestResult>>(data);
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

            var question = await _testRepository.GetQuestions(testId);
            if(question.Count == 0)
            {
                throw new ErrorException("Đề trống (không có câu hỏi)");
            }

            return new APISuccessResult<TestDTO>(TestMapper.MapToContent(test, question));
        }

        public async Task<APIResult<TestInfoDTO>> GetTestInfo(string testId)
        {
            var test = await _testRepository.GetById(Guid.Parse(testId))
                ?? throw new ErrorException("Đề không tồn tại");

            return new APISuccessResult<TestInfoDTO>(TestMapper.MapToInfo(test));
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

        public async Task<APIResult<PaginatedResult<TestItemDTO>>> GetTestsAsListItems(int pageSize, int pageIndex, List<ExpressionFilter> filters)
        {
            var tests = await _testRepository.GetPaginatedData(pageSize, pageIndex, filters);
            return new APISuccessResult<PaginatedResult<TestItemDTO>>(new PaginatedResult<TestItemDTO>
            {
                Total = tests.Total,
                Data = tests.Data.Select(x => TestMapper.MapToItem(x))
            });
        }

        public async Task<APIResult<UpdateTestDTO>> GetTestUpdateContent(ClaimsPrincipal claimsPrincipal, string testId)
        {
            var test = await _testRepository.GetById(Guid.Parse(testId))
                ?? throw new ErrorException("Đề không tồn tại");
            var question = await _testRepository.GetQuestions(testId);
            return new APISuccessResult<UpdateTestDTO>(TestMapper.MapToUpdate(test, question));
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

        public async Task<APIResult> UpdateTest(ClaimsPrincipal claimsPrincipal, string testId, UpdateTestDTO dto)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var test = await _testRepository.GetById(Guid.Parse(testId))
                ?? throw new ErrorException("Đề không tồn tại");

            if (!test.AuthorId.ToString().Equals(userId))
                throw new ForbiddenException();

            //update test
            await _fileService.DeleteFile(test.ImageUrl);
            test = TestMapper.MapFromUpdate(test, dto);

            //update question
            Dictionary<string, UpdateQuestionDTO> map = new();
            foreach (var e in dto.Questions)
            {
                if (e.Id == null)
                {
                    var newQ = new Question
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = e.ImageUrl,
                        Content = e.Content,
                        AnswerA = e.AnswerA,
                        AnswerB = e.AnswerB,
                        AnswerC = e.AnswerC,
                        AnswerD = e.AnswerD,
                        CorrectAnswer = e.CorrectAnswer,
                        TestId = test.Id,
                    };
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
                if (map.TryGetValue(e.Id.ToString(), out var updated))
                {
                    await _fileService.DeleteFile(e.ImageUrl);
                    e.ImageUrl = updated.ImageUrl;
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
    }
}
