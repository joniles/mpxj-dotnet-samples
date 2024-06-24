using MPXJ.Net;

public class MPXIgnoreTextModels
{
    public void Read()
    {
        var reader = new MPXReader();
        reader.IgnoreTextModels = false;
        var project = reader.Read("my-sample.mpx");
    }
}
