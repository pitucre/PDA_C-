using Mobile.PrinxChengShan.Bll;
//using Quartz;
//using Quartz.Impl;
using System;

/// <summary>
/// JobSchedulingPlan 的摘要说明
/// </summary>
public class JobSchedulingPlan
{
   // protected IScheduler sched;
    /// <summary>
    /// 调度计划主线程   0 59 22 * * ?
    /// </summary>
    public void Start()
    {
        try
        {
           /* ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler sched = schedFact.GetScheduler();
            //开始任务
            sched.Start();
            IJobDetail job = JobBuilder.Create<JobTrigger>().WithIdentity("myJob", "groupSid").Build();         
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("myTrigger", "groupSid")   
                .WithCronSchedule("0 0 2 * * ?")
                .Build();
            sched.ScheduleJob(job, trigger);*/
        }
        catch (Exception ex)
        {
            SystemErrorPlug.ErrorRecord(ex.ToString());
        }
    }
}
