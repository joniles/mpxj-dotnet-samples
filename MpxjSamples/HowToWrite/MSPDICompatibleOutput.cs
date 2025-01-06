using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class MSPDICompatibleOutput
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new MSPDIWriter();
        writer.MicrosoftProjectCompatibleOutput = false;
        writer.Write(project, fileName);
    }
}
