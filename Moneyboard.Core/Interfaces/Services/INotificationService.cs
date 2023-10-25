using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface INotificationService
    {
        Task SendNotificationToUser(string userId, string message);
    }
}
