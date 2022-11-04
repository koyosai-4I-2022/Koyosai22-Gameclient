using SoftGear.Strix.Unity.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDisplay : MonoBehaviour
{
    [SerializeField]
    StrixReplicator replicator;
    [SerializeField]
    GameObject shield;
    
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
        if(this.name.Contains("Clone"))
        {
            //var player = GameObject.Find("Volinier-motion2-joycon(Clone)");
            //shield.transform.parent = player.transform;
            //shield.transform.localScale = Vector3.one;
            PlayerMotion.instance.shieldEnemy = this.shield;
        }
        else
		{
            PlayerMotion.instance.shieldSelf = this.shield;
		}

        //shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
