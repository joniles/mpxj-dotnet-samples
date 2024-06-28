using MPXJ.Net;

public class PMXMLProjectID
{
    public void Read()
    {
        var reader = new PrimaveraPMFileReader();
        // TODO enable for MPXJ > 13.0.0
        //reader.ProjectID = 123;
        var project = reader.Read("my-sample.xml");
    }
}
