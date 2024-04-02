using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーが当たった場合
//・プレイヤーが持つ値が上昇
//・アイテムが破棄される

public class Item : MonoBehaviour
{
    //経験値
    private int exp = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //プレイヤーが経験値を得る
            var player = other.gameObject.GetComponent<Player>();
            player.GetExp(exp);

            Destroy(this.gameObject);
        }
    }
}    
