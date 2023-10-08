using Moneyboard.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Helpers
{
    public class ProjectContext : IProjectContext
    {
        public int ActiveProjectId { get; set; }

    }
}
