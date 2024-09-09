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
    /// �_���[�W���󂯂�
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Break();
        }
    }

    /// <summary>
    /// /����j�󂷂鏈��
    /// </summary>
    private void Break()
    {
        DropRandomItem();
        Destroy(gameObject);  
    }

    /// <summary>
    /// �����_���ŃA�C�e���𐶐�����
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
