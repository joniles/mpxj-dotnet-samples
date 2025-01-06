using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class MSPDI
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.MSPDI).Write(project, fileName);
    }
}
