using System.Text;
using MPXJ.Net;

namespace MPXJ.Samples.HowToWrite;

public class SDEFCharset
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new SDEFWriter();
        // TODO enable for MPXJ > 13.0.0
        //writer.Encoding = Encoding.GetEncoding("UTF8");
        writer.Write(project, fileName);
    }
}
