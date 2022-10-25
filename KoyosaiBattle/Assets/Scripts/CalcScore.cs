using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CalcScore : MonoBehaviour
{
    [SerializeField]
    HPGauge hpgauge;
    GameFinish gamefinish;

    float RemainTime;
    int AHitPoint;
    int BHitPoint;
    int ScoreA;
    int ScoreB;

    // Start is called before the first frame update
    void Start()
    {
        RemainTime = gamefinish.TimeLimit;
        AHitPoint = hpgauge.maxHp;
        BHitPoint = hpgauge.maxHp;
        ScoreA = 0;
        ScoreB = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!GameFinish == True)
        {
            RemainTime -= Time.deltaTime;
        }
        */
    }

    public void Score()
    {
        // スコア＝生存時間＋残りHP＋与えたダメージ
        if (AHitPoint > BHitPoint)
        {
            ScoreA = (int)gamefinish.TimeLimit + AHitPoint + (hpgauge.maxHp-BHitPoint);
            ScoreB = (int)(gamefinish.TimeLimit-RemainTime) + BHitPoint + (hpgauge.maxHp-AHitPoint);
            if (BHitPoint == 0)
            {
                ScoreA += 200; //生存ボーナス+撃破ボーナス
            }
        }
        else if (AHitPoint < BHitPoint)
        {
            ScoreA = (int)(gamefinish.TimeLimit-RemainTime) + AHitPoint + (hpgauge.maxHp-BHitPoint);
            ScoreB = (int)gamefinish.TimeLimit + BHitPoint + (hpgauge.maxHp-AHitPoint);
            if (AHitPoint == 0)
            {
                ScoreB += 200; //生存ボーナス+撃破ボーナス
            }
        }
        else
        {
            if (!(AHitPoint==0))
            ScoreA += 100; //生存ボーナス
            ScoreB += 100; //生存ボーナス
        }
    }
}