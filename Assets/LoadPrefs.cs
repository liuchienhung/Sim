using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPrefs : MonoBehaviour {
	string pathApp;
	string pathPrefs;
	System.IO.StreamReader file =null;
	string fileName;
	string folder="";

	public void setFileNameAndFold(string fileName,string folder)
	{
		this.fileName = fileName;
		this.folder=folder;
	}
	public void setFileName(string fileName)
	{
		this.fileName = fileName;
	}

	public void initialize()
	{
		pathApp= Path.GetFullPath(".");
		pathPrefs=pathApp+@"\settings"+@"\"+folder;

		if (Directory.Exists (pathPrefs)) {

            if (File.Exists(pathPrefs + @"\" + fileName + ".txt"))
            {
                file = new System.IO.StreamReader(pathPrefs + @"\" + fileName + ".txt", true);
            }
            else { print("file-"+ fileName + " not exist"); }
		}
        else { print("directory-" + pathPrefs + " not exist"); }
    }
	public LoadPrefs(){
	}
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
	public LoadPrefs(string fileName,string folder)
	{
		this.fileName = fileName;
		pathApp= Path.GetFullPath(".");
		pathPrefs=pathApp+@"\settings"+@"\"+folder;

		if (Directory.Exists (pathPrefs)) {

            if (File.Exists(pathPrefs + @"\" + fileName + ".txt"))
            {
                file = new System.IO.StreamReader(pathPrefs + @"\" + fileName + ".txt", true);
            }
            else{
                print("file - " + fileName + " not exist");
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
                    print("Loaded - " + prefName + " = " + value);
                    break;
				}
			}
		}
        else
        {
            print("file=null");
        }
        return value;
	}



	public float loadFloat(string prefName,float defaultValue)
	{
		float value=defaultValue;
		string line;
		if (file != null) {
			while (file.Peek () >= 0) {
				line = file.ReadLine ();
				string[] st = line.Split ('=');
				if (st [0].Equals (prefName)) {
					value = float.Parse (st [1]);
					file.DiscardBufferedData ();
					file.BaseStream.Seek (0, System.IO.SeekOrigin.Begin);
					print ("Loaded - " + prefName + " = " + value);
					break;
				}
			}
			file.DiscardBufferedData ();
			file.BaseStream.Seek (0, System.IO.SeekOrigin.Begin);
		}
		else {
			print ("file=null");
		}
		return value;
	}
	public bool loadBool(string prefName,bool defaultValue)
	{
		bool value= defaultValue;		
		string line;
		if (file!=null) 
		{
			while (file.Peek () >= 0) {
				line = file.ReadLine ();
				string [] st=line.Split('=');
				if (st [0].Equals (prefName)) {
					value = bool.Parse (st [1]);
					file.DiscardBufferedData ();
					file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
                    print("Loaded - " + prefName + " = " + value);
                    break;
				}
			}
		}
        else
        {
            print("file=null");
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
					file.DiscardBufferedData ();
					file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
                    print("Loaded - " + prefName + " = " + value);
                    break;
				}
			}
		}
        else
        {
            print("file=null");
        }
        return value;

	}

	public string loadString(string prefName,string defaultValue)
	{
		string value=defaultValue;
		string line;
		if (file!=null) 
		{
			while (file.Peek () >= 0) {
				line = file.ReadLine ();
				string [] st=line.Split('=');
				if (st [0].Equals (prefName)) {
					value = st [1];
					file.DiscardBufferedData ();
					file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
                    print("Loaded - " + prefName + " = " + value);
                    break;
				}
			}
		}
        else
        {
            print("file=null");
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
					file.DiscardBufferedData ();
					file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
                    print("Loaded - " + prefName + " = " + value);
                    break;
				}
			}
		}
        else
        {
            print("file=null");
        }
        return value;
	}
	public int loadInt(string prefName, int defaultValue)
	{
		int value=defaultValue;
		string line;
		if (file!=null) 
		{
			while (file.Peek () >= 0) {
				line = file.ReadLine ();
				string [] st=line.Split('=');
				if (st [0].Equals (prefName)) {
					value = int.Parse (st [1]);
					file.DiscardBufferedData ();
					file.BaseStream.Seek (0,System.IO.SeekOrigin.Begin);
                    print("Loaded - " + prefName + " = " + value);
                    break;
				}
			}
		}
        else
        {
            print("file=null");
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