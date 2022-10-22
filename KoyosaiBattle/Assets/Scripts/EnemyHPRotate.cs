using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPRotate : MonoBehaviour
{
    public Canvas canvas;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Volinier-motion2");
    }

    // Update is called once per frame
    void LateUpdate()
    {
       transform.rotation = player.transform.rotation;
    }
}
