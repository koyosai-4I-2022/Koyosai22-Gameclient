using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFinish : MonoBehaviour
{
    public float TimeLimit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeLimit -= Time.deltaTime;
        if (TimeLimit == 0)
        {
            // 制限時間になった時の終了処理
            
        }
    }

}
