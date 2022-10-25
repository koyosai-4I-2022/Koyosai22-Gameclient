using SoftGear.Strix.Unity.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMove : MonoBehaviour
{
    [SerializeField]
    StrixReplicator replicator;
    [SerializeField]
    Animator animator;

    //JoyconLibÇÃïœêî
    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;

    void Start()
    {
        // joyconÇÃê›íË
        m_joycons = JoyconManager.Instance.j;
        if(m_joycons == null || m_joycons.Count <= 0)
            return;
        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);


        if(!replicator.isLocal)
            this.transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if(replicator.isLocal)
		{
            if(m_joyconR.GetButtonDown(m_buttons[1]))
			{

            }

            float[] Rstick = m_joyconR.GetStick();

            this.transform.Translate(new Vector3(Rstick[0], 0, Rstick[1]) * Time.deltaTime * 4.0f, Space.Self);
		}
    }
}
