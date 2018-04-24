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
	// Use this for initialization
	void Start () {
		toggle1.onValueChanged.AddListener (Toggle1ValueChanged);
		toggle2.onValueChanged.AddListener (Toggle2ValueChanged);

		LoadPrefs loadField = new LoadPrefs ("Field");
		toggle1.isOn =loadField.loadBool("field_1",true);
		toggle2.isOn = loadField.loadBool("field_2",false);
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
}
