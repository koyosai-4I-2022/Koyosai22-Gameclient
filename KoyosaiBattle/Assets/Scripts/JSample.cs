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
	GameObject Arm1;
	[SerializeField]
	GameObject Arm2;
	[SerializeField]
	GameObject Shoulder;

    [SerializeField]
    ParticleSystem par;

    [SerializeField,Range(0f, 5f)]
    float p = 0.2f;
    [SerializeField,Range(0f, 200f)]
    float d = 100f;

    //JoyconLibの変数
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;

    Vector3 beforeAccel;
    Vector3 deltaAccel;
    Vector3 BasePosition;
	Vector3 BaseEuler;
	Vector3 swingPos;
	Vector3 beforeGyro;
    Vector3 gravity = new Vector3(0, -9.81f, 0);

    int count = 0;
	int count2 = 0;

	float mag = 1.2f;
	float threshold = 9f;

	float magShoulderToArm2;
	float magArm2ToArm1;
	float magArm1ToHand;

    void Start()
    {
		// フレームレートを固定 60FPS
		Application.targetFrameRate = 60;
        // joyconの設定
        m_joycons = JoyconManager.Instance.j;
        if(m_joycons == null || m_joycons.Count <= 0)
            return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        beforeAccel = m_joyconL.GetAccel();
        beforeGyro = m_joyconR.GetGyro();

		//BasePosition = Cube.transform.localPosition;
		BasePosition = new Vector3(0.4f, 0.7f, -0.1f);
			//Hand.transform.position;
		BaseEuler = new Vector3(43.502f, -12.026f, -102.049f);
		//Sword.transform.eulerAngles;
		swingPos = Vector3.zero;

		Sword.transform.rotation = m_joyconR.GetVector();
		threshold = mag * mag;

		magShoulderToArm2 = ( ( Arm2.transform.localPosition - Shoulder.transform.localPosition ) * 100f ).magnitude / 100f;
		magArm2ToArm1 = ( ( Arm1.transform.localPosition - Shoulder.transform.localPosition ) * 100f ).magnitude / 100f;
		magArm1ToHand = ( ( Hand.transform.localPosition - Shoulder.transform.localPosition ) * 100f ).magnitude / 100f;
		Debug.Log($"{magShoulderToArm2}:{magArm2ToArm1}:{magArm1ToHand}");
	}

	// Update is called once per frame
	void Update()
	{
        if(m_joycons == null || m_joycons.Count <= 0)
            return;
		count2++;

		//if(count2 > 2)
		{
			count2 = 0;

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

			// 前フレームの加速度の差を取得
			acc = m_joyconR.GetAccel() - beforeAccel;
			// 加速度の要素を修正する
			Vector3 accUnity = new Vector3(-acc.y, -acc.x, -acc.z);
			//accUnity = new Vector3(-acc.y, -acc.x, -acc.z) + gyro.normalized;
			//var dif = accUnity - deltaAccel;
			// 慣性を追加
			accUnity += deltaAccel / 2f;
			// 二乗の大きさを取得
			float sqr = accUnity.sqrMagnitude;
			beforeAccel = m_joyconR.GetAccel();
			deltaAccel = accUnity;

			if(sqr > p)
			{
				swingPos += accUnity;
				var vec = BasePosition + swingPos;
				//Sword.transform.localPosition = beforeAccel;

				if(swingPos.sqrMagnitude > threshold)
				{
					Sword.transform.localPosition = vec.normalized * mag;
				}
				else
				{
					Sword.transform.localPosition = ( vec );
				}
				count = 0;
				//if(!par.isPlaying) par.Play();
				//Debug.Log($"{accUnity}:{accUnity.sqrMagnitude}");
			}
			else
			{
				count++;
				if(count > 20)
				{
					//Sword.transform.position = Hand.transform.position;
					//Sword.transform.position = Vector3.up;
					Sword.transform.localRotation = Quaternion.Euler(BaseEuler);
					swingPos = Vector3.zero;
					Sword.transform.localPosition = BasePosition;
					count = 0;
				}
			}
			var svec = Sword.transform.position - Shoulder.transform.position;
			var svecN = svec.normalized;

			Arm2.transform.localPosition = svecN * magShoulderToArm2;
			Arm1.transform.localPosition = svecN * magArm2ToArm1;
			Hand.transform.localPosition = svecN * magArm1ToHand;

			//Hand.transform.position = Sword.transform.position;

			//C.transform.position += acc * Time.deltaTime * 1f;
		}
	}
}
