using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionDetector : MonoBehaviour {

	public HingeJoint main_rotor_model_hinge;
	public HingeJoint main_rotor_collider_hinge;
	public static bool collision;
	public GameObject rotor_model_object;
	public GameObject rotor_collider_object;
	Rigidbody rotor_rb;
	Motor motor;
    float timer=0;

    public static bool crash_main_rotor=false;


    // Use this for initialization
	public  CollisionDetector () {
		collision = false;


	}
	
	// Update is called once per frame
	public void checkCollision() {
		
        if (crash_main_rotor) {

			main_rotor_model_hinge=null;
			main_rotor_collider_hinge=null;
            Motor.rpm_r = 0;
            timer = timer + Time.deltaTime;
			collision = true;
            
            if (timer > 2) {
                crash_main_rotor = false;

                /*
				foreach(GameObject o in Object.FindObjectsOfType<GameObject>()){
					print("Destroy-"+o.name);
					Destroy (o);
				}
				*/
                print("rotor crushed");
                motor.closeLogger ();
                Destroy(GameObject.FindGameObjectWithTag("Destroy"));
                SceneManager.LoadScene("Field");
            }
        }

        

    }

    void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag == "Ground") {
           
            //motor.Collision();
            rotor_model_object =GameObject.FindGameObjectWithTag("rotor_model");
			rotor_collider_object=GameObject.FindGameObjectWithTag("rotor_collider");
			main_rotor_model_hinge = rotor_model_object.GetComponent<HingeJoint>();
			main_rotor_collider_hinge = rotor_collider_object.GetComponent<HingeJoint>();


			if (main_rotor_model_hinge != null){
				main_rotor_model_hinge.breakForce = 0.1f;
				main_rotor_model_hinge.breakTorque = 0.1f;
				}
			if (main_rotor_model_hinge != null)
			{
				main_rotor_collider_hinge.breakForce = 0.1f;
				main_rotor_collider_hinge.breakTorque = 0.1f;
			}

            crash_main_rotor = true;

        }
	}

	public bool isCollision()
	{
		return collision;
	}
	public void setMotor(Motor motor)
	{
		this.motor = motor;
	}
}
