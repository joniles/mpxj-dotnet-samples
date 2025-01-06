using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class MSPDITimephasedAggregate
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new MSPDIWriter();
        writer.WriteTimephasedData = true;
        writer.SplitTimephasedAsDays = true;
        writer.Write(project, fileName);
    }
}
