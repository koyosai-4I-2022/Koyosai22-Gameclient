using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMotion : MonoBehaviour
{
    //JoyconLibの変数
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> joycons;
    //private Joycon joyconL;
    private Joycon joyconR;

    //Animatorの変数
    public Animator animator;
    private Vector3 accel;
    private Vector3 v1 = new Vector3(1.0f,1.0f,1.0f);
    private int para = 0;

    // Start is called before the first frame update
    void Awake()
    {
        //Joy-Conの初期化
        //JoyconLibの初期化
        joycons = JoyconManager.Instance.j;
        if (joycons == null || joycons.Count <= 0) return;

        joyconR = joycons.Find( c => !c.isLeft );

		animator = GetComponent <Animator> ();
        animator.SetInteger("slash1", para);
    }

    // Update is called once per frame
    void Update()
    {
        if ( joycons == null || joycons.Count <= 0 ) return;

        foreach (var joycon in joycons)
        {
            accel = joycon.GetAccel();
        }

        if (accel.x >= v1.x && accel.y >= v1.y && accel.z >= v1.z){
                if(para <= 5) {
                    para++;
                    animator.SetInteger("slash1", para);
                }
        }
        if (para > 5){
            para = 0;
        }
    }
}
