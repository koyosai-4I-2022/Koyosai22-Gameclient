using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JSample : MonoBehaviour
{
    [SerializeField]
    GameObject Sword;
	[SerializeField]
	GameObject Cube;
	[SerializeField]
	GameObject[] sub;
	[SerializeField]
	GameObject Hand;

    [SerializeField]
    ParticleSystem par;

    [SerializeField,Range(0f, 5f)]
    float p = 0.2f;
    [SerializeField,Range(0f, 200f)]
    float d = 100f;

    //JoyconLibÇÃïœêî
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;

    Vector3 beforeAccel;
    Vector3 deltaAccel;
    Vector3 BasePosition;
	Vector3 BaseEuler;
	Vector3 beforeGyro;
    Vector3 gravity = new Vector3(0, -9.81f, 0);

    int count = 0;
	int count2 = 0;

    void Start()
    {
        // joyconÇÃê›íË
        m_joycons = JoyconManager.Instance.j;
        if(m_joycons == null || m_joycons.Count <= 0)
            return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        beforeAccel = m_joyconL.GetAccel();
        beforeGyro = m_joyconR.GetGyro();

		//BasePosition = Cube.transform.localPosition;
		BasePosition = Hand.transform.position;
		BaseEuler = Sword.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
	{
        if(m_joycons == null || m_joycons.Count <= 0)
            return;

        var acc = m_joyconR.GetAccel();
		var gyro = m_joyconR.GetGyro();
		var ori = m_joyconR.GetVector();

		count++;
		acc = new Vector3(acc.y, -acc.x, acc.x);
		//sub[1].transform.position = ((gyro + gyro1 + gyro2)/ 3f).normalized;
		if(Mathf.Abs(gyro.x) < p)
			gyro.x = 0f;
		if(Mathf.Abs(gyro.y) < p)
			gyro.y = 0f;
		if(Mathf.Abs(gyro.z) < p)
			gyro.z = 0f;
		//gyro = gyro;
		gyro = gyro.normalized;
		var m = 1f;//acc.magnitude;
		Sword.transform.Rotate(new Vector3(-gyro.y, -gyro.x, -gyro.z) * m);

		//if(m_joyconR.GetButtonDown(m_buttons[0]))
		//	//Cube.transform.rotation = Quaternion.Euler(0, 0, 0);
		//	Cube.transform.LookAt(acc * -1);
		//	//Cube.transform.rotation = ori;

		//if(count > 30)
		//{
		//	count = 0;
		//}

		//acc = m_joyconR.GetAccel();
		//      var dif = acc - beforeAccel;
		//var vel = (dif + beforeAccel) / 2f;

		//if(dif.sqrMagnitude > 0.25f)
		//{
		//	Cube.transform.localPosition += ( -vel ) * Time.deltaTime * 3f;
		//}
		//else
		//{
		//	count++;
		//	if(count > 200)
		//	{
		//		Cube.transform.localPosition = BasePosition;
		//		Cube.transform.rotation = Quaternion.Euler(0, 0, 0);
		//		count = 0;
		//	}
		//}


		//beforeAccel = dif;

		//{
		//    J.transform.rotation = ori;
		//}

		//Debug.Log($"{ori}");

		acc = m_joyconR.GetAccel() - beforeAccel;
		Vector3 angle = Vector3.zero;
		Vector3 accUnity = new Vector3(-acc.y, -acc.x, -acc.z);
		//accUnity = new Vector3(-acc.y, -acc.x, -acc.z) + gyro.normalized;
		//var dif = accUnity - deltaAccel;
		accUnity += deltaAccel / 2f;
		float sqr = accUnity.sqrMagnitude;
		beforeAccel = m_joyconR.GetAccel();
		deltaAccel = accUnity;
		if(sqr > p)
		{
			Sword.transform.localPosition += accUnity * Time.deltaTime * d;
			//if(!par.isPlaying) par.Play();
			//Debug.Log($"{accUnity}:{accUnity.sqrMagnitude}");
		}
		else
		{
			count++;
			if(count > 200)
			{
				//Sword.transform.position = Hand.transform.position;
				Sword.transform.rotation = Quaternion.Euler(0, 0, 0);
				count = 0;
			}
		}
		Hand.transform.position = Sword.transform.position;

		//C.transform.position += acc * Time.deltaTime * 1f;
	}
}
