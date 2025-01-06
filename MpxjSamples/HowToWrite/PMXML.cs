using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class PMXML
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.PMXML).Write(project, fileName);
    }
}
