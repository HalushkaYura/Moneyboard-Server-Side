using Microsoft.EntityFrameworkCore;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using Moneyboard.Core.Interfaces.Repository;

namespace Moneyboard.Infrastructure.Data.Repositories
{
    public class UserProjectRepository : IUserProjectRepository
    {
        protected readonly MoneyboardDb _dbContext;
        public UserProjectRepository(MoneyboardDb dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UserProject> GetUserProjectAsync(string userId, int projectId)
        {
            return await _dbContext.UserProject
                .Where(up => up.UserId == userId && up.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserProject>> GetProjectsForUserAsync(string userId)
        {
            return await _dbContext.UserProject
                .Where(up => up.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByIdsAsync(IEnumerable<int> projectIds)
        {
            return await _dbContext.Project
                .Where(p => projectIds.Contains(p.ProjectId))
                .ToListAsync();
        }
    }
}
