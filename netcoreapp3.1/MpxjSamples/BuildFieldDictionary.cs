using net.sf.mpxj;
using net.sf.mpxj.reader;
using Task = net.sf.mpxj.Task;
using net.sf.mpxj.MpxjUtilities;
using System.Linq;
using System.Collections.Immutable;

public class BuildFieldDictionary
{
    public void Execute()
    {
        //
        // Create a dictionary allowing us to look up field types by name
        //
        var dictionary = ImmutableArray.Create(TaskField.values()).ToDictionary<string, TaskField>(t => t.name());

        //
        // Test the lookup
        //
        var field = dictionary["TEXT1"];
        System.Console.WriteLine($"Field name={field.name()} User-visible name={field.Name} Data type={field.DataType} Parent type={field.FieldTypeClass}");
    }
}
