using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class Attack : MonoBehaviour
{
    // 同期用
    [SerializeField]
    public StrixReplicator replicator;

    public static Attack instance;

    //JoyconLibの変数
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    //private Joycon m_joyconL;
    private Joycon m_joyconR;

    //Animatorの変数
    public Animator animator;
    public int para = 0;
    private Vector3 v1 = new Vector3(0.9f, 0.5f, 0.9f);
    private Vector3 accel, accelBefore;
    bool slashBool = false;

    //コルーチンで時間制御する
    private float interval = 1.5f;
    private float tmpTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        //JoyconLibの初期化
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        //m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        //Animatorの初期化
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // 複製されたゲームオブジェクトの場合処理を行わない
        if(!replicator.isLocal)
            return;
        */

        if (m_joycons == null || m_joycons.Count <= 0) return;

        

        //コルーチンで受付時間を制限する
        tmpTime += Time.deltaTime;

        if (tmpTime >= interval)
        {
            tmpTime = 0f;

            foreach (var joycon in m_joycons)
            {
                accel = joycon.GetAccel();
            }

            var m = accel.sqrMagnitude;
            if(m > 2f) Debug.Log($"{accel}:{m}");
            accelBefore = accel;

            if (m > 2f)// || accel.z >= v1.z)
            {
                /*if (para <= 5)
                {
                    para++;
                    animator.SetInteger("slash1", para);
                }*/
                slashBool = !slashBool;
                // 冗長
                //if (slashBool == false)
                //{
                //    slashBool = !slashBool;
                //}
                //else
                //{
                //    slashBool = !slashBool;
                //}
                m_joyconR.SetRumble(160, 320, 0.6f, 200);
                animator.SetBool("slash2", slashBool);

                
            }
            if (para > 4)
            {
                para = 0;
            }
            
        }
        
        
    }
}