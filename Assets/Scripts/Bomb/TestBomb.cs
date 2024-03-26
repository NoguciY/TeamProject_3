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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyFlocking>().Dead();
            Destroy(this.gameObject);
        }
    }
}
