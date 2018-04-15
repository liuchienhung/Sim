﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPrefs : MonoBehaviour {
	string pathApp;
	string pathPrefs;
	System.IO.StreamReader file =null;
	string fileName;
	public LoadPrefs(string fileName)
	{
		this.fileName = fileName;
		pathApp= Path.GetFullPath(".");
		pathPrefs=pathApp+@"\settings";

		if (Directory.Exists (pathPrefs)) {
			
			if (File.Exists (pathPrefs + @"\"+fileName+".txt")) {
			file = new System.IO.StreamReader (pathPrefs + @"\"+fileName+".txt",true);
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
					file.DiscardBufferedData ();
					file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
					
					break;
				}
			}
		}
		return value;
	}
	public float loadFloat(string prefName,float defaultValue)
	{
		float value=-1000;
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
		}
		if (value == -1000) {
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