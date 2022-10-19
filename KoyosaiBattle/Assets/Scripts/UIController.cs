using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // 現在の状態
    PlayState state;

    // ゲーム中のパネル
    [SerializeField]
    GameObject playingPanel;
    // プレイヤー名入力(ゲームが始まる前の待機画面)用のパネル
    [SerializeField]
    GameObject inputSelectingPanel;
    // ロード中の表示用パネル
    [SerializeField]
    GameObject RoadingPanel;
    // リザルト表示用のパネル
    [SerializeField]
    GameObject resultingPanel;
    // ランキング表示用のパネル
    [SerializeField]
    GameObject rankingPanel;

    // ゲーム画面で使用するテキストと画像
    [SerializeField]
    Text[] PlayingText;
    [SerializeField]
    Image[] PlayingImage;

    // 待機選択画面で使用するテキストと画像
    [SerializeField]
    Text[] InputSelectingText;
    [SerializeField]
    Image[] InputSelectingImage;

    // ロード画面で使用するテキストと画像
    [SerializeField]
    Text[] RoadingText;
    [SerializeField]
    Image[] RoadingImage;

    // リザルト画面で使用するテキストと画像
    [SerializeField]
    Text[] ResultingText;
    [SerializeField]
    Image[] ResultingImage;

    // ランキング画面で使用するテキストと画像
    [SerializeField]
    Text[] RankingName;
    [SerializeField]
    Text[] RankingScore;
    [SerializeField]
    Image[] RankingImage;

    bool[] stateInit;

    void Start()
    {
        state = PlayState.Ranking;
        InitUI();
    }

    async void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
		{
            var result = await ServerRequestController.GetRanking();
            string s = "";
            foreach(var user in result.Users)
            {
                s += user.name + "\n";
            }
            Debug.Log(s);
        }
        switch(state)
        {
            // ゲーム画面での毎フレーム処理
            case PlayState.Playing:
                // 
                if(!stateInit[0])
                    InitPlayerUI();
                UpdatePlayingUI();
                break;
            // 中断中の処理
            case PlayState.Paused:
                
                break;
            // 入力選択画面の処理
            case PlayState.InputSelecting:
                // 最初の一回目のみ実行(メソッド内でtrueに)
                if(!stateInit[1])
                    InitInputSelectingUI();
                UpdateInputSelectingUI();
                break;
            // ロード画面の処理
            case PlayState.Roading:
                // 最初の一回目のみ実行(メソッド内でtrueに)
                if(!stateInit[2])
                    InitRoadingUI();
                UpdateRoadingUI();
                break;
            // リザルト画面の処理
            case PlayState.Resulting:
                //  最初の一回目のみ実行(メソッド内でtrueに)
                if(!stateInit[3])
                    InitResultingUI();
                UpdateResultingUI();
                break;
            // ランキング画面の処理
            case PlayState.Ranking:
                // 最初の一回目のみ実行(メソッド内でtrueに)
                if(!stateInit[4])
                    InitRankingUI();
                UpdateRankingUI();
                
                break;
            default:
                break;
        }
    }
    // UIの初期設定
    void InitUI()
	{
        stateInit = new bool[5];
	}

    // ゲーム中の描画更新
    void UpdatePlayingUI()
	{

	}
    // ゲーム画面の初期設定
    void InitPlayerUI()
	{
        stateInit[0] = true;
	}

    // 待機画面の描画更新
    void UpdateInputSelectingUI()
	{

	}
    // 待機画面の初期設定
    void InitInputSelectingUI()
	{
        stateInit[1] = true;
	}

    // ロード画面の描画更新
    void UpdateRoadingUI()
	{

	}
    // ロード画面の初期設定
    void InitRoadingUI()
	{
        stateInit[2] = true;
	}

    // リザルトの描画更新
    void UpdateResultingUI()
    {

    }
    // リザルト画面の初期設定
    void InitResultingUI()
	{
        stateInit[3] = true;
	}

    // ランキング画面の描画更新
    void UpdateRankingUI()
	{
        
	}
    // ランキング画面の初期設定
    async void InitRankingUI()
	{
        stateInit[4] = true;

        var result = await ServerRequestController.GetRanking();

        for(int i = 0;i < 8; i++)
		{

		}
	}


    // 現在の状態を表す列挙型
    // 0 ゲーム中
    // 1 中断中
    // 2 ロード中
    // 3 待機画面
    // 4 リザルト
    // 5 ランキング
    public enum PlayState
	{
        Playing = 0,
        Paused = 1,
        Roading = 2,
        InputSelecting = 3,
        Resulting = 4,
        Ranking = 5
	}
}
