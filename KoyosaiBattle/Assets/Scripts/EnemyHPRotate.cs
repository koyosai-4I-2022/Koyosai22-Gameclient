using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPRotate : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = PlayerMotion.instance.transform.rotation;
    }
}