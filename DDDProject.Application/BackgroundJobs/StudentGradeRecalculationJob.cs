
using DDDProject.Common.Localization;
using Microsoft.Extensions.Logging;
using DDDProject.Common.Resources;

namespace DDDProject.Application.BackgroundJobs
{
    public class StudentGradeRecalculationJob
    {
        private readonly ILogger<StudentGradeRecalculationJob> _logger;
        private readonly SharedLocalizer _localizer;

        public StudentGradeRecalculationJob(
            ILogger<StudentGradeRecalculationJob> logger,
            SharedLocalizer localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _logger.LogInformation(_localizer["StartingGradeRecalculation"]);
                
                _logger.LogInformation(_localizer["CompletedGradeRecalculation"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _localizer["ErrorDuringGradeRecalculation"]);
                throw;
            }
        }
    }
}