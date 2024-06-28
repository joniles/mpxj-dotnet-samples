﻿using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class PMXMLLinkCrossProject
{
    public void Read()
    {
        var reader = new PrimaveraPMFileReader();
        reader.LinkCrossProjectRelations = true;
        var files = reader.ReadAll("my-sample.xml");
    }
}
