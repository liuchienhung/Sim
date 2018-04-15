using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsHeliLoad : MonoBehaviour  {
	string pathApp;
	string pathPrefs;
	System.IO.StreamReader file =null;

	public PrefsHeliLoad()
	{
		
		pathApp= Path.GetFullPath(".");
		pathPrefs=pathApp+@"\settings";

		if (Directory.Exists (pathPrefs)) {
			
			if (File.Exists (pathPrefs + @"\HeliPrefs.txt")) {
			try{file = new System.IO.StreamReader (pathPrefs + @"\HeliPrefs.txt",true);
			}
			catch(Exception e)
			{
				print("can't Load HeliPrefs");
				print(e);
			}
			}
		}



	}

	public float loadFloat(string prefName)
	{

		float value=0;
		string line;
		if (file!=null) 
		{
			while (file.Peek () >= 0) {
				line = file.ReadLine ();
				string [] st=line.Split('=');
				if (st [0].Equals (prefName)) {
					value = float.Parse (st [1]);
					break;
				}
			}
			file.DiscardBufferedData ();
			file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
		}
		return value;
	}
	public float loadFloat(string prefName,float defaultValue)
	{
        float value=0;
		string line;
		if (file!=null) 
		{
            
            while (file.Peek () >= 0) {
				line = file.ReadLine ();
              
				string [] st=line.Split('=');
                
                if (st [0].Equals (prefName)) {
					value = float.Parse (st [1]);
					break;
				}
			}
			file.DiscardBufferedData ();
			file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
		}
		if (value == 0) {
			value = defaultValue;
          
		}
		return value;
	}
	public string loadString(string prefName)
	{
		
		string value=null;
		string line;
		if (file!=null) 
		{
			while (file.Peek () >= 0) {
				line = file.ReadLine ();
				string [] st=line.Split('=');
				if (st [0].Equals (prefName)) {
					value = st [1];
					break;
				}
			}
			file.DiscardBufferedData ();
			file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
		}
		return value;

	}

	public int loadInt(string prefName)
	{
		int value=0;
		string line;
		if (file!=null) 
		{
			while (file.Peek () >= 0) {
				line = file.ReadLine ();
				string [] st=line.Split('=');
				if (st [0].Equals (prefName)) {
					value = int.Parse (st [1]);
					break;
				}
			}
			file.DiscardBufferedData ();
			file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
		}
		return value;
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