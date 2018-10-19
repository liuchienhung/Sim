using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowLookAt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	

    public static Transform obj;
	// Update is called once per frame
	void Update () {
        transform.LookAt(obj);
	}
}
