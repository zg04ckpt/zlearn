using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.Features.Learn.Test.Question;
using ViewModels.System.Manage;

namespace Application.System.Manage
{
    public interface IAdminService
    {
        #region manage users
        Task<PagingResponse<UserManagementModel>> GetAllUsers(PagingRequest request);
        Task<UserManagementModel> GetUserById(string id);
        Task DeleteUser(string id);
        Task UpdateUser(UserManagementModel user);
        Task<List<string>> GetAllRolesOfUser(string userId);
        Task AssignRole(string userId, RoleAssignRequest request);
        Task<PagingResponse<UserManagementModel>> FindByUserName(string key, PagingRequest request);
        Task<PagingResponse<UserManagementModel>> FindByName(string key, PagingRequest request);
        Task<PagingResponse<UserManagementModel>> FindByEmail(string key, PagingRequest request);
        Task<PagingResponse<UserManagementModel>> FindByRole(string key, PagingRequest request);
        Task<PagingResponse<UserManagementModel>> FindByPhoneNum(string key, PagingRequest request);
        #endregion

        #region manage tests
        Task<PagingResponse<TestManagementModel>> GetTests(PagingRequest request);
        Task<PagingResponse<TestManagementModel>> GetTestsByUserId(string userId, PagingRequest request);
        Task<List<QuestionUpdateContent>> GetQuestions(string testId);
        Task UpdateTest(TestManagementModel request);
        Task UpdateQuestions(string testId, QuestionUpdateRequest request);
        Task DeleteTest(string testId);
        #endregion
    }
}
