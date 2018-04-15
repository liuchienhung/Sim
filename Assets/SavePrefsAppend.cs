
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class SavePrefsAppend : MonoBehaviour{
	string pathApp;
	string pathPrefs;

	System.IO.StreamWriter file ;
    string fileName;


	
	public SavePrefsAppend(string fileName)
	{
        this.fileName = fileName;
		pathApp= Path.GetFullPath(".");
		pathPrefs=pathApp+@"\settings";

	
		if (!Directory.Exists (pathPrefs)) {
			System.IO.Directory.CreateDirectory (pathPrefs);
		}
        if (File.Exists (pathPrefs + @"\"+fileName+".txt")) {

			try{File.Delete (pathPrefs + @"\" + fileName + ".txt");}
			catch(Exception e)
			{
                print("can't delete Prefs");
                print(e);
            }
		}

			try{file= File.AppendText(pathPrefs + @"\" + fileName + ".txt");}
       		catch(Exception e)
		    {
				print("can't create Prefs");
				print(e);
		    }

	}
		

	public void save(string valName, float value)
	{
		
		
            try
            {
                file.WriteLine(valName+"="+value);
               
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
