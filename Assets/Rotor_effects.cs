using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotor_effects  : MonoBehaviour{
    public Text fps_text;
    public Text text;
    public Slider slider1;
    public GameObject main_rotor;
    public GameObject mrotor_disc;
    public GameObject tail_rotor;
    public GameObject trotor_disc;

    float deltaTime;

    Color rotor_color;
    Renderer rotor_rend;
    public Renderer trotor_rend;
    Color mrotor_disc_color;
	Color mrotor_mat1_color;
	public Material mrotor_mat1;
    public Material mrotor_disc_mat;
    public Material trotor_disc_mat;

    float rpm;
    float rpm_tail;
    int pwm;


    // Use this for initialization
    void Start () {
        rotor_rend = main_rotor.GetComponent<Renderer>();
		mrotor_mat1_color=mrotor_mat1.color;
        //trotor_rend = tail_rotor.GetComponent<Renderer>();
        rotor_color = rotor_rend.material.color;
        mrotor_disc_color = mrotor_disc_mat.color;

    }
	
	// Update is called once per frame
	void Update () {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        rpm = Motor.rpm_r;
        rpm_tail = rpm * 4;

        if (rpm / 60 > 7.9f)
        {
			mrotor_mat1.color = new UnityEngine.Color(0f, 0f, 0f, 0.2f);
            rotor_rend.material.color = new UnityEngine.Color(rotor_color.r, rotor_color.g, rotor_color.b, 0.2f);
            mrotor_disc_mat.color = new UnityEngine.Color(mrotor_disc_color.r, mrotor_disc_color.g, mrotor_disc_color.b, 0.2f);

        }
        else
        {
			mrotor_mat1.color = new UnityEngine.Color(0, 0, 0, (0.2f + (1 - rpm / 60 / 7.9f)));
            rotor_rend.material.color = new UnityEngine.Color(rotor_color.r, rotor_color.g, rotor_color.b, (0.2f + (1 - rpm / 60 / 7.9f)));
            mrotor_disc_mat.color = new UnityEngine.Color(mrotor_disc_color.r, mrotor_disc_color.g, mrotor_disc_color.b, 0.2f * (rpm / 60 / 7.9f));
        }

        //rpm_tail effects
        if (rpm_tail / 60 > 7.9f)
        {
            trotor_rend.material.color = new UnityEngine.Color(rotor_color.r, rotor_color.g, rotor_color.b, 0.2f);
            trotor_disc_mat.color = new UnityEngine.Color(mrotor_disc_color.r, mrotor_disc_color.g, mrotor_disc_color.b, 0.2f);

        }
        else
        {
            trotor_rend.material.color = new UnityEngine.Color(rotor_color.r, rotor_color.g, rotor_color.b, (0.2f + (1 - rpm / 60 / 7.9f)));
            trotor_disc_mat.color = new UnityEngine.Color(mrotor_disc_color.r, mrotor_disc_color.g, mrotor_disc_color.b, 0.2f * (rpm / 60 / 7.9f));
        }

        if (CollisionDetector.crash_main_rotor) { mrotor_disc_mat.color = new UnityEngine.Color(mrotor_disc_color.r, mrotor_disc_color.g, mrotor_disc_color.b, 0); }
        /*
        if (CollisionDetectorTail.crash_tail_rotor)
        {
            trotor_disc_mat.color = new UnityEngine.Color(mrotor_disc_color.r, mrotor_disc_color.g, mrotor_disc_color.b, 0);
            trotor_rend.material.color = new UnityEngine.Color(mrotor_disc_color.r, mrotor_disc_color.g, mrotor_disc_color.b, 0);
        }
        */

        if (CollisionDetector.crash_main_rotor){mrotor_disc_mat.color = new UnityEngine.Color(mrotor_disc_color.r, mrotor_disc_color.g, mrotor_disc_color.b, 0); }
        pwm = Motor.pwm;
        text.text = "rpm="+(int)rpm+" / pwm="+pwm;
		float fps = 1.0f / deltaTime;
		fps_text.text = "FPS=" + (int)fps;
	}


}
