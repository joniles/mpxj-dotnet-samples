﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using net.sf.mpxj;
using net.sf.mpxj.reader;
using net.sf.mpxj.writer;

namespace MpxjSample
{
    class MpxjConvert
    {
        static void Main(string[] args)
        {
#if NETCOREAPP
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
            try
            {
                if (args.Length != 2)
                {
                    Console.Out.WriteLine("Usage: MpxjConvert <input file name> <output file name>");
                }
                else
                {
                    MpxjConvert convert = new MpxjConvert();
                    convert.Process(args[0], args[1]);
                }
            }

            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
            }
        }

        public void Process(string inputFile, string outputFile)
        {
            Console.Out.WriteLine("Reading input file started.");
            DateTime start = DateTime.Now;
            ProjectFile projectFile = new UniversalProjectReader().read(inputFile);
            TimeSpan elapsed = DateTime.Now - start;
            Console.Out.WriteLine("Reading input file completed in " + elapsed.TotalMilliseconds + "ms.");

            Console.Out.WriteLine("Writing output file started.");
            start = DateTime.Now;
            ProjectWriter writer = ProjectWriterUtility.getProjectWriter(outputFile);
            writer.write(projectFile, outputFile);
            elapsed = DateTime.Now - start;
            Console.Out.WriteLine("Writing output completed in " + elapsed.TotalMilliseconds + "ms.");
        }
    }
}
