using MPXJ.Net;
using Task = MPXJ.Net.Task;

public class ReadTaskFields
{
    public void Execute()
    {
        //
        // Loop through the values in the TaskField enum
        // and write them to the console.
        //
        foreach (TaskField field in TaskField.Values)
        {
            Console.WriteLine(field.ToString());
        }
        Console.WriteLine();


        //
        // Read a schedule from a sample file.
        //
        ProjectFile file = new UniversalProjectReader().Read("example.mpp");

        //
        // Let's take a simple approach to examining all of the non-null attributes
        // for each task.
        //
        foreach (Task task in file.Tasks)
        {
            //
            // Write each task's ID and Name to the console
            //
            Console.WriteLine(task.ID + ":\t" + task.Name);

            //
            // Iterate through all possible fields
            //
            foreach (TaskField field in TaskField.Values)
            {
                //
                // Retrieve the value for the current field, ignore it if it is null
                //
                object value = task.GetCachedValue(field);
                if (value == null)
                {
                    continue;
                }

                //
                // Write the field name and field value.
                // Here we ae relying on the ToString method to give
                // us the string representation from the "raw" type.
                //
                System.Console.WriteLine("\t" + field.ToString()
                    + ":\t" + value.ToString());
            }
        }
        System.Console.WriteLine();

        //
        // Now let's see how we can work directly with the type values
        // rather than relying on the ToString method.
        //
        foreach (Task task in file.Tasks)
        {
            //
            // Write each task's ID and Name to the console
            //
            Console.WriteLine(task.ID + ":\t" + task.Name);

            //
            // Iterate through all possible fields
            //
            foreach (TaskField field in TaskField.Values)
            {
                //
                // Retrieve the value for the current field, ignore it if it is null
                //
                object value = task.GetCachedValue(field);
                if (value == null)
                {
                    continue;
                }

                //
                // A slightly more refined approach would be to look at the type
                // of the value we've been given and work with the raw value.
                //
                switch (field.DataType)
                {
                    // This nasty syntax is unfortunately required to
                    // ensure that the original Java enum values can be used in
                    // dot net code.
                    case DataType.Date:
                        {
                            // Now we know we are working with a DATE, we
                            // can manipulate the "raw" value directly, rather than
                            // relying on the ToString method. In this example we'll
                            // create a variable of the correct type:
                            var dateTimeValue = value as DateTime?;
                            System.Console.WriteLine("\t" + field.ToString()
                                + ":\t" + dateTimeValue.ToString());
                            break;
                        }

                    // Here's another example, if we know we're working with a currency
                    // we can cast to the original Java type, and from there we can
                    // retrieve a dot net double, making it much easier to work with.
                    // In this case we're formatting the value as a currency.
                    case DataType.Currency:
                        {
                            var numberValue = value as double?;
                            System.Console.WriteLine("\t" + field.ToString()
                                + ":\t" + numberValue?.ToString("C2"));
                            break;
                        }

                    // Last example: the STRING data type is alreday a dot net string
                    // so we can use that directly.
                    case DataType.String:
                        {
                            var stringValue = value as string;
                            System.Console.WriteLine("\t" + field.ToString()
                                + ":\t" + stringValue);
                            break;
                        }
                }
            }
        }

        //
        // Rather than looking at all of the values in the TaskField
        // enumeration, let's just examine the fields which are populated
        // with non-null, non-default values. In this case we're working with
        // FieldType which is the supertype of TaskField, ResourceField and so on.
        // All of the populated fields in the project from all of the different entities
        // are reported by PopulatedFields property.
        //
        var populatedFields = file.Tasks.PopulatedFields;

        foreach (Task task in file.Tasks)
        {
            // Write each task's ID and Name to the console
            Console.WriteLine(task.ID + ":\t" + task.Name);

            // Iterate through all of the populate fields
            foreach (var field in populatedFields)
            {
                var value = task.GetCachedValue(field);

                // Although the field may be populated in at least one task in the
                // schedule, it might not be populated in this particular one, so
                // check for null here.
                if (value != null)
                {
                    // Just write the field value using ToString
                    System.Console.WriteLine("\t" + field.ToString()
                        + ":\t" + value.ToString());
                }
            }
        }
    }
}
