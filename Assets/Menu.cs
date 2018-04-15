using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {


	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Star_sim()
	{
		SceneManager.LoadScene("Field");
	}

	public void Transmitter()
	{
		SceneManager.LoadScene("Transmitter");
	}
	public void Heli_Settings()
	{
		SceneManager.LoadScene("HeliSettings");
	}
	public void Exit()
	{
		Application.Quit();
	}
}
