using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�����������ꍇ
//�E�v���C���[�����l���㏸
//�E�A�C�e�����j�������

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}    
