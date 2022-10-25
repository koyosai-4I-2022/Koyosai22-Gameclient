using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CalcScore : MonoBehaviour
{
    [SerializeField]
    HPGauge hpgauge;
    // [SerializeField]
    // UIController UIController;
    GameFinish gamefinish;

    // スコア調整
    //[SerializeField]
    public float TimeRate; //生き残った時間
    //[SerializeField]
    public float HPRate; //残りHP
    //[SerializeField]
    public float DamegeRate; //与ダメージ
    //[SerializeField]
    public int SurvivalBonus; //生存ボーナス
    //[SerializeField]
    public int KillBonus; //撃破ボーナス

    private int AHP;
    private int BHP;

    // プレイ時間の計測
    private bool beingMeasured;
    private float startTime;
    private float elapsedTime;
    
    

    // Start is called before the first frame update
    void Start()
    {
        AHP = UIController.instance.playerData.HitPoint;
        BHP = UIController.instance.playerDataClone.HitPoint;
        UIController.instance.playerData.Score = 0;
        UIController.instance.playerDataClone.Score = 0;
        beingMeasured = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Timer()
    {
        // 時間計測
        if (UIController.instance.isStart == true)
        {
            startTime = Time.time;
        }
        if (UIController.instance.isFinish == true)
        {
            elapsedTime = Time.time - startTime;
            UIController.instance.isFinish = false;
        }
    }

    public void Score()
    {
        // スコア＝生存時間＋残りHP＋与えたダメージ
        // 生存ボーナス：100pt, 撃破ボーナス:100pt
        if (AHP > BHP)
        {
            UIController.instance.playerData.Score = (int)((gamefinish.TimeLimit * TimeRate) + (AHP*HPRate) + ((hpgauge.maxHp-BHP)*DamegeRate));
            UIController.instance.playerDataClone.Score = (int)(((gamefinish.TimeLimit-elapsedTime)*TimeRate) + (BHP*HPRate) + ((hpgauge.maxHp-AHP)*DamegeRate));
            if (BHP == 0)
            {
                //生存ボーナス+撃破ボーナス
                UIController.instance.playerData.Score += SurvivalBonus;
                UIController.instance.playerData.Score += KillBonus;
            }
        }
        else if (AHP < BHP)
        {
            UIController.instance.playerData.Score = (int)(((gamefinish.TimeLimit-elapsedTime)*TimeRate) + (AHP*HPRate) + ((hpgauge.maxHp-BHP)*DamegeRate));
            UIController.instance.playerDataClone.Score = (int)((gamefinish.TimeLimit*TimeRate) + (BHP*HPRate) + ((hpgauge.maxHp-AHP)*DamegeRate));
            if (AHP == 0)
            {
                //生存ボーナス+撃破ボーナス
                UIController.instance.playerData.Score += SurvivalBonus;
                UIController.instance.playerData.Score += KillBonus;
            }
        }
        else
        {
            if (!(AHP==0))
            UIController.instance.playerData.Score += SurvivalBonus; //生存ボーナス
            UIController.instance.playerDataClone.Score += SurvivalBonus; //生存ボーナス
        }
    }
}