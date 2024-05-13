using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

//爆弾
//敵に当たった場合、
//・敵を破壊する
//・アイテムを生成する
//・自身を破壊する

public class PlantedBomb : MonoBehaviour
{
    //爆発パーティクル
    //[System.NonSerialized]ではなく[HideInInspector]を設定することで
    //フィールド非表示時も値を保持し続けるようになる
    [HideInInspector]
    public GameObject explosionParticle;

    //爆発するまでの時間
    public float fuseTime;

    //範囲
    public float explosionRadius;

    [SerializeField, Header("爆発後から爆弾を破棄するまでの時間(秒)")]
    private float bombLifeSpan = 5f;

    [SerializeField, Header("爆発パーティクル生成後から破棄するまでの時間(秒)")]
    private float particleLifeSpan = 3f;

    //コライダーコンポーネント
    [SerializeField]
    private SphereCollider sphereCollider;

    private Transform myTransform;

    //レイを飛ばす最大距離
    private float maxDistance;

    private void Start()
    {
        myTransform = transform;
        maxDistance = 0;

        Invoke("Detonate", fuseTime);
    }

    //爆発させる
    private void Explosion()
    {
        if (explosionParticle != null)
        {
            //爆発を生成
            GameObject particle =
                Instantiate(explosionParticle, myTransform.position, Quaternion.identity);

            particle.GetComponent<ParticleSystem>().Play();

            //particleLifeSpan秒後にパーティクルを消す
            Destroy(particle, particleLifeSpan);

            Debug.Log("爆発!!");
        }
    }

    //範囲内にいるオブジェクトにダメージ
    void Detonate()
    {
        //自身を非表示
        gameObject.SetActive(false);
        
        //爆発する
        Explosion();

        //球状のレイにヒットした全てのコライダーを取得する：引数(球の中心、球の半径、レイを飛ばす方向、飛ばす最大距離)
        RaycastHit[] hits = Physics.SphereCastAll(myTransform.position, explosionRadius, Vector3.forward, maxDistance);
        
        foreach (var hit in hits)
        {
            //Player playe = hit.collider.gameObject.GetComponent<Player>();
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //敵に当たった場合、ダメージを与える
                hit.collider.gameObject.GetComponent<EnemyFlocking>().Dead();
            }
        }

        //爆弾を爆発パーティクル破棄後に破棄する
        Destroy(this.gameObject, bombLifeSpan);
    }


    //爆弾の高さの半分を取得する
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight = 
            transform.localScale.y * sphereCollider.radius;
    }
}
