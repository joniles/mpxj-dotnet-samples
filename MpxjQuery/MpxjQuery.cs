﻿using System;
using System.Collections.Generic;
using MPXJ.Net;

namespace MpxjQuery
{
    class MpxjQuery
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args">command line arguments</param>
        static void Main(string[] args)
        {
#if NETCOREAPP
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif

            try
            {
                if (args.Length != 1)
                {
                    System.Console.WriteLine("Usage: MpxQuery <input file name>");
                }
                else
                {
                    Query(args[0]);
                }
            }

            catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
            }

        }

        /// <summary>
        /// This method demonstrates reading data from a project.
        /// </summary>
        /// <param name="filename">name of the project file</param>
        private static void Query(String filename)
        {
            var file = new UniversalProjectReader().Read(filename);

            ListProjectHeader(file);

            ListResources(file);

            ListTasks(file);

            ListAssignments(file);

            ListAssignmentsByTask(file);

            ListAssignmentsByResource(file);

            ListHierarchy(file);

            ListTaskNotes(file);

            ListResourceNotes(file);

            ListRelationships(file);

            ListSlack(file);

            ListCalendars(file);

            ListCustomFields(file);
        }

        /// <summary>
        /// Reads basic summary details from the project header.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListProjectHeader(ProjectFile file)
        {
            var header = file.ProjectProperties;
            var formattedStartDate = header.StartDate == null ? "(none)" : header.StartDate.ToString();
            var formattedFinishDate = header.FinishDate == null ? "(none)" : header.FinishDate.ToString();

            System.Console.WriteLine("Project Header: StartDate=" + formattedStartDate + " FinishDate=" + formattedFinishDate);
            System.Console.WriteLine();
        }

        /// <summary>
        /// This method lists all resources defined in the file.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListResources(ProjectFile file)
        {
            foreach (Resource resource in file.Resources)
            {
                System.Console.WriteLine("Resource: " + resource.Name + " (Unique ID=" + resource.UniqueID + ") Start=" + resource.Start + " Finish=" + resource.Finish);
            }
            System.Console.WriteLine();
        }

        /// <summary>
        /// This method lists all tasks defined in the file.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListTasks(ProjectFile file)
        {
            foreach (Task task in file.Tasks)
            {
                var date = task.Start;
                var startDate = date == null ? "(no date supplied)" : date.ToString();

                date = task.Finish;
                var finishDate = date == null ? "(no date supplied)" : date.ToString();

                var dur = task.Duration;
                var duration = dur == null ? "(no duration supplied)" : dur.ToString();

                var baselineDuration = task.BaselineDurationText;
                if (baselineDuration == null)
                {
                    dur = task.BaselineDuration;
                    duration = dur == null ? "(no duration supplied)" : dur.ToString();
                }

                System.Console.WriteLine("Task: " + task.Name + " ID=" + task.ID + " Unique ID=" + task.UniqueID + " (Start Date=" + startDate + " Finish Date=" + finishDate + " Duration=" + duration + " Baseline Duration=" + baselineDuration + " Outline Level=" + task.OutlineLevel + " Outline Number=" + task.OutlineNumber + " Recurring=" + task.Recurring + ")");
            }
            System.Console.WriteLine();
        }

        /// <summary>
        /// This method lists all tasks defined in the file in a hierarchical format, 
        /// reflecting the parent-child relationships between them.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListHierarchy(ProjectFile file)
        {
            foreach (Task task in file.ChildTasks)
            {
                System.Console.WriteLine("Task: " + task.Name);
                ListHierarchy(task, " ");
            }

            System.Console.WriteLine();
        }

        /// <summary>
        /// Helper method called recursively to list child tasks.
        /// </summary>
        /// <param name="task">Task instance</param>
        /// <param name="indent">print indent</param>
        private static void ListHierarchy(Task task, String indent)
        {
            foreach (Task child in task.ChildTasks)
            {
                System.Console.WriteLine(indent + "Task: " + child.Name);
                ListHierarchy(child, indent + " ");
            }
        }

        /// <summary>
        /// This method lists all resource assignments defined in the file.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListAssignments(ProjectFile file)
        {
            foreach (ResourceAssignment assignment in file.ResourceAssignments)
            {
                var task = assignment.Task;
                var taskName = task == null ? "(null task)" : task.Name;

                var resource = assignment.Resource;
                var resourceName = resource == null ? "(null resource)" : resource.Name;

                System.Console.WriteLine("Assignment: Task=" + taskName + " Resource=" + resourceName);
            }

            System.Console.WriteLine();
        }

        /// <summary>
        /// This method displays the resource assignments for each task. 
        /// This time rather than just iterating through the list of all 
        /// assignments in the file, we extract the assignments on a task-by-task basis.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListAssignmentsByTask(ProjectFile file)
        {
            foreach (Task task in file.Tasks)
            {
                System.Console.WriteLine("Assignments for task " + task.Name + ":");

                foreach (ResourceAssignment assignment in task.ResourceAssignments)
                {
                    Resource resource = assignment.Resource;
                    var resourceName = resource == null ? "(null resource)" : resource.Name;

                    System.Console.WriteLine("   " + resourceName);
                }
            }

            System.Console.WriteLine();
        }

        /// <summary>
        /// This method displays the resource assignments for each resource. 
        /// This time rather than just iterating through the list of all 
        /// assignments in the file, we extract the assignments on a resource-by-resource basis.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListAssignmentsByResource(ProjectFile file)
        {
            foreach (Resource resource in file.Resources)
            {
                System.Console.WriteLine("Assignments for resource " + resource.Name + ":");

                foreach (ResourceAssignment assignment in resource.TaskAssignments)
                {
                    Task task = assignment.Task;
                    System.Console.WriteLine("   " + task.Name);
                }
            }

            System.Console.WriteLine();
        }

        /// <summary>
        /// This method lists any notes attached to tasks..
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListTaskNotes(ProjectFile file)
        {
            foreach (Task task in file.Tasks)
            {
                String notes = task.Notes;

                if (notes != null && notes.Length != 0)
                {
                    System.Console.WriteLine("Notes for " + task.Name + ": " + notes);
                }
            }

            System.Console.WriteLine();
        }

        /// <summary>
        /// This method lists any notes attached to resources.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListResourceNotes(ProjectFile file)
        {
            foreach (Resource resource in file.Resources)
            {
                String notes = resource.Notes;

                if (notes != null && notes.Length != 0)
                {
                    System.Console.WriteLine("Notes for " + resource.Name + ": " + notes);
                }
            }

            System.Console.WriteLine();
        }

        /// <summary>
        /// This method lists task predecessor and successor relationships.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListRelationships(ProjectFile file)
        {
            foreach (Task task in file.Tasks)
            {
                System.Console.Write(task.ID);
                System.Console.Write('\t');
                System.Console.Write(task.Name);
                System.Console.Write('\t');

                DumpRelationList(task.Predecessors);
                System.Console.Write('\t');
                DumpRelationList(task.Successors);
                System.Console.WriteLine();
            }
        }

        /// <summary>
        /// Internal utility to dump relationship lists in a structured format that can 
        /// easily be compared with the tabular data in MS Project.
        /// </summary>
        /// <param name="relations">project file</param>
        private static void DumpRelationList(IList<Relation> relations)
        {
            if (relations == null || relations.Count == 0)
            {
                return;
            }

            if (relations.Count > 1)
            {
                System.Console.Write('"');
            }

            var first = true;
            foreach (var relation in relations)
            {
                if (!first)
                {
                    System.Console.Write(',');
                }

                first = false;
                System.Console.Write(relation.TargetTask.ID);
                var lag = relation.Lag;
                if (relation.Type != RelationType.FinishStart || lag.DurationValue != 0)
                {
                    System.Console.Write(relation.Type);
                }

                if (lag.DurationValue != 0)
                {
                    if (lag.DurationValue > 0)
                    {
                        System.Console.Write("+");
                    }
                    System.Console.Write(lag);
                }
            }
            if (relations.Count > 1)
            {
                System.Console.Write('"');
            }
        }

        /// <summary>
        /// List the slack values for each task.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListSlack(ProjectFile file)
        {
            foreach (Task task in file.Tasks)
            {
                System.Console.WriteLine(task.Name + " Total Slack=" + task.TotalSlack + " Start Slack=" + task.StartSlack + " Finish Slack=" + task.FinishSlack);
            }
        }

        /// <summary>
        /// List details of all calendars in the file.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListCalendars(ProjectFile file)
        {
            foreach (ProjectCalendar cal in file.Calendars)
            {
                System.Console.WriteLine(cal.ToString());
            }
        }

        /// <summary>
        /// List details of custom fields in the file.
        /// </summary>
        /// <param name="file">project file</param>
        private static void ListCustomFields(ProjectFile file)
        {
            foreach (CustomField field in file.CustomFields)
            {
                System.Console.WriteLine(field.ToString());
            }
        }
    }
}
