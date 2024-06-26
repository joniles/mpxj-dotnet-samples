using MPXJ.Net;

public class MSPDIIgnoreErrors
{
    public void Read()
    {
        var reader = new MSPDIReader();
        reader.IgnoreErrors = false;
        var project = reader.Read("my-sample.xml");
    }
}
