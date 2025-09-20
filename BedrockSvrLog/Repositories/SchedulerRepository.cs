using Quartz;
using Quartz.Impl;

namespace BedrockSvrLog.Repositories;

/* Scheduler

- Create tasks in the createJobs method add them to the list

- Create the task as a seperate class that implements IJob

- Time configuration format:

 ┌───────────── second (0 - 59)
 │ ┌───────────── minute (0 - 59)
 │ │ ┌───────────── hour (0 - 23)
 │ │ │ ┌───────────── day of the month (1 - 31)
 │ │ │ │ ┌───────────── month (1 - 12)
 │ │ │ │ │ ┌───────────── day of the week (0 - 6) (Sunday to Saturday;
 │ │ │ │ │ │                                       7 is also Sunday on some systems)
 │ │ │ │ │ │ ┌───────────── Year (optional)
 │ │ │ │ │ │ │
 * * * * * * *
 

Example 0 0 6 ? * SUN - At 06:00 AM, only on Sunday

  * = all values
  ? = no specific value

see https://www.freeformatter.com/cron-expression-generator-quartz.html

 */

public class SchedulerRepository
{
    private IScheduler? scheduler;

    public IList<Job> JobList { get; set; } = new List<Job>();

    private readonly NewsPaperRepository _newsPaperRepository;
    public static NewsPaperRepository? StaticNewsPaperRepository { get; set; }
    
    public SchedulerRepository(NewsPaperRepository newsPaperRepository)
    {
        _newsPaperRepository = newsPaperRepository;
        StaticNewsPaperRepository = newsPaperRepository;
    }

    public async Task StartScheduler()
    {
        try
        {
            // Create scheduler
            StdSchedulerFactory factory = new StdSchedulerFactory();
            scheduler = await factory.GetScheduler();
            await scheduler.Start();

            CreateJobs();

            CreateScheduledJobs();

            CreateJobTriggers();

            await ScheduleJobsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Scheduler Error: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }

    public string GetStringOfJobs()
    {
        if (JobList.Count == 0)
        {
            return "No scheduled jobs.";
        }
        var jobDescriptions = JobList.Select(job =>
            $"Job Name: {job.Name}, Group: {job.Group}, Trigger Time: {job.TriggerTime}");

        return string.Join(Environment.NewLine, jobDescriptions);
    }

    // Create scheduled jobs here
    private void CreateJobs()
    {
        JobList.Add(
            new Job
            {
                Name = "newspaperJob",
                Group = "group1",
                TriggerTime = "* * 7 ? * SAT",  // At 07:00 AM, only on Saturday
                Ijob = new NewspaperJob()
            });
    }

    private async Task ScheduleJobsAsync()
    {
        foreach (var job in JobList)
        {
            if (job.JobDetail == null || job.Trigger == null || scheduler == null) 
            {
                continue;
            }
            
            try
            {
                await scheduler.ScheduleJob(job.JobDetail, job.Trigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to schedule job {job.Name}: {ex.Message}");
            }
        }
    }

    private void CreateJobTriggers()
    {
        foreach (var job in JobList)
        {
            if (job.JobDetail == null || scheduler == null) continue;

            job.Trigger = TriggerBuilder.Create()
                .WithIdentity($"{job.Name}Trigger", job.Group)
                .WithCronSchedule(job.TriggerTime)
                .ForJob(job.JobDetail)
                .Build();
        }
    }

    private void CreateScheduledJobs()
    {
        foreach (var job in JobList)
        {
            job.JobDetail = JobBuilder.Create(job.Ijob.GetType())
                .WithIdentity(job.Name, job.Group)
                .Build();
        }
    }

    public record Job
    {
        public required string Name { get; init; }
        public required string Group { get; init; }
        public required string TriggerTime { get; init; }
        public required IJob Ijob { get; init; }
        public IJobDetail? JobDetail { get; set; }
        public ITrigger? Trigger { get; set; }
    }

    public class NewspaperJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                string logMessage = $"Newspaper task running at {DateTime.Now}";
                Console.WriteLine(logMessage);
                
                // Get the repository from static property
                if (StaticNewsPaperRepository != null)
                {
                    await StaticNewsPaperRepository.GeneratePaper(maxNumberOfArticles: 4, CancellationToken.None);
                }
                else
                {
                    Console.WriteLine("NewsPaperRepository is null - cannot generate paper");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"NewspaperJob error at {DateTime.Now}: {ex.Message}";
                Console.WriteLine(errorMessage);
            }
        }
    }

}
