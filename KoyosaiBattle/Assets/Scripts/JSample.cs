using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JSample : MonoBehaviour
{
    [SerializeField]
    GameObject Sword;
	[SerializeField]
	GameObject Hand;
	[SerializeField]
	GameObject Arm1;
	[SerializeField]
	GameObject Arm2;
	[SerializeField]
	GameObject Shoulder;
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
	Vector3 stayAccel;
	Vector3 beforeGyro;
    Vector3 gravity = new Vector3(0, -9.81f, 0);

	Vector3 Arm2DefaultAngle = new Vector3(57.8f, 271.0f, 182.5f);
	Vector3 difShoulder = new Vector3(32.108f, 2.953f, 0.071f);

    int count = 0;

	float disArm2ToHand;

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

		BasePosition = new Vector3(0.4f, 0.7f, -0.1f);
		BaseEuler = new Vector3(43.502f, -12.026f, -102.049f);
		swingPos = Vector3.zero;

		Sword.transform.rotation = m_joyconR.GetVector();

		Arm2DefaultAngle = Hand.transform.position - Arm2.transform.position;//new Vector3(1.6f, -1.0f, 0.1f);
		disArm2ToHand = (Hand.transform.position - Arm2.transform.position).magnitude;
		Debug.Log((Arm1.transform.position - Arm2.transform.position).magnitude);
		stayAccel = Vector3.zero;
	}

	// Update is called once per frame
	void Update()
	{
		if(m_joycons == null || m_joycons.Count <= 0)
			return;

		var acc = m_joyconR.GetAccel();
		var gyro = m_joyconR.GetGyro();

		if(Mathf.Abs(gyro.x) < 0.25f)
			gyro.x = 0f;
		if(Mathf.Abs(gyro.y) < 0.25f)
			gyro.y = 0f;
		if(Mathf.Abs(gyro.z) < 0.25f)
			gyro.z = 0f;
		//gyro = gyro;
		gyro = gyro.normalized;
		var m = 1f;//acc.magnitude;
		Sword.transform.Rotate(new Vector3(-gyro.y, -gyro.x, -gyro.z) * m);

		// 前フレームの加速度の差を取得
		acc = m_joyconR.GetAccel() - beforeAccel;
		// 加速度の要素を修正する
		Vector3 accUnity = new Vector3(-acc.y, -acc.x, -acc.z) + stayAccel;
		// 慣性を追加
		accUnity += deltaAccel / 2f;
		// 二乗の大きさを取得
		float sqr = accUnity.sqrMagnitude;
		beforeAccel = m_joyconR.GetAccel();
		deltaAccel = accUnity;

		if(sqr > 0.25f)
		{
			swingPos += accUnity;
			var vec = BasePosition + swingPos.normalized * 0.72f;
			stayAccel = Vector3.zero;
			Sword.transform.localPosition = vec;
			count = 0;
		}
		else
		{
			count++;
			if(count > 20)
			{

				Sword.transform.localRotation = Quaternion.Euler(BaseEuler);
				swingPos = Vector3.zero;
				Sword.transform.localPosition = BasePosition;// + Vector3.up * 0.3f;
				count = 0;
			}
		}
		// 腕を追従させる
		{
			float dis = disArm2ToHand - (Sword.transform.position - Arm2.transform.position).magnitude;
			var Arm2ToArm1 = Arm1.transform.TransformPoint(Arm1.transform.localPosition) - Arm2.transform.position;
			var Arm2ToSword = (Sword.transform.position - Arm2.transform.position);
			var A2SNor = Arm2ToSword.normalized * 1.906625f;

			float angle_x;
			float angle_y = 0f;
			float angle_z;
			if(Arm2ToSword.x > 0f)
			{
				angle_x = Mathf.Atan2(Arm2ToSword.y, Mathf.Abs(Arm2ToSword.x)) * Mathf.Rad2Deg;
				angle_z = Mathf.Atan2(Arm2ToSword.z, Mathf.Abs(Arm2ToSword.x)) * Mathf.Rad2Deg;
			}
			else
			{
				angle_x = -Mathf.Atan2(Arm2ToSword.y, Mathf.Abs(Arm2ToSword.x)) * Mathf.Rad2Deg;
				angle_z = 180 - Mathf.Atan2(Arm2ToSword.z, Mathf.Abs(Arm2ToSword.x)) * Mathf.Rad2Deg;
			}

			Arm2.transform.localRotation = Quaternion.Euler(new Vector3(-angle_x, angle_y, angle_z) - difShoulder);
		}
	}
}
