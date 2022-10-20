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
    Text[] ResultingName;
    [SerializeField]
    Text[] ResultingScore;
    [SerializeField]
    Image[] ResultingImage;

    // ランキング画面で使用するテキストと画像
    [SerializeField]
    Text[] RankingName;
    [SerializeField]
    Text[] RankingScore;
    [SerializeField]
    Text[] RankingAroundRank;
    [SerializeField]
    Text[] RankingAroundName;
    [SerializeField]
    Text[] RankingAroundScore;
    [SerializeField]
    Image[] RankingImage;

    bool[] stateInit;
    bool finishFrag = false;

    void Start()
    {
        state = PlayState.Resulting;
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
                // 最初の一回目のみ実行(メソッド内でtrueに)
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
        // 初期設定したのでtrue
        stateInit[0] = true;
	}

    // 待機画面の描画更新
    void UpdateInputSelectingUI()
	{

	}
    // 待機画面の初期設定
    void InitInputSelectingUI()
    {
        // 初期設定したのでtrue
        stateInit[1] = true;
	}

    // ロード画面の描画更新
    void UpdateRoadingUI()
	{

	}
    // ロード画面の初期設定
    void InitRoadingUI()
    {
        // 初期設定したのでtrue
        stateInit[2] = true;

	}

    // リザルトの描画更新
    void UpdateResultingUI()
    {

    }
    // リザルト画面の初期設定
    async void InitResultingUI()
    {
        // 初期設定したのでtrue
        stateInit[3] = true;

        // リザルトパネルを表示それ以外を非表示
        SetPanelActives(PlayState.Ranking);

        // 
        foreach(var dic in PlayerData.DictionaryID)
        {
            var result = await ServerRequestController.GetScore(dic.Value);
        }
    }

    // ランキング画面の描画更新
    void UpdateRankingUI()
	{
        if(finishFrag)
		{
            state = PlayState.Roading;
		}
	}
    // ランキング画面の初期設定
    async void InitRankingUI()
	{
        // 初期設定したのでtrue
        stateInit[4] = true;

        // ランキングパネルを表示それ以外を非表示
        SetPanelActives(PlayState.Ranking);
        
        // ランキング上位から取得
        var result = await ServerRequestController.GetRanking();

        // ランキングを上位から8今で表示
        for(int i = 0;i < 8; i++)
		{
            RankingName[i].text = result.Users[i].name;
            RankingScore[i].text = result.Users[i].rate.ToString();
		}
        // ユーザ周辺のランキングを表示
        var result2 = await ServerRequestController.GetUserRanking(PlayerData.UserId);

        // 自分より上の順位の人の数
        int higherCount = result2.higher_around_rank_users.Length;
        // 自分より下の順位の人の数
        int lowerCount = result2.lower_around_rank_users.Length;

        // 周辺順位の表示は上下2人ずつなので数が2以上なら2に2つ未満なら数を入れる
        // 各TEXT配列は[0-1]上位、[2]自分、[3-4]下位の構成
        for(int i = 0; i < (higherCount < 2 ? higherCount : 2); i++)
		{
            RankingAroundRank[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].rank.ToString();
            RankingAroundName[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].name;
            RankingAroundScore[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].rate.ToString();
        }
        // 下位
        for(int j = 0; j < (lowerCount < 2 ? lowerCount : 2); j++)
		{
            // 配列の引数に[3-4]を入れるために3を足す
            RankingAroundRank[3 + j].text = result2.lower_around_rank_users[j].rank.ToString();
            RankingAroundName[3 + j].text = result2.lower_around_rank_users[j].name;
            RankingAroundScore[3 + j].text = result2.lower_around_rank_users[j].rate.ToString();
        }
        // 自分の順位と名前、スコアを真ん中に表示[2]
        RankingAroundRank[2].text = result2.self.rank.ToString();
        RankingAroundName[2].text = result2.self.name;
        RankingAroundScore[2].text = result2.self.rate.ToString();
    }

    void SetPanelActives(PlayState playState)
	{
        playingPanel.SetActive(state == playState);
        inputSelectingPanel.SetActive(state == playState);
        RoadingPanel.SetActive(state == playState);
        rankingPanel.SetActive(state == playState);
        resultingPanel.SetActive(state == playState);
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
