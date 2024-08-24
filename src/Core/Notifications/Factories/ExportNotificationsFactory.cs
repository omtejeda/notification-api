using System.Linq;
using System.Collections.Generic;
using NotificationService.Core.Common.Enums;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Contracts.Interfaces.Factories;

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