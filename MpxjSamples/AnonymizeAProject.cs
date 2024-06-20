using net.sf.mpxj.utility;

/// <summary>
/// Illustrates how to anonymize a project by replacing the text it contains with nonsense.
/// Preserves the structure of the file to allow debugging MPXJ issues without
/// revealing potentially sensitive content.
/// </summary>
public class AnonymizeAProject
{
	public void Execute(string filename)
	{
        var name = Path.GetFileName(filename);
        new ProjectCleanUtility().process(filename, $"clean-{name}");
    }
}
