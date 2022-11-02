using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDisplay : MonoBehaviour
{
    [SerializeField]
    StrixReplicator replicator;
    
    //ä÷êîÇÃéQè∆
    public static ShieldDisplay instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!replicator.isLocal)
        {
            var player = GameObject.Find("Volinier-motion2-joycon(Clone)");
            this.transform.parent = player.transform;
            this.transform.localScale = Vector3.one;
        }

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create()
    {
        gameObject.SetActive(true);
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
