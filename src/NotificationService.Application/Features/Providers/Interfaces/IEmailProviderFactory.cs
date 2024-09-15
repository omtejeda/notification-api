namespace NotificationService.Application.Features.Providers.Interfaces
{
    public interface IEmailProviderFactory
    {
        Task<IEmailProvider> CreateProviderAsync(string providerName, string createdBy);
    }
}