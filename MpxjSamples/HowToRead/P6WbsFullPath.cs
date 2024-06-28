using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class P6WbsFullPath
{
    public void Read()
    {
        var reader = new PrimaveraDatabaseReader();
        reader.WbsIsFullPath = false;
    }
}
