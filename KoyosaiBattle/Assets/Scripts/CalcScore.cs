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

    float GameTimeMax = 180f;

    public static CalcScore instance;

	private void Awake()
	{
		instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        //AHP = UIController.instance.playerData.HitPoint;
        //BHP = UIController.instance.playerDataClone.HitPoint;
        //UIController.instance.playerData.Score = 0;
        //UIController.instance.playerDataClone.Score = 0;
        beingMeasured = false;
        TimeRate = 1.2f;
        HPRate = 1.4f;
        DamegeRate = 1.5f;
        SurvivalBonus = 150;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Timer()
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
        // HPを取得してスコア計算する
        AHP = UIController.instance.playerData.HitPoint;
        BHP = UIController.instance.playerDataClone.HitPoint;
        // スコア＝生存時間＋残りHP＋与えたダメージ
        // 生存ボーナス：100pt, 撃破ボーナス:100pt
        if (AHP > BHP)  // Aが勝者
        {
            UIController.instance.playerData.Score = ( int ) (      //playerAのスコア(勝者)
                ( (GameTimeMax - elapsedTime) * TimeRate )          //速く倒すとスコアアップ
                + ( AHP * HPRate )                                  //体力が多く残っているとスコアアップ
                + ( ( hpgauge.maxHp - BHP ) * DamegeRate ) );       //敵に多くダメージを与えるとスコアアップ
            UIController.instance.playerDataClone.Score = ( int ) ( //playerAのスコア(敗者)
                ( ( GameTimeMax - elapsedTime ) * TimeRate )        //速く倒すとスコアアップ
                + ( BHP * HPRate )                                  //体力が多く残っているとスコアアップ
                + ( ( hpgauge.maxHp - AHP ) * DamegeRate ) );       //敵に多くダメージを与えるとスコアアップ
            if (BHP == 0)
            {
                //PlayerA撃破ボーナス
                UIController.instance.playerData.Score += KillBonus;
            }
            else
			{
                //PlayerB生存ボーナス
                UIController.instance.playerDataClone.Score += SurvivalBonus;
            }
            //PlayerA生存ボーナス
            UIController.instance.playerData.Score += SurvivalBonus;
        }
        else if (AHP < BHP)
        {
            UIController.instance.playerData.Score = ( int ) (
                ( ( GameTimeMax - elapsedTime ) * TimeRate )
                + ( AHP * HPRate )
                + ( ( hpgauge.maxHp - BHP ) * DamegeRate ) );
            UIController.instance.playerDataClone.Score = ( int ) ( 
                ( (GameTimeMax - elapsedTime) * TimeRate )
                + ( BHP * HPRate )
                + ( ( hpgauge.maxHp - AHP ) * DamegeRate ) );
            if(AHP == 0)
            {
                //PlayerB撃破ボーナス
                UIController.instance.playerDataClone.Score += KillBonus;
            }
            else
            {
                //PlayerA生存ボーナス
                UIController.instance.playerData.Score += SurvivalBonus;
            }
            //PlayerB生存ボーナス
            UIController.instance.playerDataClone.Score += SurvivalBonus;
        }
        else
        {
            //引き分けは２００固定
            UIController.instance.playerData.Score += 200;
            UIController.instance.playerDataClone.Score += 200;
        }
    }
}