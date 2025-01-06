using System.Globalization;
using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class MPXWithLocale
{
    public void Read()
    {
        var reader = new MPXReader();
        reader.Culture = CultureInfo.GetCultureInfo("de");
        var project = reader.Read("my-sample.mpx");
    }
}
