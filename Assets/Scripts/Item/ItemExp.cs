using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�����������ꍇ
//�E�v���C���[�����l���㏸
//�E�A�C�e�����j�������

public class ItemExp : MonoBehaviour
{
    //�o���l
    private int exp = 1;

    private void OnTriggerEnter(Collider other)
    {
        //�A�C�e�����擾�ł���I�u�W�F�N�g���擾
        var gettableItemObject = other.gameObject.GetComponent<IGettableItem>();

        if(gettableItemObject != null)
        {
            //�o���l���擾������
            gettableItemObject.GetExp(exp);

            Destroy(this.gameObject);
        }
    }
}    
