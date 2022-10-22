using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : StrixBehaviour
{
    public static int PlayerId;

    public static Dictionary<string, int> DictionaryID;

    public static string[] Names;

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

    public void SetDictionaryID(string name, int id)
	{
        DictionaryID.Add(name, id);
	}
    public void SetUserId(int id) => PlayerId = id;
}
