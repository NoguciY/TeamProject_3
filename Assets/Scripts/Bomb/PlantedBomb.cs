using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//爆弾
//敵に当たった場合、
//・敵を破壊する
//・アイテムを生成する
//・自身を破壊する

public class PlantedBomb : MonoBehaviour
{
    public float fuseTime;          //爆発するまでの時間
    public float explosionRadius;   //範囲

    //コライダーコンポーネント
    [SerializeField]
    private SphereCollider sphereCollider;


    private void Start()
    {
        Invoke("Detonate", fuseTime);
    }


    //範囲内にいるオブジェクトにダメージ
    void Detonate()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.forward);
        
        foreach (var hit in hits)
        {
            Player playe = hit.collider.gameObject.GetComponent<Player>();
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //敵に当たった場合、ダメージを与える
                hit.collider.gameObject.GetComponent<EnemyFlocking>().Dead();
            }
        }

        Destroy(this.gameObject);
    }


    //爆弾の半分の高さを取得する
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight = 
            transform.localScale.y * sphereCollider.radius;
    }
}
