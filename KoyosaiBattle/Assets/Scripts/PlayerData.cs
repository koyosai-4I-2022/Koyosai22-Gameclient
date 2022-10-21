using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : StrixBehaviour
{
    [StrixSyncField]
    public static int PlayerId;

    [StrixSyncField]
    public static Dictionary<string, int> DictionaryID;

    [StrixSyncField]
    public static string[] Names;

    [StrixSyncField]
    public int N = 1;

    void Start()
    {
        SetUserId(10);
        Init();
        Debug.Log($"{name}:{isLocal}");
    }

    void Update()
    {

    }
    void Init()
	{
        DictionaryID = new Dictionary<string, int>();
	}

    public static void SetDictionaryID(string name, int id)
	{
        DictionaryID.Add(name, id);
	}
    public static void SetUserId(int id) => PlayerId = id;
}
