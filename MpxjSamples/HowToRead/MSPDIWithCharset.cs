using System.Text;
using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class MSPDIWithLocale
{
    public void Read()
    {
        var reader = new MSPDIReader();
        reader.Encoding = Encoding.GetEncoding("GB2312");
        var project = reader.Read("my-sample.xml");
    }
}
