
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class SavePrefs : MonoBehaviour{
	string pathApp;
	string pathPrefs;

	System.IO.StreamWriter file ;
    string fileName;
	string folder;

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
		if (!Directory.Exists (pathPrefs)) {
			System.IO.Directory.CreateDirectory (pathPrefs);
		}
		try{file = new System.IO.StreamWriter (pathPrefs + @"\"+fileName+".txt",false);}
		catch(Exception e)
		{
			print("can't create Prefs - "+ fileName);
			print(e);
		}
	}

	public SavePrefs(string fileName)
	{
        this.fileName = fileName;
		pathApp= Path.GetFullPath(".");
		pathPrefs=pathApp+@"\settings";

	
		if (!Directory.Exists (pathPrefs)) {
			System.IO.Directory.CreateDirectory (pathPrefs);
		}


			try{file = new System.IO.StreamWriter (pathPrefs + @"\"+fileName+".txt",false);}
            catch(Exception e)
			{
				print("can't create Prefs - "+ fileName);
                print(e);
            }

	}
	public SavePrefs(string fileName,string folder)
	{
		this.fileName = fileName;
		pathApp= Path.GetFullPath(".");
		pathPrefs=pathApp+@"\settings"+@"\"+folder;


		if (!Directory.Exists (pathPrefs)) {
			System.IO.Directory.CreateDirectory (pathPrefs);
		}


		try{file = new System.IO.StreamWriter (pathPrefs + @"\"+fileName+".txt",false);}
		catch(Exception e)
		{
			print("can't create Prefs - "+ fileName);
			print(e);
		}
	}	

	public void save(string valName, float value)
	{
            try
            {
                file.WriteLine(valName+"="+value);
			file.Flush();
            }
            catch (Exception e)
            {
			print("can't write - "+valName +" to "+ fileName);
                print(e);
            }   
	}

	public void save(string valName, string value)
	{
            try
            {
                file.WriteLine(valName+"="+value);
				file.Flush();
            }
            catch (Exception e)
            {
			print("can't write - "+valName +" to "+ fileName);
                print(e);
            }   
	}
	
	public void save(string valName, bool value)
	{
            try
            {
                file.WriteLine(valName+"="+value);
			file.Flush();
            }
            catch (Exception e)
            {
			print("can't write - "+valName +" to "+ fileName);
                print(e);
            }   
	}
	public void close()
	{
		if (file != null) {
			file.Close ();
		}
	}


}
