using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Motor: MonoBehaviour {
	
	
	string heliName;

		
    int i = 0;
	Logger motorLogger;
    bool closeLogs = false;
    public static object motor;
    float Power_m;
	float Torq_m ;
    public static float e = 0;
    static float last_e = 0;
    public static float input = 0;
    public static float output = 0f;
    public static float errSum = 0;
    public static float dErr = 0;
    
    static float kp = 0.02f;
    static float ki = 0.01f;
    static float kd = 0.00005f;
    public static float rpm_r = 0;
    public float rpm_set;
    float rpm_max = 2600;
    
	float Ic = 0.0009f;
	
    public static int pwm = 0;
    static float  autorotation=1;
	static float deltaTime;

    float angle;
    float anglet;
    bool collision;
    public Transform rotor_collider;
	public Transform rotor_model;
    public Transform trotor;
    public Transform trotor_center;
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
	public void setMotorPrwRpm(float Power,float rpm_max)
	{
		this.Power_m = Power;
		this.rpm_max = rpm_max;
	}

	public void Init()
	{
		collision = false;
		motorLogger = gameObject.AddComponent<Logger>() as Logger;
		motorLogger.Init("MotorLogger",0.1f);
		LoadPrefs loadHeli = gameObject.AddComponent<LoadPrefs>() as LoadPrefs;
        loadHeli.setFileName("Field");
        loadHeli.initialize();
        heliName =loadHeli.loadString ("heliName", "Logo600");
       
        loadHeli.setFileName("Field");
		loadHeli.initialize ();
		loadHeli.close ();

		LoadPrefs loadHeliPrefs = gameObject.AddComponent< LoadPrefs>() as LoadPrefs;
		loadHeliPrefs.setFileNameAndFold (heliName,"Helis");
		loadHeliPrefs.initialize ();
		Ic  = loadHeliPrefs.loadFloat("Ic", Ic);
		rpm_max  = loadHeliPrefs.loadFloat("rpm_max", 0f);
		if (Ic == 0) {Ic = 0.001f;}
		if (rpm_max == 0) {rpm_max = rpm_set*1.2f;}
		loadHeliPrefs.close ();
     
	}

	public  float getRpm(float Power_c,float Power_oversp, float deltaTime)
    {

        if (rpm_r < 1) { errSum = 0; }
        if (rpm_r > rpm_max) { errSum = 0; }
        float rpm = rpm_set;
        
		float Torq0 = 0;
        Torq_m = Power_m / rpm;
      
        if (rpm_r > rpm_max)
        {
            Torq_m = 0;
        }
        //Load 
		if(rpm_r<1){
			Torq0=0;
           
		}
		else{Torq0=100f / rpm;}
		rpm_r = rpm_r + deltaTime * (Power_oversp / rpm * 4f + Power_m / rpm * output - (Power_c * (rpm_r / rpm) / rpm +Torq0)) / (Ic)*10f;
		/*
		print ("Torq="+(Power_oversp / rpm * 4f + Torq_m * output - (Power_c * (rpm_r / rpm) / rpm +Torq0)) );
		print ("Torq_m="+ Torq_m);
		print ("Power_m="+ Power_m);
		print ("rpm="+ rpm);
		print ("output="+output);
		print ("Power_m / rpm="+Power_m / rpm);
		*/
        e = rpm*autorotation - rpm_r;
		errSum = errSum * autorotation + e * deltaTime;
		dErr = (e - last_e) / deltaTime;
		if (errSum >  200) { errSum =  200; }
		if (errSum < -200) { errSum = -200; }
		if (Mathf.Abs(e) < 0.01f) { errSum = 0; }
        output = kp * e + ki * errSum + kd * dErr;
        /**/
        if (output > 1)
        {
            output = 1;
        }
        if (output < 0)
        {
            output = 0;
        }


        
        last_e = e;
        //print("rpm=" + rpm_r + " / output=" + output + " / error=" + e + " /  errSum="+ errSum + " /  dErr=" + dErr);
        //input = output;
        output =output* autorotation;
        pwm = (int)(output * 100);


        //rotor_rb.AddRelativeTorque(0, 100f, 0);

        //if (rpm_r / 60 > 7.9f){rotor_rb.maxAngularVelocity = 50;}
        //else {rotor_rb.maxAngularVelocity = rpm_r / 60 * 360 / (180 / 3.14f);}

        if (rpm_r < 1) { rpm_r = 0; }
        angle = angle + rpm_r / 60 * deltaTime * 360;
        anglet = anglet + rpm_r * 4 / 60 * deltaTime * 360;
        
        //print("angle=" + angle);
        while (angle > 360)
        {
            angle = angle - 360;
        }
        while (anglet > 360)
        {
            anglet = anglet - 360;
        }
        float rotTailBy = anglet - trotor.eulerAngles.x;
        if (rpm_r < 1) { rotTailBy = 0; }
        
        rotor_collider.Rotate(0, (angle - rotor_collider.eulerAngles.y), 0);
		rotor_model.Rotate(0, (angle - rotor_model.eulerAngles.y), 0);
        trotor.RotateAround(trotor.transform.position,trotor.right, (rotTailBy));
        //trotor.SetPositionAndRotation(trotor.position,new Quaternion(0,0,0,0));
        //Logs
        motorLogger.saveLog ("rpm_r",(int)rpm_r);
		motorLogger.saveLog("pwm",(int)pwm);
		motorLogger.saveLog("e",e);
		motorLogger.saveLog("errSum",errSum);
		motorLogger.saveLog("dErr",dErr);
		motorLogger.saveLog ("rpm",(int)rpm);

        if (!collision||!closeLogs){ motorLogger.writeCycle(); }
		//
        return rpm_r;
    }

    public  int getPwm()
    { return pwm; }
	public  float getError()
	{ return e; }
	public  float getRpm()
	{
		return rpm_r;
	}
	public void closeLogger()
	{
        closeLogs= true;
        motorLogger.close ();
	}

    
}
