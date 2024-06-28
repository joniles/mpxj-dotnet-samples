using MPXJ.Net;

namespace MPXJ.Samples.HowToWrite;

public class PMXML
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.PMXML).Write(project, fileName);
    }
}
