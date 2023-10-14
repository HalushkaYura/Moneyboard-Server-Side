using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Interfaces.Repository
{
    public interface IUserProjectRepository
    {
        Task<UserProject> GetUserProjectAsync(string userId, int projectId);
        Task<IEnumerable<UserProject>> GetProjectsForUserAsync(string userId);
        Task<IEnumerable<Project>> GetProjectsByIdsAsync(IEnumerable<int> projectIds);
    }
}
