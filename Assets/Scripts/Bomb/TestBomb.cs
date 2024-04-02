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
    public float fuseTime;          //��������܂ł̎���
    public float explosionRadius;   //�͈�

    private void Start()
    {
        Invoke("Detonate", fuseTime);
    }


    //�͈͓��ɂ���I�u�W�F�N�g�Ƀ_���[�W
    void Detonate()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.forward);
        
        foreach (var hit in hits)
        {
            Player playe = hit.collider.gameObject.GetComponent<Player>();
            //�G�Ƀ_���[�W
            //if (playe != null)
            //{
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<EnemyFlocking>().Dead();
                }
            //}
        }

        Destroy(this.gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        other.gameObject.GetComponent<EnemyFlocking>().Dead();
    //        Destroy(this.gameObject);
    //    }
    //}
}
