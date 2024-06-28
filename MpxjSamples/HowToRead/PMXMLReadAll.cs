using MPXJ.Net;

public class PMXMLReadAll
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.ReadAll("my-sample.xml");
    }
}
