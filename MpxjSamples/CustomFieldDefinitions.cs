using MPXJ.Net;


/// <summary>
/// Illustrates retrieving custom field configuration details.
/// </summary>
public class CustomFieldDefinitions
{
    public void Execute(string filename)
    {
        var file = new UniversalProjectReader().Read(filename);
        foreach(var field in file.CustomFields)
        {
            // Show the name of the field (and the entity to which it belongs)
            System.Console.WriteLine($"{field.FieldType.FieldTypeClass}.{field.FieldType}");

            // If the field has been given a new name, display it
            if (field.Alias != null)
            {
                System.Console.WriteLine($"\tAlias: {field.Alias}");
            }

            // Display the data type
            System.Console.WriteLine($"\tData Type: {field.FieldType.DataType}");
        }
    }
}

