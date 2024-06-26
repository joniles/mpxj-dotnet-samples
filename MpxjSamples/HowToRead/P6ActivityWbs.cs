using MPXJ.Net;

public class P6ActivityWbs
{
    public void Read()
    {
        var reader = new PrimaveraDatabaseReader();
        reader.MatchPrimaveraWBS = false;
    }
}
