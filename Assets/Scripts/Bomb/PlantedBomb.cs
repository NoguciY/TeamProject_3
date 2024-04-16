using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//���e
//�G�ɓ��������ꍇ�A
//�E�G��j�󂷂�
//�E�A�C�e���𐶐�����
//�E���g��j�󂷂�

public class PlantedBomb : MonoBehaviour
{
    public float fuseTime;          //��������܂ł̎���
    public float explosionRadius;   //�͈�

    //�R���C�_�[�R���|�[�l���g
    [SerializeField]
    private SphereCollider sphereCollider;


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
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //�G�ɓ��������ꍇ�A�_���[�W��^����
                hit.collider.gameObject.GetComponent<EnemyFlocking>().Dead();
            }
        }

        Destroy(this.gameObject);
    }


    //���e�̔����̍������擾����
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight = 
            transform.localScale.y * sphereCollider.radius;
    }
}
