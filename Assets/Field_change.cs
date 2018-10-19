using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Field_change : MonoBehaviour {

	public Toggle toggle1;
	public Toggle toggle2;
	bool t1;
	bool t2;

	public Toggle toggle_Goblin380;
	public Toggle toggle_Logo600;
	bool b_Goblin380;
	bool b_Logo600;

	// Use this for initialization
	void Start () {
		toggle1.onValueChanged.AddListener (Toggle1ValueChanged);
		toggle2.onValueChanged.AddListener (Toggle2ValueChanged);
		toggle_Goblin380.onValueChanged.AddListener (Toggle_Goblin380_ValueChanged);
		toggle_Logo600.onValueChanged.AddListener   (Toggle_Logo600_ValueChanged);
		LoadPrefs loadField = new LoadPrefs ("Field");
		toggle1.isOn = loadField.loadBool("field_1",true);
		toggle2.isOn = loadField.loadBool("field_2",false);
		toggle_Goblin380.isOn = loadField.loadBool("Goblin380",true);
		toggle_Logo600.isOn   = loadField.loadBool("Logo600",false);
		loadField.close ();

	}
	
	// Update is called once per frame
	void Update () {

	
	}
	
	public void Back()
	{
		SavePrefs saveField = new SavePrefs ("Field");
		saveField.save ("field_1",toggle1.isOn);
		saveField.save ("field_2",toggle2.isOn);
		saveField.save ("Goblin380",toggle_Goblin380.isOn);
		saveField.save ("Logo600"  ,toggle_Logo600.isOn);

		if (toggle_Goblin380.isOn) {saveField.save ("heliName","Goblin380");}
		if (toggle_Logo600.isOn)   {saveField.save ("heliName","Logo600");}
		saveField.close ();
		
		SceneManager.LoadScene ("Main_menu");
	}

	void Toggle1ValueChanged(bool t1)
	{
		print ("Toggle 1 - changed");
	}

	void Toggle2ValueChanged(bool t2)
	{
		print ("Toggle 2 - changed");
	}

	void Toggle_Goblin380_ValueChanged(bool b_Goblin380)
	{
		print ("Changed to Goblin 380");
	}

	void Toggle_Logo600_ValueChanged(bool b_Logo600)
	{
		print ("Changed to Logo 600");
	}
}
