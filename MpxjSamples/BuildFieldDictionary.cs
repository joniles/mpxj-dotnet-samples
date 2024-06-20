using MPXJ.Net;

public class BuildFieldDictionary
{
    public void Execute()
    {
        //
        // Create a dictionary allowing us to look up field types by name
        //
        var dictionary = TaskField.Values.ToDictionary(t => t.ObjectName);

        //
        // Test the lookup
        //
        var field = dictionary["TEXT1"];
        System.Console.WriteLine($"Field name={field.ObjectName} User-visible name={field.FieldName} Data type={field.DataType} Parent type={field.FieldTypeClass}");

        //
        // Alternative approach using simple iteration
        //
        dictionary = new Dictionary<string, TaskField>();
        foreach(TaskField t in TaskField.Values)
        {
            dictionary.Add(t.ObjectName, t);
        }

        field = dictionary["TEXT1"];
        System.Console.WriteLine($"Field name={field.ObjectName} User-visible name={field.FieldName} Data type={field.DataType} Parent type={field.FieldTypeClass}");
    }
}
