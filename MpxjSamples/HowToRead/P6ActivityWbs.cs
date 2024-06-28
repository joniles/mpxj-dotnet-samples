using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class P6ActivityWbs
{
    public void Read()
    {
        var reader = new PrimaveraDatabaseReader();
        reader.MatchPrimaveraWBS = false;
    }
}
