using MPXJ.Net;

namespace MPXJ.Samples.HowToWrite;

public class SDEF
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.SDEF).Write(project, fileName);
    }
}
