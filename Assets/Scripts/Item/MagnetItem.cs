using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetItem : MonoBehaviour
{


    private Transform _playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        //アイテムを取得できるオブジェクトを取得
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();
        if (gettableItemObject != null)
        {
            _playerTransform = other.gameObject.transform;
            foreach (var item in GameManager.Instance.items)
            {
                item.Collect(_playerTransform);
            }
            // アイテム取得処理
            Destroy(gameObject);
            
        }
    }
}
