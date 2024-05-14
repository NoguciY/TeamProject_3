using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UIElements;

public class ThrowingBomb : MonoBehaviour
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
    private float damage;

    [SerializeField,Header("���ː�")]
    private int count;

    [SerializeField, Header("�e��(m/s)")]
    private float speed;

    [SerializeField, Header("���ˊԊu")]
    private float interval;

    [SerializeField, Header("1�b�Ԃɉ�]����p�x(degree/s)")]
    private float angleOfRotationPerSecond;

    [SerializeField, Header("�����p�[�e�B�N�������ォ��j������܂ł̎���(�b)")]
    private float particleLifeSpan;

    [SerializeField, Header("�j�����鎞��(s)")]
    private float bombLifeSpan;
   
    //�g�����X�t�H�[��
    private Transform myTransform;

    //���x
    private Vector3 velocity;

    //���˃^�C�~���O���Ǘ�
    //private float timer;

    private void Start()
    {
        myTransform = transform;
    }

    // ���t���[���Ăяo�����֐�
    private void Update()
    {
        //timer += Time.deltaTime;

        //if(playerTransform != null)
        // �ړ�����
        myTransform.localPosition += velocity * Time.deltaTime;

        //                                            �v���C���[�̃|�W�V����
        //var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        ////
        //var direction = Input.mousePosition - screenPos;

        ////
        //var angle = Utilities.GetAngle(Vector3.zero, direction);

        //if (timer < interval) return;

        //�e�̔��˃^�C�~���O���Ǘ�����^�C�}�[�����Z�b�g����
        //timer = 0;

        //�e�𔭎˂���
        //ShootNWay(speed, count);

        //��]����
        Rotate();

        Destroy(gameObject, bombLifeSpan);
    }

    // �e�𔭎˂��鎞�ɏ��������邽�߂̊֐�
    public void Init()
    {
        if (playerTransform != null)
        {
            // �e�̔��ˊp�x���x�N�g���ɕϊ�����
            var direction = playerTransform.forward;

            // ���ˊp�x�Ƒ������瑬�x�����߂�
            velocity = direction * speed;
        }
    }

    //
    //private void ShootNWay(float speed, int count)
    //{
    //    var pos = transform.localPosition;  //�v���C���[�̈ʒu
    //    var rot = transform.localRotation;  //�v���C���[�̌���

    //    if (count == 1)
    //    {
    //        //���˂���e�𐶐�����
    //        var shot = Instantiate(gameObject, pos, rot);

    //        //�e�𔭎˂�������Ƒ�����ݒ肷��
    //        //Init(speed);
    //    }
    //}

    //x�������ɉ�]����
    private void Rotate()
    {
        //�p���x(�P�ʂ�rad/s)�ɂ���)
        //float angularVelocity = angleOfRotationPerSecond * Mathf.Rad2Deg;

        //1�t���[���ɉ�]����p�x
        //float angle = angularVelocity * Time.deltaTime;
        float angle = angleOfRotationPerSecond * Time.deltaTime;

        //���t���[����]������
        myTransform.Rotate(angle, 0, 0);
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
            Explosion();

            //�_���[�W��^����
            applicableDamageObject.ReceiveDamage(damage);

            //���g��j�󂷂�
            Destroy(gameObject, bombLifeSpan);
        }
    }
}
