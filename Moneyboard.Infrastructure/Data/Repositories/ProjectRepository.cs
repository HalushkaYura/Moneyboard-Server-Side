using Microsoft.EntityFrameworkCore;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Interfaces.Repository;

namespace Moneyboard.Infrastructure.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly MoneyboardDb _dbContext;

        public ProjectRepository(MoneyboardDb context)
        {
            _dbContext = context;
        }


        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(string userId)
        {
            return await _dbContext.UserProject
                .Where(up => up.UserId == userId)
                .Select(up => up.Project)
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
