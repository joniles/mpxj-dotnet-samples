using System;
using System.Collections.Generic;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MPXJ.Net;

namespace ScheduleSpreadsheet;

/// <summary>
/// Example of creating a spreadsheet from a schedule.
/// This uses Microsoft's own OpenXml libraries, although the internet suggests that the ClosedXml package is easier to use.
/// </summary>
public class ScheduleSpreadsheet
{
    // Fields to add to the project sheet
    private static readonly ProjectField[] ProjectFields = {
        ProjectField.Name,
        ProjectField.ProjectTitle,
        ProjectField.UniqueId,
        ProjectField.Guid,
        ProjectField.FileApplication,
        ProjectField.FileType
    };

    // Fields to add to the tasks sheet
    private static readonly TaskField[] TaskFields = {
        TaskField.Id,
        TaskField.UniqueID,
        TaskField.Name,
        TaskField.Duration,
        TaskField.Work,
        TaskField.Start,
        TaskField.Finish
    };

    // Fields to add to the resource sheet
    private static readonly ResourceField[] ResourceFields = {
        ResourceField.Id,
        ResourceField.UniqueId,
        ResourceField.Name
    };

    // Fields to add to the resource assignment sheet
    private static readonly AssignmentField[] AssignmentFields = {
        AssignmentField.UniqueId,
        AssignmentField.TaskUniqueId,
        AssignmentField.ResourceUniqueId,
        AssignmentField.AssignmentUnits
    };

    private SpreadsheetDocument _spreadsheetDocument;
    private WorkbookPart _workbookPart;
    private Sheets _sheets;

    /// <summary>
    /// Main entry point for command line use.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: ScheduleSpreadsheet <input schedule file name> <output xlsx file name>");
            return;
        }
        new ScheduleSpreadsheet().Process(args[0], args[1]);
    }

    /// <summary>
    /// Main entry point
    /// </summary>
    /// <param name="scheduleFile">input schedule file name</param>
    /// <param name="spreadsheetFile">output xlsx file name</param>
    public void Process(string scheduleFile, string spreadsheetFile)
    {
        // Read the schedule
        var schedule = new UniversalProjectReader().Read(scheduleFile);

        // Populate and save the spreadsheet
        try
        {
            CreateSpreadsheet(spreadsheetFile);
            CreateWorksheet(1, "Project", new List<IFieldContainer> { schedule.ProjectProperties }, ProjectFields);
            CreateWorksheet(2, "Tasks", schedule.Tasks, TaskFields);
            CreateWorksheet(3, "Resources", schedule.Resources, ResourceFields);
            CreateWorksheet(4, "Assignments", schedule.ResourceAssignments, AssignmentFields);
            _workbookPart.Workbook.Save();
        }

        finally
        {
            _spreadsheetDocument?.Dispose();
        }
    }

    /// <summary>
    /// Boilerplate code to create the spreadsheet and populate a style to allow
    /// dates to be formatted in the user's current date/time format.
    /// </summary>
    /// <param name="spreadsheetFile"></param>
    private void CreateSpreadsheet(string spreadsheetFile)
    {
        // Create the spreadsheet
        _spreadsheetDocument = SpreadsheetDocument.Create(spreadsheetFile, SpreadsheetDocumentType.Workbook);
        _workbookPart = _spreadsheetDocument.AddWorkbookPart();
        _workbookPart.Workbook = new Workbook();
        _sheets = _workbookPart.Workbook.AppendChild(new Sheets());

        // Create the stylesheet
        var stylesPart = _workbookPart.AddNewPart<WorkbookStylesPart>();
        stylesPart.Stylesheet = new Stylesheet();

        // Default font 
        stylesPart.Stylesheet.Fonts = new Fonts();
        stylesPart.Stylesheet.Fonts.Count = 1;
        stylesPart.Stylesheet.Fonts.AppendChild(new Font());

        // Date format
        stylesPart.Stylesheet.NumberingFormats = new NumberingFormats();
        stylesPart.Stylesheet.NumberingFormats.AppendChild(new NumberingFormat { NumberFormatId = 1, FormatCode = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern });
        stylesPart.Stylesheet.NumberingFormats.Count = 1;

        // Default fills
        stylesPart.Stylesheet.Fills = new Fills();
        stylesPart.Stylesheet.Fills.Count = 1;
        stylesPart.Stylesheet.Fills.AppendChild(new Fill());

        // Default borders
        stylesPart.Stylesheet.Borders = new Borders();
        stylesPart.Stylesheet.Borders.Count = 1;
        stylesPart.Stylesheet.Borders.AppendChild(new Border());

        // Default cell style formats
        stylesPart.Stylesheet.CellStyleFormats = new CellStyleFormats();
        stylesPart.Stylesheet.CellStyleFormats.Count = 1;
        stylesPart.Stylesheet.CellStyleFormats.AppendChild(new CellFormat());

        // Cell formats
        stylesPart.Stylesheet.CellFormats = new CellFormats();
        stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat());
        stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, NumberFormatId = 1 });
        stylesPart.Stylesheet.CellFormats.Count = 1;
    }

    /// <summary>
    /// Create and populate a worksheet.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    ///  <typeparam name="U"></typeparam>
    /// <param name="id">integer ID of the sheet</param>
    /// <param name="name">worksheet title</param>
    /// <param name="items">collection of entities, each one will be a row on the sheet</param>
    /// <param name="fields">fields from each entity to show as columns</param>
    private void CreateWorksheet<T, U>(uint id, string name, IList<T> items, U[] fields) where T : IFieldContainer where U : IFieldType
    {
        var worksheetPart = _workbookPart.AddNewPart<WorksheetPart>();
        var data = new SheetData();
        worksheetPart.Worksheet = new Worksheet(data);       

        var sheet = new Sheet
        {
            Id = _workbookPart.GetIdOfPart(worksheetPart),
            SheetId = id,
            Name = name
        };
        _sheets.Append(sheet);

        var row = new Row();
        data.Append(row);

        foreach (var field in fields)
        {
            AddCell(row, field.FieldName);
        }

        foreach (var container in items)
        {
            row = new Row();
            data.Append(row);

            foreach (var field in fields)
            {
                AddCell(row, container.Get(field));
            }
        }
    }

    /// <summary>
    /// Adds a cell and trys to interpret the data type of the value appropriately
    /// so the result is correctly formatted in Excel. Falls back on formatting the value
    /// as a string if it doesn't recognise it.
    /// </summary>
    /// <param name="row">row to add the cell to</param>
    /// <param name="value">value for the cell</param>
    private void AddCell(Row row, object value)
    {
        var cell = new Cell();
        row.Append(cell);
        if (value == null)
        {
            return;
        }

        if (value is string stringValue)
        {
            cell.CellValue = new CellValue(stringValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
            return;
        }

        if (value is int intValue)
        {
            cell.CellValue = new CellValue(intValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            return;
        }

        if (value is double doubleValue)
        {
            cell.CellValue = new CellValue(doubleValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            return;
        }

        if (value is DateOnly dateOnlyValue)
        {            
            cell.CellValue = new CellValue(dateOnlyValue.ToDateTime(new TimeOnly(0,0)));
            cell.DataType = new EnumValue<CellValues>(CellValues.Date);
            cell.StyleIndex = 1;
            return;
        }

        if (value is DateTime dateTimeValue)
        {
            cell.CellValue = new CellValue(dateTimeValue);
            cell.DataType = new EnumValue<CellValues>(CellValues.Date);
            cell.StyleIndex = 1;
            return;
        }

        cell.CellValue = new CellValue(value.ToString() ?? string.Empty);
        cell.DataType = new EnumValue<CellValues>(CellValues.String);
    }
}