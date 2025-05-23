﻿using org.mpxj.utility;

namespace MpxjSamples;

/// <summary>
/// Illustrates how to anonymize a project by replacing the text it contains with nonsense.
/// Preserves the structure of the file to allow debugging MPXJ issues without
/// revealing potentially sensitive content.
/// </summary>
public class AnonymizeAProject
{
	public void Execute(string filename)
	{
		var name = System.IO.Path.GetFileName(filename);
		new ProjectCleanUtility().process(filename, $"clean-{name}");
	}
}