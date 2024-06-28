using MPXJ.Net;

namespace MPXJ.Samples.HowToWrite;

public class MSPDISaveVersion
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new MSPDIWriter();
        writer.SaveVersion = SaveVersion.Project2002;
        writer.Write(project, fileName);
    }
}
