using UnityEngine;

public class ItemHeal : MonoBehaviour
{
    [SerializeField, Header("�񕜗�")]
    private float healValue;

    /// <summary>
    /// �����蔻��
    /// </summary>
    /// <param name="other">�v���C���[</param>
    private void OnTriggerEnter(Collider other)
    {
        //�A�C�e�����擾�ł���I�u�W�F�N�g���擾
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();

        if (gettableItemObject != null)
        {
            gettableItemObject.Heal(healValue);
            Destroy(this.gameObject);
            Debug.Log($"{healValue}�񕜂���");
        }
    }

}
