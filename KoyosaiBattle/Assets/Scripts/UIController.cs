using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SoftGear.Strix.Unity.Runtime;

public class UIController : MonoBehaviour
{
    // 現在の状態
    // 0 ゲーム中
    // 1 中断中
    // 2 ロード中
    // 3 待機画面
    // 4 リザルト
    // 5 ランキング
    // 6 接続
    [NonSerialized]
    public PlayState state;

    public static UIController instance;

    // 接続確認用パネル
    [SerializeField]
    GameObject connectionPanel;
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
    Image[] InputSelectingReady;
    [SerializeField]
    Text[] InputSelectRankingName;
    [SerializeField]
    Text[] InputSelectRankingScore;
    [SerializeField]
    Image[] InputSelectingImage;
    [SerializeField]
    InputField[] InputSelectingInputName;

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

    bool[] selectIsReady;
    bool selectIsSendName;
    bool selectIsReceive;

    bool isJoyconButtom;
    
    int ConflictId;
    string ConflictName;

    [SerializeField]
    public PlayerData playerData;
    [NonSerialized]
    public PlayerData playerDataClone;

    //JoyconLibの変数
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    //関数の参照
    //public static UIController UIinstance;
    //public void Awake()
    //{
    //    if (UIinstance == null)
    //    {
    //        UIinstance = this;
    //    }
    //}

    // これをゲーム開始時にTrueにする
    public bool isStart = false;
    // これをゲーム終了時にTrueにする
    public bool isFinish = false;

    private void Awake()
    {
        // データを共有するためにインスタンスを生成
        instance = this;
    }
    void Start()
    {
        state = PlayState.Connection;
        InitUI();
    }

    async void Update()
    {
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
            // 接続処理
            case PlayState.Connection:
                if(!stateInit[5])
                    InitConnectionUI();
                UpdateConnectionUI();
                break;
            default:
                break;
        }
    }
    // UIの初期設定
    void InitUI()
	{
        // 各処理の初期設定用の真偽型
        stateInit = new bool[6];

        // joyconの設定
        m_joycons = JoyconManager.Instance.j;
        if(m_joycons == null || m_joycons.Count <= 0)
            return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        isJoyconButtom = true;
    }
    // ゲーム中の描画更新
    void UpdatePlayingUI()
	{
        if(isFinish)
        {
            CalcScore.instance.Score();
            state = PlayState.Resulting;
        }
	}
    // ゲーム画面の初期設定
    void InitPlayerUI()
    {
        // 初期設定したのでtrue
        stateInit[0] = true;

        // 初期化処理はここに書く


        // プレイパネルを表示それ以外を非表示
        SetPanelActives();
    }

    // 待機画面の描画更新
    async void UpdateInputSelectingUI()
	{
        // InputFieldのテキストを取得
        string playerName1 = InputSelectingInputName[0].text;

        // debugで使用あとで消す
        if(Input.GetKeyDown(KeyCode.K))
        {
            selectIsReady[1] = true;
        }
        // 名前が入力されていたらフラグをtrueに
        if(!selectIsReady[0] && playerName1 != string.Empty)
		{
            selectIsReady[0] = true;
		}
        // すべてのフラグがtrueでサーバにPOSTしてない時にPOST処理
        if(selectIsReady[0] && selectIsReady[1] && !selectIsSendName)
        {
            // POSTを複数回連続で行わないようにtrue
            selectIsSendName = true;

            // Readyを黄色にする
            InputSelectingReady[0].color = new Color(1f,  0.9f, 0);

            InputSelectingInputName[0].text = String.Empty;

            // POST
            var result = await ServerRequestController.PostUser(playerName1);

            // すでに登録されている名前だった場合に再度入力をしてもらう
            if(result.id == -1)
            {
                // すでに登録された名前だった場合にもう一度入力する
                selectIsSendName = false;
                InputSelectingInputName[0].text = string.Empty;
                selectIsReady[0] = false;
                selectIsReady[1] = false;
                return;
            }

            playerData.SetUser(result.name, result.id);
            Debug.Log($"{playerData.PlayerId}:{playerData.Name}");

            selectIsReceive = true;
        }
        // 相手の名前を取得を確認
        if(selectIsReceive) // 自分のデータをPOST済み
		{
            if(playerDataClone != null) // 接続が完了して同期出来ている
            {
                if(playerData.PlayerId != -1 && playerData.Name != string.Empty) // NameとIDが登録されている
                { // 
                    if(playerDataClone.PlayerId != -1 && playerDataClone.Name != string.Empty) 
                    { // 同期されたNameとIDが登録されている

                        // ゲーム画面への遷移
                        // 本来はInputSelecting->Roading->Playingの順に遷移
                        state = PlayState.Resulting;
                        //state = PlayState.Roading;
                    }
                }
            }
		}
	}
    // 待機画面の初期設定
    async void InitInputSelectingUI()
    {
        // 初期設定したのでtrue
        stateInit[1] = true;
        selectIsReady = new bool[2];
        selectIsSendName = false;
        selectIsReceive = false;

        ConflictId = -1;
        ConflictName = string.Empty;

        // リザルトパネルを表示それ以外を非表示
        SetPanelActives();

        // Readyを白に
        InputSelectingReady[0].color = new Color(1f, 1f, 1f);

        // ランキングを取得
        var result = await ServerRequestController.GetRanking();

        for(int i = 0; i < 3;i++)
		{
            InputSelectRankingName[i].text = $"{result.Users[i].name}";
            InputSelectRankingScore[i].text = $"{result.Users[i].rate}";
		}
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

        // リザルトパネルを表示それ以外を非表示
        SetPanelActives();
    }

    // リザルトの描画更新
    void UpdateResultingUI()
    {
        // Aボタンを押したときにRankingに遷移
        if(isJoyconButtom && m_joyconR.GetButtonDown(m_buttons[1]))
		{
            state = PlayState.Ranking;
            isJoyconButtom = false;
            Invoke(nameof(InvokeTransOffset), 4f); // 4秒後にisJoyconButtonをtrueにしてボタンに反応するように
		}
    }
    // リザルト画面の初期設定
    async void InitResultingUI()
    {
        // 初期設定したのでtrue
        stateInit[3] = true;

        // リザルトパネルを表示それ以外を非表示
        SetPanelActives();

        // 自分と相手のスコアと名前を表示
        ResultingName[0].text = playerData.Name;
        ResultingScore[0].text = playerData.Score.ToString();

        ResultingName[1].text = playerDataClone.Name;
        ResultingScore[1].text = playerDataClone.Score.ToString();
        
        // スコアをサーバへ送信
        var result = await ServerRequestController.PostScore(playerData.Score, playerData.PlayerId);
    }
    // ランキング画面の描画更新
    void UpdateRankingUI()
	{
        // Aボタンでセレクト画面に遷移,リザルト画面を表示している時はランキングに戻す
        // isJoyconButtomで連続で遷移するのを防ぐ
        if(isJoyconButtom && m_joyconR.GetButtonDown(m_buttons[1]))
        {
            // stateInit[3]を使ってリザルト表示中はセレクトに遷移しないようにする
            if(stateInit[3])
            {
                // セレクト画面に遷移
                // ローディング画面挟む？
                state = PlayState.InputSelecting;

                stateInit = new bool[6];
            }
            else
            {
                // ランキングを表示、リザルトを非表示
                resultingPanel.SetActive(false);
                rankingPanel.SetActive(true);

                stateInit[3] = true;
                isJoyconButtom = false;
                // 4秒後にisJoyconButtomをtrueにしてボタンが反応するようにする
                Invoke(nameof(InvokeTransOffset), 4f);
            }
        }
        // Bボタンでリザルト画面に逆遷移
        if(isJoyconButtom && m_joyconR.GetButtonDown(m_buttons[0]))
		{
            //
            resultingPanel.SetActive(true);
            rankingPanel.SetActive(false);

            // 4秒後にisJoyconButtomをtrueにしてボタンが反応するようにする
            stateInit[3] = false;
            isJoyconButtom = false;
            Invoke(nameof(InvokeTransOffset), 4f);
        }
	}
    // ランキング画面の初期設定
    async void InitRankingUI()
	{
        // 初期設定したのでtrue
        stateInit[4] = true;

        // ランキングパネルを表示それ以外を非表示
        SetPanelActives();
        
        // ランキング上位から取得
        var result = await ServerRequestController.GetRanking();

        // ランキングを上位から8今で表示
        for(int i = 0;i < 6; i++)
		{
            RankingName[i].text = result.Users[i].name;
            RankingScore[i].text = result.Users[i].rate.ToString();
		}
        // ユーザ周辺のランキングを表示
        var result2 = await ServerRequestController.GetUserRanking(playerData.PlayerId);

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
    void UpdateConnectionUI()
	{

	}
    void InitConnectionUI()
    {
        stateInit[5] = true;

        // 接続画面以外を非表示
        SetPanelActives();
    }
    // 時間差の実行用
    void InvokeTransOffset() => isJoyconButtom = true;

    void SetPanelActives()
	{
        playingPanel.SetActive(state == PlayState.Playing);
        inputSelectingPanel.SetActive(state == PlayState.InputSelecting);
        RoadingPanel.SetActive(state == PlayState.Roading);
        rankingPanel.SetActive(state == PlayState.Ranking);
        resultingPanel.SetActive(state == PlayState.Resulting);
        connectionPanel.SetActive(state == PlayState.Connection);
    }

    // 現在の状態を表す列挙型
    // 0 ゲーム中
    // 1 中断中
    // 2 ロード中
    // 3 待機画面
    // 4 リザルト
    // 5 ランキング
    // 6 接続
    public enum PlayState
	{
        Playing = 0,
        Paused = 1,
        Roading = 2,
        InputSelecting = 3,
        Resulting = 4,
        Ranking = 5,
        Connection = 6
	}
}