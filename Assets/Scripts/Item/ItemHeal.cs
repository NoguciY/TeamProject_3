using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeal : MonoBehaviour
{
    //回復量
    [SerializeField]
    private float healValue;

    private void OnTriggerEnter(Collider other)
    {
        //アイテムを取得できるオブジェクトを取得
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();

        if (gettableItemObject != null)
        {
            gettableItemObject.Heal(healValue);
            Destroy(this.gameObject);
        }
    }

}
