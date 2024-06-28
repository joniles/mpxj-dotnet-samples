using MPXJ.Net;

namespace MPXJ.Samples.HowToWrite;

public class XER
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.XER).Write(project, fileName);
    }
}
