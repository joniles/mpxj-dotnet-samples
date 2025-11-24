using System.Collections.Generic;
using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class XER
{
    public void Write(ProjectFile project, string fileName)
    {
        new UniversalProjectWriter(FileFormat.XER).Write(project, fileName);
    }
    
    public void Write(IList<ProjectFile> projects, string fileName)
    {
        new UniversalProjectWriter(FileFormat.XER).Write(projects, fileName);
    }
}
