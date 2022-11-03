using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPRotate : MonoBehaviour
{
    [SerializeField]
    StrixReplicator replicator;

	private void Start()
	{
		if(replicator.isLocal)
		{
			this.gameObject.SetActive(false);
		}
		else
		{
			this.gameObject.SetActive(true);
		}
	}

	// Update is called once per frame
	void LateUpdate()
    {
        transform.rotation = PlayerMotion.instance.transform.rotation;
    }
}