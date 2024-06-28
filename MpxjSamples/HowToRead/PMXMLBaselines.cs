using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class PMXMLBaselines
{
    public void Read()
    {
        var reader = new PrimaveraPMFileReader();
        // TODO enable for MPXJ > 13.0.0
        //reader.BaselineStrategy = PrimaveraBaselineStrategy.CurrentAttributes;
        var files = reader.ReadAll("my-sample.xml");
    }
}
