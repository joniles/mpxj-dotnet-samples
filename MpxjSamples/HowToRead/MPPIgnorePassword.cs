﻿using MPXJ.Net;

namespace MpxjSamples.HowToRead;

public class MPPIgnorePassword
{
    public void Read()
    {
        var reader = new MPPReader();
        reader.RespectPasswordProtection = false;
        var project = reader.Read("my-sample.mpp");
    }
}
