using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class MSPDITimephased
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new MSPDIWriter();
        writer.WriteTimephasedData = true;
        writer.Write(project, fileName);
    }
}
