using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//仮の爆弾
//敵に当たった場合、
//・敵を破壊する
//・アイテムを生成する
//・自身を破壊する

public class TestBomb : MonoBehaviour
{
    public float fuseTime;          //爆発するまでの時間
    public float explosionRadius;   //範囲

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
            //敵にダメージ
            //if (playe != null)
            //{
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<EnemyFlocking>().Dead();
                }
            //}
        }

        Destroy(this.gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        other.gameObject.GetComponent<EnemyFlocking>().Dead();
    //        Destroy(this.gameObject);
    //    }
    //}
}
