using System.Threading.Tasks;
using NotificationService.Core.Providers.Interfaces;

namespace NotificationService.Core.Providers.Factories.Interfaces
{
    public interface IEmailProviderFactory
    {
        Task<IEmailProvider> CreateProviderAsync(string providerName, string createdBy);
    }
}