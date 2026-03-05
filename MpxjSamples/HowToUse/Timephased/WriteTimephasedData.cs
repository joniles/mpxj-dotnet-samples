using System;
using MPXJ.Net;

namespace MpxjSamples.HowToUse.Timephased;

public class WriteTimephasedData
{
    public void Execute(string outputFileName)  
    {
      var file = new ProjectFile();
      file.AddDefaultBaseCalendar();

      AddFlatDistributionTask(file);
      AddFlatDistributionPartiallyCompleteTask(file);
      AddFlatDistributionCompleteTask(file);

      AddCustomDistributionTask(file);
      AddCustomDistributionPartiallyCompleteTask(file);
      AddCustomDistributionCompleteTask(file);

      //addSplitTask(file);
      AddSplitTaskAlternate(file);
      AddSplitTaskPartiallyCompleteSplit(file);
      AddSplitTaskFirstSplitComplete(file);
      AddSplitTaskSecondSplitPartiallyComplete(file);
      AddSplitTaskComplete(file);

      var writer = new MSPDIWriter();

      // By default, timephased data is not written, so we need to enable it
      writer.WriteTimephasedData = true;
      
      writer.Write(file, outputFileName);
   }
    
    /// <summary>
    /// We don't need to write timephased data for the resource assignment as
    /// we're undertaking the default amount of work per day.
    /// </summary>
    /// <param name="file">parent file</param>
   private void AddFlatDistributionTask(ProjectFile file) 
   {
      var resource = file.AddResource();
      resource.Name = "Resource 1";

      var task = file.AddTask();
      task.Name = "Task 1 - Flat Distribution";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.Finish = new DateTime(2024, 3, 8, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(40, TimeUnit.Hours);

      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.WorkContour = WorkContour.Flat;
   }
    
    /// <summary>
    /// 40h task, 10h complete. Flat distribution, so no timephased data needed.
    /// </summary>
    /// <param name="file">parent file</param>
   private void AddFlatDistributionPartiallyCompleteTask(ProjectFile file) 
   {
      var resource = file.AddResource();
      resource.Name = "Resource 2";

      var task = file.AddTask();
      task.Name = "Task 2 - Flat Distribution Partially Complete";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.ActualStart = task.Start;
      task.Finish = new DateTime(2024, 3, 8, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.ActualWork = Duration.GetInstance(10, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(30, TimeUnit.Hours);

      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.ActualStart = task.Start;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.ActualWork = Duration.GetInstance(10, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(30, TimeUnit.Hours);
      assignment.WorkContour = WorkContour.Flat;
   }

    /// <summary>
    /// 40h task, 40h complete. Flat distribution, so no timephased data needed.
    /// </summary>
    /// <param name="file">parent file</param>
   private void AddFlatDistributionCompleteTask(ProjectFile file)
   {
      var resource = file.AddResource();
      resource.Name = "Resource 3";

      var task = file.AddTask();
      task.Name = "Task 3 - Flat Distribution Complete";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.ActualStart = task.Start;
      task.Finish = new DateTime(2024, 3, 8, 17, 0, 0);
      task.ActualFinish = task.ActualFinish;
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.ActualWork = Duration.GetInstance(40, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(0, TimeUnit.Hours);

      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.ActualStart = task.Start;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.ActualWork = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(0, TimeUnit.Hours);
      assignment.WorkContour = WorkContour.Flat;
   }

   /**
    * We'll create timephased data to allow us to show that we work
    * 10h on day 1, 6h on day 2, then 8h per day for the remaining days.
    *
    * @param file parent file
    */
   private void AddCustomDistributionTask(ProjectFile file)
   {
      var resource = file.AddResource();
      resource.Name = "Resource 4";

      var task = file.AddTask();
      task.Name = "Task 4 - Custom Distribution";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.Finish = new DateTime(2024, 3, 8, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(40, TimeUnit.Hours);

      // Create a resource assignment
      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(40, TimeUnit.Hours);

      // Day 1 - 10h
      var day1RemainingWork = new TimephasedWork();
      day1RemainingWork.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      day1RemainingWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day1RemainingWork.AmountPerHour = Duration.GetInstance(1.25, TimeUnit.Hours);
      day1RemainingWork.TotalAmount = Duration.GetInstance(10, TimeUnit.Hours);

      // Day 2 - 6h
      TimephasedWork day2RemainingWork = new TimephasedWork();
      day2RemainingWork.Start = new DateTime(2024, 3, 5, 8, 0, 0);
      day2RemainingWork.Finish = new DateTime(2024, 3, 5, 17, 0, 0);
      day2RemainingWork.AmountPerHour = Duration.GetInstance(0.75, TimeUnit.Hours);
      day2RemainingWork.TotalAmount = Duration.GetInstance(6, TimeUnit.Hours);

      // Remaining days - 8h/day
      TimephasedWork remainingWork = new TimephasedWork();
      remainingWork.Start = new DateTime(2024, 3, 6, 8, 0, 0);
      remainingWork.Finish = new DateTime(2024, 3, 8, 17, 0, 0);
      remainingWork.AmountPerHour = Duration.GetInstance(1, TimeUnit.Hours);
      remainingWork.TotalAmount = Duration.GetInstance(24, TimeUnit.Hours);

      // Add timephased data to the resource assignment
      var remainingRegularWork = assignment.RawTimephasedRemainingRegularWork; 
      remainingRegularWork.Add(day1RemainingWork); 
      remainingRegularWork.Add(day2RemainingWork);
      remainingRegularWork.Add(remainingWork);
   }

   /// <summary>
   /// We'll create timephased data to allow us to show that we work
   /// 10h on day 1, 6h on day 2, then 8h per day for the remaining days.
   /// Day 1 has 4h of actual work.
   /// </summary>
   /// <param name="file">parent file</param>
   private void AddCustomDistributionPartiallyCompleteTask(ProjectFile file) {
      var resource = file.AddResource();
      resource.Name = "Resource 5";

      var task = file.AddTask();
      task.Name = "Task 5 - Custom Distribution Partially Complete";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.ActualStart = task.Start;
      task.Finish = new DateTime(2024, 3, 8, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.ActualWork = Duration.GetInstance(4, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(36, TimeUnit.Hours);

      // Create a resource assignment
      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.ActualStart = task.Start;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.ActualWork = Duration.GetInstance(5, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(35, TimeUnit.Hours);

      // Important - MS Project needs this as well as the timephased data
      // to correctly represent the actual and remaining work
      assignment.Stop = new DateTime(2024, 3, 4, 12, 0, 0);
      assignment.Resume = new DateTime(2024, 3, 4, 13, 0, 0);

      // Day 1 actual work - 5h
      var day1ActualWork = new TimephasedWork();
      day1ActualWork.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      day1ActualWork.Finish = new DateTime(2024, 3, 4, 12, 0, 0);
      day1ActualWork.AmountPerHour = Duration.GetInstance(1.25, TimeUnit.Hours);
      day1ActualWork.TotalAmount = Duration.GetInstance(5, TimeUnit.Hours);

      // Day 1 remaining - 5h
      var day1RemainingWork = new TimephasedWork();
      day1RemainingWork.Start = new DateTime(2024, 3, 4, 13, 0, 0);
      day1RemainingWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day1RemainingWork.AmountPerHour = Duration.GetInstance(1.25, TimeUnit.Hours);
      day1RemainingWork.TotalAmount = Duration.GetInstance(5, TimeUnit.Hours);

      // Day 2 remaining - 6h
      var day2RemainingWork = new TimephasedWork();
      day2RemainingWork.Start = new DateTime(2024, 3, 5, 8, 0, 0);
      day2RemainingWork.Finish = new DateTime(2024, 3, 5, 17, 0, 0);
      day2RemainingWork.AmountPerHour = Duration.GetInstance(0.75, TimeUnit.Hours);
      day2RemainingWork.TotalAmount = Duration.GetInstance(6, TimeUnit.Hours);

      // Remaining days - 8h/day
      var remainingWork = new TimephasedWork();
      remainingWork.Start = new DateTime(2024, 3, 6, 8, 0, 0);
      remainingWork.Finish = new DateTime(2024, 3, 8, 17, 0, 0);
      remainingWork.AmountPerHour = Duration.GetInstance(1, TimeUnit.Hours);
      remainingWork.TotalAmount = Duration.GetInstance(24, TimeUnit.Hours);

      assignment.RawTimephasedActualRegularWork.Add(day1ActualWork);
      var remainingRegularWork = assignment.RawTimephasedRemainingRegularWork;
      remainingRegularWork.Add(day1RemainingWork);
      remainingRegularWork.Add(day2RemainingWork);
      remainingRegularWork.Add(remainingWork);
   }

   /// <summary>
   /// We'll create timephased data to allow us to show that we work
   /// 10h on day 1, 6h on day 2, then 8h per day for the remaining days.
   /// All work is complete.
   /// </summary>
   /// <param name="file">parent file</param>
   private void AddCustomDistributionCompleteTask(ProjectFile file)
   {
      var resource = file.AddResource();
      resource.Name = "Resource 6";

      var task = file.AddTask();
      task.Name = "Task 6 - Custom Distribution Complete";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.ActualStart = task.Start;
      task.Finish = new DateTime(2024, 3, 8, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.ActualWork = Duration.GetInstance(40, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(0, TimeUnit.Hours);

      // Create a resource assignment
      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.ActualStart = task.Start;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.ActualWork = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(0, TimeUnit.Hours);

      // Day 1 actual work - 10h
      var day1ActualWork = new TimephasedWork();
      day1ActualWork.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      day1ActualWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day1ActualWork.AmountPerHour = Duration.GetInstance(1.25, TimeUnit.Hours);
      day1ActualWork.TotalAmount = Duration.GetInstance(10, TimeUnit.Hours);

      // Day 2 actual - 6h
      var day2ActualWork = new TimephasedWork();
      day2ActualWork.Start = new DateTime(2024, 3, 5, 8, 0, 0);
      day2ActualWork.Finish = new DateTime(2024, 3, 5, 17, 0, 0);
      day2ActualWork.AmountPerHour = Duration.GetInstance(0.75, TimeUnit.Hours);
      day2ActualWork.TotalAmount = Duration.GetInstance(6, TimeUnit.Hours);

      // Remaining days - 8h/day
      var actualWork = new TimephasedWork();
      actualWork.Start = new DateTime(2024, 3, 6, 8, 0, 0);
      actualWork.Finish = new DateTime(2024, 3, 8, 17, 0, 0);
      actualWork.AmountPerHour = Duration.GetInstance(1, TimeUnit.Hours);
      actualWork.TotalAmount = Duration.GetInstance(24, TimeUnit.Hours);

      var actualRegularWork = assignment.RawTimephasedActualRegularWork;
      actualRegularWork.Add(day1ActualWork);
      actualRegularWork.Add(day2ActualWork);
      actualRegularWork.Add(actualWork);
   }

   /// <summary>
   /// Create a split task, 1 working day, 1 non-working day
   /// followed by the rest of the work.
   /// </summary>
   /// <param name="file">parent file</param>
   private void AddSplitTask(ProjectFile file)
   {
      var resource = file.AddResource();
      resource.Name = "Resource 7";

      var task = file.AddTask();
      task.Name = "Task 7 - Split";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(40, TimeUnit.Hours);

      // Create a resource assignment
      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(40, TimeUnit.Hours);

      // This is important - MS Project will accept the timephased data without this,
      // but the split won't show up on the Gantt Chart unless this is set
      assignment.WorkContour = WorkContour.Contoured;

      // Day 1 - 8h
      var day1RemainingWork = new TimephasedWork();
      day1RemainingWork.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      day1RemainingWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day1RemainingWork.AmountPerHour = Duration.GetInstance(1, TimeUnit.Hours);
      day1RemainingWork.TotalAmount = Duration.GetInstance(8, TimeUnit.Hours);

      // Day 2 - split

      // Remaining days - 8h/day
      // Note the gap between the end of the first working day and the start of the next working day.
      // This gives us the split.
      var remainingWork = new TimephasedWork();
      remainingWork.Start = new DateTime(2024, 3, 6, 8, 0, 0);
      remainingWork.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      remainingWork.AmountPerHour = Duration.GetInstance(1, TimeUnit.Hours);
      remainingWork.TotalAmount = Duration.GetInstance(32, TimeUnit.Hours);

      var remainingRegularWork = assignment.RawTimephasedRemainingRegularWork;
      remainingRegularWork.Add(day1RemainingWork);
      remainingRegularWork.Add(remainingWork);
   }

   private void AddSplitTaskAlternate(ProjectFile file) 
   {
      var resource = file.AddResource();
      resource.Name = "Resource 7";

      var task = file.AddTask();
      task.Name = "Task 7 - Split";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(40, TimeUnit.Hours);

      // Create a resource assignment
      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(40, TimeUnit.Hours);

      // This is important - MS Project will accept the timephased data without this,
      // but the split won't show up on the Gantt Chart unless this is set
      assignment.WorkContour = WorkContour.Contoured;

      // Day 1 - 8h
      var day1RemainingWork = new TimephasedWork();
      day1RemainingWork.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      day1RemainingWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day1RemainingWork.AmountPerHour = Duration.GetInstance(1, TimeUnit.Hours);
      day1RemainingWork.TotalAmount = Duration.GetInstance(8, TimeUnit.Hours);

      // Day 2 - split
      var day2RemainingWork = new TimephasedWork();
      day2RemainingWork.Start = new DateTime(2024, 3, 4, 8, 0,0);
      day2RemainingWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day2RemainingWork.AmountPerHour = Duration.GetInstance(0, TimeUnit.Hours);
      day2RemainingWork.TotalAmount = Duration.GetInstance(0, TimeUnit.Hours);

      // Remaining days - 8h/day
      var remainingWork = new TimephasedWork();
      remainingWork.Start = new DateTime(2024, 3, 6, 8, 0, 0);
      remainingWork.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      remainingWork.AmountPerHour = Duration.GetInstance(1, TimeUnit.Hours);
      remainingWork.TotalAmount = Duration.GetInstance(32, TimeUnit.Hours);

      var remainingRegularWork = assignment.RawTimephasedRemainingRegularWork;
      remainingRegularWork.Add(day1RemainingWork);
      remainingRegularWork.Add(day2RemainingWork);
      remainingRegularWork.Add(remainingWork);
   }

   /// <summary>
   /// Split task with 1 day, a gap of 1 day, then the remaining work.
   /// 4h of work has been done on the first day.
   /// </summary>
   /// <param name="file">parent file</param>
   private void AddSplitTaskPartiallyCompleteSplit(ProjectFile file)
   {
      var resource = file.AddResource();
      resource.Name = "Resource 8";

      var task = file.AddTask();
      task.Name = "Task 8 - Split Partially Complete";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.ActualStart = task.Start;
      task.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.ActualWork = Duration.GetInstance(4, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(36, TimeUnit.Hours);

      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.ActualStart = task.ActualStart;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.ActualWork = Duration.GetInstance(4, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(36, TimeUnit.Hours);

      // Important - MS Project needs this as well as the timephased data
      // to correctly represent the actual and remaining work
      assignment.Stop = new DateTime(2024, 3, 4, 12, 0, 0);
      assignment.Resume = new DateTime(2024, 3, 4, 13, 0, 0);

      // This is important - MS Project will accept the timephased data without this,
      // but the split won't show up on the Gantt Chart unless ths is set
      assignment.WorkContour = WorkContour.Contoured;

      // Day 1 actual - 4h
      var day1ActualWork = new TimephasedWork();
      day1ActualWork.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      day1ActualWork.Finish = new DateTime(2024, 3, 4, 12, 0, 0);
      day1ActualWork.TotalAmount = Duration.GetInstance(4, TimeUnit.Hours);

      // Day 1 remaining - 4h
      var day1RemainingWork = new TimephasedWork();
      day1RemainingWork.Start = new DateTime(2024, 3, 4, 13, 0, 0);
      day1RemainingWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day1RemainingWork.TotalAmount = Duration.GetInstance(4, TimeUnit.Hours);

      // Day 2 - split

      // Remaining days - 8h/day
      // Note the gap between the end of the first working day and the start of the next working day.
      // This gives us the split.
      var remainingWork = new TimephasedWork();
      remainingWork.Start = new DateTime(2024, 3, 6, 8, 0, 0);
      remainingWork.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      remainingWork.TotalAmount = Duration.GetInstance(36, TimeUnit.Hours);

      assignment.RawTimephasedActualRegularWork.Add(day1ActualWork);
      var remainingRegularWork = assignment.RawTimephasedRemainingRegularWork;
      remainingRegularWork.Add(day1RemainingWork);
      remainingRegularWork.Add(remainingWork);
   }

   /// <summary>
   /// Split task with 1 day, a gap of 1 day, then the remaining work.
   /// The first split (8h) is complete.
   /// </summary>
   /// <param name="file">parent file</param>
   private void AddSplitTaskFirstSplitComplete(ProjectFile file)
   {
      var resource = file.AddResource();
      resource.Name = "Resource 9";

      var task = file.AddTask();
      task.Name = "Task 9 - Split First Split Complete";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.ActualStart = task.Start;
      task.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.ActualWork = Duration.GetInstance(8, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(32, TimeUnit.Hours);

      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.ActualStart = task.ActualStart;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.ActualWork = Duration.GetInstance(8, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(32, TimeUnit.Hours);

      // Important - MS Project needs this as well as the timephased data
      // to correctly represent the actual and remaining work
      assignment.Stop = new DateTime(2024, 3, 4, 17, 0, 0);
      assignment.Resume = new DateTime(2024, 3, 4, 17, 0, 0);

      // This is important - MS Project will accept the timephased data without this,
      // but the split won't show up on the Gantt Chart unless ths is set
      assignment.WorkContour = WorkContour.Contoured;

      // Day 1 actual - 4h
      var day1ActualWork = new TimephasedWork();
      day1ActualWork.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      day1ActualWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day1ActualWork.TotalAmount = Duration.GetInstance(8, TimeUnit.Hours);

      // Day 2 - split

      // Remaining days - 8h/day
      // Note the gap between the end of the first working day and the start of the next working day.
      // This gives us the split.
      var remainingWork = new TimephasedWork();
      remainingWork.Start = new DateTime(2024, 3, 6, 8, 0, 0);
      remainingWork.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      remainingWork.TotalAmount = Duration.GetInstance(32, TimeUnit.Hours);

      assignment.RawTimephasedActualRegularWork.Add(day1ActualWork);
      assignment.RawTimephasedRemainingRegularWork.Add(remainingWork);
   }

   /// <summary>
   /// Split task with 1 day, a gap of 1 day, then the remaining work.
   /// The first split (8h) is complete, second spit has 4h actual work.
   /// </summary>
   /// <param name="file">parent file</param>
   private void AddSplitTaskSecondSplitPartiallyComplete(ProjectFile file)
   {
      var resource = file.AddResource();
      resource.Name = "Resource 10";

      var task = file.AddTask();
      task.Name = "Task 10 - Split Second Split Partially Complete";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.ActualStart = task.Start;
      task.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.ActualWork = Duration.GetInstance(16, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(24, TimeUnit.Hours);

      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.ActualStart = task.ActualStart;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.ActualWork = Duration.GetInstance(16, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(24, TimeUnit.Hours);

      // Important - MS Project needs this as well as the timephased data
      // to correctly represent the actual and remaining work
      assignment.Stop = new DateTime(2024, 3, 6, 17, 0, 0);
      assignment.Resume = new DateTime(2024, 3, 6, 17, 0, 0);

      // This is important - MS Project will accept the timephased data without this,
      // but the split won't show up on the Gantt Chart unless ths is set
      assignment.WorkContour = WorkContour.Contoured;

      // Day 1 actual - 4h
      var day1ActualWork = new TimephasedWork();
      day1ActualWork.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      day1ActualWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day1ActualWork.TotalAmount = Duration.GetInstance(8, TimeUnit.Hours);

      // Day 2 - split

      // Day 3 actual work
      var day3ActualWork = new TimephasedWork();
      day3ActualWork.Start = new DateTime(2024, 3, 6, 8, 0, 0);
      day3ActualWork.Finish = new DateTime(2024, 3, 6, 17, 0, 0);
      day3ActualWork.TotalAmount = Duration.GetInstance(8, TimeUnit.Hours);

      // Remaining days - 8h/day
      var remainingWork = new TimephasedWork();
      remainingWork.Start = new DateTime(2024, 3, 7, 8, 0, 0);
      remainingWork.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      remainingWork.TotalAmount = Duration.GetInstance(24, TimeUnit.Hours);

      var actualRegularWork = assignment.RawTimephasedActualRegularWork;
      actualRegularWork.Add(day1ActualWork);
      actualRegularWork.Add(day3ActualWork);
      assignment.RawTimephasedRemainingRegularWork.Add(remainingWork);
   }

   /// <summary>
   /// Split task with 1 day, a gap of 1 day, then the remaining work.
   /// Entire task is complete.
   /// </summary>
   /// <param name="file">parent file</param>
   private void AddSplitTaskComplete(ProjectFile file)
   {
      var resource = file.AddResource();
      resource.Name = "Resource 11";

      var task = file.AddTask();
      task.Name = "Task 11 - Split Complete";
      task.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      task.ActualStart = task.Start;
      task.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      task.Duration = Duration.GetInstance(40, TimeUnit.Hours);
      task.Work = Duration.GetInstance(40, TimeUnit.Hours);
      task.ActualWork = Duration.GetInstance(40, TimeUnit.Hours);
      task.RemainingWork = Duration.GetInstance(0, TimeUnit.Hours);

      var assignment = task.AddResourceAssignment(resource);
      assignment.Start = task.Start;
      assignment.ActualStart = task.Start;
      assignment.Work = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.ActualWork = Duration.GetInstance(40, TimeUnit.Hours);
      assignment.RemainingWork = Duration.GetInstance(0, TimeUnit.Hours);

      // This is important - MS Project will accept the timephased data without this,
      // but the split won't show up on the Gantt Chart unless ths is set
      assignment.WorkContour = WorkContour.Contoured;

      // Day 1 - 8h
      var day1ActualWork = new TimephasedWork();
      day1ActualWork.Start = new DateTime(2024, 3, 4, 8, 0, 0);
      day1ActualWork.Finish = new DateTime(2024, 3, 4, 17, 0, 0);
      day1ActualWork.TotalAmount = Duration.GetInstance(8, TimeUnit.Hours);

      // Day 2 - split

      // Remaining days - 8h/day
      // Note the gap between the end of the first working day and the start of the next working day.
      // This gives us the split.
      TimephasedWork actualWork = new TimephasedWork();
      actualWork.Start = new DateTime(2024, 3, 6, 8, 0, 0);
      actualWork.Finish = new DateTime(2024, 3, 11, 17, 0, 0);
      actualWork.TotalAmount = Duration.GetInstance(32, TimeUnit.Hours);

      var actualRegularWork = assignment.RawTimephasedActualRegularWork;
      actualRegularWork.Add(day1ActualWork);
      actualRegularWork.Add(actualWork);
   }
}