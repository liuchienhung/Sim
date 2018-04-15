using System.IO;
using System;
using UnityEngine;

public class PrefsHeliSave : MonoBehaviour {
	string pathApp;
	string pathPrefs;
	System.IO.StreamWriter file ;

	public PrefsHeliSave()
	{
		pathApp= Path.GetFullPath(".");
		pathPrefs=pathApp+@"\settings";

		if (!Directory.Exists (pathPrefs)) {
			System.IO.Directory.CreateDirectory (pathPrefs);
		}
		//if (!File.Exists (pathPrefs + @"\HeliPrefs.txt")) {
		try{file = new System.IO.StreamWriter (pathPrefs + @"\HeliPrefs.txt",false);}
		catch(Exception e)
		{
			print("can't create HeliPrefs");
			print(e);
		}
		//}
		//print("create HeliPrefs");


	}

	public void savePrefs(string prefName, float value)
	{
		try{
			file.WriteLine (prefName+"="+value);
			//print("saved-"+prefName);
		}
		catch (Exception e)
		{
			print("can't write - " + prefName +" to HeliPrefs");
		print(e);
		}
	}
	public void savePrefs(string prefName, string value)
	{
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
