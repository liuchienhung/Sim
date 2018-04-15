using System.IO;
using UnityEngine;

public class PrefsSave  {
	string pathApp;
	string pathPrefs;
	System.IO.StreamWriter file ;

	public PrefsSave()
	{
		pathApp= Path.GetFullPath(".");
		pathPrefs=pathApp+@"\settings";

		if (!Directory.Exists (pathPrefs)) {
			System.IO.Directory.CreateDirectory (pathPrefs);
		}
		//if (!File.Exists (pathPrefs + @"\userPrefs.txt")) {
		file = new System.IO.StreamWriter (pathPrefs + @"\userPrefs.txt",false);
		//}


	}

	public void savePrefs(string prefName, float value)
	{
		file.WriteLine (prefName+"="+value);
	}
	public void savePrefs(string prefName, string value)
	{
		//print ("saving- "+prefName+"="+value);
		file.WriteLine (prefName+"="+value);
	}
	public void savePrefs(string prefName, int value)
	{
		file.WriteLine (prefName+"="+value);
	}
	public void savePrefs(string prefName, bool value)
	{
		file.WriteLine (prefName+"="+value);
	}




	public string getAppPath()
	{
		return pathApp;
	}

	public void close()
	{
		if (file != null) {
			file.Close ();
		}
	}
}
