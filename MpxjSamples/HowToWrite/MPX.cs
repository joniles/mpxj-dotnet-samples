using MPXJ.Net;

namespace MPXJ.Samples.HowToWrite;

public class MPX
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.MPX).Write(project, fileName);
    }
}
