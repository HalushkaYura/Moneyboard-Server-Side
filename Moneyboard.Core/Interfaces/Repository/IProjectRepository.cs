using Moneyboard.Core.Entities.ProjectEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Interfaces.Repository
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(string userId);
        Task<IEnumerable<Project>> GetProjectsByIdsAsync(IEnumerable<int> projectIds);

    }
}
