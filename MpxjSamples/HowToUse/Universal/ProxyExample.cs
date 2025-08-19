namespace MpxjSamples.HowToUse.Universal;

using MPXJ.Net;

public class ProxyExample
{
    public void Process(string file)
    {
        var upr = new UniversalProjectReader();

        // Retrieve the proxy
        using var proxy = upr.GetProjectReaderProxy(file);
        
        // Retrieve the reader class
        var reader = proxy.ProjectReader;

        // Determine if we want to continue processing this file type.
        // In this example we are ignoring SDEF files.
        if (reader is SDEFReader)
        {
            return;
        }

        // Provide configuration for specific reader types.
        // In this example we are changing the behavior of the Phoenix reader.
        var phoenixReader = reader as PhoenixReader;
        if (phoenixReader != null)
        {
            phoenixReader.UseActivityCodesForTaskHierarchy = false;
        }

        // Finally, we read the schedule
        var project = proxy.Read();

        // Now we can work with the schedule data...
    }
}