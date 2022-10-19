using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitcollision : MonoBehaviour
{
    private void OnTriggerEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag ("swordcolor002"))
        {
            //hp -= collision.gameObject.GetComponent<EnemySwordManager>().powerEnemy;
        }
    }
}
