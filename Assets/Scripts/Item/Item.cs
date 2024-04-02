using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�����������ꍇ
//�E�v���C���[�����l���㏸
//�E�A�C�e�����j�������

public class Item : MonoBehaviour
{
    //�o���l
    private int exp = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //�v���C���[���o���l�𓾂�
            var player = other.gameObject.GetComponent<Player>();
            player.GetExp(exp);

            Destroy(this.gameObject);
        }
    }
}    
