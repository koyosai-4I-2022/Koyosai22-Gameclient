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
        
    }

    // Update is called once per frame
    void Update()
    {
        //1秒で1ずつHPが減る
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            Damage(1);
            timer = 0;
        }
    }

    //被ダメージ関数
    public void Damage(int damage)
    {
        if(slider.value > 0)
            slider.value -= damage;
    }
}
