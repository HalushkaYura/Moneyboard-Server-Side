using System.Threading.Tasks;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface ITemplateService
    {
        Task<string> GetTemplateHtmlAsStringAsync<T>(string viewName, T model) where T : class, new();
    }
}
