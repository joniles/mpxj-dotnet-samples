using System.Globalization;
using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class MPXWithLocale
{
    public void Read()
    {
        var reader = new MPXReader();
        reader.Culture = CultureInfo.GetCultureInfo("de");
        var project = reader.Read("my-sample.mpx");
    }
}
