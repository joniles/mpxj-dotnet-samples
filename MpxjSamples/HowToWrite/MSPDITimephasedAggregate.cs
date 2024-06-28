using MPXJ.Net;

namespace MPXJ.Samples.HowToWrite;

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
