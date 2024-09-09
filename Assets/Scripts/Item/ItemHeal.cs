using UnityEngine;

public class ItemHeal : MonoBehaviour
{
    [SerializeField, Header("回復量")]
    private float healValue;

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
            gettableItemObject.Heal(healValue);
            Destroy(this.gameObject);
            Debug.Log($"{healValue}回復した");
        }
    }

}
