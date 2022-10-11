using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleController : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float speed;

    [SerializeField]
    Animator animator;

    StrixNetwork strixNetwork;

    void Start()
    {
        strixNetwork = StrixNetwork.instance;
    }

    void Update()
    {
        if(strixNetwork.room != null)
		{
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if(Input.GetKeyDown(KeyCode.V))
			{
                animator.SetBool("action", true);

                Invoke(nameof(ActionFinish), 1f);
			}

            player.transform.position += new Vector3(x, 0, z) * speed * Time.deltaTime;
		}
    }

    void ActionFinish() => animator.SetBool("action", false);
}
