using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Heli_physics : MonoBehaviour 
{

	public static bool back=false;
	public Material sky1;
	public Material sky2;

	String heliName;

	Logger heliLogger;
	Logger powerLogger;
	SavePrefs savePrefsAll;
	SavePrefsAppend savePrefsAppend;
	CollisionDetector colDetector;
	CollisionDetectorTail colDetectorTail;
	bool closePrefs=false;
	public Camera cam;
    //Sound
	public float volemeBlade;
	public float CyclicVolume;
	public AudioSource audioS;
	public AudioSource audioBlade;
	public AudioSource CyclicSound;
	public AudioSource audioTail;
	float motor_vol=1f;
	float pitch_vol=1f;
	float cyclic_vol=1f;
	float tail_vol=1f;
    
	public GameObject Heli;
	public Transform HeliTransform;
	public Transform RotorCol_tr;
	public Rigidbody CG_rb;
    
	public Transform rotor_collider;
	public Transform rotor_model;
	public Transform trotor;
	public Transform trotor_center;
	public Rigidbody tusha;

    
	public Transform Light_rb;
	//public Transform Plane_tr;

	 Vector3 start_pos= new Vector3(0,1,0);
	float vel=0;
	float timer;

	public float Pitch ;
	public float Rudder;
	public float Aileron;
	public float Elevator;
	public float Throttle;

	float Pitch_tr ;
	float Rudder_tr;
	float Aileron_tr;
	float Elevator_tr;
	float Throttle_tr;

	string Pitch_axis ;
	string Rudder_axis;
	string Aileron_axis;
	string Elevator_axis;
	string Throttle_axis;

	//
	float p_calib;
	float r_calib;
	float a_calib;
	float e_calib;
	float t_calib;

	//Reverses
	float rev_p;
	float rev_r;
	float rev_a;
	float rev_e;
	float rev_t;

	//Expo
	int exp_p;
	int exp_r;
	int exp_a;
	int exp_e;
	int exp_t;

	//Calibration max/min
	float pitch_max=0.01f;
	float elev_max=0.01f;
	float ail_max=0.01f;
	float rud_max=0.01f;
	float thr_max = 0.01f;
	float pitch_min = -0.01f;
	float elev_min = -0.01f;
	float ail_min = -0.01f;
	float rud_min = -0.01f;
	float thr_min = -0.01f;
	
	//Rotor phisics
	float rpm_max=2600f;
	float rpm_set= 2200f;//target rpm
	float Ic=0.0009f;
	float Power_m=3500f; //motor power
	float thr_norm=20f;
	float thr_idle1=60f;
	float thr_idle2=80f;
	float rpm_norm=1600f; 
	float rpm_idle1=2000f;
	float rpm_idle2=2200f;
	public float rpm = 0;

	float mblade_length=600;
	float mblade_width=55;
	float tblade_length=95;
	float tblade_width=20;
	float mass=4f;
	float Ro=1.25f;
	float Cy=0;
	float Blade_area;
	float Pitch_angle=0;
	float vel_onComing = 0;//Airflow velocity from oncoming stream
	float vel_rInd=0;      //Airflow velocity induced by blades
	float vel_Lift = 0;    //Result airflow speed
	float Heli_lift_vel=0; //Actual Heli lift velocity
	float Lift=0f;         
	float lift_factor=1f;
	float Heli_mass=3.5f;
	float Coef_onc_stream = 1f; //influence on Lift by oncoming stream 
	
	//Rotor power consuption fuctors 
	float Power_0_factor=1f;        // at 0 deg
	float Power_cyclic_factor=1f;   // of cyclic
	float Power_pitch_factor=1f;    // of pitch
	float Power_tail_factor=1f;     // of tail
	
	//Overspeed factor
	float Power_oversp_factor = 1f;  //Power induced by oncoming flow
    
	Motor motor;
	
	//FBL
	FBL fbl;
	//Elevator
	float elev_torq;
	float Max_Flip_rate;
	float Max_elev_torq=200f;
	
	//Aileron
	float ail_torq;
	float Max_Roll_rate;
	float Max_ail_torq=200f;
		
	//Rudder
	float Rud_torq;
	float Max_Piro_rate;
	float Max_rud_torq = 700;
	
	//Angular rotation rates
	float Flip_rate_deg=310;
	float Roll_rate_deg=310;
	float Piro_rate_deg=650;
	
	//Drag
	float Area_front=100;
	float Area_side=400;
	float Area_bottom = 1;
	float Coef_drag_front=1f;
	float Coef_drag_side = 1f;
	float Coef_drag_bottom = 1f;

    

    //Motor
	 public int pwm; //Throttle percent
	public float err; 
	
	float autorotation = 1;
	float distance;

	void Load()
	{
        print("Heliphisics - load prefs");
        List<string> Drop_Axis = new List<string> () { "Axis_1", "Axis_2", "Axis_3", "Axis_4", "Axis_5", "Axis_6", "Axis_7", "Axis_8", "Axis_9" , "Axis_10" };
//LoadSettings
	LoadPrefs loadField = gameObject.AddComponent<LoadPrefs>() as LoadPrefs;
	loadField.setFileName("Field");
	loadField.initialize ();
	if (loadField.loadBool("field_1", true))  { RenderSettings.skybox = sky1; }
	if (loadField.loadBool("field_2", false)) { RenderSettings.skybox = sky2; }
	heliName=loadField.loadString ("heliName", "Logo600");
	loadField.close ();

        print("Heliphisics - load Transmitter prefs");
	LoadPrefs loadSettings = gameObject.AddComponent< LoadPrefs>() as LoadPrefs;
	loadSettings.setFileName ("userPrefs");
	loadSettings.initialize ();

	Pitch_axis    = loadSettings.loadString("Pitch_axis");
	Rudder_axis   = loadSettings.loadString("Rudder_axis") ;
	Aileron_axis  = loadSettings.loadString("Aileron_axis") ;
	Elevator_axis = loadSettings.loadString("Elevator_axis");
	Throttle_axis = loadSettings.loadString("Throttle_axis");
	//

	p_calib = loadSettings.loadFloat( "p_calib");
	r_calib = loadSettings.loadFloat( "r_calib");
	a_calib = loadSettings.loadFloat( "a_calib");
	e_calib = loadSettings.loadFloat( "e_calib");
	t_calib = loadSettings.loadFloat( "t_calib");

//Reverses
	rev_p = loadSettings.loadFloat( "rev_p");
	rev_r = loadSettings.loadFloat( "rev_r");
	rev_a = loadSettings.loadFloat( "rev_a");
	rev_e = loadSettings.loadFloat( "rev_e");
	rev_t = loadSettings.loadFloat( "rev_t");

//EXPO
	exp_p = loadSettings.loadInt( "exp_p");
	exp_r = loadSettings.loadInt( "exp_r");
	exp_a = loadSettings.loadInt( "exp_a");
	exp_e = loadSettings.loadInt( "exp_e");
	exp_t = loadSettings.loadInt( "exp_t");

//EndPoints
	pitch_max =loadSettings.loadFloat( "pitch_max",0.1f);
	elev_max  =loadSettings.loadFloat( "elev_max" ,0.1f);
	ail_max   =loadSettings.loadFloat( "ail_max"  ,0.1f);
	rud_max   =loadSettings.loadFloat( "rud_max"  ,0.1f);
	thr_max   =loadSettings.loadFloat( "thr_max"  ,0.1f);

	pitch_min=loadSettings.loadFloat( "pitch_min",0.1f);
	elev_min =loadSettings.loadFloat( "elev_min",0.1f);
	ail_min  =loadSettings.loadFloat( "ail_min",0.1f);
	rud_min  =loadSettings.loadFloat( "rud_min",0.1f);
	thr_min  =loadSettings.loadFloat( "thr_min",0.1f);
	//Close
	loadSettings.close();

//Load Heli settings
	LoadPrefs loadHeliPrefs = gameObject.AddComponent< LoadPrefs>() as LoadPrefs;
	loadHeliPrefs.setFileNameAndFold (heliName,"Helis");
	loadHeliPrefs.initialize ();
	print ("Loading Heli - "+heliName);
	mblade_length  = loadHeliPrefs.loadFloat("mblade_length", mblade_length);

	mblade_width   = loadHeliPrefs.loadFloat("mblade_width" , mblade_width);
	tblade_length  = loadHeliPrefs.loadFloat("tblade_length", tblade_length);
	tblade_width   = loadHeliPrefs.loadFloat("tblade_width" , tblade_width);
	mass           = loadHeliPrefs.loadFloat("mass", mass);
	Ic  = loadHeliPrefs.loadFloat("Ic", Ic);
	if (Ic == 0) {Ic = 0.001f;}


    Power_m       = loadHeliPrefs.loadFloat("Power_m", Power_m);

	lift_factor   = loadHeliPrefs.loadFloat("lift_factor",lift_factor);
	Flip_rate_deg = loadHeliPrefs.loadFloat("Flip_rate_deg",Flip_rate_deg);
	Max_Flip_rate = Flip_rate_deg/180f*3.14f;
	Roll_rate_deg = loadHeliPrefs.loadFloat("Roll_rate_deg",Roll_rate_deg);
	Max_Roll_rate = Roll_rate_deg/180f*3.14f;
	Piro_rate_deg = loadHeliPrefs.loadFloat("Piro_rate_deg",Piro_rate_deg);
	Max_Piro_rate = Piro_rate_deg/180f*3.14f;
	Power_0_factor      = loadHeliPrefs.loadFloat("Power_0_factor", Power_0_factor);
	Power_cyclic_factor = loadHeliPrefs.loadFloat("Power_cyclic_factor", Power_cyclic_factor);
	Power_pitch_factor  = loadHeliPrefs.loadFloat("Power_pitch_factor", Power_pitch_factor);
	Power_tail_factor   = loadHeliPrefs.loadFloat("Power_tail_factor", Power_tail_factor);
	Power_oversp_factor = loadHeliPrefs.loadFloat("Power_oversp_factor", Power_oversp_factor);

	Coef_drag_front  = loadHeliPrefs.loadFloat("Coef_drag_front", Coef_drag_front);
	Coef_drag_side   = loadHeliPrefs.loadFloat("Coef_drag_side", Coef_drag_side);
	Coef_drag_bottom = loadHeliPrefs.loadFloat("Coef_drag_bottom", Coef_drag_bottom);
	Coef_onc_stream  = loadHeliPrefs.loadFloat("Coef_onc_stream", Coef_onc_stream);

	thr_norm =loadHeliPrefs.loadFloat("thr_norm", thr_norm);
	thr_idle1=loadHeliPrefs.loadFloat("thr_idle1", thr_idle1);
	thr_idle2=loadHeliPrefs.loadFloat("thr_idle2", thr_idle2);

	rpm_max  =loadHeliPrefs.loadFloat("rpm_max", rpm_max);
	rpm_norm =loadHeliPrefs.loadFloat("rpm_norm", rpm_norm);
	rpm_idle1=loadHeliPrefs.loadFloat("rpm_idle1", rpm_idle1);
	rpm_idle2=loadHeliPrefs.loadFloat("rpm_idle2", rpm_idle2);

	motor_vol  =loadHeliPrefs.loadFloat ("motor_vol",motor_vol);
	pitch_vol  =loadHeliPrefs.loadFloat ("pitch_vol",pitch_vol);
	cyclic_vol =loadHeliPrefs.loadFloat ("cyclic_vol",cyclic_vol);
	tail_vol   =loadHeliPrefs.loadFloat ("tail_vol",tail_vol);
        
        loadHeliPrefs.close ();
	print ("prefsLoaded");
	savePrefs ();
    }

	
	public void savePrefs (){
	savePrefsAll =  gameObject.AddComponent<SavePrefs>() as SavePrefs ;
	savePrefsAll.setFileNameAndFold (heliName,"Helis");
	savePrefsAll.initialize ();
	savePrefsAll.save("mblade_length", mblade_length);
	savePrefsAll.save("mblade_width" , mblade_width);
	savePrefsAll.save("tblade_length", tblade_length);
	savePrefsAll.save("tblade_width" , tblade_width);
	savePrefsAll.save("Ic", Ic);
	savePrefsAll.save("mass", mass);
	savePrefsAll.save("Power_m", Power_m);
	savePrefsAll.save("lift_factor",lift_factor);
	savePrefsAll.save("Flip_rate_deg",Flip_rate_deg);
	savePrefsAll.save("Roll_rate_deg",Roll_rate_deg);
	savePrefsAll.save("Piro_rate_deg",Piro_rate_deg); 
	savePrefsAll.save("Power_0_factor", Power_0_factor);
	savePrefsAll.save("Power_cyclic_factor",Power_cyclic_factor);
	savePrefsAll.save("Power_pitch_factor",Power_pitch_factor);
	savePrefsAll.save("Power_tail_factor",Power_tail_factor);
	savePrefsAll.save("Power_oversp_factor",Power_oversp_factor);
	savePrefsAll.save("Coef_drag_front",Coef_drag_front);
	savePrefsAll.save("Coef_drag_side",Coef_drag_side);
	savePrefsAll.save("Coef_drag_bottom",Coef_drag_bottom);
	savePrefsAll.save("Coef_onc_stream",Coef_onc_stream);
	savePrefsAll.save("thr_norm",thr_norm);
	savePrefsAll.save("thr_idle1",thr_idle1);
	savePrefsAll.save("thr_idle2",thr_idle2);
	savePrefsAll.save("rpm_max", rpm_max);
	savePrefsAll.save("rpm_norm",rpm_norm);
	savePrefsAll.save("rpm_idle1",rpm_idle1);
	savePrefsAll.save("rpm_idle2",rpm_idle2);
	savePrefsAll.save("motor_vol",motor_vol);
	savePrefsAll.save("pitch_vol",pitch_vol);
	savePrefsAll.save("cyclic_vol",cyclic_vol);
	savePrefsAll.save("tail_vol",tail_vol);
	savePrefsAll.close();
   }


	float GetSign(float numb)
	{
		if (numb == 0) {return 1f;} 
		else {return numb / Mathf.Abs (numb);}
	}

	public LineRenderer line;

   

    // Use this for initialization
    void Start () 
	{
	//Logging
	heliLogger  =  gameObject.AddComponent< Logger>() as Logger;
	heliLogger.Init("heliLogger",0.01f);
		
	powerLogger =  gameObject.AddComponent< Logger>() as Logger;
	powerLogger.Init("powerLogger",0.05f);
        //
	colDetector     = gameObject.AddComponent<CollisionDetector>() as CollisionDetector ;
	colDetectorTail = gameObject.AddComponent<CollisionDetectorTail>() as CollisionDetectorTail ;
	Load ();
        print("Heliphisics -  start");
	
       
	//Motor
		motor = gameObject.AddComponent<Motor>() as Motor;
		motor.setMotorPrwRpm(Power_m,rpm_max);
		motor.rotor_model=rotor_model;
		motor.rotor_collider = rotor_collider;
		colDetector = gameObject.AddComponent< CollisionDetector>() as CollisionDetector ;
		colDetector.setMotor (motor);
		motor.Init ();
	//FBL
		fbl= gameObject.AddComponent<FBL>() as FBL;
		fbl.setFBl_CG(CG_rb);
		fbl.initialize ();
        float cg = Heli.GetComponent<Rigidbody> ().centerOfMass.z - CG_rb.centerOfMass.z;
		Heli.GetComponent<Rigidbody> ().centerOfMass = CG_rb.centerOfMass;
        
		Heli.GetComponent<Transform> ().position=start_pos;
		//CG_tr.position = new Vector3 (CG_rb.position.x,CG_rb.position.y-20,CG_rb.position.z);
		//
		CG_rb = GetComponent<Rigidbody>();
		CG_rb.maxAngularVelocity = 37;

		
		CG_rb.inertiaTensor = new Vector3 (1f,1f, 1f);
		CG_rb.mass = mass;

        DynamicGI.UpdateEnvironment();

		//Heli.GetComponent<Rigidbody> ().mass = 0.01f;
	Heli.GetComponent<Rigidbody> ().centerOfMass = new Vector3 (CG_rb.centerOfMass.x, CG_rb.centerOfMass.y-0.1f*0, CG_rb.centerOfMass.z);
	 back=false;
	}

    //


    Vector3 lastMouseCoordinate;
    float mousetime;
    // Update is called once per frame
    void FixedUpdate()
    {
        //Hide Mouse Coursore
        Vector3 mousePositionCurrent = Input.mousePosition;

        if (timer - mousetime > 1f)
        {
            if (mousePositionCurrent.Equals(lastMouseCoordinate)) { Cursor.visible = false; }
            else { Cursor.visible = true; }
            mousetime = timer;
        }
        lastMouseCoordinate = Input.mousePosition;
        //


        Vector3 heli_position = CG_rb.GetComponent<Transform>().position;
        
        distance = Mathf.Sqrt(Mathf.Pow(heli_position.x,2f) + Mathf.Pow(heli_position.y, 2f) + Mathf.Pow(heli_position.z, 2f));
        float soundDecr = 2.5f - Mathf.Log10(distance/1f);
        //print("distance=" + (int)distance);
        if (soundDecr > 1f) { soundDecr = 1f; }
        if (soundDecr<0.1f) { soundDecr = 0.1f; }
        //
        if (Input.GetKey (KeyCode.Escape)) { 
			
			Back();
		}
		if (Input.GetKey (KeyCode.R)) 
		{
            Cursor.visible = true;
            SavePrefs savePrefsCam = gameObject.AddComponent<SavePrefs>() as SavePrefs;
            savePrefsCam.setFileName("CamPrefs");
            savePrefsCam.initialize();
            savePrefsCam.save("camera_zoom", cam.fieldOfView);
            savePrefsCam.close();
            motor.closeLogger();
            Destroy(GameObject.FindGameObjectWithTag("Destroy"));
            SceneManager.LoadScene("Field");
            /*
			CG_rb.GetComponent<Transform> ().position=start_pos;
			CG_rb.GetComponent<Transform> ().rotation = Quaternion.Euler (0,90, 0);
			CG_rb.GetComponent<Rigidbody> ().velocity=new Vector3 (0,0,0);
			CG_rb.GetComponent<Rigidbody> ().velocity=new Vector3 (0,0,0);

			RotorCol_tr.GetComponent<Transform> ().rotation = Quaternion.Euler (0, 0, 0);
			RotorCol_tr.GetComponent<Rigidbody> ().velocity=new Vector3 (0,0,0);
			RotorCol_tr.GetComponent<Rigidbody> ().angularVelocity=new Vector3 (0,0,0);
            */
        }



     //INPUTS
        Pitch_tr = Input.GetAxis(Pitch_axis) + p_calib;
        if (Pitch_tr >= 0) { Pitch = Pitch_tr / pitch_max; }
        else { Pitch = Pitch_tr / Mathf.Abs(pitch_min); }
        Pitch = Expo(Pitch, exp_p) * rev_p;
        //
        Rudder_tr   = Input.GetAxis(Rudder_axis)+r_calib;
		if (Rudder_tr >= 0) {Rudder = Rudder_tr / rud_max;} 
		else {Rudder = Rudder_tr / Mathf.Abs(rud_min);}
		Rudder      = Expo(Rudder, exp_r)* rev_r;
		//
		Aileron_tr  = Input.GetAxis(Aileron_axis)+a_calib;
		if (Aileron_tr >= 0) {Aileron = Aileron_tr / ail_max;} 
		else {Aileron = Aileron_tr / Mathf.Abs(ail_min);}
		Aileron     = Expo(Aileron, exp_a)* rev_a;
		//
		Elevator_tr = Input.GetAxis(Elevator_axis)+e_calib;
		if (Elevator_tr >= 0) {Elevator = Elevator_tr / elev_max;} 
		else {Elevator = Elevator_tr / Mathf.Abs(elev_min);}
		Elevator    = Expo(Elevator, exp_e)* rev_e;
		//
		Throttle_tr = Input.GetAxis(Throttle_axis)+t_calib;
		if (Throttle_tr >= 0) {Throttle = Throttle_tr / thr_max;} 
		else {Throttle = Throttle_tr / Mathf.Abs(thr_min);}
		Throttle    = Expo(Throttle, exp_t)* rev_t;

        
        if (Input.GetKey (KeyCode.L)) {Aileron = 1f;}
		if (Input.GetKey (KeyCode.J)) {Aileron = -1f;}
		//
		if (Input.GetKey (KeyCode.I)) {Elevator = 1f;}
		if (Input.GetKey (KeyCode.K)) {Elevator = -1f;}
        //
		if (Input.GetKey (KeyCode.W)) {Pitch = 1f;}
		if (Input.GetKey (KeyCode.Q)) {Pitch = 0.3f;}
		if (Input.GetKey (KeyCode.S)) {Pitch = -1f;}
		if (Input.GetKey (KeyCode.X)) {Pitch = -0.2f;}
		//
		if (Input.GetKey (KeyCode.D)) {Rudder= 1f;}
		if (Input.GetKey (KeyCode.A)) {Rudder = -1f;}
		//
		if (Input.GetKey (KeyCode.Space)) {Throttle=-1;print("autorotation");}

		//Throttle
		if((Throttle * 50 + 50f) < (thr_norm)) {autorotation = 0;}
		if((Throttle * 50 + 50f) > (thr_norm)) {rpm_set=rpm_norm;autorotation = 1;}
		if((Throttle * 50 + 50f) > (thr_idle1)){rpm_set=rpm_idle1;autorotation = 1; }
		if((Throttle * 50 + 50f) > (thr_idle2)){rpm_set=rpm_idle2;autorotation = 1; }
        
    //Airflow velocity induced by blades
		float vel_0 = 2f * 3.14f * mblade_length / 1000 * (rpm / 60) / 2;
    //Elevator
		float Flip_rate=CG_rb.transform.InverseTransformVector (CG_rb.angularVelocity).z;
	//Resistance
		Max_elev_torq=vel_0* 3 * rpm / rpm_set ;//vel_0*3;

		float Flip_resist_torq=-Flip_rate*Mathf.Abs(Flip_rate)*7f;
        elev_torq=fbl.getElevTorq (Max_Flip_rate*Elevator,Time.fixedDeltaTime)*Max_elev_torq;
		CG_rb.AddRelativeTorque (0, 0, elev_torq+Flip_resist_torq, ForceMode.Acceleration);


	//Aileron
		float Roll_rate=CG_rb.transform.InverseTransformVector (CG_rb.angularVelocity).x; 
	//Resistance
		Max_ail_torq=vel_0* 3 * rpm / rpm_set ;
		float Roll_resist_torq=-Roll_rate*Mathf.Abs(Roll_rate)*5.5f;
        ail_torq=fbl.getAilTorq (Max_Roll_rate*Aileron, Time.fixedDeltaTime) *Max_ail_torq;
		CG_rb.AddRelativeTorque ( ail_torq+Roll_resist_torq,0,0, ForceMode.Acceleration);

	//Pitch
		Vector3 grav = new Vector3 (0, -10, 0);
		Blade_area=mblade_width/1000*mblade_length/1000;
		Heli_lift_vel = transform.InverseTransformDirection(CG_rb.velocity).y/1000/Mathf.Sin (13 * 3.14f / 180) ;  

		vel_onComing =GetSign(Pitch) * transform.InverseTransformDirection(CG_rb.velocity).y/2 * Coef_onc_stream;
		vel_rInd = Mathf.Abs(Pitch) *  vel_0 ;
        if (Mathf.Abs(vel_onComing) > Mathf.Abs(vel_rInd*3)) { vel_onComing = vel_rInd - 1f; }
        vel_Lift = vel_rInd - vel_onComing;
        Pitch_angle = GetSign(Pitch) * 13f;
		Cy = Pitch_angle * 1.2f / 16;
		Lift = Cy * Ro * vel_Lift * vel_Lift / 2 * Blade_area*rpm/rpm_set * rpm / rpm_set;
		//print ("Lift="+Lift);
		CG_rb.AddForce((grav*mass+transform.up*Lift*lift_factor*4f ) * Time.fixedDeltaTime, ForceMode.VelocityChange);

    //Overspeed
        float vel_oversp = (vel_rInd - 2f * vel_onComing) < 0 ? Mathf.Abs(vel_Lift) : 0;
        //Power
		float Power_0 = (rpm / 1000) * (rpm / 1000)*mblade_length *mblade_length/2400*Power_0_factor;
		float Power_pitch = vel_Lift* mblade_length *mblade_length * Mathf.Abs (Pitch) * 0.05f/600*Power_pitch_factor;
		float Power_cyclic = (Mathf.Abs(elev_torq) * Mathf.Abs(elev_torq)+ Mathf.Abs(ail_torq) * Mathf.Abs(ail_torq))/20f*mblade_length/600f*Power_cyclic_factor;
		float Power_oversp = vel_oversp *2f* mblade_length/10f* (Pitch) * GetSign (transform.InverseTransformDirection (CG_rb.velocity).y)* Power_oversp_factor;

        float Power_rotor = Power_pitch + Power_cyclic + Power_0;


		float torq_main_rotor;
		if (rpm < 1) {torq_main_rotor = 0;} 

		else
        {   //Overspeed
            if (rpm > rpm_set+100) { torq_main_rotor = 0; }
            else{
                torq_main_rotor = (Power_rotor- Power_cyclic) / (rpm / 60);
            }
        }
		float force_tail = torq_main_rotor / 0.7f;

	//Rudder
		float Piro_rate=CG_rb.transform.InverseTransformVector (CG_rb.angularVelocity).y;
	//Piro
		float x=fbl.getRud(Max_Piro_rate*Rudder, Pitch,Aileron,Elevator,Time.fixedDeltaTime);

		float tail_angle=10+x*35;
		float force_tail_rotor = rpm / 60 * tail_angle/1f*tblade_length*tblade_width/2000f - CG_rb.transform.InverseTransformDirection(tusha.velocity).z*15f;
        float torq_tail_rotor = force_tail_rotor * 0.7f/8;
		float Piro_resist_torq=-Piro_rate*1f;

		CG_rb.AddRelativeTorque (0,(-torq_main_rotor*8*autorotation+torq_tail_rotor*8+Piro_resist_torq),0, ForceMode.Acceleration);
		float Power_tail = torq_tail_rotor*4f;

    //Motor
        motor.trotor = trotor;
        motor.trotor_center = trotor_center;
        
        motor.rpm_set = rpm_set;
	motor.setAutorotation(autorotation);
	rpm = motor.getRpm((Power_rotor+Power_tail*Power_tail_factor),Power_oversp , Time.fixedDeltaTime);
        pwm = motor.getPwm();

    //Sound
	//Motor
	audioS.pitch = rpm / 2200*0.9f;
	audioS.volume = motor_vol* soundDecr;
	//Cyclic
        CyclicVolume= (Mathf.Abs(elev_torq)* Mathf.Abs(elev_torq)*0.7f+ Mathf.Abs(ail_torq)* Mathf.Abs(ail_torq)) /300000f;
	CyclicSound.volume = CyclicVolume *cyclic_vol* soundDecr;
	if(CyclicVolume < 0.01f) { CyclicVolume = 0; }
	//Pitch
        audioBlade.volume = (  Mathf.Exp(vel_Lift / 20) / 300)*2f ;
        
        if (audioBlade.volume > 0.35f) { audioBlade.volume = 0.35f; }
        audioBlade.volume = audioBlade.volume*pitch_vol* soundDecr;
        audioBlade.volume = audioBlade.volume < 0.01f ? 0 : audioBlade.volume;
       //Tail
        audioTail.volume = Mathf.Abs(torq_tail_rotor) / 120f  * 0.1f*tail_vol* soundDecr;
        if (audioTail.volume < 0.001f) { audioTail.volume = 0.0f; }
		//
		err = motor.getError();
        timer = timer + Time.fixedDeltaTime;

        
       
        //Vector3 Vel_loc =transform.InverseTransformDirection(CG_rb.velocity);


     //Drag
		CG_rb.AddForce(-0.1f * CG_rb.transform.right * CG_rb.transform.InverseTransformDirection(tusha.velocity).x * Coef_drag_front * Area_front * 0.02f, ForceMode.Acceleration);
		CG_rb.AddForce(-0.1f  * CG_rb.transform.InverseTransformDirection(tusha.velocity).z * Coef_drag_side * Area_side * 0.02f* CG_rb.transform.forward- CG_rb.transform.forward*force_tail_rotor/50f, ForceMode.Acceleration);
		CG_rb.AddForce(-0.00005f*CG_rb.transform.up * CG_rb.transform.InverseTransformDirection(tusha.velocity).y * Coef_drag_bottom * Area_bottom*mblade_length/600f *  0.02f*rpm * rpm, ForceMode.Acceleration);
       
	//
	colDetector.checkCollision();
	heliLogger.saveLog ("time",timer);
	//heliLogger.saveLog ("rpm",(int)rpm);
	//heliLogger.saveLog("Flip_rate",(int)Flip_rate);
	heliLogger.saveLog("elev_torq",(int)elev_torq);
	heliLogger.saveLog("ail_torq", (int)ail_torq);
	//heliLogger.saveLog("Elevator",Elevator*1);
	//heliLogger.saveLog("ErSumElev", fbl.getErSumElev());
	//heliLogger.saveLog("Flip_resist_torq",(int)Flip_resist_torq);
	//heliLogger.saveLog("Roll_rate",(int)Roll_rate);

        //
	heliLogger.saveLog("Pitch", Pitch);
	heliLogger.saveLog("Vel_oversp", vel_oversp);
	heliLogger.saveLog("Aileron", Aileron);
	heliLogger.saveLog("Elevator", Elevator);
	heliLogger.saveLog("Rudder", Rudder);
	heliLogger.saveLog("ErrorRud", fbl.getErrorRud());
	heliLogger.saveLog("ErrSumRud",fbl.getErSumRud());
        
	heliLogger.saveLog("out_I", fbl.getOutIrud());
	heliLogger.saveLog("out_P", fbl.getOutPrud());
	heliLogger.saveLog("out_D", fbl.getOutDrud());
	heliLogger.saveLog("torq_main_rotor", torq_main_rotor);
	heliLogger.saveLog("torq_tail_rotor",torq_tail_rotor);
	heliLogger.saveLog("force_tail",force_tail);
	heliLogger.saveLog("Piro_rate",Piro_rate);
	heliLogger.saveLog("tail_limt",fbl.getRudOutput());
	heliLogger.saveLog("tail_angle",tail_angle);
	heliLogger.saveLog("Pcomp",fbl.getPcomp());

        //
	powerLogger.saveLog("Power_pitch", (int)Power_pitch);
	powerLogger.saveLog("Power_cyclic",(int)Power_cyclic);
	powerLogger.saveLog("Power_tail",  (int)Power_tail);
	powerLogger.saveLog("Power_0",     (int)Power_0);
	powerLogger.saveLog("Power_oversp",(int)Power_oversp);
		
        if (!colDetector.isCollision ()||!back) { 
			heliLogger.writeCycle ();
			powerLogger.writeCycle (); 
		}
		else {
			if (!closePrefs) {
				SavePrefs savePrefsCam = new SavePrefs("CamPrefs");
				savePrefsCam.save ("camera_zoom", cam.fieldOfView);
				savePrefsCam.close ();
			}
		}


		if (back) {
			Back ();
		}

    }

	public float Expo(float tr_signal, int expo)
	{
		float exp = expo/10 + 0.001f;//to avoid division by 0
		return  GetSign(tr_signal) *(Mathf.Exp(exp * Mathf.Abs(tr_signal)) - 1) / (Mathf.Exp(exp) - 1);
	}

	public void Back()
	{
		print ("Back");
	Cursor.visible = true;
	SavePrefs savePrefsCam = gameObject.AddComponent<SavePrefs> () as SavePrefs;
	savePrefsCam.setFileName("CamPrefs");
	savePrefsCam.initialize();
	savePrefsCam.save ("camera_zoom", cam.fieldOfView);
	savePrefsCam.close ();
	motor.closeLogger ();
	Destroy (GameObject.FindGameObjectWithTag("Destroy"));
	SceneManager.LoadScene ("Main_menu");
		
	}

	public Heli_physics getHeliPhisics()
	{
		return this;
	}

	public void closeMotorlogs()
	{
		motor.closeLogger ();
	}

}