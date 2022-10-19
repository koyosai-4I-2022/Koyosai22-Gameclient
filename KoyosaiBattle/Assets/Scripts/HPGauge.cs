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

    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        //SliderのValueを初期化
        slider.value = maxHp;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Spaceを押したらダメージを受けてHPが減る
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(10);
        }

    }

    //被ダメージ関数
    void Damage(int damage)
    {
        if(slider.value > 0)
            slider.value -= damage;
    }
}
