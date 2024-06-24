using System.Globalization;
using MPXJ.Net;

public class MPXWithLocale
{
    public void Read()
    {
        var reader = new MPXReader();
        reader.Culture = CultureInfo.GetCultureInfo("de");
        var project = reader.Read("my-sample.mpx");
    }
}
