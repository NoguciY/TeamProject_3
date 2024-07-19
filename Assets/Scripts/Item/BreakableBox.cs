using System.Collections;
using System.Collections.Generic;
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

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Break();
        }
    }

    void Break()
    {
        DropRandomItem();
        Destroy(gameObject);  // ����j��
    }

    void DropRandomItem()
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

                //�A�C�e�����}�O�l�b�g�̏ꍇ�A�X�|�[���}�l�[�W���[�̎Q�Ƃ��~����

                return;
            }
            else
            {
                randomPoint -= item.dropChance;
            }
        }
    }
}
