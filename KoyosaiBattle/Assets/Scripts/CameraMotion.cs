using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//subarus
public class CameraMotion : MonoBehaviour
{
    //JoyconLibの変数
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;

    // Start is called before the first frame update
    void Start()
    {
        //JoyconLibの初期化
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    // Update is called once per frame
    void Update()
    {
        //右スティックで向きを変える
        float[] Rstick = m_joyconR.GetStick();
        //確認用を外したときに実行させる
        //transform.Rotate(0, Rstick[0] * 3, 0);
    }
    
    //カメラの角度を得る関数
    public float getCamera_deg()
    {
        float camdeg = Camera.main.transform.localEulerAngles.y;
        return camdeg;
    }
}
