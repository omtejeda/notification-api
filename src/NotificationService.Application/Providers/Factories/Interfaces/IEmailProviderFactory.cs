using System.Threading.Tasks;
using NotificationService.Application.Providers.Interfaces;

namespace NotificationService.Application.Providers.Factories.Interfaces
{
    public interface IEmailProviderFactory
    {
        Task<IEmailProvider> CreateProviderAsync(string providerName, string createdBy);
    }
}