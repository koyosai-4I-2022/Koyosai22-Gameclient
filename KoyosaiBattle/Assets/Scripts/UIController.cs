using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SoftGear.Strix.Unity.Runtime;

public class UIController : MonoBehaviour
{
    // ���݂̏��
    // 0 �Q�[����
    // 1 ���f��
    // 2 ���[�h��
    // 3 �ҋ@���
    // 4 ���U���g
    // 5 �����L���O
    // 6 �ڑ�
    [NonSerialized]
    public PlayState state;

    public static UIController instance;

    // �ڑ��m�F�p�p�l��
    [SerializeField]
    GameObject connectionPanel;
    // �Q�[�����̃p�l��
    [SerializeField]
    GameObject playingPanel;
    // �v���C���[������(�Q�[�����n�܂�O�̑ҋ@���)�p�̃p�l��
    [SerializeField]
    GameObject inputSelectingPanel;
    // ���[�h���̕\���p�p�l��
    [SerializeField]
    GameObject RoadingPanel;
    // ���U���g�\���p�̃p�l��
    [SerializeField]
    GameObject resultingPanel;
    // �����L���O�\���p�̃p�l��
    [SerializeField]
    GameObject rankingPanel;

    // �Q�[����ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] PlayingText;
    [SerializeField]
    Image[] PlayingImage;

    // �ҋ@�I���ʂŎg�p����e�L�X�g�Ɖ摜
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

    // ���[�h��ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] LoadingText;
    [SerializeField]
    Image[] LoadingImage;
    [SerializeField]
    float TextChangeSpeed = 1.0f;

    // ���U���g��ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] ResultingName;
    [SerializeField]
    Text[] ResultingScore;
    [SerializeField]
    Image[] ResultingImage;

    // �����L���O��ʂŎg�p����e�L�X�g�Ɖ摜
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

    // �e�����̏��������x����
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

    //JoyconLib�̕ϐ�
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    //�֐��̎Q��
    //public static UIController UIinstance;
    //public void Awake()
    //{
    //    if (UIinstance == null)
    //    {
    //        UIinstance = this;
    //    }
    //}

    // �����Q�[���J�n����True�ɂ���
    public bool isStart = false;
    // �����Q�[���I������True�ɂ���
    public bool isFinish = false;

    private void Awake()
    {
        // �f�[�^����L���邽�߂ɃC���X�^���X�𐶐�
        instance = this;
    }
    void Start()
    {
        // �ŏ���Connection��\������
        state = PlayState.Connection;
        InitUI();
    }

    void Update()
    {
        if(m_joycons == null || m_joycons.Count <= 0)
            return;
        switch(state)
        {
            // �Q�[����ʂł̖��t���[������
            case PlayState.Playing:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h���true��)
                if(!stateInit[0])
                    InitPlayerUI();
                UpdatePlayingUI();
                break;
            // ���f���̏���
            case PlayState.Paused:
                
                break;
            // ���͑I���ʂ̏���
            case PlayState.InputSelecting:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h���true��)
                if(!stateInit[1])
                    InitInputSelectingUI();
                UpdateInputSelectingUI();
                break;
            // ���[�h��ʂ̏���
            case PlayState.Loading:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h���true��)
                if(!stateInit[2])
                    InitLoadingUI();
                //UpdateLoadingUI();
                break;
            // ���U���g��ʂ̏���
            case PlayState.Resulting:
                //  �ŏ��̈��ڂ̂ݎ��s(���\�b�h���true��)
                if(!stateInit[3])
                    InitResultingUI();
                UpdateResultingUI();
                break;
            // �����L���O��ʂ̏���
            case PlayState.Ranking:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h���true��)
                if(!stateInit[4])
                    InitRankingUI();
                UpdateRankingUI();
                
                break;
            // �ڑ�����
            case PlayState.Connection:
                if(!stateInit[5])
                    InitConnectionUI();
                UpdateConnectionUI();
                break;
            default:
                break;
        }
    }
    // UI�̏����ݒ�
    void InitUI()
	{
        // �e�����̏����ݒ�p�̐^�U�^
        stateInit = new bool[6];

        // joycon�̐ݒ�
        m_joycons = JoyconManager.Instance.j;
        if(m_joycons == null || m_joycons.Count <= 0)
            return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        isJoyconButtom = true;
    }
    // �Q�[�����̕`��X�V
    void UpdatePlayingUI()
	{
        if(isFinish)
        {
            CalcScore.instance.Score();
            state = PlayState.Resulting;
        }
	}
    // �Q�[����ʂ̏����ݒ�
    void InitPlayerUI()
    {
        // �����ݒ肵���̂�true
        stateInit[0] = true;

        // �����������͂����ɏ���
        isStart = true;

        // �v���C�p�l����\������ȊO���\��
        SetPanelActives();
    }

    // �ҋ@��ʂ̕`��X�V
    async void UpdateInputSelectingUI()
	{
        // InputField�̃e�L�X�g��擾
        string playerName1 = InputSelectingInputName[0].text;

        // debug�Ŏg�p���Ƃŏ���
        //if(Input.GetKeyDown(KeyCode.K))
        //{
        //    selectIsReady[1] = true;
        //}
        // ���O�����͂���Ă�����t���O��true��
        if(!selectIsReady[0] && playerName1 != string.Empty)
		{
            selectIsReady[0] = true;
		}
        if(selectIsReady[0] && m_joyconR.GetAccel().sqrMagnitude > 4f)
		{
            selectIsReady[1] = true;
		}
        // ���ׂẴt���O��true�ŃT�[�o��POST���ĂȂ�����POST����
        if(selectIsReady[0] && selectIsReady[1] && !selectIsSendName)
        {
            // POST�𕡐���A���ōs��Ȃ��悤��true
            selectIsSendName = true;

            // Ready����F�ɂ���
            InputSelectingReady[0].color = new Color(1f,  0.9f, 0);

            InputSelectingInputName[0].text = String.Empty;

            // POST
            var result = await ServerRequestController.PostUser(playerName1);

            // ���łɓo�^����Ă��閼�O�������ꍇ�ɍēx���͂���Ă�炤
            if(result.id == -1)
            {
                // ���łɓo�^���ꂽ���O�������ꍇ�ɂ����x���͂���
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
        // ����̖��O��擾��m�F
        if(selectIsReceive) // �����̃f�[�^��POST�ς�
		{
            if(playerDataClone != null) // �ڑ����������ē����o���Ă���
            {
                if(playerData.PlayerId != -1 && playerData.Name != string.Empty) // Name��ID���o�^����Ă���
                { // 
                    if(playerDataClone.PlayerId != -1 && playerDataClone.Name != string.Empty) 
                    { // �������ꂽName��ID���o�^����Ă���

                        // �Q�[����ʂւ̑J��
                        // �{����InputSelecting->Roading->Playing�̏��ɑJ��
                        state = PlayState.Resulting;
                        //state = PlayState.Roading;
                    }
                }
            }
		}
	}
    // �ҋ@��ʂ̏����ݒ�
    async void InitInputSelectingUI()
    {
        // �����ݒ肵���̂�true
        stateInit[1] = true;
        selectIsReady = new bool[2];
        selectIsSendName = false;
        selectIsReceive = false;

        ConflictId = -1;
        ConflictName = string.Empty;

        // ���U���g�p�l����\������ȊO���\��
        SetPanelActives();

        // Ready�𔒂�
        InputSelectingReady[0].color = new Color(1f, 1f, 1f);

        // �����L���O��擾
        var result = await ServerRequestController.GetRanking();

        for(int i = 0; i < 3;i++)
		{
            InputSelectRankingName[i].text = $"{result.Users[i].name}";
            InputSelectRankingScore[i].text = $"{result.Users[i].rate}";
		}
    }

    // ���[�h��ʂ̕`��X�V
    IEnumerator UpdateLoadingUI()
    {
        // TextChangeSpeed�̕b����Ƀe�L�X�g��ύX
        LoadingText[0].text = "Now Loading";
        yield return new WaitForSeconds(TextChangeSpeed);
        LoadingText[0].text = "Now Loading.";
        yield return new WaitForSeconds(TextChangeSpeed);
        LoadingText[0].text = "Now Loading..";
        yield return new WaitForSeconds(TextChangeSpeed);
        LoadingText[0].text = "Now Loading...";
        yield return new WaitForSeconds(TextChangeSpeed);
        FinishLoading(stateInit[1]);
    }
    // ���[�h��ʂ̏����ݒ�
    void InitLoadingUI()
    {
        // �����ݒ肵���̂�true
        stateInit[2] = true;

        // ���U���g�p�l����\������ȊO���\��
        SetPanelActives();

        // �R���[�`��(�񓯊�����)����s
        StartCoroutine(nameof(UpdateLoadingUI));
    }
    // ���[�h��ʂ̏I������
    void FinishLoading(bool isPlayLoad)
	{
        if(isPlayLoad)
            state = PlayState.Playing;
        else
            state = PlayState.InputSelecting;

	}

    // ���U���g�̕`��X�V
    void UpdateResultingUI()
    {
        // A�{�^����������Ƃ���Ranking�ɑJ��
        if(isJoyconButtom && m_joyconR.GetButtonDown(m_buttons[1]))
		{
            state = PlayState.Ranking;
            isJoyconButtom = false;
            Invoke(nameof(InvokeTransOffset), 1f); // 4�b���isJoyconButton��true�ɂ��ă{�^���ɔ�������悤��
		}
    }
    // ���U���g��ʂ̏����ݒ�
    async void InitResultingUI()
    {
        // �����ݒ肵���̂�true
        stateInit[3] = true;

        // ���U���g�p�l����\������ȊO���\��
        SetPanelActives();

        // �����Ƒ���̃X�R�A�Ɩ��O��\��
        ResultingName[0].text = playerData.Name;
        ResultingScore[0].text = playerData.Score.ToString();

        ResultingName[1].text = playerDataClone.Name;
        ResultingScore[1].text = playerDataClone.Score.ToString();
        
        // �X�R�A��T�[�o�֑��M
        var result = await ServerRequestController.PostScore(playerData.Score, playerData.PlayerId);
    }
    // �����L���O��ʂ̕`��X�V
    void UpdateRankingUI()
	{
        // A�{�^���ŃZ���N�g��ʂɑJ��,���U���g��ʂ�\�����Ă��鎞�̓����L���O�ɖ߂�
        // isJoyconButtom�ŘA���őJ�ڂ���̂�h��
        if(isJoyconButtom && m_joyconR.GetButtonDown(m_buttons[1]))
        {
            // stateInit[3]��g���ă��U���g�\�����̓Z���N�g�ɑJ�ڂ��Ȃ��悤�ɂ���
            if(stateInit[3])
            {
                // �Z���N�g��ʂɑJ��
                // ���[�f�B���O��ʋ��ށH
                state = PlayState.InputSelecting;

                // ������
                stateInit = new bool[6];
            }
            else
            {
                // �����L���O��\���A���U���g���\��
                resultingPanel.SetActive(false);
                rankingPanel.SetActive(true);

                stateInit[3] = true;
                isJoyconButtom = false;
                // 4�b���isJoyconButtom��true�ɂ��ă{�^������������悤�ɂ���
                Invoke(nameof(InvokeTransOffset), 1f);
            }
        }
        // B�{�^���Ń��U���g��ʂɋt�J��
        if(isJoyconButtom && m_joyconR.GetButtonDown(m_buttons[0]))
		{
            //
            resultingPanel.SetActive(true);
            rankingPanel.SetActive(false);

            // 4�b���isJoyconButtom��true�ɂ��ă{�^������������悤�ɂ���
            stateInit[3] = false;
            isJoyconButtom = false;
            Invoke(nameof(InvokeTransOffset), 1f);
        }
	}
    // �����L���O��ʂ̏����ݒ�
    async void InitRankingUI()
	{
        // �����ݒ肵���̂�true
        stateInit[4] = true;

        // �����L���O�p�l����\������ȊO���\��
        SetPanelActives();
        
        // �����L���O��ʂ���擾
        var result = await ServerRequestController.GetRanking();

        // �����L���O���ʂ���8���ŕ\��
        for(int i = 0;i < 6; i++)
		{
            RankingName[i].text = result.Users[i].name;
            RankingScore[i].text = result.Users[i].rate.ToString();
		}
        // ���[�U���ӂ̃����L���O��\��
        var result2 = await ServerRequestController.GetUserRanking(playerData.PlayerId);

        // ��������̏��ʂ̐l�̐�
        int higherCount = result2.higher_around_rank_users.Length;
        // ������艺�̏��ʂ̐l�̐�
        int lowerCount = result2.lower_around_rank_users.Length;

        // ���ӏ��ʂ̕\���͏㉺2�l���Ȃ̂Ő���2�ȏ�Ȃ�2��2�����Ȃ琔������
        // �eTEXT�z���[0-1]��ʁA[2]�����A[3-4]���ʂ̍\��
        for(int i = 0; i < (higherCount < 2 ? higherCount : 2); i++)
		{
            RankingAroundRank[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].rank.ToString();
            RankingAroundName[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].name;
            RankingAroundScore[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].rate.ToString();
        }
        // ����
        for(int j = 0; j < (lowerCount < 2 ? lowerCount : 2); j++)
		{
            // �z��̈�����[3-4]�����邽�߂�3�𑫂�
            RankingAroundRank[3 + j].text = result2.lower_around_rank_users[j].rank.ToString();
            RankingAroundName[3 + j].text = result2.lower_around_rank_users[j].name;
            RankingAroundScore[3 + j].text = result2.lower_around_rank_users[j].rate.ToString();
        }
        // �����̏��ʂƖ��O�A�X�R�A��^�񒆂ɕ\��[2]
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

        // �ڑ���ʈȊO���\��
        SetPanelActives();
    }
    // ���ԍ��̎��s�p
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

    // ���݂̏�Ԃ�\���񋓌^
    // 0 �Q�[����
    // 1 ���f��
    // 2 ���[�h��
    // 3 �ҋ@���
    // 4 ���U���g
    // 5 �����L���O
    // 6 �ڑ�
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