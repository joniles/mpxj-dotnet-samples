using net.sf.mpxj;
using net.sf.mpxj.reader;

/// <summary>
/// Illustrates retrieving custom field configuration details.
/// </summary>
public class CustomFieldDefinitions
{
    public void Execute()
    {
        ProjectFile file = new UniversalProjectReader().read("example.mpp");
        foreach(CustomField field in file.CustomFields)
        {
            // Show the name of the field (and the entity to which it belongs)
            System.Console.WriteLine($"{field.FieldType.FieldTypeClass.ToString()}.{field.FieldType.ToString()}");

            // If the field has been given a new name, display it
            if (field.Alias != null)
            {
                System.Console.WriteLine($"\tAlias: {field.Alias}");
            }

            // Display the data type
            System.Console.WriteLine($"\tData Type: {field.FieldType.DataType.ToString()}");
        }
    }
}

