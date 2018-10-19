using System.Collections;
using System.Collections.Generic;
using UnityEngine;


   /*
    * Flybarless System - Heli flight PID controller system
    * Target value is angular velocity.
    * FBL receives angular velocity from transmitter stick
    */


public  class FBL: MonoBehaviour {
	string heliName;
	public void setFBl_CG(Rigidbody rb)
	{
		this.heliCG_rb = rb;
	}

	public void initialize(){
		collision = false;

		LoadPrefs loadHeli = gameObject.AddComponent< LoadPrefs>() as LoadPrefs;
		heliName=loadHeli.loadString ("heliName", "Logo600");
		loadHeli.setFileName("Field");
		loadHeli.initialize ();
		loadHeli.close ();
		LoadPrefs FBL_loadSet = gameObject.AddComponent<LoadPrefs>() as LoadPrefs;
		FBL_loadSet.setFileNameAndFold(heliName+"_FBL","Helis");
		FBL_loadSet.initialize ();
		kI_rud = FBL_loadSet.loadFloat ("kI_rud", kI_rud);
		kP_rud = FBL_loadSet.loadFloat ("kP_rud", kP_rud)/1000f;

		kI_elev = FBL_loadSet.loadFloat ("kI_cyclic", kI_elev);
		kI_ail =kI_elev;
		kP_ail = FBL_loadSet.loadFloat("kP_ail", kP_ail);
		kP_elev = FBL_loadSet.loadFloat("kP_elev", kP_elev);
		FBL_loadSet.close ();

	}

   public FBL(){}
	 
    public static object motor;
	//Elevator
	float Flip_rate;
	public static float e_elev = 0;
	static float last_e_elev = 0;
	public static float errSum_elev = 0;
	public static float dErr_elev = 0;
    static float kP_elev = 0.3f;
    static float kI_elev = 5f;
	static float kD_elev = 0.0005f;
	//Aileron
	float Roll_rate;
	public static float e_ail = 0;
	static float last_e_ail = 0;
	public static float errSum_ail = 0;
	public static float dErr_ail = 0;
    static float kP_ail = 0.05f;
    static float kI_ail = 5f;
	static float kD_ail = 0.0005f;


    public static float rpm = 0;
    
    
   
    static float  autorotation=1;
	static float deltaTime;
	public static float input = 0;
    public static float output = 0f;
    float angle;
    bool collision;
	public Rigidbody heliCG_rb;
	
    public void Collision()
    {
        collision = true;
    }
    public  void setAutorotation(float autor)
    {
        autorotation = autor;
    }
	public  void setTime(float dt)
	{
		deltaTime =dt;
	}

	
	public  float getAilTorq(float rotRate, float deltaTime)
    {
        Roll_rate=heliCG_rb.transform.InverseTransformVector (heliCG_rb.angularVelocity).x;
		e_ail = rotRate - Roll_rate;
		errSum_ail = errSum_ail + e_ail * deltaTime;
		dErr_ail = (e_ail - last_e_ail) / deltaTime;
        //Integral error Cut-off
        float erAil_max = 0.25f;
        if (errSum_ail > erAil_max)  { errSum_ail = erAil_max; }
        if (errSum_ail < -erAil_max) { errSum_ail = -erAil_max; }
        if (Mathf.Abs(e_ail) < 0.01f) { errSum_ail = 0; }
        output = kP_ail * e_ail + kI_ail * errSum_ail + kD_ail * dErr_ail;
       //Output Cut-off
	    if (output > 1){output = 1;}
        if (output < -1){output = -1;}


        last_e_ail = e_ail;
       
        return output;
    }
	//ELEVATOR
	public  float getElevTorq(float rotRate, float deltaTime)
    {
        Flip_rate=heliCG_rb.transform.InverseTransformVector (heliCG_rb.angularVelocity).z;
		e_elev = rotRate - Flip_rate;
		errSum_elev = errSum_elev + e_elev * deltaTime;
		dErr_elev = (e_elev - last_e_elev) / deltaTime;
        //Integral error Cut-off
        float erElev_max = 0.25f;
        if (errSum_elev > erElev_max) { errSum_elev = erElev_max; }
        if (errSum_elev < -erElev_max) { errSum_elev = -erElev_max; }
        if (Mathf.Abs(e_elev) < 0.01f) { errSum_elev = 0; }
        output = kP_elev * e_elev + kI_elev * errSum_elev + kD_elev * dErr_elev;
		//Output Cut-off
		if (output > 1){output = 1;}
        if (output < -1){output = -1;}


        last_e_elev = e_elev;
       
		//Logs
		//FBL_Logger.saveLog ("rpm_r",(int)rpm_r)
		//if (!collision){ FBL_Logger.writeCycle(); }

        return output;
    }

	//RUDDER

	float Piro_rate;
	public static float e_rud = 0;
	static float last_e_rud = 0;
	public static float errSum_rud = 0;
	public static float dErr_rud = 0;
	static float kP_rud = 0.06f;
	static float kI_rud =60f;
	static float kD_rud = 0.00004f*0;
    static float outP_rud;
    static float outI_rud;
    static float outD_rud;
    float Pitch_last;
    float Cyclic_last;
    float p_comp=1f;
	float outp_comp=0;
	float decr_p_comp=0;
    static float output_rud;
    float Cyclic;
    float c_comp = 0f;
    float outc_comp = 0;
    float decr_c_comp = 0;
	float timer;
	float tlast=0;

    public  float getRud(float rotRate, float Pitch,float Cyclic_ail, float Cyclic_elev, float deltaTime)
	{
		deltaTime = deltaTime * 0.1f;
       

		//Pitch compensation
		outp_comp = (Mathf.Abs(Pitch)-Mathf.Abs(Pitch_last))*10f;
		if (outp_comp >  p_comp) { outp_comp =  p_comp; }
		if (outp_comp < -p_comp) { outp_comp = -p_comp; }
		//if (timer - tlast > 0.2f) {tlast = timer;}
        //Rate
        Piro_rate =heliCG_rb.transform.InverseTransformVector (heliCG_rb.angularVelocity).y;
		//print(Piro_rate);
		e_rud = rotRate - Piro_rate+outp_comp;
		errSum_rud = errSum_rud + e_rud * deltaTime;
		dErr_rud = (e_rud - last_e_rud) / deltaTime;
		//Integral error Cut-off
		float erRud_max = 0.05f;
		if (errSum_rud >  erRud_max) { errSum_rud =  erRud_max; }
		if (errSum_rud < -erRud_max) { errSum_rud = -erRud_max; }
        //if (Mathf.Abs(errSum_rud) >0) { errSum_rud = errSum_rud - deltaTime * 0.02f; }
        //if (Mathf.Abs(errSum_rud) <0) { errSum_rud = errSum_rud + deltaTime * 0.02f; }

		outP_rud = kP_rud * e_rud;
        outI_rud= kI_rud * errSum_rud;
        outD_rud= kD_rud * dErr_rud;
        //float outP_rud_max = 0.3f;
        //if (outP_rud >  outP_rud_max) { outP_rud= outP_rud_max; }
        //if (outP_rud < -outP_rud_max) { outP_rud= -outP_rud_max; }
        /**/
       
       
        output_rud = outP_rud + outI_rud + outD_rud + outc_comp*0;
		//Output Cut-off
		if (output_rud > 1){output_rud = 1;}
		if (output_rud < -1){output_rud = -1;}
		//
		
		last_e_rud = e_rud;

        
        Pitch_last = Pitch;
		return output_rud;
	}
    public float getErSumElev()
    {
        return errSum_elev;
    }
	public float getErSumAil()
	{
		return errSum_ail;
	}
	public float getErSumRud()
	{
		return errSum_rud;
	}

    public float getErrorRud()
    {
        return e_rud;
    }
	public float getPcomp()
	{
		return outp_comp;
	}
	public void closeLogger()
	{
		//FBL_Logger.close ();
	}
    public float getOutPrud()
    {
        return outP_rud;
    }
    public float getOutIrud()
    {
        return outI_rud;
    }
    public float getOutDrud()
    {
        return outD_rud;
    }

    public float getRudOutput()
    {
        return output_rud;
     }
}
