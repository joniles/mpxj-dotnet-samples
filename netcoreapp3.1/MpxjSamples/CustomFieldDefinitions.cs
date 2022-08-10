using net.sf.mpxj;
using net.sf.mpxj.reader;

/// <summary>
/// Illustrates retrieving custom field configuration details.
/// </summary>
public class CustomFieldDefinitions
{
    public void Execute()
    {
        ProjectFile file = new UniversalProjectReader().read("/Users/joniles/Downloads/Enterprise Project Test 8.mpp");
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

            // Special case: Enterprise Custom Fields have a data type of CUSTOM
            // the actual type of the data they contain is held separately,
            // we'll retrieve that detail here if it has been configured.
            if (field.CustomFieldDataType != null)
            {
                System.Console.WriteLine($"\tCustom Field Data Type: {field.CustomFieldDataType}");
            }
        }
    }
}

