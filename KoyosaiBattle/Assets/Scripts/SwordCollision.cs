using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log($"Hit:{collision.gameObject.tag}");
	}
}
