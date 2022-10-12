using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //JoyconLib‚Ì•Ï”
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    //private Joycon m_joyconL;
    private Joycon m_joyconR;

    //Animator‚Ì•Ï”
    public Animator animator;
    public int para = 0;
    private Vector3 v1 = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 accel;

    // Start is called before the first frame update
    void Start()
    {
        //JoyconLib‚Ì‰Šú‰»
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        //m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        //Animator‚Ì‰Šú‰»
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var joycon in m_joycons)
        {
            accel = joycon.GetAccel();
        }

        if (accel.x >= v1.x && accel.y >= v1.y && accel.z >= v1.z)
        {
            if (para <= 5)
            {
                para++;
                animator.SetInteger("slash1", para);
            }
        }
        if (para > 5)
        {
            para = 0;
        }
    }
}