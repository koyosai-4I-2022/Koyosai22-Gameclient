using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    //SerializeFieldでInspectorからSliderを操作
    [SerializeField]
    Slider energy;

    //色を変えるため
    [SerializeField]
    Image sliderImage;

    //SliderのMaxValueと同じにする
    public int maxEnergy = 10;

    //関数の参照
    public static EnergyGauge instance;
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
        energy.value = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMotion.instance.guard)
        {
            //Energyが増加する部分
            if (maxEnergy > energy.value)
                energy.value += Time.deltaTime;
        }
        
        //ゲージの色を変える
        if (energy.value >= 5)//エネルギーが5以上の時
            sliderImage.color = new Color32(253, 244, 1, 255);//黄色
        else sliderImage.color = new Color32(253, 161, 1, 255);//オレンジ
    }

    //エネルギー消費関数
    public void EnergyLoss (int i)
    {
        if (CanUse(i))
        {
            energy.value  -= i;
        }
    }

    //エネルギーが引数以上あるかを判断する関数
    public bool CanUse(int k)
    {
        if (energy.value - k >= 0)
            return true;
        else return false;
    }
}
