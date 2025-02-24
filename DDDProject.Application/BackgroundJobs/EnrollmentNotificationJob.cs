using Microsoft.Extensions.Logging;
using DDDProject.Common.Localization;

namespace DDDProject.Application.BackgroundJobs
{
    public class EnrollmentNotificationJob
    {
        private readonly ILogger<EnrollmentNotificationJob> _logger;
        private readonly SharedLocalizer _localizer;

        public EnrollmentNotificationJob(
            ILogger<EnrollmentNotificationJob> logger,
            SharedLocalizer localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _logger.LogInformation(_localizer["StartingEnrollmentNotifications"]);
                
                
                _logger.LogInformation(_localizer["CompletedEnrollmentNotifications"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _localizer["ErrorDuringEnrollmentNotifications"]);
                throw;
            }
        }
    }
}