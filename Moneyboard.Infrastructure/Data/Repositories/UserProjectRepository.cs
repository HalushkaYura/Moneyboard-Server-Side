using Microsoft.EntityFrameworkCore;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using Moneyboard.Core.Interfaces.Repository;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
namespace Moneyboard.Infrastructure.Data.Repositories
{
    public class UserProjectRepository : IUserProjectRepository
    {
        protected readonly MoneyboardDb _dbContext;
        private readonly ILogger<UserProjectRepository> _logger;

        public UserProjectRepository(ILogger<UserProjectRepository> logger, MoneyboardDb dbContext)
        {
            _logger = logger; 
            _dbContext = dbContext;

        }
    public UserProjectRepository(ILogger<UserProjectRepository> logger)
    {
        _logger = logger;
    }

        public async Task<UserProject> GetUserProjectAsync(string userId, int projectId)
        {
            // Ініціалізуйте об'єкт логування
            System.Diagnostics.Debug.WriteLine("Запуск GetUserProjectAsync для userId: {userId} та projectId: {projectId}", userId, projectId);

            // Виконайте запит до бази даних
            var userProject = await _dbContext.UserProject
                .Where(up => up.UserId == userId && up.ProjectId == projectId)
                .FirstOrDefaultAsync();

            // Логування результату
            if (userProject != null)
            {
                System.Diagnostics.Debug.WriteLine("Знайдено UserProject з Id: {userProjectId}", userProject.UserProjectId);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("UserProject не знайдено для userId: {userId} та projectId: {projectId}", userId, projectId);
            }

            return userProject;
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
