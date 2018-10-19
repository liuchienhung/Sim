using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class Menu_HeliSettings : MonoBehaviour {

	float mblade_length=600f;
	float mblade_width=55f;
	float tblade_length=95f;
	float tblade_width=20f;
	float Ic=0.0001f;
	float mass=4f;
	float Power_m=1000f ;
	float lift_factor=1f;
	float Flip_rate_deg=300f; 
	float Roll_rate_deg =300f;
	float Piro_rate_deg =650f;
    float Power_0_factor = 1f;
    float Power_cyclic_factor = 1f;
    float Power_pitch_factor = 1f;
    float Power_tail_factor = 1f;
    float Power_oversp_factor = 1f;
    float Coef_drag_front = 1f;
    float Coef_drag_side = 1f;
    float Coef_drag_bottom = 1f;
    float Coef_onc_stream = 1f;

	float thr_norm=20f;
	float thr_idle1=60f;
	float thr_idle2=80f;
	float rpm_norm=1600f;
	float rpm_idle1=2000f;
	float rpm_idle2=2200f;
	float rpm_max=2600f;

	float motor_vol=1f;
	float pitch_vol=1f;
	float cyclic_vol=1f;
	float tail_vol=1f;

    float fieldOfView=25f;

	//FBL
    float kI_cyclic=5f;
	float kI_rud=60f;
	float kP_rud=60f;
    static float kP_elev = 0.3f;
    static float kP_ail = 0.05f;

	public InputField mblade_length_f;
	public InputField mblade_width_f;
	public InputField tblade_length_f;
	public InputField tblade_width_f;
	public InputField Ic_f;
	public InputField mass_f;

    public InputField Power_m_f;
	public InputField lift_factor_f;
	public InputField Flip_rate_deg_f;
	public InputField Roll_rate_deg_f;
	public InputField Piro_rate_deg_f;

    public InputField Power_0_factor_f;
    public InputField Power_cyclic_factor_f;
    public InputField Power_pitch_factor_f;
    public InputField Power_tail_factor_f;
    public InputField Power_oversp_factor_f;

    public InputField Coef_drag_front_f;
    public InputField Coef_drag_side_f;
    public InputField Coef_drag_bottom_f;
    public InputField Coef_onc_stream_f;

	public InputField thr_norm_f;
	public InputField thr_idle1_f;
	public InputField thr_idle2_f;
	public InputField rpm_max_f;
	public InputField rpm_norm_f;
	public InputField rpm_idle1_f;
	public InputField rpm_idle2_f;

	public InputField motor_vol_f;
	public InputField pitch_vol_f;
	public InputField cyclic_vol_f;
	public InputField tail_vol_f;
	
	public InputField kI_cyclic_f;
	public InputField kI_rud_f;
	public InputField kP_rud_f;
    public InputField kP_elev_f;
    public InputField kP_ail_f;

	public InputField heliName_f;

	string heliName;

    // Use this for initialization
    void Start () {
		LoadPrefs loadHeli = gameObject.AddComponent< LoadPrefs>() as LoadPrefs;
		loadHeli.setFileName("Field");
		loadHeli.initialize ();
		heliName=loadHeli.loadString ("heliName", "Logo600");
		loadHeli.close ();

		LoadPrefs loadHeliPrefs = gameObject.AddComponent< LoadPrefs>() as LoadPrefs;
		loadHeliPrefs.setFileNameAndFold (heliName,"Helis");
		loadHeliPrefs.initialize ();

		mblade_length  = loadHeliPrefs.loadFloat("mblade_length", mblade_length);
		mblade_width   = loadHeliPrefs.loadFloat("mblade_width" , mblade_width);
		tblade_length  = loadHeliPrefs.loadFloat("tblade_length", tblade_length);
		tblade_width   = loadHeliPrefs.loadFloat("tblade_width" , tblade_width);
		Ic             = loadHeliPrefs.loadFloat("Ic", Ic);
		mass           = loadHeliPrefs.loadFloat("mass", mass);
		if (Ic == 0) {Ic = 0.001f;}
		if (mass == 0) {mass = 4f;}
		Power_m       = loadHeliPrefs.loadFloat("Power_m", Power_m);
		lift_factor   = loadHeliPrefs.loadFloat("lift_factor",lift_factor);
		Flip_rate_deg = loadHeliPrefs.loadFloat("Flip_rate_deg",Flip_rate_deg);
		Roll_rate_deg = loadHeliPrefs.loadFloat("Roll_rate_deg",Roll_rate_deg);
		Piro_rate_deg = loadHeliPrefs.loadFloat("Piro_rate_deg",Piro_rate_deg);
        //Power
        Power_0_factor      = loadHeliPrefs.loadFloat("Power_0_factor", Power_0_factor);
        Power_cyclic_factor = loadHeliPrefs.loadFloat("Power_cyclic_factor", Power_cyclic_factor);
        Power_pitch_factor  = loadHeliPrefs.loadFloat("Power_pitch_factor", Power_pitch_factor);
        Power_tail_factor   = loadHeliPrefs.loadFloat("Power_tail_factor", Power_tail_factor);
        Power_oversp_factor = loadHeliPrefs.loadFloat("Power_oversp_factor", Power_oversp_factor);
        //Drag
        Coef_drag_front  = loadHeliPrefs.loadFloat("Coef_drag_front", Coef_drag_front);
        Coef_drag_side   = loadHeliPrefs.loadFloat("Coef_drag_side", Coef_drag_side);
        Coef_drag_bottom = loadHeliPrefs.loadFloat("Coef_drag_bottom", Coef_drag_bottom);
        Coef_onc_stream  = loadHeliPrefs.loadFloat("Coef_onc_stream", Coef_onc_stream);

		thr_norm=loadHeliPrefs.loadFloat("thr_norm", thr_norm);
		thr_idle1=loadHeliPrefs.loadFloat("thr_idle1", thr_idle1);
		thr_idle2=loadHeliPrefs.loadFloat("thr_idle2", thr_idle2);

		rpm_max   = loadHeliPrefs.loadFloat("rpm_max", rpm_max);
		rpm_norm=loadHeliPrefs.loadFloat("rpm_norm", rpm_norm);
		rpm_idle1=loadHeliPrefs.loadFloat("rpm_idle1", rpm_idle1);
		rpm_idle2=loadHeliPrefs.loadFloat("rpm_idle2", rpm_idle2);

		motor_vol  = loadHeliPrefs.loadFloat ("motor_vol",motor_vol);
		pitch_vol  = loadHeliPrefs.loadFloat ("pitch_vol",pitch_vol);
		cyclic_vol = loadHeliPrefs.loadFloat ("cyclic_vol",cyclic_vol);
		tail_vol   = loadHeliPrefs.loadFloat ("tail_vol",tail_vol);

        fieldOfView = loadHeliPrefs.loadFloat("camera_zoom", 1f);
		loadHeliPrefs.close ();

		//------------------
		mblade_length_f.text = ""+mblade_length;
		tblade_length_f.text = ""+tblade_length;
		mblade_width_f.text  = ""+mblade_width;
		tblade_width_f.text  = ""+tblade_width;
		mass_f.text          = "" + mass;
		Ic_f.text            = ""+Ic;

		Power_m_f.text = ""+Power_m;
		lift_factor_f.text = ""+lift_factor;
		Flip_rate_deg_f.text = ""+Flip_rate_deg;
		Roll_rate_deg_f.text = ""+Roll_rate_deg;
		Piro_rate_deg_f.text = ""+Piro_rate_deg;

        Power_0_factor_f.text = "" + Power_0_factor;
        Power_cyclic_factor_f.text = "" + Power_cyclic_factor;
        Power_pitch_factor_f.text = "" + Power_pitch_factor;
        Power_tail_factor_f.text = "" + Power_tail_factor;
        Power_oversp_factor_f.text = "" + Power_oversp_factor;

        Coef_drag_front_f.text  = "" + Coef_drag_front;
        Coef_drag_side_f.text   = "" + Coef_drag_side;
        Coef_drag_bottom_f.text = "" + Coef_drag_bottom;
        Coef_onc_stream_f.text  = "" + Coef_onc_stream;

		thr_norm_f.text  = "" + thr_norm;
		thr_idle1_f.text  = "" + thr_idle1;
		thr_idle2_f.text  = "" + thr_idle2;

		rpm_max_f.text    = "" + rpm_max;
		rpm_norm_f.text   = "" + rpm_norm;
		rpm_idle1_f.text  = "" + rpm_idle1;
		rpm_idle2_f.text  = "" + rpm_idle2;

		motor_vol_f.text  = "" + motor_vol;
		pitch_vol_f.text  = "" + pitch_vol;
		cyclic_vol_f.text = "" + cyclic_vol;
		tail_vol_f.text   = "" + tail_vol;
		
		//FBL
		LoadPrefs FBL_loadSet = gameObject.AddComponent<LoadPrefs>() as LoadPrefs;
		FBL_loadSet.setFileNameAndFold(heliName+"_FBL","Helis");
		FBL_loadSet.initialize ();
		kI_rud = FBL_loadSet.loadFloat ("kI_rud", kI_rud);
		kP_rud = FBL_loadSet.loadFloat ("kP_rud", kP_rud);
		kI_cyclic = FBL_loadSet.loadFloat ("kI_cyclic", kI_cyclic);
        kP_elev = FBL_loadSet.loadFloat("kP_elev", kP_elev);
        kP_ail = FBL_loadSet.loadFloat("kP_ail", kP_ail);
        FBL_loadSet.close ();
		
		kI_rud_f.text  = "" + kI_rud;
		kP_rud_f.text  = "" + kP_rud;
		kI_cyclic_f.text  = "" + kI_cyclic*10f;

        kP_ail_f.text = "" + kP_ail;
        kP_elev_f.text = "" + kP_elev;

		print (heliName);
		heliName_f.text = heliName;
    }
	
	// Update is called once per frame
	void Update () {
		try
		{
			
			mblade_length = int.Parse(mblade_length_f.text);
			tblade_length = int.Parse(tblade_length_f.text);
			mblade_width  = int.Parse(mblade_width_f.text);
			tblade_width  = int.Parse(tblade_width_f.text);
		    

			Power_m = int.Parse(Power_m_f.text);
			lift_factor = float.Parse(lift_factor_f.text);
			Flip_rate_deg= int.Parse(Flip_rate_deg_f.text);
			Roll_rate_deg= int.Parse(Roll_rate_deg_f.text);
			Piro_rate_deg= int.Parse(Piro_rate_deg_f.text);
            //
            Power_0_factor      = float.Parse(Power_0_factor_f.text);
            Power_cyclic_factor = float.Parse(Power_cyclic_factor_f.text);
            Power_pitch_factor  = float.Parse(Power_pitch_factor_f.text);
            Power_tail_factor   = float.Parse(Power_tail_factor_f.text);
            Power_oversp_factor = float.Parse(Power_oversp_factor_f.text);
            //
            Coef_drag_front     = float.Parse(Coef_drag_front_f.text);
            Coef_drag_side      = float.Parse(Coef_drag_side_f.text);
            Coef_drag_bottom    = float.Parse(Coef_drag_bottom_f.text);
            Coef_onc_stream     = float.Parse(Coef_onc_stream_f.text);

			thr_norm =float.Parse(thr_norm_f.text);
			thr_idle1=float.Parse(thr_idle1_f.text);
			thr_idle2=float.Parse(thr_idle2_f.text);

			rpm_max  =float.Parse(rpm_max_f.text);
			rpm_norm =float.Parse(rpm_norm_f.text);
			rpm_idle1=float.Parse(rpm_idle1_f.text);
			rpm_idle2=float.Parse(rpm_idle2_f.text);

			motor_vol =float.Parse(motor_vol_f.text) ;
			pitch_vol =float.Parse(pitch_vol_f.text) ;
			cyclic_vol=float.Parse(cyclic_vol_f.text) ;
			tail_vol  =float.Parse(tail_vol_f.text) ;

			if(motor_vol>2f) {motor_vol=2f;}
			if(pitch_vol>2f) {pitch_vol=2f;}
			if(cyclic_vol>2f){cyclic_vol=2f;}
		    if(tail_vol>2f)  {tail_vol=2f;}

			if(motor_vol<0) {motor_vol=0;}
			if(pitch_vol<0) {pitch_vol=0;}
			if(cyclic_vol<0){cyclic_vol=0;}
			if(tail_vol<0)  {tail_vol=0;}

			//FBL
			kI_rud= float.Parse(kI_rud_f.text);
			kP_rud= float.Parse(kP_rud_f.text);
			kI_cyclic= float.Parse(kI_cyclic_f.text)/10f;
            kP_ail = float.Parse(kP_ail_f.text);
            kP_elev = float.Parse(kP_elev_f.text);

			Ic  = float.Parse(Ic_f.text);
			if (Ic == 0) {Ic = 0.001f;}
			mass  = float.Parse(mass_f.text);
			if (mass == 0) {mass = 4f;}
        }
		catch {
			//Power_m_f.text = "3000";
			//lift_factor_f.text = "1";
			//Flip_rate_deg_f.text = "300";
			//Roll_rate_deg_f.text = "300";
			//Piro_rate_deg_f.text = "650";
			print("Parse error ");
		}
	}
	public void Save()
	{
		SavePrefs savePrefsAll =  gameObject.AddComponent<SavePrefs>() as SavePrefs ;
		savePrefsAll.setFileNameAndFold (heliName,"Helis");
		savePrefsAll.initialize ();
		savePrefsAll.save("mblade_length", mblade_length);
		savePrefsAll.save("mblade_width",  mblade_width);
		savePrefsAll.save("tblade_length", tblade_length);
		savePrefsAll.save("tblade_width",  tblade_width);
		savePrefsAll.save("Ic", Ic);
		savePrefsAll.save("mass", mass);
		savePrefsAll.save("Power_m", Power_m);
		savePrefsAll.save("lift_factor",lift_factor);
		savePrefsAll.save("Flip_rate_deg",Flip_rate_deg);
		savePrefsAll.save("Roll_rate_deg",Roll_rate_deg);
		savePrefsAll.save("Piro_rate_deg",Piro_rate_deg); savePrefsAll.save("Power_0_factor", Power_0_factor);
        savePrefsAll.save("Power_cyclic_factor", Power_cyclic_factor);
        savePrefsAll.save("Power_pitch_factor", Power_pitch_factor);
        savePrefsAll.save("Power_tail_factor", Power_tail_factor);
        savePrefsAll.save("Power_oversp_factor", Power_oversp_factor);
        savePrefsAll.save("Coef_drag_front", Coef_drag_front);
        savePrefsAll.save("Coef_drag_side", Coef_drag_side);
        savePrefsAll.save("Coef_drag_bottom", Coef_drag_bottom);
		savePrefsAll.save("Coef_onc_stream", Coef_onc_stream);
		savePrefsAll.save("thr_norm", thr_norm);
		savePrefsAll.save("thr_idle1", thr_idle1);
		savePrefsAll.save("thr_idle2", thr_idle2);
		savePrefsAll.save("rpm_max", rpm_max);
		savePrefsAll.save("rpm_norm", rpm_norm);
		savePrefsAll.save("rpm_idle1", rpm_idle1);
		savePrefsAll.save("rpm_idle2", rpm_idle2);
		savePrefsAll.save("motor_vol",motor_vol);
		savePrefsAll.save("pitch_vol",pitch_vol);
		savePrefsAll.save("cyclic_vol",cyclic_vol);
		savePrefsAll.save("tail_vol",tail_vol);
        savePrefsAll.save("camera_zoom",fieldOfView);
		savePrefsAll.close ();
		
		SavePrefs savePrefsFBL = gameObject.AddComponent<SavePrefs>() as SavePrefs ;
		savePrefsFBL.setFileNameAndFold (heliName+"_FBL","Helis");
		savePrefsFBL.initialize ();
		savePrefsFBL.save("kI_rud", kI_rud);
		savePrefsFBL.save("kP_rud", kP_rud);
		savePrefsFBL.save("kI_cyclic", kI_cyclic);
        savePrefsFBL.save("kP_elev", kP_elev);
        savePrefsFBL.save("kP_ail", kP_ail);
        savePrefsFBL.close ();
		
	}
	public void Back()
	{
		Save ();
		Application.LoadLevel ("Main_menu");
	}
}
