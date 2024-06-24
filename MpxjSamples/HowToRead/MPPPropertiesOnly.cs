using MPXJ.Net;

public class MPPPropertiesOnly
{
    public void Read()
    {
        var reader = new MPPReader();
        reader.ReadPropertiesOnly = true;
        var project = reader.Read("my-sample.mpp");
    }
}
