using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class Logger : MonoBehaviour{
	string pathApp;
	string pathLogs;
	StreamWriter file ;
	string header;
	StringBuilder line=new StringBuilder();
	bool firstLine;
	float timer=0;
	float logTimeStep;//Seconds
	int cycle=0;
    string logFileName;

	ArrayList names =new ArrayList();
	Dictionary <string,float> row = new Dictionary <string,float> ();

	public void Init(string fileName,float setLogTime)
	{
		this.logFileName = fileName;
		pathApp= Path.GetFullPath(".");
		pathLogs=pathApp+@"\Logs";
		logTimeStep = setLogTime;

		if (!Directory.Exists (pathLogs)) {
			System.IO.Directory.CreateDirectory (pathLogs);
		}
		if (File.Exists (pathLogs + @"\"+fileName+".txt")) {
			try{File.Delete (pathLogs + @"\" + fileName + ".txt");}
			catch(Exception e)
			{
				print("can't delete Logs");
				print(e);
			}
		}
		//file = new System.IO.StreamWriter (pathLogs + @"\"+fileName+".txt",false);
		try{file= File.AppendText(pathLogs + @"\" + fileName + ".txt");}
		catch(Exception e)
		{
			print("can't create Logs");
			print(e);
		}
		firstLine =true;
	}

		
	public void saveLog(string valName, float value)
	{

		if (!row.ContainsKey (valName)) {
			names.Add (valName);
			row.Add (valName, value);
		} 
		else {
			row[valName]=value;	
		}
		//file.WriteLine (prefName+"="+value);
	}

	public void writeCycle()
	{
		isFirstLine ();

		firstLine = false;
		timer = timer + Time.fixedDeltaTime;
		if (timer > logTimeStep*cycle) {
			
			foreach (string n in names) {
				line.Append( row [n] + ";");
			}
			cycle=cycle+1;
            try
            {
                file.WriteLine(line);
                file.Flush();
            }
            catch (Exception e)
            {
                print("can't write - " + logFileName);
                print(e);
            }

                line.Length=0;
		}

		//file.Flush ();

		
	}

    void isFirstLine()
	{
		if(firstLine)
		{
			//
			foreach(string n in names)
			{
				header=header+n+";" ;
			}
			try
			{
				file.WriteLine (header);
				file.Flush ();
		}
		catch (Exception e)
		{
			print("can't write header - " + logFileName);
			print(e);
		}
		}
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
