using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

public class Hitcollision : MonoBehaviour
{
    [SerializeField]
    [Tooltip("EF_HIT_M_null")]
    private ParticleSystem particle;
    [SerializeField]
    StrixReplicator replicator;
    private AudioSource slashAudio;

    void Start() => slashAudio = GetComponent<AudioSource>();

    private void OnTriggerEnter(Collider collision)
    {
        if(!replicator.isLocal)
            return;

        if (collision.gameObject.CompareTag("swordcolor002"))
        {
            string swordName = collision.gameObject.transform.parent.name + "(";
            string playerName = collision.gameObject.transform.parent.parent.name + "(";
            if(swordName.Split('(')[1] != playerName.Split('(')[1])
            {
                //if (Attack.instance.replicator.isLocal)
                //return;
                UIController.instance.playerData.HitPoint -= 2;
                // パーティクルシステムのインスタンスを生成する。
                ParticleSystem newParticle = Instantiate(particle);
                //　パーティクル発生場所を取得し、その位置に生成する。
                Vector3 hitPos = collision.ClosestPointOnBounds(this.transform.position);
                newParticle.transform.position = hitPos;
                //　パーティクルの発生
                newParticle.Play();
                //　インスタンス化したパーティクルの消去
                Destroy(newParticle.gameObject, 1.0f);
            }
        } 
    }
}