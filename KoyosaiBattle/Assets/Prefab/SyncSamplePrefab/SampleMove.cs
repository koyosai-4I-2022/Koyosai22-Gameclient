using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMove : MonoBehaviour
{
    [SerializeField]
    StrixReplicator replicator;
    [SerializeField]
    Animator animator;

    void Start()
    {
        if(!replicator.isLocal)
            this.transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if(replicator.isLocal)
		{
            if(Input.GetKeyDown(KeyCode.K))
			{
                animator.Play("walk2");
            }

            float x = Input.GetAxis("Horizontal 1");
            float z = Input.GetAxis("Vertical 1");

            this.transform.Translate(new Vector3(x, 0, z) * Time.deltaTime * 4.0f, Space.Self);
		}
    }
}
