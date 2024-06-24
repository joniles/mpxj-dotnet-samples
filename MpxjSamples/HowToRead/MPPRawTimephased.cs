using MPXJ.Net;

public class MPPRawTimephased
{
    public void Read()
    {
        var reader = new MPPReader();
        reader.UseRawTimephasedData = true;
        var project = reader.Read("my-sample.mpp");
    }
}
