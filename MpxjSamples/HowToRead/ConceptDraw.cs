using MPXJ.Net;

public class ConceptDraw
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.cdpx");
    }
}
