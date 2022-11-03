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
    [SerializeField]
    private AudioSource slashAudio;

    int damage = 6;
    int hitCount = 0;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("swordcolor002"))
        {
            var pparent = collision.transform.parent.parent.gameObject;
            if(pparent != this.gameObject)
            {
                if(UIController.instance.playerDataClone.isGuard)
                {
                    if(!replicator.isLocal)
                        return;
                    //if (Attack.instance.replicator.isLocal)
                    //return;
                    UIController.instance.playerData.HitPoint -= (damage - hitCount) / 2;
                    hitCount++;
                    if(hitCount > damage)
                        hitCount = damage;
                }
                else
                {
                    slashAudio.Play();
                    // パーティクルシステムのインスタンスを生成する。
                    ParticleSystem newParticle = Instantiate(particle);
                    //　パーティクル発生場所を取得し、その位置に生成する。
                    Vector3 hitPos = collision.ClosestPointOnBounds(this.transform.position);
                    newParticle.transform.position = hitPos;
                    //　パーティクルの発生
                    newParticle.Play();
                    //　インスタンス化したパーティクルの消去
                    Destroy(newParticle.gameObject, 1.2f);
                    if(!replicator.isLocal)
                        return;
                    //if (Attack.instance.replicator.isLocal)
                    //return;
                    UIController.instance.playerData.HitPoint -= (damage - hitCount);
                    hitCount++;
                    if(hitCount > damage)
                        hitCount = damage;
                }
            }
        } 
    }
}