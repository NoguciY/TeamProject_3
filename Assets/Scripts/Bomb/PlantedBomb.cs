using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

//���e
//�G�ɓ��������ꍇ�A
//�E�G��j�󂷂�
//�E�A�C�e���𐶐�����
//�E���g��j�󂷂�

public class PlantedBomb : MonoBehaviour
{
    //�����p�[�e�B�N��
    //[System.NonSerialized]�ł͂Ȃ�[HideInInspector]��ݒ肷�邱�Ƃ�
    //�t�B�[���h��\�������l��ێ���������悤�ɂȂ�
    [HideInInspector]
    public GameObject explosionParticle;

    //��������܂ł̎���
    public float fuseTime;

    //�͈�
    public float explosionRadius;

    [SerializeField, Header("�����ォ�甚�e��j������܂ł̎���(�b)")]
    private float bombLifeSpan = 5f;

    [SerializeField, Header("�����p�[�e�B�N�������ォ��j������܂ł̎���(�b)")]
    private float particleLifeSpan = 3f;

    //�R���C�_�[�R���|�[�l���g
    [SerializeField]
    private SphereCollider sphereCollider;

    private Transform myTransform;

    //���C���΂��ő勗��
    private float maxDistance;

    private void Start()
    {
        myTransform = transform;
        maxDistance = 0;

        Invoke("Detonate", fuseTime);
    }

    //����������
    private void Explosion()
    {
        if (explosionParticle != null)
        {
            //�����𐶐�
            GameObject particle =
                Instantiate(explosionParticle, myTransform.position, Quaternion.identity);

            particle.GetComponent<ParticleSystem>().Play();

            //particleLifeSpan�b��Ƀp�[�e�B�N��������
            Destroy(particle, particleLifeSpan);

            Debug.Log("����!!");
        }
    }

    //�͈͓��ɂ���I�u�W�F�N�g�Ƀ_���[�W
    void Detonate()
    {
        //���g���\��
        gameObject.SetActive(false);
        
        //��������
        Explosion();

        //����̃��C�Ƀq�b�g�����S�ẴR���C�_�[���擾����F����(���̒��S�A���̔��a�A���C���΂������A��΂��ő勗��)
        RaycastHit[] hits = Physics.SphereCastAll(myTransform.position, explosionRadius, Vector3.forward, maxDistance);
        
        foreach (var hit in hits)
        {
            //Player playe = hit.collider.gameObject.GetComponent<Player>();
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //�G�ɓ��������ꍇ�A�_���[�W��^����
                hit.collider.gameObject.GetComponent<EnemyFlocking>().Dead();
            }
        }

        //���e�𔚔��p�[�e�B�N���j����ɔj������
        Destroy(this.gameObject, bombLifeSpan);
    }


    //���e�̍����̔������擾����
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight = 
            transform.localScale.y * sphereCollider.radius;
    }
}
