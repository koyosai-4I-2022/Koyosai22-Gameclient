using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class SwordController : MonoBehaviour
{
    [SerializeField]
    StrixReplicator replicator;

    bool isInit = false;

    void Start()
    {
        if(!replicator.isLocal && JoyConAttack.instance != null)
		{
            this.transform.parent = JoyConAttack.instance.gameObject.transform;
            isInit = true;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInit && !replicator.isLocal && JoyConAttack.instance != null)
        {
            this.transform.parent = JoyConAttack.instance.gameObject.transform;
            isInit = true;
        }
    }
}
