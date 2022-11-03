using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPRotate : MonoBehaviour
{
    [SerializeField]
    StrixReplicator replicator;

	[SerializeField]
	Slider slider;
	
	// Update is called once per frame
	void LateUpdate()
	{
		if(replicator.isLocal)
		{
			slider.gameObject.SetActive(false);
		}
		else
		{
			slider.gameObject.SetActive(true);
			if(UIController.instance.state == UIController.PlayState.Playing)
			{
				slider.value = UIController.instance.playerDataClone.HitPoint;
				transform.rotation = PlayerMotion.instance.transform.rotation;
			}
		}
    }
}