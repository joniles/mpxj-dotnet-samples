﻿using MPXJ.Net;

namespace MPXJ.Samples.HowToRead;

public class MPPPropertiesOnly
{
    public void Read()
    {
        var reader = new MPPReader();
        reader.ReadPropertiesOnly = true;
        var project = reader.Read("my-sample.mpp");
    }
}
