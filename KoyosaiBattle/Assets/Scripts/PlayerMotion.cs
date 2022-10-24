using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    //JoyconLibの変数
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    
    //Animatorの変数
    public Animator Animator;
    
    //スティックのsetting
    private float degree;

    //gurdとrunの状態を表す変数
    public bool guard;
    public bool run;

    //関数の参照
    public static PlayerMotion instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //JoyconLibの初期化
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        //Animatorの初期化
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //JoyconLib
        if (m_joycons == null || m_joycons.Count <= 0) return;
        float[] Lstick = m_joyconL.GetStick();
        float[] Rstick = m_joyconR.GetStick();

        //エネルギーが3以上あるとき
        if (EnergyGauge.instance.CanUse(3))
        {
            //ZLボタンを押したとき
            if (m_joyconL.GetButtonDown(m_buttons[12]))
            {
                guard = true;
                //エネルギーを3使用する
                EnergyGauge.instance.EnergyLoss(3);
            }
        }

        //ガード状態(ZLボタンを押している場合)
        if (guard)
        {
            //シールドの表示
            ShieldDisplay.instance.Create();
        }

        //ZLボタンを離したとき
        if (m_joyconL.GetButtonUp(m_buttons[12]))
        {
            ShieldDisplay.instance.Destroy();
            guard = false;
        }

        //エネルギーが2以上あるとき
        if (EnergyGauge.instance.CanUse(2))
        {
            //Lボタンを押したとき
            if (m_joyconL.GetButtonDown(m_buttons[11]))
            {
                run = true;
                //エネルギーを2使用する
                EnergyGauge.instance.EnergyLoss(2);
            }
        }

        //Lボタンを離したとき
        if (m_joyconL.GetButtonUp(m_buttons[11]))
        {
            run = false;
        }

        //右スティックでPlayerの向きを変える
        transform.Rotate(new Vector3(0, Rstick[0] * 3, 0));

        //左スティックの倒した向きから角度を得る
        degree = Mathf.Atan2(Lstick[0], Lstick[1]);

        //ガードしてない状態の時
        if (!guard)
        {
            //Walk Shoot Front
            if (Lstick[0] != 0 || Lstick[1] != 0)//スティックを倒しているとき
            {
                //前方への移動
                if (degree * 180 / Mathf.PI > -15 && degree * 180 / Mathf.PI < 15)
                {
                    //前に走る(Lボタンを押している場合)
                    if (m_joyconL.GetButton(m_buttons[11]))
                    {
                        Vector3 vector = new Vector3(0, 0, 1);
                        //transform.Translate(vector * Time.deltaTime * 3);
                        Animator.Play("run1");
                        //エネルギー使用率を+1する
                        //EnergyGauge.instance.EnergyLossPerSec(en + 1);
                        //run = true;
                    }
                    //前に歩く
                    else Animator.Play("walk1");
                }

            }

            //斜め左前
            if (degree * 180 / Mathf.PI <= -15 && degree * 180 / Mathf.PI >= -75)
            {
                Animator.Play("Jog Forward Diagonal");
            }

            //Walk Left
            if (degree * 180 / Mathf.PI < -75 && degree * 180 / Mathf.PI > -105)
            {
                Animator.Play("strafe2");
            }

            //斜め左後
            if (degree * 180 / Mathf.PI <= -105 && degree * 180 / Mathf.PI >= -165)
            {
                Animator.Play("Jog Backward Diagonal");
            }

            //Walk Back
            if (degree * 180 / Mathf.PI < -165 || degree * 180 / Mathf.PI > 165)
            {
                Animator.Play("walk2");
            }

            //斜め右後
            if (degree * 180 / Mathf.PI >= 105 && degree * 180 / Mathf.PI <= 165)
            {
                Animator.Play("Jog Backward Diagonal (1)");
            }

            //Walk Right
            if (degree * 180 / Mathf.PI > 75 && degree * 180 / Mathf.PI < 105)
            {
                Animator.Play("strafe1");
            }

            //斜め右前
            if (degree * 180 / Mathf.PI >= 15 && degree * 180 / Mathf.PI <= 75)
            {
                Animator.Play("Jog Forward Diagonal (1)");
            }

            //左スティックを倒しているとき
            if (Lstick[0] != 0 || Lstick[1] != 0)
            {
                if (run)
                {
                    //Playerがスティックを倒した方向に進む
                    Vector3 Runvector = new Vector3(Mathf.Sin(degree), 0, Mathf.Cos(degree));
                    transform.Translate(Runvector * Time.deltaTime * 30);
                }
                //Playerがスティックを倒した方向に進む
                Vector3 vector = new Vector3(Mathf.Sin(degree), 0, Mathf.Cos(degree));
                transform.Translate(vector * Time.deltaTime * 5);
            }
        }
    }
}
