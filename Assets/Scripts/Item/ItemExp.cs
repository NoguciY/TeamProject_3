using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーが当たった場合
//・プレイヤーが持つ値が上昇
//・アイテムが破棄される

public class ItemExp : MonoBehaviour
{
    //経験値
    private int exp = 1;

    private void OnTriggerEnter(Collider other)
    {
        //アイテムを取得できるオブジェクトを取得
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();

        if(gettableItemObject != null)
        {
            //経験値を取得させる
            gettableItemObject.GetExp(exp);

            Destroy(this.gameObject);
        }
    }
}    
