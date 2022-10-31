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
        if(!replicator.isLocal && UIController.instance.playerDataClone != null)
		{
            this.transform.parent = UIController.instance.playerDataClone.gameObject.transform;
            isInit = true;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInit && !replicator.isLocal && UIController.instance.playerDataClone != null)
        {
            this.transform.parent = UIController.instance.playerDataClone.gameObject.transform;
            isInit = true;
        }
    }
}
