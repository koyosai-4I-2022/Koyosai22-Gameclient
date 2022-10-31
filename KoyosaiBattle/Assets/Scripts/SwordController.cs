using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class SwordController : MonoBehaviour
{
    [SerializeField]
    StrixReplicator replicator;

    void Start()
    {
        if(!replicator.isLocal)
		{
            this.transform.parent = UIController.instance.playerDataClone.gameObject.transform;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
