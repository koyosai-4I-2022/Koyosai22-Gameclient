using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoftGear.Strix.Unity.Runtime;

public class SwordController : MonoBehaviour
{
    [SerializeField]
    StrixReplicator replicator;

    [SerializeField]
    Text txt;

    bool isInit = false;

    void Start()
    {
        if(!replicator.isLocal && JoyConAttack.instance.clone != null)
		{
            this.transform.parent = JoyConAttack.instance.clone.gameObject.transform;
            this.transform.localScale = Vector3.one;
            isInit = true;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInit && !replicator.isLocal && JoyConAttack.instance.clone != null)
        {
            Debug.Log("Set Parent");
            this.transform.parent = JoyConAttack.instance.clone.gameObject.transform;
            this.transform.localScale = Vector3.one;
            isInit = true;
        }
        string s = "Player:" + UIController.instance.playerData.HitPoint.ToString() + "\n";
        if(UIController.instance.playerDataClone != null)
            s += "Enemy:" + UIController.instance.playerDataClone.HitPoint.ToString();
        txt.text = s;
    }
}
