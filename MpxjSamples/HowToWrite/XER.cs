using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class XER
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.XER).Write(project, fileName);
    }
}
