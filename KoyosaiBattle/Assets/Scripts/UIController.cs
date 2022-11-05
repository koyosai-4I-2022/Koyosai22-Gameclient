using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SoftGear.Strix.Unity.Runtime;
using System.Text;

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
    [SerializeField]
    GameObject StartObject;

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
    [SerializeField]
    Text[] InputSelectExpText;
    [SerializeField]
    Slider SwingGauge;
    int existingNum = 0;
    int SwingCount = 0;

    // ロード画面で使用するテキストと画像
    [SerializeField]
    Text[] LoadingText;
    [SerializeField]
    Image[] LoadingImage;
    [SerializeField]
    float TextChangeSpeed = 4.5f;

    // リザルト画面で使用するテキストと画像
    [SerializeField]
    Text[] ResultingName;
    [SerializeField]
    Text[] ResultingScore;
    [SerializeField]
    Image[] ResultingImage;
    bool isSend = false;

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

    [SerializeField]
    Camera loadCamera;
    [SerializeField]
    Camera playCamera;

    // 各処理の初期化を一度しか
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
    [SerializeField]
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

    string[] topicsStr = new string[4]
    {
        "Topics\n連続攻撃でダメージが下がる\n攻撃には緩急が大切だ！！",
        "Topics\nガード時は受けるダメージが減るが動けないので注意しろ",
        "Topics\n周囲に気を付けて楽しくプレイ！！",
        "Topics\n遊んでくれてありがとう！！\n投票もおねがいします"
    };

    private void Awake()
    {
        // データを共有するためにインスタンスを生成
        instance = this;
    }
    void Start()
    {
        // 最初にConnectionを表示する
        state = PlayState.Connection;
        InitUI();
    }

    void Update()
    {
        if(m_joycons == null || m_joycons.Count <= 0)
            return;
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.K))
		{
            playerDataClone = new PlayerData()
            {
                Name = "test",
                Score = 200,
                EnemyScore = 200,
                PlayerId = 10,
            };
            playerData.Name = "ひだ(開発者)";
            playerData.PlayerId = 808;
            playerData.Score = 200;
            playerData.EnemyScore = 200;
            isFinish = true;
		}
#endif
        switch(state)
        {
            // ゲーム画面での毎フレーム処理
            case PlayState.Playing:
                // 最初の一回目のみ実行(メソッド内でtrueに)
                if (!stateInit[0])
                    StartCoroutine(InitPlayerUI());
                UpdatePlayingUI();
                break;
            // 中断中の処理
            case PlayState.Paused:

                break;
            // 入力選択画面の処理
            case PlayState.InputSelecting:
                // 最初の一回目のみ実行(メソッド内でtrueに)
                if (!stateInit[1])
                    InitInputSelectingUI();
                UpdateInputSelectingUI();
                break;
            // ロード画面の処理
            case PlayState.Loading:
                // 最初の一回目のみ実行(メソッド内でtrueに)
                if (!stateInit[2])
                    InitLoadingUI();
                //UpdateLoadingUI();
                break;
            // リザルト画面の処理
            case PlayState.Resulting:
                //  最初の一回目のみ実行(メソッド内でtrueに)
                if (!stateInit[3])
                    InitResultingUI();
                UpdateResultingUI();
                break;
            // ランキング画面の処理
            case PlayState.Ranking:
                // 最初の一回目のみ実行(メソッド内でtrueに)
                if (!stateInit[4])
                    InitRankingUI();
                UpdateRankingUI();

                break;
            // 接続処理
            case PlayState.Connection:
                if (!stateInit[5])
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
        if (m_joycons == null || m_joycons.Count <= 0)
            return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        isJoyconButtom = true;
    }
    // ゲーム中の描画更新
    void UpdatePlayingUI()
    {
        if(playerDataClone != null && playerDataClone.HitPoint <= 0)
		{
            Debug.Log("Finish win");
            state = PlayState.Loading;
            finishFrag = true;
        }
        if (isFinish)
        {
            Debug.Log("Finish lose");
            isStart = false;
            CalcScore.instance.Timer();
            CalcScore.instance.Score();
            state = PlayState.Loading;
            finishFrag = true;
        }
    }
    // ゲーム画面の初期設定
    IEnumerator InitPlayerUI()
    {
        isStart = true;
        isFinish = false;
        finishFrag = false;
        // プレイパネルを表示それ以外を非表示
        SetPanelActives();

        loadCamera.gameObject.SetActive(false);
        playCamera.gameObject.SetActive(true);

        StartObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        StartObject.SetActive(false);

        // 初期設定したのでtrue
        stateInit[0] = true;
        stateInit[1] = false;
        stateInit[2] = false;

        // 初期化処理はここに書く
        CalcScore.instance.Timer();
        EnergyGauge.instance.InitializeEnergyGauge();
        HPGauge.instance.InitializeHPGauge();
        Timer.instance.InitializeTimer();
        AnimationEvent.instance.InitializeAnimationEvent();

        if(StrixNetwork.instance.isRoomOwner)
		{
            JoyConAttack.instance.gameObject.transform.position = Vector3.back * 20f;
            JoyConAttack.instance.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            JoyConAttack.instance.gameObject.transform.rotation = Quaternion.Euler(0, 180f, 0);
            JoyConAttack.instance.gameObject.transform.position = Vector3.forward * 20f;
		}
        playerData.PlayInit();

    }
    void FalseStartObjectActive() => StartObject.SetActive(false);

    // 待機画面の描画更新
    async void UpdateInputSelectingUI()
    {
        // InputFieldのテキストを取得
        string playerName1 = InputSelectingInputName[0].text;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(existingNum == 0)
            {
                existingNum = 1;
                InputSelectExpText[0].gameObject.SetActive(false);
                InputSelectExpText[1].gameObject.SetActive(true);
            }
            else
			{
                existingNum = 0;
                InputSelectExpText[0].gameObject.SetActive(true);
                InputSelectExpText[1].gameObject.SetActive(false);
            }
        }
        // 名前が入力されていたらフラグをtrueに
        if (!selectIsReady[0] && playerName1 != string.Empty)
        {
            selectIsReady[0] = true;
        }
        if (selectIsReady[0] && m_joyconR.GetAccel().sqrMagnitude > 50f)
        {
            SwingCount++;
            if(SwingCount > 4)
            {
                SwingGauge.value += 1;
                SwingCount = 0;
            }
            if(SwingGauge.value >= SwingGauge.maxValue)
                selectIsReady[1] = true;
        }
        // すべてのフラグがtrueでサーバにPOSTしてない時にPOST処理
        if (selectIsReady[0] && selectIsReady[1] && !selectIsSendName)
        {
            if(existingNum == 0)
            {
                // POSTを複数回連続で行わないようにtrue
                selectIsSendName = true;

                //InputSelectingInputName[0].text = String.Empty;

                int len = playerName1.Length;
                int range = len <= 8 ? len : 8;
                string postName = playerName1.Substring(0, range);
                // POST
                var result = await ServerRequestController.PostUser(postName);

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

                // Readyを黄色にする
                InputSelectingReady[0].color = new Color(1f, 0.9f, 0);

                playerData.SetUser(result.name, result.id);
                playerData.Name = result.name;
                playerData.PlayerId = result.id;

                InputSelectingInputName[0].interactable = false;
                selectIsReceive = true;
            }
            else
            {
                // POSTを複数回連続で行わないようにtrue
                selectIsSendName = true;

                var result = await ServerRequestController.PostExstingUser(playerName1);

                if(result.id == -1)
                {
                    Debug.Log("Faild");
                    // すでに登録された名前だった場合にもう一度入力する
                    selectIsSendName = false;
                    InputSelectingInputName[0].text = string.Empty;
                    selectIsReady[0] = false;
                    selectIsReady[1] = false;
                    return;
                }
                // Readyを黄色にする
                InputSelectingReady[0].color = new Color(1f, 0.9f, 0);


                playerData.SetUser(result.name, result.id);
                playerData.Name = result.name;
                playerData.PlayerId = result.id;
                Debug.Log($"{result.id}:{result.name}");
                Debug.Log($"{playerData.PlayerId}:{playerData.Name}");

                InputSelectingInputName[0].interactable = false;
                selectIsReceive = true;
            }
        }
        // 相手の名前を取得を確認
        if (selectIsReceive) // 自分のデータをPOST済み
        {
            if (playerDataClone != null) // 接続が完了して同期出来ている
            {
                if (playerData.PlayerId != -1 && playerData.Name != string.Empty) // NameとIDが登録されている
                { // 
                    if (playerDataClone.PlayerId != -1 && playerDataClone.Name != string.Empty)
                    { // 同期されたNameとIDが登録されている

                        // ゲーム画面への遷移
                        // 本来はInputSelecting->Loading->Playingの順に遷移
                        //state = PlayState.Resulting;
                        state = PlayState.Loading;
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
        isSend = false;

        playerData.Init();

        SwingGauge.value = 0;
        ConflictId = -1;
        ConflictName = string.Empty;

        InputSelectingInputName[0].text = "";

        // リザルトパネルを表示それ以外を非表示
        SetPanelActives();

        // Readyを白に
        InputSelectingReady[0].color = new Color(1f, 1f, 1f);

        // ランキングを取得
        var result = await ServerRequestController.GetRanking();

        InputSelectingInputName[0].interactable = true;

        for (int i = 0; i < 3; i++)
        {
            InputSelectRankingName[i].text = $"{result.Users[i].name}";
            InputSelectRankingScore[i].text = $"{result.Users[i].rate}";
        }

        loadCamera.gameObject.SetActive(true);
        playCamera.gameObject.SetActive(false);
    }

    // ロード画面の描画更新
    IEnumerator UpdateLoadingUI()
    {
        if(!finishFrag)
        {
            // TextChangeSpeedの秒数後にテキストを変更
            LoadingText[0].text = "Now Loading";
            LoadingText[1].text = topicsStr[0];
            yield return new WaitForSeconds(TextChangeSpeed);
            LoadingText[0].text = "Now Loading.";
            LoadingText[1].text = topicsStr[1];
            yield return new WaitForSeconds(TextChangeSpeed);
            LoadingText[0].text = "Now Loading..";
            LoadingText[1].text = topicsStr[2];
            yield return new WaitForSeconds(TextChangeSpeed);
            FinishLoading(stateInit[1]);
        }
        else
        {
            // TextChangeSpeedの秒数後にテキストを変更
            LoadingText[0].text = "Now Loading";
            LoadingText[1].text = topicsStr[3];
            yield return new WaitForSeconds(TextChangeSpeed);
            LoadingText[0].text = "Now Loading.";
            yield return new WaitForSeconds(TextChangeSpeed);
            LoadingText[0].text = "Now Loading..";
            yield return new WaitForSeconds(TextChangeSpeed);
            FinishLoading(stateInit[1]);
        }
    }
    // ロード画面の初期設定
    void InitLoadingUI()
    {
        // 初期設定したのでtrue
        stateInit[2] = true;

        // リザルトパネルを表示それ以外を非表示
        SetPanelActives();

        loadCamera.gameObject.SetActive(true);
        playCamera.gameObject.SetActive(false);

        AnimationEvent.instance.InitializeAnimationEvent();

        // コルーチン(非同期処理)を実行
        StartCoroutine(nameof(UpdateLoadingUI));
    }
    // ロード画面の終了処理
    void FinishLoading(bool isPlayLoad)
    {
        if(isPlayLoad)
        {
            state = PlayState.Playing;
        }
        else
        {
            if(finishFrag)
            {
                state = PlayState.Resulting;
            }
            else
            {
                state = PlayState.InputSelecting;
            }
        }

    }

    // リザルトの描画更新
    void UpdateResultingUI()
	{
		{
            
        }

        // Aボタンを押したときにRankingに遷移
        if (isJoyconButtom && m_joyconR.GetButtonDown(m_buttons[1]))
        {
            state = PlayState.Ranking;
            isJoyconButtom = false;
            Invoke(nameof(InvokeTransOffset), 1f); // 4秒後にisJoyconButtonをtrueにしてボタンに反応するように
        }
    }
    // リザルト画面の初期設定
    async void InitResultingUI()
    {
        // 初期設定したのでtrue
        stateInit[3] = true;

        // リザルトパネルを表示それ以外を非表示
        SetPanelActives();

        if(isFinish)
		{
            ResultingName[0].text = playerData.Name;
            ResultingName[1].text = playerDataClone.Name;

            ResultingScore[0].text = playerData.Score.ToString();
            ResultingScore[1].text = playerData.EnemyScore.ToString();

            if(playerData.Score > playerData.EnemyScore)
            {
                ResultingImage[0].gameObject.SetActive(true);
                ResultingImage[1].gameObject.SetActive(false);

                int nlen = playerData.Name == "" ? 0 : playerData.Name.Length;
                ResultingImage[0].rectTransform.anchoredPosition = new Vector2(-50 - 50 * nlen, 200f);
            }
            else
            {
                ResultingImage[0].gameObject.SetActive(false);
                ResultingImage[1].gameObject.SetActive(true);

                int nlen = playerData.Name == "" ? 0 : playerData.Name.Length;
                ResultingImage[1].rectTransform.anchoredPosition = new Vector2(-50 - 50 * nlen, 200f);
            }
        }
        else
        {
            playerData.Score = playerDataClone.EnemyScore;
            playerData.EnemyScore = playerDataClone.Score;

			ResultingName[0].text = playerData.Name;
            ResultingName[1].text = playerDataClone.Name;

            ResultingScore[0].text = playerData.Score.ToString();
            ResultingScore[1].text = playerData.EnemyScore.ToString();

            if(playerData.Score > playerData.EnemyScore)
            {
                ResultingImage[0].gameObject.SetActive(true);
                ResultingImage[1].gameObject.SetActive(false);

                int nlen = playerData.Name == "" ? 0 : playerData.Name.Length;
                ResultingImage[0].rectTransform.anchoredPosition = new Vector2(-50 - 50 * nlen, 200f);
            }
            else
            {
                ResultingImage[0].gameObject.SetActive(false);
                ResultingImage[1].gameObject.SetActive(true);

                int nlen = playerData.Name == "" ? 0 : playerData.Name.Length;
                ResultingImage[1].rectTransform.anchoredPosition = new Vector2(-50 - 50 * nlen, 200f);
            }
        }

        // スコアをサーバへ送信
        await ServerRequestController.PostScore(playerData.Score, playerData.PlayerId);
    }
    // ランキング画面の描画更新
    void UpdateRankingUI()
    {
        // Aボタンでセレクト画面に遷移,リザルト画面を表示している時はランキングに戻す
        // isJoyconButtomで連続で遷移するのを防ぐ
        if (isJoyconButtom && m_joyconR.GetButtonDown(m_buttons[1]))
        {
            // stateInit[3]を使ってリザルト表示中はセレクトに遷移しないようにする
            if (stateInit[3])
            {
                // セレクト画面に遷移
                // ローディング画面挟む？
                state = PlayState.InputSelecting;

                // 初期化
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
                Invoke(nameof(InvokeTransOffset), 1f);
            }
        }
        // Bボタンでリザルト画面に逆遷移
        if (isJoyconButtom && m_joyconR.GetButtonDown(m_buttons[0]))
        {
            //
            resultingPanel.SetActive(true);
            rankingPanel.SetActive(false);

            // 4秒後にisJoyconButtomをtrueにしてボタンが反応するようにする
            stateInit[3] = false;
            isJoyconButtom = false;
            Invoke(nameof(InvokeTransOffset), 1f);
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
        for (int i = 0; i < 6; i++)
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

        // 順位の上から順に埋めていく
        int RankCount = 0;
        int rangeHigh = higherCount;
        if(higherCount > 2)
            rangeHigh = 2;
        for(int i = rangeHigh - 1;i >= 0; i--)
		{
            RankingAroundRank[RankCount].text = result2.higher_around_rank_users[higherCount - i - 1].rank.ToString();
            RankingAroundName[RankCount].text = result2.higher_around_rank_users[higherCount - i - 1].name;
            RankingAroundScore[RankCount].text = result2.higher_around_rank_users[higherCount - i - 1].rate.ToString();
            RankingAroundRank[RankCount].color = Color.white;
            RankingAroundName[RankCount].color = Color.white;
            RankingAroundScore[RankCount].color = Color.white;
            RankCount++;
        }
        // 自分の順位と名前、スコアを真ん中に表示[2]
        RankingAroundRank[RankCount].text = result2.self.rank.ToString();
        RankingAroundName[RankCount].text = result2.self.name;
        RankingAroundScore[RankCount].text = result2.self.rate.ToString();
        RankingAroundRank[RankCount].color = new Color(1f, 176f / 255f, 0f);
        RankingAroundName[RankCount].color = new Color(1f, 176f / 255f, 0f);
        RankingAroundScore[RankCount].color = new Color(1f, 176f / 255f, 0f);
        RankCount++;

        for(int i = 0; i < lowerCount; i++)
        {
            RankingAroundRank[RankCount].text = result2.lower_around_rank_users[i].rank.ToString();
            RankingAroundName[RankCount].text = result2.lower_around_rank_users[i].name;
            RankingAroundScore[RankCount].text = result2.lower_around_rank_users[i].rate.ToString();
            RankingAroundRank[RankCount].color = Color.white;
            RankingAroundName[RankCount].color = Color.white;
            RankingAroundScore[RankCount].color = Color.white;
            RankCount++;
            if(RankCount >= 5)
                break;
        }

        //// 周辺順位の表示は上下2人ずつなので数が2以上なら2に2つ未満なら数を入れる
        //// 各TEXT配列は[0-1]上位、[2]自分、[3-4]下位の構成
        //for (int i = 0; i < (higherCount < 2 ? higherCount : 2); i++)
        //{
        //    RankingAroundRank[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].rank.ToString();
        //    RankingAroundName[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].name;
        //    RankingAroundScore[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].rate.ToString();
        //}
        //// 下位
        //for (int j = 0; j < (lowerCount < 2 ? lowerCount : 2); j++)
        //{
        //    // 配列の引数に[3-4]を入れるために3を足す
        //    RankingAroundRank[3 + j].text = result2.lower_around_rank_users[j].rank.ToString();
        //    RankingAroundName[3 + j].text = result2.lower_around_rank_users[j].name;
        //    RankingAroundScore[3 + j].text = result2.lower_around_rank_users[j].rate.ToString();
        //}
    }
    void UpdateConnectionUI()
    {

    }
    void InitConnectionUI()
    {
        stateInit[5] = true;

        loadCamera.gameObject.SetActive(true);
        playCamera.gameObject.SetActive(false);
        // 接続画面以外を非表示
        SetPanelActives();
    }
    // 時間差の実行用
    void InvokeTransOffset() => isJoyconButtom = true;

    void SetPanelActives()
    {
        playingPanel.SetActive(state == PlayState.Playing);
        inputSelectingPanel.SetActive(state == PlayState.InputSelecting);
        RoadingPanel.SetActive(state == PlayState.Loading);
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
        Loading = 2,
        InputSelecting = 3,
        Resulting = 4,
        Ranking = 5,
        Connection = 6
    }
}