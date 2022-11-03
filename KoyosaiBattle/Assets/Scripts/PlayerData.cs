using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : StrixBehaviour
{
    [StrixSyncField]
    public int PlayerId;

    [StrixSyncField]
    public string Name;

    [StrixSyncField]
    public int Score;

    [StrixSyncField]
    public int HitPoint;

    [SerializeField]
    int defaultHP = 150;
    [StrixSyncField]
    public bool isFinish = false;
    [StrixSyncField]
    public bool isGuard = false;

    private void Awake()
    {
        if (!isLocal)
        {
            UIController.instance.playerDataClone = this;
        }
    }

    void Start()
    {
        SetUserId(10);
        Init();
        if(!isLocal)
		{
            UIController.instance.playerDataClone = this;
		}
    }

    void Update()
    {

    }
    public void Init()
    {
        PlayerId = -1;
        Name = string.Empty;
        Score = -1;
        HitPoint = HPGauge.instance.maxHp;
        isFinish = false;
        isGuard = false;
    }
    public void PlayInit()
	{
        Score = -1;
        HitPoint = HPGauge.instance.maxHp;
        isFinish = false;
        isGuard = false;
    }

    public void SetUser(string name, int id)
	{
        PlayerId = id;
        Name = name;
	}
    public void SetUserId(int id) => PlayerId = id;
    public void SetScore(int s) => Score = s;
}
