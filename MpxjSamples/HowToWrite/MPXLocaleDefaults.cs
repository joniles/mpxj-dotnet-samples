using System.Globalization;
using MPXJ.Net;

namespace MPXJ.Samples.HowToWrite;

public class MPXLocaleDefaults
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new MPXWriter();
        writer.Culture = CultureInfo.GetCultureInfo("de");
        writer.UseCultureDefaults = false;
        writer.Write(project, fileName);
    }
}
