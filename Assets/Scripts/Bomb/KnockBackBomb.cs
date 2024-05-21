using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

//�m�b�N�o�b�N���e
//�v���C���[�̎�������
//���������G�̓m�b�N�o�b�N����

public class KnockbackBomb : MonoBehaviour
{
    //BombManager�Œl���Z�b�g���邽��public�ɂ���
    //�v���C���[�̈ʒu
    [System.NonSerialized]
    public Transform playerTransform;

    //�����p�[�e�B�N��
    //[System.NonSerialized]�ł͂Ȃ�[HideInInspector]��ݒ肷�邱�Ƃ�
    //�t�B�[���h��\�������l��ێ���������悤�ɂȂ�
    [HideInInspector]
    public GameObject explosionParticle;

    [SerializeField, Header("�_���[�W��")]
    private float damage = 0;

    [SerializeField, Header("�m�b�N�o�b�N��")]
    private float knockbackForce = 10f;

    [SerializeField, Header("�p���x(�P�b������ɐi�ފp�x)")]
    private float angleSpeed = 180f;

    [SerializeField, Header("�v���C���[����̋���")]
    private float toPlayerDistance = 5f;

    [SerializeField, Header("�����ォ�甚�e��j������܂ł̎���(�b)")]
    private float bombLifeSpan = 5f;

    [SerializeField, Header("�����p�[�e�B�N�������ォ��j������܂ł̎���(�b)")]
    private float particleLifeSpan = 3f;

    [SerializeField, Header("��������锚�e��")]
    private int generatedBombNum = 2;

    [SerializeField, Header("�N�[���^�C��(�b)")]
    private float coolTime = 60f;

    //�R���C�_�[�R���|�[�l���g
    [SerializeField]
    private SphereCollider sphereCollider;

    //��]��
    private Vector3 axis = Vector3.up;

    //transform�ɖ���A�N�Z�X����Əd���̂ŁA�L���b�V�����Ă���
    private Transform myTransform;

    //���e�̍����̔���
    private float bombHalfHeight;

    //�ړ�����
    private Vector3 movingDirection = Vector3.zero;

    //�Q�b�^�[
    public float GetToPlayerDistance => toPlayerDistance;
    public int GetGeneratedBombNum => generatedBombNum;
    public float GetCoolTime => coolTime;

    private void Start()
    {
        myTransform = transform;
        bombHalfHeight = GetHalfHeight();
    }

    private void Update()
    {
        //�v���C���[�̎�������
        RevolveAboutPlayer();
    }


    //���e�̍����̔������擾����
    public float GetHalfHeight()
    {
        float halfHeight;
        return halfHeight =
            transform.localScale.y * sphereCollider.radius;
    }

    //�v���C���[�𒆐S�ɂ���y�������ɉ�]����
    private void RevolveAboutPlayer()
    {
        if (playerTransform != null)
        {
            //�G�ɓ��������ꍇ�Ɉړ��������~��������
            Vector3 previousPosition = myTransform.position;

            //�v���C���[�̈ʒu�𒆐S�ɉ�]����
            Vector3 centerPos = playerTransform.position;

            //�P�t���[���ŉ�]����p�x
            var angle = angleSpeed * Time.deltaTime;

            //�w�肵���p�x�Ǝ������](�N�I�[�^�j�I��)���쐬
            var angleAxis = Quaternion.AngleAxis(angle, axis);

            //�~�^���̈ʒu�v�Z
            //���S����̑��΃x�N�g��(���e�̌��݈ʒu - �v���C���[�̈ʒu)
            Vector3 fromCenterVec = myTransform.position - centerPos;

            //���΃x�N�g���̌���
            Vector3 direction = fromCenterVec.normalized;
            
            //�ʒu�𒆐S�_����̑��ΓI�Ȍ��������c���āA�~�^���̔��a��ݒ�
            Vector3 pos = direction * toPlayerDistance;

            //�~�^���̋O����̈ʒu�Ɉړ�������
            pos = angleAxis * pos;

            //���S�_�����Z���A���e�̈ʒu���X�V
            myTransform.position = pos + centerPos;

            //�m�b�N�o�b�N����ɓn�����e�̈ړ�����
            movingDirection = myTransform.position - previousPosition;
        }
    }

    //����������
    private void Explode()
    {
        if (explosionParticle != null)
        {
            //�����𐶐�
            GameObject particle =
                Instantiate(explosionParticle, myTransform.position, Quaternion.identity);

            particle.GetComponent<PlayParticles>().Play();

            //particleLifeSpan�b��Ƀp�[�e�B�N��������
            Destroy(particle, particleLifeSpan);

            //���ʉ����Đ�
            SoundManager.uniqueInstance.Play("����1");

            Debug.Log("����!!");
        }
        else
            Debug.LogWarning("�p�[�e�B�N��������܂���");
    }


    //�G�ɓ��������ꍇ�̏���
    private void OnTriggerEnter(Collider other)
    {
        //�_���[�W���󂯂邱�Ƃ��ł���I�u�W�F�N�g���擾
        var applicableDamageObject = other.gameObject.GetComponent<IApplicableDamageEnemy>();
        if (applicableDamageObject != null)
        {
            //���g���\��
            gameObject.SetActive(false);

            //��������
            Explode();

            //�_���[�W��^����
            applicableDamageObject.ReceiveDamage(damage);

            //�m�b�N�o�b�N�ł���I�u�W�F�N�g�̏ꍇ
            var knockbackObject = other.gameObject.GetComponent<IApplicableKnockback>();
            if (knockbackObject != null)
            {
                //�m�b�N�o�b�N����
                knockbackObject.Knockback(knockbackForce, movingDirection);
            }

            //���g��j�󂷂�
            Destroy(gameObject, bombLifeSpan);
        }
    }
}
