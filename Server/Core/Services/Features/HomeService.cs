using Core.Common;
using Core.DTOs;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Features;
using Core.Mappers;

namespace Core.Services.Features
{
    public class HomeService : IHomeService
    {
        private readonly ITestRepository _testRepository;
        private readonly IUserRepository _userRepository;

        public HomeService(ITestRepository testRepository, IUserRepository userRepository)
        {
            _testRepository = testRepository;
            _userRepository = userRepository;
        }

        public async Task<APIResult<List<TestItemDTO>>> GetRandomTests(int amount)
        {
            var tests = await _testRepository.GetAll();
            Random random = new ();
            var data = tests
                .OrderBy(x => random.Next())
                .Take(amount)
                .Select(x => TestMapper.MapToItem(x))
                .ToList();
            return new APISuccessResult<List<TestItemDTO>>(data);
        }

        public async Task<APIResult<List<TestItemDTO>>> GetTopTests(int amount)
        {
            var tests = await _testRepository.GetTopByAttempt(amount);
            return new APISuccessResult<List<TestItemDTO>>(tests.Select(e => TestMapper.MapToItem(e)).ToList());
        }

        public async Task<APIResult<List<UserInfoDTO>>> GetTopUser(int amount)
        {
            var users = await _userRepository.GetTopByLikes(amount);
            return new APISuccessResult<List<UserInfoDTO>>(users.Select(e => UserMapper.MapToInfo(e)).ToList());
        }
    }
}
