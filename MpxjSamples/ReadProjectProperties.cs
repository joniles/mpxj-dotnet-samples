﻿using MPXJ.Net;

public class ReadProjectProperties
{
    public void Execute(string filename)
    {
        //
        // Read a schedule from a sample file.
        //
        ProjectFile file = new UniversalProjectReader().Read(filename);


        //
        // Iterate through allproject property fields
        //
        var projectProperties = file.ProjectProperties;
        foreach (ProjectField field in ProjectField.Values)
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
