using System;
using MPXJ.Net;

public class ReadTaskFields
{
    public void Execute()
    {
        //
        // Loop through the values in the TaskField enum
        // and write them to the console.
        //
        foreach (var field in TaskField.Values)
        {
            Console.WriteLine(field.ToString());
        }
        Console.WriteLine();


        //
        // Read a schedule from a sample file.
        //
        var file = new UniversalProjectReader().Read("example.mpp");

        //
        // Let's take a simple approach to examining all of the non-null attributes
        // for each task.
        //
        foreach (var task in file.Tasks)
        {
            //
            // Write each task's ID and Name to the console
            //
            Console.WriteLine(task.ID + ":\t" + task.Name);

            //
            // Iterate through all possible fields
            //
            foreach (var field in TaskField.Values)
            {
                //
                // Retrieve the value for the current field, ignore it if it is null
                //
                var value = task.GetCachedValue(field);
                if (value == null)
                {
                    continue;
                }

                //
                // Write the field name and field value.
                // Here we ae relying on the ToString method to give
                // us the string representation from the "raw" type.
                //
                Console.WriteLine("\t" + field
                    + ":\t" + value);
            }
        }
        Console.WriteLine();

        //
        // Now let's see how we can work directly with the type values
        // rather than relying on the ToString method.
        //
        foreach (var task in file.Tasks)
        {
            //
            // Write each task's ID and Name to the console
            //
            Console.WriteLine(task.ID + ":\t" + task.Name);

            //
            // Iterate through all possible fields
            //
            foreach (var field in TaskField.Values)
            {
                //
                // Retrieve the value for the current field, ignore it if it is null
                //
                var value = task.GetCachedValue(field);
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
                    case DataType.Date:
                        {
                            // Now we know we are working with a DATE, we
                            // can manipulate the "raw" value directly, rather than
                            // relying on the ToString method. In this example we'll
                            // create a variable of the correct type:
                            var dateTimeValue = value as DateTime?;
                            Console.WriteLine("\t" + field
                                + ":\t" + dateTimeValue);
                            break;
                        }

                    case DataType.Currency:
                        {
                            // Here's another example, if we know we're working with a currency
                            // we can cast the value to a double
                            // In this case we're also formatting the value as a currency.
                            var numberValue = value as double?;
                            Console.WriteLine("\t" + field
                                + ":\t" + numberValue?.ToString("C2"));
                            break;
                        }

                    case DataType.String:
                        {
                            // Last example: the STRING data type is already a dot net string
                            // so we can use that directly.
                            var stringValue = value as string;
                            Console.WriteLine("\t" + field
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

        foreach (var task in file.Tasks)
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
                    Console.WriteLine("\t" + field
                        + ":\t" + value);
                }
            }
        }
    }
}
