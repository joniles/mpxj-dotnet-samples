using MPXJ.Net;

public class MSPDICompatibleInput
{
    public void Read()
    {
        var reader = new MSPDIReader();
        reader.MicrosoftProjectCompatibleInput = false;
        var project = reader.Read("my-sample.xml");
    }
}
