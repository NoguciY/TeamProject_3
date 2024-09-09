using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    private Transform _playerTransform;

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other">プレイヤー</param>
    private void OnTriggerEnter(Collider other)
    {
        //アイテムを取得できるオブジェクトを取得
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();
        if (gettableItemObject != null)
        {
            _playerTransform = other.gameObject.transform;
            if (GameManager.Instance.items.Count != 0)
            {
                foreach (var item in GameManager.Instance.items)
                {
                    item.Collect(_playerTransform);
                    Debug.Log($"経験値を{GameManager.Instance.items.Count}個回収した");
                }
            }
                // アイテム取得処理
                Destroy(gameObject);
            
        }
    }
}
