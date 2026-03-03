using System;
using System.Collections.Generic;
using System.Linq;
using MPXJ.Net;


namespace MpxjSamples.HowToUse.Timephased;

public class TimephasedSamples
{
    public void TimescaleRanges()
    {
        {
            // Single range
            var today = new DateTimeRange(
                new DateTime(2026, 2, 18),
                new DateTime(2026, 2, 19));
        }

        {
            // Multiple ranges
            var day1 = new DateTimeRange(
                new DateTime(2026, 2, 18),
                new DateTime(2026, 2, 19));
            var day2 = new DateTimeRange(
                new DateTime(2026, 2, 19),
                new DateTime(2026, 2, 20));
            var timescale = new List<DateTimeRange> { day1, day2 };
        }

        {
            // Timescale with segment count
            var startDate = new DateTime(2026, 2, 16);
            var ranges = new TimescaleHelper()
                .CreateTimescale(startDate, 5, TimescaleUnits.Days);
        }

        {
            // Timescale with date range
            var startDate = new DateTime(2026, 2, 16);
            var endDate = new DateTime(2026, 2, 20);
            var ranges = new TimescaleHelper()
                .CreateTimescale(startDate, endDate, TimescaleUnits.Days);
        }
    }
    
    public void TimephasedData()
    {
        var file = new UniversalProjectReader().Read("_TestData/timephased-sample.mpp");

        foreach (var resource in file.Resources)
        {
            System.Console.WriteLine(resource);
        }

        foreach (var task in file.Tasks)
        {
            System.Console.WriteLine(task);
        }

        foreach (var assignment in file.ResourceAssignments)
        {
            System.Console.WriteLine(assignment);
        }
        
        System.Console.WriteLine();

        var ranges = new TimescaleHelper()
            .CreateTimescale(new DateTime(2026, 2, 18), 7, TimescaleUnits.Days);

        // Work
        {
            var assignment = file.ResourceAssignments.GetByUniqueID(6);
            var work = assignment.GetTimephasedWork(ranges, TimeUnit.Hours);
            WriteTableHeader(ranges);
            WriteTableRow("Work", work);
            System.Console.WriteLine();
        }
        
        // Actual and remaining work
        {
            var assignment = file.ResourceAssignments.GetByUniqueID(6);
            var actualWork = assignment.GetTimephasedActualWork(ranges, TimeUnit.Hours);
            var remainingWork = assignment.GetTimephasedRemainingWork(ranges, TimeUnit.Hours);
            WriteTableHeader(ranges);
            WriteTableRow("Actual Work", actualWork);
            WriteTableRow("Remaining Work", remainingWork);
            System.Console.WriteLine();
        }

        // Resource 2
        {
            var resource2 = file.GetResourceByID(2);
            var work = resource2.GetTimephasedWork(ranges, TimeUnit.Hours);
            WriteTableHeader(ranges);
            WriteTableRow("Resource 2 Work", work);
            System.Console.WriteLine();
        }

        // Tasks
        {
            var summaryTask = file.GetTaskByID(1);
            var task1 = file.GetTaskByID(2);
            var task2 = file.GetTaskByID(3);
            var summaryWork = summaryTask.GetTimephasedWork(ranges, TimeUnit.Hours);
            var task1Work = task1.GetTimephasedWork(ranges, TimeUnit.Hours);
            var task2Work = task2.GetTimephasedWork(ranges, TimeUnit.Hours);
            WriteTableHeader(ranges);
            WriteTableRow("Summary Work", summaryWork);
            WriteTableRow("Task 1 Work", task1Work);
            WriteTableRow("Task 2 Work", task2Work);
            System.Console.WriteLine();
        }
        
        // Costs
        {
            var summaryTask = file.GetTaskByID(1);
            var task1 = file.GetTaskByID(2);
            var task2 = file.GetTaskByID(3);
            var summaryCost = summaryTask.GetTimephasedCost(ranges);
            var task1Cost = task1.GetTimephasedCost(ranges);
            var task2Cost = task2.GetTimephasedCost(ranges);
            WriteTableHeader(ranges);
            WriteTableRow("Summary Cost", summaryCost);
            WriteTableRow("Task 1 Cost", task1Cost);
            WriteTableRow("Task 2 Cost", task2Cost);
            System.Console.WriteLine();
        }

        // Material
        {
            // Retrieve an assignment for a  material resource
            var assignment = file.ResourceAssignments.GetByUniqueID(11);

            // Create labels using the correct units for the resource
            var materialUnits = "(" + assignment.Resource.MaterialLabel + ")";
            var actualMaterialLabel = "Actual Material " + materialUnits;
            var remainingMaterialLabel = "Remaining Material " + materialUnits;
            var materialLabel = "Material " + materialUnits;

            // Retrieve the timephased values
            var actualMaterial = assignment.GetTimephasedActualMaterial(ranges);
            var remainingMaterial = assignment.GetTimephasedRemainingMaterial(ranges);
            var material = assignment.GetTimephasedMaterial(ranges);

            // Present the values as a table
            WriteTableHeader(ranges);
            WriteTableRow(actualMaterialLabel, actualMaterial);
            WriteTableRow(remainingMaterialLabel, remainingMaterial);
            WriteTableRow(materialLabel, material);
            System.Console.WriteLine();
        }

        // Retrieve work from tasks using parametrised methods
        {
            // Retrieve tasks
            var summaryTask = file.GetTaskByID(1);
            var task1 = file.GetTaskByID(2);
            var task2 = file.GetTaskByID(3);

            // Retrieve timephased work
            var summaryWork = summaryTask.GetTimephasedDurationValues(TaskField.Work, ranges, TimeUnit.Hours);
            var task1Work = task1.GetTimephasedDurationValues(TaskField.Work, ranges, TimeUnit.Hours);
            var task2Work = task2.GetTimephasedDurationValues(TaskField.Work, ranges, TimeUnit.Hours);

            // Present the values as a table
            WriteTableHeader(ranges);
            WriteTableRow("Summary Work", summaryWork);
            WriteTableRow("Task 1 Work", task1Work);
            WriteTableRow("Task 2 Work", task2Work);
            System.Console.WriteLine();
        }

        // Retrieve costs from a resource assignment using parametrised methods
        {
            // Retrieve a resource assignment
            var assignment = file.ResourceAssignments.GetByUniqueID(6);

            // Retrieve timephased costs
            var actualCost = assignment.GetTimephasedNumericValues(AssignmentField.ActualCost, ranges);
            var remainingCost = assignment.GetTimephasedNumericValues(AssignmentField.RemainingCost, ranges);
            var cost = assignment.GetTimephasedNumericValues(AssignmentField.Cost, ranges);

            // Present the values as a table
            WriteTableHeader(ranges);
            WriteTableRow("Actual Cost", actualCost);
            WriteTableRow("Remaining Cost", remainingCost);
            WriteTableRow("Cost", cost);
            System.Console.WriteLine();
        }

        // Retrieve material utilisation from a resource using parametrised methods
        {
            // Retrieve a material resource
            var resource = file.GetResourceByID(4);

            // Create labels using the correct units for the resource
            var materialUnits = "(" + resource.MaterialLabel + ")";
            var actualMaterialLabel = "Actual Material " + materialUnits;
            var remainingMaterialLabel = "Remaining Material " + materialUnits;
            var materialLabel = "Material " + materialUnits;

            // Retrieve the timephased values
            var actualMaterial = resource.GetTimephasedNumericValues(ResourceField.ActualMaterial, ranges);
            var remainingMaterial = resource.GetTimephasedNumericValues(ResourceField.RemainingMaterial, ranges);
            var material = resource.GetTimephasedNumericValues(ResourceField.Material, ranges);

            // Present the values as a table
            WriteTableHeader(ranges);
            WriteTableRow(actualMaterialLabel, actualMaterial);
            WriteTableRow(remainingMaterialLabel, remainingMaterial);
            WriteTableRow(materialLabel, material);
            System.Console.WriteLine();
        }
    }
    
    private void WriteTableHeader(IList<DateTimeRange> ranges)
    {
        var labels = String.Join('|', ranges
            .Select(r => r.Start.Value.DayOfWeek.ToString().Substring(0, 1)));
        System.Console.WriteLine("||" + labels + "|");

        var separator = String.Join('|', ranges.Select(r => "---"));
        System.Console.WriteLine("|---|" + separator+ "|");
    }

    private void WriteTableRow(string label, IList<Duration> data) 
    { 
        var values = String.Join('|', 
            data.Select(d => d == null ? "null" : d.ToString()));
        System.Console.WriteLine("|" + label + "|" + values + "|");
    }

    private void WriteTableRow(string label, IList<double?> data)
    {
        var values = String.Join('|', 
            data.Select(d => d == null ? "null" : d.ToString()));
        System.Console.WriteLine("|" + label + "|" + values + "|");
    }
}