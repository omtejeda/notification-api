using System.Linq;
using System.Collections.Generic;
using NotificationService.Domain.Enums;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.Interfaces.Factories;

namespace NotificationService.Application.Notifications.Factories
{
    public class ExportNotificationsFactory : IExportNotificationsFactory
    {
    
        private readonly IEnumerable<IExportNotificationsService> _exportNotificationsServices;
        public ExportNotificationsFactory(IEnumerable<IExportNotificationsService> exportNotificationsServices)
        {
            _exportNotificationsServices = exportNotificationsServices;
        }

        public IExportNotificationsService Create(ExportFormat exportFormat)
        {
           return _exportNotificationsServices.FirstOrDefault(x => x.ExportFormat == exportFormat);
        }
    }
}