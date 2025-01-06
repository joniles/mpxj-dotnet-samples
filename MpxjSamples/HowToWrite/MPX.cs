using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class MPX
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.MPX).Write(project, fileName);
    }
}
