﻿using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class P3Directory
{
    public void Read()
    {
        var reader = new UniversalProjectReader();
        var project = reader.Read("my-p3-directory");
    }
}
