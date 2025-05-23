﻿using System.Globalization;
using MPXJ.Net;

namespace MpxjSamples.HowToWrite;

public class MPXLocale
{
    public void Write(ProjectFile project, string fileName)
    {
        var writer = new MPXWriter();
        writer.Culture = CultureInfo.GetCultureInfo("de");
        writer.Write(project, fileName);
    }
}
