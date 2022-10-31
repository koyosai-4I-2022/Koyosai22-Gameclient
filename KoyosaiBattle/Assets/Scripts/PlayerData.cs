using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : StrixBehaviour
{
    [StrixSyncField, NonSerialized]
    public int PlayerId;

    [StrixSyncField, NonSerialized]
    public string Name;

    [StrixSyncField, NonSerialized]
    public int Score;

    [StrixSyncField, NonSerialized]
    public int HitPoint;

    [SerializeField]
    int defaultHP = 100;

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
    void Init()
	{
        PlayerId = -1;
        Name = string.Empty;
        Score = 0;
        HitPoint = defaultHP;
	}

    public void SetUser(string name, int id)
	{
        PlayerId = id;
        Name = name;
	}
    public void SetUserId(int id) => PlayerId = id;
    public void SetScore(int s) => Score = s;
}
