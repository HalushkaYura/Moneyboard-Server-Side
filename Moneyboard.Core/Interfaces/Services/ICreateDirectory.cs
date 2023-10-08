using System.Threading.Tasks;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface ICreateDirectory
    {
        Task CreateDirectoryAsync(string folderPath);
    }
}
