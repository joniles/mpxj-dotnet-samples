using System;
using System.Collections.Generic;
using System.IO;
using MPXJ.Net;

namespace MpxjSample
{
    class MpxjConvert
    {
        private static readonly Dictionary<string, FileFormat> FileFormatDictionary = new Dictionary<string, FileFormat>()
        {
            { "MPX", FileFormat.MPX },
            { "XML", FileFormat.MSPDI },
            { "PMXML", FileFormat.PMXML },
            { "PLANNER", FileFormat.PLANNER },
            { "JSON", FileFormat.JSON },
            { "SDEF", FileFormat.SDEF },
            { "XER", FileFormat.XER },
        };


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
            var start = DateTime.Now;
            var projectFile = new UniversalProjectReader().Read(inputFile);
            var elapsed = DateTime.Now - start;
            Console.Out.WriteLine("Reading input file completed in " + elapsed.TotalMilliseconds + "ms.");

            var extension = Path.GetExtension(outputFile);
            if (extension == null || extension.Length == 0)
            {
                throw new ArgumentException("Filename has no extension");
            }

            extension = extension.Substring(1).ToUpper();
            if (!FileFormatDictionary.TryGetValue(extension, out var format))
            {
                throw new ArgumentException($"Unsupported file extension: {extension}");
            }

            Console.Out.WriteLine("Writing output file started.");
            start = DateTime.Now;
            new UniversalProjectWriter(format).Write(projectFile, outputFile);
            elapsed = DateTime.Now - start;
            Console.Out.WriteLine("Writing output completed in " + elapsed.TotalMilliseconds + "ms.");
        }
    }
}
