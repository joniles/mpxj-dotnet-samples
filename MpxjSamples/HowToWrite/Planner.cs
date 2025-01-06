using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class Planner
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.PLANNER).Write(project, fileName);
    }
}
