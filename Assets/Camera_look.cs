using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera_look : MonoBehaviour{
	public static Transform target;
    public Transform cam;
    public Camera camera;

    float smoothTime = 0f;
    float q;

    Vector3 targetPosition;
    Vector3 camPosition;
    float x1 ;
    float y1 ;
    float z1 ;
    float x2 ;
    float y2 ;
    float z2 ;
    float stepx ;
    float stepy ;

    float fieldOfView;
    // Use this for initialization 
    void Start()
    {
		LoadPrefs loadHeliPrefs =  gameObject.AddComponent< LoadPrefs>() as LoadPrefs;
		loadHeliPrefs.setFileName("CamPrefs");
		loadHeliPrefs.initialize();
		camera.fieldOfView = loadHeliPrefs.loadFloat("camera_zoom",30f);
		loadHeliPrefs.close ();
        fieldOfView = camera.fieldOfView;


    }



    void LateUpdate()
	{
        camPosition = transform.position;

        Vector3 lookPos = target.position - cam.position;
        //if (Quaternion.LookRotation(lookPos).eulerAngles.magnitude < 180) { q = Quaternion.LookRotation(lookPos).eulerAngles.magnitude; }
        //else { q = 360 - Quaternion.LookRotation(lookPos).eulerAngles.magnitude; }

        x1 = cam.rotation.x;
        y1 = cam.rotation.y;
        z1 = cam.rotation.z;
        float x = target.transform.position.x - cam.transform.position.x;//Quaternion.LookRotation(lookPos).x; 
        float y = target.transform.position.y - cam.transform.position.y;
        float z = target.transform.position.z - cam.transform.position.z;
        x2 = Quaternion.LookRotation(lookPos).x;
        y2 = Quaternion.LookRotation(lookPos).y;



        float qx = Mathf.Abs(toAngle(x2) - toAngle(x1));
        float qy = Mathf.Abs(toAngle(y2) - toAngle(y1));
        q= qx > qy? qx: qy;
        float max_q =3f;
        if (q > max_q) { q = max_q; }
      // print(q);
        smoothTime =10f;
       // if (q < 1) { smoothTime = 1f; }
        float speed = smoothTime*0.5f + smoothTime * Mathf.Sqrt(q) *4f;
        
		//transform.rotation = Quaternion.RotateTowards(cam.rotation, Quaternion.LookRotation(lookPos),speed* Time.deltaTime);
        transform.LookAt(target);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) { fieldOfView--; }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) { fieldOfView++; }
        if (fieldOfView > 90) { fieldOfView = 90; }
        if (fieldOfView < 5) { fieldOfView = 5; }
		if (Input.GetKey(KeyCode.Minus) ) { fieldOfView = fieldOfView+0.5f; } 
		if (Input.GetKey(KeyCode.Equals)) { fieldOfView = fieldOfView - 0.5f; }
        camera.fieldOfView = fieldOfView;
		q = 0;
    }

    public float toAngle(float r)
    {
        float v = 546.98f * r * r * r * r - 865.93f * Mathf.Abs(r * r * r) + 440.72f * r * r + 50.5f * Mathf.Abs(r) + 1.209f;
        if (r < 0) { v = -v; }
        return v;
    }
}
