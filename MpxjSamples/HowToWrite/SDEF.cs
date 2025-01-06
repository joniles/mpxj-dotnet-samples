using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class SDEF
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.SDEF).Write(project, fileName);
    }
}
