using System.Text;
using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class PlannerCharset
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new PlannerWriter();
        writer.Encoding = Encoding.GetEncoding("GB2312");
        writer.Write(project, fileName);
    }
}
