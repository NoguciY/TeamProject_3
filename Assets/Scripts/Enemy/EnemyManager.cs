using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�X�e�[�g
public enum StateType
{
    Idle,           //�ҋ@
    Move,           //�ړ�
    Attack,         //�U��
    ReceiveDamage,  //��_���[�W
    Dead,           //���S
}

public class EnemyManager : MonoBehaviour, IApplicableKnockback, IApplicableDamageEnemy
{
    //�G�̌Q��ł̈ړ��p�R���|�[�l���g
    [SerializeField]
    private EnemyFlocking enemyFlocking;

    [SerializeField]
    private Rigidbody rigidb;

    //�A�j���[�V�����p�R���|�[�l���g
    [SerializeField]
    private Animator animator;

    //���e�ɓ����������ǂ���
    public bool isHitting = false;

    //�m�b�N�o�b�N���̓G�̒�~����(�b)
    private float knockbackPauseTime = 1f;

    //�Q�b�^�[
    public EnemyFlocking GetEnemyFlocking => enemyFlocking;
    public Rigidbody GetRigidb => rigidb;
    public Animator GetAnimator => animator;

    //�X�e�[�g�}�V��
    //private StateMachine<EnemyManager> stateMachine;

    private void Start()
    {
        //�X�e�[�g�}�V����`
        //stateMachine = new StateMachine<EnemyManager>(this);
        //stateMachine.Add<StateEnemyIdle>((int)StateType.Idle);
        //stateMachine.Add<StateEnemyMove>((int)StateType.Move);
        //stateMachine.Add<StateEnemyAttack>((int)StateType.Attack);
        //stateMachine.Add<StateEnemyReceiveDamage>((int)StateType.ReceiveDamage);
        //stateMachine.Add<StateEnemyDead>((int)StateType.Dead);

        ////�X�e�[�g�J�n
        //stateMachine.OnStart((int)StateType.Idle);
    }

    private void Update()
    {
        //stateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        //stateMachine.OnFixedUpdate();

        //�ړ�����
        if (!isHitting)
        {
            Vector3 moveForce = enemyFlocking.Move();
            rigidb.velocity = new Vector3(moveForce.x, rigidb.velocity.y, moveForce.z);
        }
    }

    //�m�b�N�o�b�N����(�C���^�[�t�F�[�X�Ŏ���)
    public void Knockback(float knockbackForce, Vector3 bombMovingDirection)
    {
        //���g�̓������~�߂�
        isHitting = true;

        //����2�b��ɍĎn��
        Invoke("RestartMoving", knockbackPauseTime);

        //�m�b�N�o�b�N����
        Vector3 knockbackDistance = (bombMovingDirection - rigidb.velocity).normalized;

        //�m�b�N�o�b�N������knockbackForce�̈З͂œG���m�b�N�o�b�N
        rigidb.AddForce(knockbackDistance * knockbackForce, ForceMode.Impulse);

        Debug.Log($"{knockbackDistance * knockbackForce}�Ńm�b�N�o�b�N!!");
    }

    //�ړ����ĊJ������
    private void RestartMoving()
    {
        isHitting = false;
    }

    //�_���[�W���󂯂�(�C���^�[�t�F�[�X�Ŏ���)
    public void ReceiveDamage(float damage)
    {
        Debug.Log($"�G��{damage} ���󂯂�");
    }

    private void OnTriggerEnter(Collider other)
    {
        //�_���[�W���󂯂邱�Ƃ��ł���I�u�W�F�N�g���擾
        var applicableDamageObject = other.gameObject.GetComponent<IApplicableDamage>();

        if (applicableDamageObject != null)
        {
            //�_���[�W���󂯂�����
            applicableDamageObject.ReceiveDamage(10);
        }
    }
}
