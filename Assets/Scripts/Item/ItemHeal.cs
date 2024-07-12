using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeal : MonoBehaviour
{
    //�񕜗�
    [SerializeField]
    private float healValue;

    private void OnTriggerEnter(Collider other)
    {
        //�A�C�e�����擾�ł���I�u�W�F�N�g���擾
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();

        if (gettableItemObject != null)
        {
            gettableItemObject.Heal(healValue);
            Destroy(this.gameObject);
        }
    }

}
