using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heli_change : MonoBehaviour {
	float heli_number;
	string heliName;
	List<string> Helis = new List<string> () { 
		"Logo600", 
		"Goblin380", };
	// Use this for initialization
	void Start () {
		LoadPrefs loadHeli = gameObject.AddComponent< LoadPrefs>() as LoadPrefs;
		loadHeli.setFileName("Field");
		loadHeli.initialize ();
		heliName = loadHeli.loadString ("heliName", "Logo600");
		heli_number=Helis.FindIndex(h=> h.Contains(heliName));
		print ("Loaded Heli - "+heliName+", number - "+heli_number);
		loadHeli.close ();


		int i = 0;
		foreach(Transform heliChange in transform)
		{
			if (i == heli_number) {
				heliChange.gameObject.SetActive (true);
				Camera_look.target = heliChange.GetChild(0).transform;
                shadowLookAt.obj   = heliChange.GetChild(0).transform;
            } 
			else {
				heliChange.gameObject.SetActive (false);
			}
			i++;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
