using System.Text;
using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class SDEFCharset
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new SDEFWriter();
        writer.Encoding = Encoding.GetEncoding("UTF8");
        writer.Write(project, fileName);
    }
}
