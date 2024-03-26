using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーが当たった場合
//・プレイヤーが持つ値が上昇
//・アイテムが破棄される

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}    
