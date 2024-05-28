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

    //�Q�̐����ƊǗ�������R���|�[�l���g
    public EnemyFlockManager flockManager;

    //�o���l�I�u�W�F�N�g
    [SerializeField]
    private GameObject expPrefab;

    [SerializeField]
    private Rigidbody rigidb;

    //�A�j���[�V�����p�R���|�[�l���g
    [SerializeField]
    private Animator animator;

    //���e�ɓ����������ǂ���
    public bool isHitting;

    //�G�̏��
    public EnemySetting.EnemyData enemyData;

    //�m�b�N�o�b�N���̓G�̒�~����(�b)
    private float knockbackPauseTime;

    //�̗�
    private float health;

    //���ς�臒l
    private float innerProductThred;

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

        //�G�̏�����
        isHitting = false;
        health = enemyData.maxHealth;
        knockbackPauseTime = 1f;
        //����p����ς�臒l�Ɏg��
        innerProductThred = Mathf.Cos(enemyData.fieldOfView * Mathf.Deg2Rad);
    }

    private void Update()
    {
        //�ߗׂ̌̂��擾����
        enemyFlocking.AddNeighbors(flockManager, innerProductThred);
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

    //�m�b�N�o�b�N���ꂽ�ꍇ�Ɉړ����ĊJ������
    private void RestartMoving()
    {
        isHitting = false;
    }

    //�_���[�W���󂯂�(�C���^�[�t�F�[�X�Ŏ���)
    public void ReceiveDamage(float damage)
    {
        health -= damage;
        Debug.Log($"�G��{damage} ���󂯂�");

        //�̗͂�0�ɂȂ����ꍇ�A���S
        if (health <= 0)
            Dead();
    }

    //���S�������̏���
    private void Dead()
    {
        //�Q���玩�g���폜
        if (flockManager != null)
            flockManager.boids.Remove(this.gameObject);

        //�|�����G�̐��𑝂₷
        GameManager.Instance.deadEnemyMun++;

        //���g��j��
        Destroy(this.gameObject);
        //�A�C�e���𐶐�
        Instantiate(expPrefab, this.transform.position, expPrefab.transform.rotation);
    }

    //�v���C���[��isTrigger�łȂ��R���C�_�[�Ɠ����蔻����s��
    private void OnCollisionEnter(Collision collision)
    {
        //�_���[�W���󂯂邱�Ƃ��ł���I�u�W�F�N�g���擾
        var applicableDamageObject = collision.gameObject.GetComponent<IApplicableDamage>();

        if (applicableDamageObject != null)
            //�_���[�W���󂯂�����
            applicableDamageObject.ReceiveDamage(enemyData.attack);
    }
}
