using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Motor : MonoBehaviour{
    int i = 0;
    Logger motorLogger= new Logger("MotorLogger",0.1f);
    public static object motor;
    float Power_m;
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
    static float rpm_max = 2600;
    
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

    public Motor(float Power)
    {
        this.Power_m = Power;
        motor = this;
        collision = false;
     }
	public  float getRpm(float Power_c,float Power_oversp, float deltaTime)
    {

        if (rpm_r < 1) { errSum = 0; }
        if (rpm_r > rpm_max) { errSum = 0; }
        float rpm = rpm_set;
        
		float Torq0 = 0;
        float Torq_m = Power_m / rpm;

        float Ic = 0.0009f;
      
        if (rpm_r > rpm_max)
        {
            Torq_m = 0;
        }
        //Load 
		if(rpm_r<1){
			Torq0=0;
           
		}
		else{Torq0=100f / rpm;}
		rpm_r = rpm_r + deltaTime * (Power_oversp / rpm * 4f + Torq_m * output - (Power_c * (rpm_r / rpm) / rpm +Torq0)) / Ic;

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

        if (!collision){ motorLogger.writeCycle(); }
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
		motorLogger.close ();
	}

    
}
