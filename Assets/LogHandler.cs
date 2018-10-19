//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class LogHandler : MonoBehaviour
{
    private GameObject obj;
    string logName;
    private  StreamWriter _writer;
    void Awake()
    {
       
        obj = gameObject;
        logName = gameObject.transform.name;
        print("Logger- " + logName + " awake");


        string pathApp = Path.GetFullPath(".");

		if (File.Exists (pathApp + @"\"+logName+".txt")) {
			
				if (new System.IO.FileInfo (pathApp + @"\" + logName + ".txt").Length/100>400) {
				try {
					File.Delete (pathApp + @"\" + logName + ".txt");
					print ("delete ,logs");
				} catch (Exception e) {
					print ("can't delete ,logs");
				}
			}
		}
       
        _writer = File.AppendText(logName + ".txt");
        
        _writer.WriteLine("\n\n=============== Logs "+logName+" started ================\n\n");
        DontDestroyOnLoad(gameObject);
        Application.RegisterLogCallback(HandleLog);
    }

    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        var logEntry = string.Format("\n {0} {1} \n {2}\n {3}"
            , DateTime.Now, type, condition, stackTrace);
        
        try
        {
            _writer.Write(logEntry);
            _writer.Flush();
        }
        catch (Exception e)
        {
            print("Can't Write Game Log");
            print(e);
        }
    }

    void OnDestroy()
    {
        try {
            _writer.Close();
            print("Logger-"+logName+" closed");

        }
        catch (Exception e)
        {
            print("Can't close onDestroy");
            print(e);
        }
    }
}