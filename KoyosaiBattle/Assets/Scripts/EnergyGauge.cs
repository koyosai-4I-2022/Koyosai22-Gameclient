using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGauge : MonoBehaviour
{
    //SerializeFieldでInspectorからSliderを操作
    [SerializeField]
    Slider energy;

    //SliderのMaxValueと同じにする
    public int maxEnergy = 200;

    //時間経過を表す変数
    private float timer;

    //1秒間のエネルギー損失度
    public int LossPerSec;

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

        LossPerSec = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            EnergyLossPerSec(10);
        }

        if (Input.GetKeyUp(KeyCode.Space))
            EnergyLossPerSec(1);

        /*
        if (PlayerMotion.instance.run)
        {
            EnergyLossPerSec(5);
        }
        */

        //Energyが減る部分
        timer += Time.deltaTime;
        if (timer * LossPerSec >= 1)
        {
            energy.value -= 1;
            timer = 0;
        }
    }

    public void EnergyLossPerSec (int i)
    {
        if (energy.value > 0)
        {
            LossPerSec = i;
        }
    }
}
