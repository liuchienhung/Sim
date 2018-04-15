//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class LogHandler : MonoBehaviour
{
    private  StreamWriter _writer;
    void Awake()
    {
        print("Logger Awake");

        //_writer = new System.IO.StreamWriter(File.Create("log.txt"));
        //_writer = new System.IO.StreamWriter(File.Create("log.txt"));

        string pathApp = Path.GetFullPath(".");

		if (File.Exists (pathApp + @"\logField.txt")) {
			
				if (new System.IO.FileInfo (pathApp + @"\logField.txt").Length/100>400) {
				try {
					File.Delete (pathApp + @"\logField.txt");
					print ("delete ,logs");
				} catch (Exception e) {
					print ("can't delete ,logs");
				}
			}
		}
       
        _writer = File.AppendText("logField.txt");
        _writer.WriteLine("\n\n=============== Game started ================\n\n");
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
            //print("Can't Write Game Log");
            //print(e);
        }
    }

    void OnDestroy()
    {
        try {  _writer.Close(); }
        catch (Exception e)
        {
            print("Can't close onDestroy");
            print(e);
        }
    }
}