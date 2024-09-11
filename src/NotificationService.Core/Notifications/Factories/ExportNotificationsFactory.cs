using System.Linq;
using System.Collections.Generic;
using NotificationService.Domain.Enums;
using NotificationService.Core.Contracts.Interfaces.Services;
using NotificationService.Core.Contracts.Interfaces.Factories;

namespace NotificationService.Core.Notifications.Factories
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