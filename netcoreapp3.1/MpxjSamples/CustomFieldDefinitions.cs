using net.sf.mpxj;
using net.sf.mpxj.reader;

public class CustomFieldDefinitions
{
    public void Execute()
    {
        ProjectFile file = new UniversalProjectReader().read("/Users/joniles/Downloads/Enterprise Project Test 8.mpp");
        foreach(CustomField field in file.CustomFields)
        {            
            System.Console.WriteLine($"{field.FieldType.FieldTypeClass.ToString()}.{field.FieldType.ToString()}");

            if (field.Alias != null)
            {
                System.Console.WriteLine($"\tAlias: {field.Alias}");
            }

            System.Console.WriteLine($"\tData Type: {field.FieldType.DataType.ToString()}");

            if (field.CustomFieldDataType != null)
            {
                System.Console.WriteLine($"\tCustom Field Data Type: {field.CustomFieldDataType}");
            }
        }
    }
}

