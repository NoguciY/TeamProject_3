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
            if(GameManager.Instance.items.Count != 0)
            { 
                foreach (var item in GameManager.Instance.items)
                {
                    item.Collect(_playerTransform);
                    Debug.Log($"経験値を{GameManager.Instance.items.Count}個回収した");
                }
                // アイテム取得処理
                Destroy(gameObject);
            }
        }
    }
}
