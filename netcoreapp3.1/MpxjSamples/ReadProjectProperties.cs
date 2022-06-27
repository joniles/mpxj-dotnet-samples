using net.sf.mpxj;
using net.sf.mpxj.reader;
using Task = net.sf.mpxj.Task;
using net.sf.mpxj.MpxjUtilities;

public class ReadProjectProperties
{
    public void Execute()
    {
        //
        // Read a schedule from a sample file.
        //
        ProjectFile file = new UniversalProjectReader().read("example.mpp");


        //
        // Iterate through allproject property fields
        //
        var projectProperties = file.ProjectProperties;
        foreach (ProjectField field in ProjectField.values())
        {
            //
            // Retrieve the value for the current field, ignore it if it is null
            //
            object value = projectProperties.GetCachedValue(field);
            if (value == null)
            {
                continue;
            }

            //
            // Write the field name and field value.
            // Here we are relying on the ToString method to give
            // us the string representation from the "raw" type.
            //
            System.Console.WriteLine(field.ToString()
                + ":\t" + value.ToString());
        }
    }
}
