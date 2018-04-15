using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transmitter : MonoBehaviour 
{

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
	//
	public Slider Slider_1;
	public Slider Slider_2;
	public Slider Slider_3;
	public Slider Slider_4;
	public Slider Slider_Throttle;

	public Slider Axis_1;
	public Slider Axis_2;
	public Slider Axis_3;
	public Slider Axis_4;
	public Slider Axis_5;
	public Slider Axis_6;
	public Slider Axis_7;
	public Slider Axis_8;
    public Slider Axis_9;
    public Slider Axis_10;

    //
    public	Dropdown Dropdown_pitch;
	public	Dropdown Dropdown_rudder;
	public	Dropdown Dropdown_aileron;
	public	Dropdown Dropdown_elevator;
	public	Dropdown Dropdown_Throttle;
	//Reverses
	public Toggle rev_p_t;
	public Toggle rev_r_t;
	public Toggle rev_a_t;
	public Toggle rev_e_t;
	public Toggle rev_t_t;
	float rev_p;
	float rev_r;
	float rev_a;
	float rev_e;
	float rev_t;
	//Expo
	public InputField exp_p_f;
	public InputField exp_r_f;
	public InputField exp_a_f;
	public InputField exp_e_f;
	public InputField exp_t_f;
    int exp_p;
    int exp_r;
    int exp_a;
	int exp_e;
	int exp_t;

	//Calibration max/min
	public Toggle calib_Max_Min;
	float pitch_max = 0.01f;
    float elev_max = 0.01f;
	float ail_max = 0.01f;
	float rud_max = 0.01f;
	float thr_max = 0.01f;

	float pitch_min = 0.01f;
	float elev_min = 0.01f;
	float ail_min = 0.01f;
	float rud_min = 0.01f;
	float thr_min = 0.01f;


	public void Save()
	{
		SavePrefs saveSettings = new SavePrefs ("userPrefs");
		//Axis

		saveSettings.save("Pitch_axis", Pitch_axis) ;
		saveSettings.save("Rudder_axis", Rudder_axis) ;
		saveSettings.save("Aileron_axis", Aileron_axis) ;
		saveSettings.save("Elevator_axis", Elevator_axis) ;
		saveSettings.save("Throttle_axis", Throttle_axis) ;
		//Calibration
		saveSettings.save( "p_calib",p_calib);
        saveSettings.save( "r_calib",r_calib);
		saveSettings.save( "a_calib",a_calib);
		saveSettings.save( "e_calib",e_calib);
		saveSettings.save( "t_calib",t_calib);
		//Reverses
		saveSettings.save( "rev_p", rev_p);
		saveSettings.save( "rev_r", rev_r);
		saveSettings.save( "rev_a", rev_a);
		saveSettings.save( "rev_e", rev_e);
		saveSettings.save( "rev_t", rev_t);
		//EXPO
		saveSettings.save( "exp_p", exp_p);
		saveSettings.save( "exp_r", exp_r);
		saveSettings.save( "exp_a", exp_a);
		saveSettings.save( "exp_e", exp_e);
		saveSettings.save( "exp_t", exp_t);
		
		//EndPoints
		saveSettings.save( "pitch_max", pitch_max);
		saveSettings.save( "elev_max", elev_max);
		saveSettings.save( "ail_max", ail_max);
		saveSettings.save( "rud_max", rud_max);
		saveSettings.save( "thr_max", thr_max);

		saveSettings.save( "pitch_min", pitch_min);
		saveSettings.save( "elev_min", elev_min);
		saveSettings.save( "ail_min", ail_min);
		saveSettings.save( "rud_min", rud_min);
		saveSettings.save( "thr_min", thr_min);
		saveSettings.close ();

	}

	void Load()
	{
		List<string> Drop_Axis = new List<string> () { "Axis_1", "Axis_2", "Axis_3", "Axis_4", "Axis_5", "Axis_6", "Axis_7", "Axis_8", "Axis_9", "Axis_10" };
		//Axis
		LoadPrefs loadSettings = new LoadPrefs ("userPrefs");
		Pitch_axis    = loadSettings.loadString ("Pitch_axis");
		Rudder_axis   = loadSettings.loadString("Rudder_axis") ;
		Aileron_axis  = loadSettings.loadString("Aileron_axis") ;
		Elevator_axis = loadSettings.loadString("Elevator_axis");
		Throttle_axis = loadSettings.loadString("Throttle_axis");

		Dropdown_pitch.GetComponent<Dropdown> ().value=Drop_Axis.IndexOf(Pitch_axis);
		Dropdown_rudder.GetComponent<Dropdown> ().value=Drop_Axis.IndexOf(Rudder_axis);
		Dropdown_aileron.GetComponent<Dropdown> ().value=Drop_Axis.IndexOf(Aileron_axis);
		Dropdown_elevator.GetComponent<Dropdown> ().value=Drop_Axis.IndexOf(Elevator_axis);
		Dropdown_Throttle.GetComponent<Dropdown> ().value=Drop_Axis.IndexOf(Throttle_axis);
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

		if(rev_p>0){rev_p_t.isOn = false;}
		else{rev_p_t.isOn = true;}
		if(rev_r>0){rev_r_t.isOn = false;}
		else{rev_r_t.isOn = true;}
		if(rev_a>0){rev_a_t.isOn = false;}
		else{rev_a_t.isOn = true;}
		if(rev_e>0){rev_e_t.isOn = false;}
		else{rev_e_t.isOn = true;}
		if(rev_t>0){rev_t_t.isOn = false;}
		else{rev_t_t.isOn = true;}

		//EXPO
		exp_p = loadSettings.loadInt( "exp_p");
		exp_r = loadSettings.loadInt( "exp_r");
		exp_a = loadSettings.loadInt( "exp_a");
		exp_e = loadSettings.loadInt( "exp_e");
		exp_t = loadSettings.loadInt( "exp_t");
		
	
		if(exp_p>0){exp_p_f.text = ""+exp_p;}
		else{exp_p_f.text = ""+exp_p;}

		if(exp_r>0){exp_r_f.text =""+ exp_r;}
		else{exp_r_f.text =""+ exp_r;}

		if(exp_a>0){exp_a_f.text = ""+exp_a;}
		else{exp_a_f.text = ""+exp_a;}

		if(exp_e>0){exp_e_f.text = ""+exp_e;}
		else{exp_e_f.text = ""+exp_e;}

		if(exp_t>0){exp_t_f.text = ""+exp_t;}
		else{exp_t_f.text = ""+exp_t;}
        //EndPoints
		pitch_max=loadSettings.loadFloat( "pitch_max",0.1f);
		elev_max=loadSettings.loadFloat( "elev_max",0.1f);
		ail_max=loadSettings.loadFloat( "ail_max",0.1f);
		rud_max=loadSettings.loadFloat( "rud_max",0.1f);
		thr_max=loadSettings.loadFloat( "thr_max",0.1f);

		pitch_min=loadSettings.loadFloat( "pitch_min",-0.1f);
		elev_min =loadSettings.loadFloat( "elev_min" ,-0.1f);
		ail_min  =loadSettings.loadFloat( "ail_min"  ,-0.1f);
		rud_min  =loadSettings.loadFloat( "rud_min"  ,-0.1f);
		thr_min  =loadSettings.loadFloat( "thr_min"  ,-0.1f);
		loadSettings.close();

    }

	float GetSign(float numb)
	{
		if (numb == 0) {return 1f;} 
		else {return numb / Mathf.Abs (numb);}
	}
	// Use this for initialization
	void Start () 
	{
	   List<string> Drop_Axis = new List<string> () { "Axis_1", "Axis_2", "Axis_3", "Axis_4", "Axis_5", "Axis_6", "Axis_7", "Axis_8", "Axis_9", "Axis_10" };
		Dropdown_pitch.AddOptions(Drop_Axis);
		Dropdown_rudder.AddOptions(Drop_Axis);
		Dropdown_aileron.AddOptions(Drop_Axis);
		Dropdown_elevator.AddOptions(Drop_Axis);
		Dropdown_Throttle.AddOptions(Drop_Axis);

		Dropdown_pitch.value = 0;
		Dropdown_rudder.value = 1;
		Dropdown_aileron.value = 2;
		Dropdown_elevator.value = 3;
		Dropdown_Throttle.value = 4;

		//
		Load();

        
	}
	
	// Update is called once per frame
	void Update () 
	{

		Axis_1.value = Input.GetAxisRaw ("Axis_1");
		Axis_2.value = Input.GetAxisRaw("Axis_2");
		Axis_3.value = Input.GetAxisRaw("Axis_3");
		Axis_4.value = Input.GetAxisRaw("Axis_4");
		Axis_5.value = Input.GetAxisRaw("Axis_5");
		Axis_6.value = Input.GetAxisRaw("Axis_6");
		Axis_7.value = Input.GetAxisRaw("Axis_7");
		Axis_8.value = Input.GetAxisRaw("Axis_8");
        Axis_9.value = Input.GetAxisRaw("Axis_9");
        Axis_10.value= Input.GetAxisRaw("Axis_10");

        //
        Pitch_axis    = Dropdown_pitch.GetComponent<Dropdown> ().options [Dropdown_pitch.value].text;
		Rudder_axis   = Dropdown_rudder.GetComponent<Dropdown> ().options [Dropdown_rudder.value].text;
		Aileron_axis  = Dropdown_aileron.GetComponent<Dropdown> ().options [Dropdown_aileron.value].text;
		Elevator_axis = Dropdown_elevator.GetComponent<Dropdown> ().options [Dropdown_elevator.value].text;
		Throttle_axis = Dropdown_Throttle.GetComponent<Dropdown> ().options [Dropdown_Throttle.value].text;

		//Reversing
			//pitch
		if (rev_p_t.isOn){rev_p = -1;}else{rev_p = 1;}
			//rudder
		if (rev_r_t.isOn){rev_r = -1;}else{rev_r = 1;}
			//aileron
		if (rev_a_t.isOn){rev_a = -1;}else{rev_a = 1;}
			//elevator
		if (rev_e_t.isOn){rev_e = -1;}else{rev_e = 1;}
		//Throttle
		if (rev_t_t.isOn){rev_t = -1;}else{rev_t = 1;}
        //EXPO
        try{exp_p = int.Parse(exp_p_f.text);} catch {exp_p= 0;}
		try{exp_r = int.Parse(exp_r_f.text);} catch {exp_r= 0;}
		try{exp_a = int.Parse(exp_a_f.text);} catch {exp_a= 0;}
		try{exp_e = int.Parse(exp_e_f.text);} catch {exp_e= 0;}
		try{exp_t = int.Parse(exp_t_f.text);} catch {exp_t= 0;}

		if (exp_p >  99) {exp_p =  99;}
		if (exp_p < -99) {exp_p = -99;}
		if (exp_r >  99) {exp_r =  99;}
		if (exp_r < -99) {exp_r = -99;}
		if (exp_a >  99) {exp_a =  99;}
		if (exp_a < -99) {exp_a = -99;}
		if (exp_e >  99) {exp_e =  99;}
		if (exp_e < -99) {exp_e = -99;}
		if (exp_t >  99) {exp_t =  99;}
		if (exp_t < -99) {exp_t = -99;}
		exp_p_f.text = exp_p+"";
		exp_r_f.text = exp_r+"";
		exp_a_f.text = exp_a+"";
		exp_e_f.text = exp_e+"";
		exp_t_f.text = exp_t+"";


        if (calib_Max_Min.isOn) {
			CalibrateMinMax ();
		}

        
        
		Pitch_tr    = Input.GetAxis (Pitch_axis)+p_calib;
		if (Pitch_tr >= 0) {Pitch = Pitch_tr / pitch_max;} 
		else {Pitch =  Pitch_tr / Mathf.Abs(pitch_min);}
        Pitch    	= Expo(Pitch,exp_p)* rev_p;
       
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


        //print (Pitch + " / " + Rudder + " / " + Aileron + " / " + Elevator+" / "+rev_p);
        //
        //print("calibmaxmin="+calib_Max_Min.isOn);

        Slider_1.value=Pitch;
		Slider_2.value=Rudder;
		Slider_3.value=Aileron;
		Slider_4.value=Elevator;
		Slider_Throttle.value = Throttle;
	}


	public void Calibrate_center()
	{
		p_calib=0;
		r_calib=0;
		a_calib=0;
		e_calib=0;
		t_calib=0;

		//
		Pitch    = Input.GetAxis (Pitch_axis)+p_calib;
		Rudder   = Input.GetAxis (Rudder_axis)+r_calib;
		Aileron  = Input.GetAxis (Aileron_axis)+a_calib;
		Elevator = Input.GetAxis (Elevator_axis)+e_calib;
		Throttle = Input.GetAxis (Throttle_axis)+t_calib;
		//
		p_calib=-Pitch;
		r_calib=-Rudder;
		a_calib=-Aileron;
		e_calib=-Elevator;
		t_calib=-Throttle;

	}

	public void Back()
	{
		Application.LoadLevel ("Main_menu");
		Save ();
	}


    public void CalibrateMinMax()
    {
        //Pitch
        if (Pitch_tr > pitch_max) {pitch_max =Pitch_tr;}
		if (Pitch_tr < pitch_min) {pitch_min =Pitch_tr;}
		//Elev
        if (Elevator_tr > elev_max) {elev_max =Elevator_tr;}
		if (Elevator_tr < elev_min) {elev_min =Elevator_tr;}
		//Ail
        if (Aileron_tr > ail_max) {ail_max =Aileron_tr;}
		if (Aileron_tr < ail_min) {ail_min =Aileron_tr;}
		//Rudder
        if (Rudder_tr > rud_max) {rud_max =Rudder_tr;}
		if (Rudder_tr < rud_min) {rud_min =Rudder_tr;}
		//Throttle
		if (Throttle_tr > thr_max) {thr_max =Throttle_tr;}
		if (Throttle_tr < thr_min) {thr_min =Throttle_tr;}
	}

	public void ResetMinMax()
	{
		 pitch_max=0.1f;
		 elev_max=0.1f;
		 ail_max=0.1f;
		 rud_max=0.1f;
		 thr_max=0.1f;

		pitch_min=-0.1f;
		elev_min=-0.1f;
		ail_min=-0.1f;
		rud_min=-0.1f;
		thr_min=-0.1f;
	}

    public float Expo(float tr_signal, int expo)
    {
        float exp = expo/10 + 0.001f;//to avoid division by 0
		return  GetSign(tr_signal) *(Mathf.Exp(exp * Mathf.Abs(tr_signal)) - 1) / (Mathf.Exp(exp) - 1);
    }
}
