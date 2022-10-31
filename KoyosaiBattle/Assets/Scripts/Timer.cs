using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //Textで時間を表示
    [SerializeField]
    Text text;

    //1秒をはかるため
    private float timer;
    
    //時間経過のカウント
    [SerializeField]
    private int count;

    //instanceで参照するため
    public static Timer instance;
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
        //初期化
        InitializeTimer();
    }

    // Update is called once per frame
    void Update()
    {
        //Playing状態でない時、実行しない
        if (UIController.instance.state != UIController.PlayState.Playing)
        {
            return;
        }

        //1秒に1ずつカウントが減る
        timer += Time.deltaTime;
        if (timer >= 1 && count > 0)
        {
            count--;
            timer = 0;
        }
        //Textでcountを表示する
        text.text = string.Format("{0:00}",count) ; 
    }

    public void InitializeTimer()
    {
        //初期化
        timer = 0;
        count = 180;
    }
}
