using System;
using System.Collections.Generic;
using System.IO;
using MPXJ.Net;

namespace MpxjSample
{
    class MpxjCreate
    {
        private static readonly Dictionary<string, FileFormat> FileFormatDictionary = new Dictionary<string, FileFormat>()
        {
            { "MPX", FileFormat.MPX },
            { "XML", FileFormat.MSPDI },
            { "PMXML", FileFormat.PMXML },
            { "PLANNER", FileFormat.PLANNER },
            { "JSON", FileFormat.JSON },
            { "SDEF", FileFormat.SDEF },
            { "XER", FileFormat.XER },
        };

        static void Main(string[] args)
        {
#if NETCOREAPP
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif

            try
            {
                if (args.Length != 1)
                {
                    Console.Out.WriteLine("Usage: MpxjCreate <output file name>");
                }
                else
                {
                    MpxjCreate create = new MpxjCreate();
                    create.process(args[0]);
                }
            }

            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
            }
        }

        public void process(string filename)
        {
            //
            // Create a ProjectFile instance
            //
            ProjectFile file = new ProjectFile();

            //
            // Add a default calendar called "Standard"
            //
            ProjectCalendar calendar = file.AddDefaultBaseCalendar();

            //
            // Add a holiday to the calendar to demonstrate calendar exceptions
            //            
            calendar.AddCalendarException(DateOnly.Parse("2006-03-13"), DateOnly.Parse("2006-03-13"));

            //
            // Retrieve the project properties and set the start date. Note Microsoft
            // Project appears to reset all task dates relative to this date, so this
            // date must match the start date of the earliest task for you to see
            // the expected results. If this value is not set, it will default to
            // today's date.
            //
            ProjectProperties properties = file.ProjectProperties;
            properties.StartDate = DateTime.Parse("2003-01-01 08:00");

            //
            // Set a couple more properties just for fun
            //
            properties.ProjectTitle = "Created by MPXJ";
            properties.Author = "Jon Iles";

            //
            // Add resources
            //
            Resource resource1 = file.AddResource();
            resource1.Name = "Resource1";

            Resource resource2 = file.AddResource();
            resource2.Name = "Resource2";
            // This resource is only available 50% of the time
            resource2.Availability.Add(new Availability(DateTimeHelper.StartDateNA, DateTimeHelper.EndDateNA, 50.0));

            //
            // Create a summary task
            //
            Task task1 = file.AddTask();
            task1.Name = "Summary Task";

            //
            // Create the first sub task
            //
            Task task2 = task1.AddTask();
            task2.Name = "First Sub Task";
            task2.Duration = Duration.GetInstance(10.5, TimeUnit.Days);
            task2.Start = DateTime.Parse("2003-01-01 08:00");

            //
            // We'll set this task up as being 50% complete. If we have no resource
            // assignments for this task, this is enough information for MS Project.
            // If we do have resource assignments, the assignment record needs to
            // contain the corresponding work and actual work fields set to the
            // correct values in order for MS project to mark the task as complete
            // or partially complete.
            //
            task2.PercentageComplete = 50.0;
            task2.ActualStart = DateTime.Parse("2003-01-01 08:00");

            //
            // Create the second sub task
            //
            Task task3 = task1.AddTask();
            task3.Name = "Second Sub Task";
            task3.Start = DateTime.Parse("2003-01-11 08:00");
            task3.Duration = Duration.GetInstance(10, TimeUnit.Days);

            //
            // Link these two tasks
            //            
            task3.AddPredecessor(new Relation.Builder(file).TargetTask(task2).Type(RelationType.FinishStart));

            //
            // Add a milestone
            //
            Task milestone1 = task1.AddTask();
            milestone1.Name = "Milestone";
            milestone1.Start = DateTime.Parse("2003-01-21 08:00");
            milestone1.Duration = Duration.GetInstance(0, TimeUnit.Days);            
            milestone1.AddPredecessor(new Relation.Builder(file).TargetTask(task2).Type(RelationType.FinishStart));

            //
            // This final task has a percent complete value, but no
            // resource assignments. This is an interesting case it it requires
            // special processing to generate the MSPDI file correctly.
            //
            Task task4 = file.AddTask();
            task4.Name = "Next Task";
            task4.Duration = Duration.GetInstance(8, TimeUnit.Days);
            task4.Start = DateTime.Parse("2003-01-01 08:00");
            task4.PercentageComplete = 70.0;
            task4.ActualStart = DateTime.Parse("2003-01-01 08:00");

            //
            // Assign resources to tasks
            //
            ResourceAssignment assignment1 = task2.AddResourceAssignment(resource1);
            ResourceAssignment assignment2 = task3.AddResourceAssignment(resource2);

            //
            // As the first task is partially complete, and we are adding
            // a resource assignment, we must set the work and actual work
            // fields in the assignment to appropriate values, or MS Project
            // won't recognise the task as being complete or partially complete
            //
            assignment1.Work = Duration.GetInstance(80, TimeUnit.Hours);
            assignment1.ActualWork = Duration.GetInstance(40, TimeUnit.Hours);

            //
            // If we were just generating an MPX file, we would already have enough
            // attributes set to create the file correctly. If we want to generate
            // an MSPDI file, we must also set the assignment start dates and
            // the remaining work attribute. The assignment start dates will normally
            // be the same as the task start dates.
            //
            assignment1.RemainingWork = Duration.GetInstance(40, TimeUnit.Hours);
            assignment2.RemainingWork = Duration.GetInstance(80, TimeUnit.Hours);
            assignment1.Start = DateTime.Parse("2003-01-01 08:00");
            assignment2.Start = DateTime.Parse("2003-01-11 08:00");

            //
            // Write a 100% complete task
            //
            Task task5 = file.AddTask();
            task5.Name = "Last Task";
            task5.Duration = Duration.GetInstance(3, TimeUnit.Days);
            task5.Start = DateTime.Parse("2003-01-01 08:00");
            task5.PercentageComplete = 100.0;
            task5.ActualStart = DateTime.Parse("2003-01-01 08:00");

            //
            // Write the file
            //
            var extension = Path.GetExtension(filename);
            if (extension == null || extension.Length == 0)
            {
                throw new ArgumentException("Filename has no extension");
            }

            extension = extension.Substring(1).ToUpper();
            if (!FileFormatDictionary.TryGetValue(extension, out var format))
            {
                throw new ArgumentException($"Unsupported file extension: {extension}");
            }

            new UniversalProjectWriter(format).Write(file, filename);
        }
    }
}
