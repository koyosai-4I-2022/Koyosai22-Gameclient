using SoftGear.Strix.Unity.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    // 同期用
    [SerializeField]
    StrixReplicator replicator;

    [SerializeField]
    float moveSpeed = 10f;

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

    private float RunTime;

    //gurdとrunの状態を表す変数
    public bool guard;
    public bool run;

    public GameObject shieldSelf;
    public GameObject shieldEnemy;

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
        Animator.enabled = false;

        //時間の初期化
        RunTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!replicator.isLocal)
            return;

        //Playing状態でない時、実行しない
        if (UIController.instance.state != UIController.PlayState.Playing)
        {
            return;
        }
        if(!UIController.instance.isStart)
            return;

        UIController.instance.playerData.isGuard = guard;

        //Animatorを再生
        if (!Animator.enabled)
            Animator.enabled = true;

        //JoyconLib
        if (m_joycons == null || m_joycons.Count <= 0) return;
        float[] Lstick = m_joyconL.GetStick();
        float[] Rstick = m_joyconR.GetStick();

        if(shieldSelf != null)
            shieldSelf.SetActive(UIController.instance.playerData.isGuard);
        if(shieldEnemy != null)
            shieldEnemy.SetActive(UIController.instance.playerDataClone.isGuard);


        //エネルギーが3以上あるとき
        if (EnergyGauge.instance.CanUse(3))
        {
            //ZLボタンを押したとき
            if (m_joyconL.GetButtonDown(m_buttons[12])&&!guard)
            {
                guard = true;
                //エネルギーを3使用する
                EnergyGauge.instance.EnergyLoss(3);
            }
        }

        //ZLボタンを離したとき
        if (m_joyconL.GetButtonUp(m_buttons[12]))
        {
            //ShieldDisplay.instance.Destroy();
            guard = false;
        }

        //エネルギーが2以上あるとき
        if (!guard && EnergyGauge.instance.CanUse(2))
        {
            
            //Lボタンを押したとき
            if (m_joyconL.GetButtonDown(m_buttons[11])&&!run)
            {
                if (Lstick[0] != 0 || Lstick[1] != 0)
                {
                    run = true;
                    //エネルギーを2使用する
                    EnergyGauge.instance.EnergyLoss(2);
                }
            }
        }

        //Lボタンを離したとき
        if (m_joyconL.GetButtonUp(m_buttons[11]))
        {
            run = false;
        }

        if(RunTime >= 0.2)
        {
            run = false;
            RunTime = 0;
        }

        //右スティックでPlayerの向きを変える
        transform.Rotate(new Vector3(0, Rstick[0] * 5, 0));

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
                    if (run)
                        Animator.SetBool("run1", true);
                    //前に歩く
                    else
                    {
                        Animator.SetBool("walk1", true);
                    }
                }

            }

            //斜め左前
            if (degree * 180 / Mathf.PI <= -15 && degree * 180 / Mathf.PI >= -75)
            {
                Animator.SetBool("Jog Forward Diagonal",true);
            }

            //Walk Left
            if (degree * 180 / Mathf.PI < -75 && degree * 180 / Mathf.PI > -105)
            {
                Animator.SetBool("strafe2",true);
            }

            //斜め左後
            if (degree * 180 / Mathf.PI <= -105 && degree * 180 / Mathf.PI >= -165)
            {
                Animator.SetBool("Jog Backward Diagonal",true);
            }

            //Walk Back
            if (degree * 180 / Mathf.PI < -165 || degree * 180 / Mathf.PI > 165)
            {
                if (run)
                    Animator.SetBool("run2", true);
                else
                {
                    Animator.SetBool("walk2", true);
                }
            }

            //斜め右後
            if (degree * 180 / Mathf.PI >= 105 && degree * 180 / Mathf.PI <= 165)
            {
                Animator.SetBool("Jog Backward Diagonal (1)",true);
            }

            //Walk Right
            if (degree * 180 / Mathf.PI > 75 && degree * 180 / Mathf.PI < 105)
            {
                Animator.SetBool("strafe1",true);
            }

            //斜め右前
            if (degree * 180 / Mathf.PI >= 15 && degree * 180 / Mathf.PI <= 75)
            {
                Animator.SetBool("Jog Forward Diagonal (1)",true);
            }

            //左スティックを倒しているとき
            if (Lstick[0] != 0 || Lstick[1] != 0)
            {
                MotionStrict();//動ける距離を制限
                //Playerがスティックを倒した方向に進む
                Vector3 vector = new Vector3(Mathf.Sin(degree), 0, Mathf.Cos(degree));
                if (run)
                {
                    RunTime += Time.deltaTime;
                    //Playerがスティックを倒した方向に進む
                    transform.Translate(vector * Time.deltaTime * 6f * moveSpeed);
                }
                transform.Translate(vector * Time.deltaTime * moveSpeed);
            }
        }
        if(UIController.instance.playerData.HitPoint <= 0)
        {
            Animator.SetBool("death1",true);
            UIController.instance.isFinish = true;
        }
    }

    private void MotionStrict()
    {
        float posx = transform.position.x;//x座標
        float posz = transform.position.z;//z座標

        float absposx = Mathf.Abs(posx);//xの絶対値

        int repos = 2; //外に出たときにどれだけ戻すか

        if (absposx > 105 )
        {
            transform.position += Vector3.left * repos * Mathf.Sign(posx);
        }

        if(posz > 321)
        {
            transform.position += Vector3.back * repos * Mathf.Sign(posz);
        }
        else if(posz < -161)
        {
            transform.position += Vector3.back * repos * Mathf.Sign(posz);
        }
    }
}
