using UnityEngine;

public class BreakableBox : MonoBehaviour, IApplicableDamageEnemy
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject itemPrefab;
        public float dropChance;
    }

    public DropItem[] dropItems;
    public float health = 100f;

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Break();
        }
    }

    /// <summary>
    /// /箱を破壊する処理
    /// </summary>
    private void Break()
    {
        DropRandomItem();
        Destroy(gameObject);  
    }

    /// <summary>
    /// ランダムでアイテムを生成する
    /// </summary>
    private void DropRandomItem()
    {
        float total = 0f;
        foreach (DropItem item in dropItems)
        {
            total += item.dropChance;
        }

        float randomPoint = Random.value * total;

        foreach (DropItem item in dropItems)
        {
            if (randomPoint < item.dropChance)
            {
                Instantiate(item.itemPrefab, transform.position, transform.rotation);
                return;
            }
            else
            {
                randomPoint -= item.dropChance;
            }
        }
    }
}
