﻿using junit.framework;
using junit.textui;
using org.mpxj.junit;

namespace MpxjSample
{
    class MpxjTest
    {
        static void Main(string[] args)
        {
#if NETCOREAPP
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif

            java.util.TimeZone.setDefault(java.util.TimeZone.getTimeZone("Europe/London"));

            if (args.Length == 0 || args.Length > 3)
            {
                System.Console.Out.WriteLine("Usage: MpxjTest <mpxj test data directory> [<private test data directory>] [<private test data baseline directory>]");
            }
            else
            {
                java.lang.System.setProperty("mpxj.junit.datadir", args[0]);
                if (args.Length > 1)
                {
                    java.lang.System.setProperty("mpxj.junit.privatedir", args[1]);
                }

                if (args.Length > 2)
                {
                    java.lang.System.setProperty("mpxj.junit.baselinedir", args[2]);
                }

                TestRunner.runAndWait(new JUnit4TestAdapter(new MpxjTestSuite().getClass()));
            }
        }
    }
}