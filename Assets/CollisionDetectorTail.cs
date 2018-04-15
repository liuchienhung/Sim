using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionDetectorTail : MonoBehaviour {


	public static bool collisionTail;

	Rigidbody rotor_rb;
	Motor motor;
    float timer=0;

    public static bool crash_tail_rotor=false;


    // Use this for initialization
	public  CollisionDetectorTail () {
		collisionTail = false;
	}
	
	// Update is called once per frame
	public void checkCollision() {
		
        if (crash_tail_rotor) {
                crash_tail_rotor = false;
                print("tail rotor crushed");
			collisionTail = true;
                //Destroy(GameObject.FindGameObjectWithTag("tail"));
         
            }
   
    }

    void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag == "Ground") {
           
            //motor.Collision();



            crash_tail_rotor = true;

        }
	}

	public bool isCollision()
	{
		return collisionTail;
	}
	public void setMotor(Motor motor)
	{
		this.motor = motor;
	}
}
