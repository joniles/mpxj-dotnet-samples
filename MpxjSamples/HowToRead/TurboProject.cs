﻿using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class TurboProject
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-sample.pep");
    }
}
