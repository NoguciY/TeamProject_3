using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//���̔��e
//�G�ɓ��������ꍇ�A
//�E�G��j�󂷂�
//�E�A�C�e���𐶐�����
//�E���g��j�󂷂�

public class TestBomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyFlocking>().Dead();
            Destroy(this.gameObject);
        }
    }
}
