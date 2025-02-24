using DDDProject.Application.BackgroundJobs;
using Hangfire;

namespace DDDProject.Infrastructure.BackgroundJobs
{
    public class HangfireJobScheduler
    {
        public static void ScheduleRecurringJobs()
        {
            RecurringJob.AddOrUpdate<StudentGradeRecalculationJob>(
                "RecalculateGrades",
                job => job.ExecuteAsync(),
                Cron.Hourly);

            RecurringJob.AddOrUpdate<EnrollmentNotificationJob>(
                "EnrollmentNotifications",
                job => job.ExecuteAsync(),
                Cron.Daily);
        }
    }
}