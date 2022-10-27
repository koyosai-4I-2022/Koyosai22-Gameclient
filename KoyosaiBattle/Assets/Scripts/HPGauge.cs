using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    //SerializeFieldでInspectorからSliderを操作
    [SerializeField]
    Slider slider;

    //SliderのMaxValueと同じにする
     public int maxHp = 100;

    //時間経過を表す変数
    private float timer;

    //関数の参照
    public static HPGauge instance;
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
        //SliderのValueを初期化
        slider.value = maxHp;
        UIController.instance.playerData.HitPoint = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        //現在のHitPointをsliderに適用
        slider.value = UIController.instance.playerData.HitPoint;
    }

    //被ダメージ関数
    /*public void Damage()
    {
        if (slider.value > 0)
        {
            UIController.instance.playerData.HitPoint -= 10;
            slider.value = UIController.instance.playerData.HitPoint;
        }
    }*/
}
