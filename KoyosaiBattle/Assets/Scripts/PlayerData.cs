using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static int PlayerId { get; private set; }

    public static Dictionary<string, int> DictionaryID { get; private set; }

    void Start()
    {
        SetUserId(10);
        Init();
    }

    void Update()
    {

    }
    void Init()
	{
		DictionaryID = new Dictionary<string, int>
		{
			{ "001", 1 },
			{ "002", 3 }
		};
	}

    protected static void SetDictionaryID(string name, int id)
	{
        DictionaryID.Add(name, id);
	}
    protected static void SetUserId(int id) => PlayerId = id;
}
