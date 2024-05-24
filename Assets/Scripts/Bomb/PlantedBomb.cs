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

    [SerializeField, Header("�_���[�W��")]
    private float damage = 3;

    //��������܂ł̎���
    public float fuseTime;

    //�͈�
    public float explosionRadius;

    [SerializeField, Header("�����ォ�甚�e��j������܂ł̎���(�b)")]
    private float bombLifeSpan = 5f;

    [SerializeField, Header("�����p�[�e�B�N�������ォ��j������܂ł̎���(�b)")]
    private float particleLifeSpan = 3f;

    [SerializeField, Header("�N�[���^�C��(�b)")]
    private float coolTime = 3f;

    //�R���C�_�[�R���|�[�l���g
    [SerializeField]
    private SphereCollider sphereCollider;

    private Transform myTransform;

    //���C���΂��ő勗��
    private float maxDistance;

    //�Q�b�^�[
    public float GetCoolTime => coolTime;

    private void Start()
    {
        myTransform = transform;
        maxDistance = 0;

        Invoke("Detonate", fuseTime);
    }

    //����������
    private void Explode()
    {
        if (explosionParticle != null)
        {
            //�����𐶐�
            GameObject particle =
                Instantiate(explosionParticle, myTransform.position, Quaternion.identity);

            particle.GetComponent<ParticleSystem>().Play();

            //particleLifeSpan�b��Ƀp�[�e�B�N��������
            Destroy(particle, particleLifeSpan);

            //���ʉ����Đ�
            SoundManager.uniqueInstance.Play("����1");

            Debug.Log("����!!");
        }
    }

    //�͈͓��ɂ���I�u�W�F�N�g�Ƀ_���[�W
    void Detonate()
    {
        //���g���\��
        gameObject.SetActive(false);
        
        //��������
        Explode();

        //����̃��C�Ƀq�b�g�����S�ẴR���C�_�[���擾����F����(���̒��S�A���̔��a�A���C���΂������A��΂��ő勗��)
        RaycastHit[] hits = Physics.SphereCastAll(myTransform.position, explosionRadius, Vector3.forward, maxDistance);
        
        foreach (var hit in hits)
        {
            //�_���[�W���󂯂邱�Ƃ��ł���I�u�W�F�N�g���擾
            var applicableDamageObject = hit.collider.gameObject.GetComponent<IApplicableDamageEnemy>();
            if (applicableDamageObject != null)
                applicableDamageObject.ReceiveDamage(damage);
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
