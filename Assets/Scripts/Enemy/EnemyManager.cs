using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


public class EnemyManager : MonoBehaviour, IApplicableKnockback, IApplicableDamageEnemy
{
    //�G�̌Q��ł̈ړ��p�R���|�[�l���g
    [SerializeField]
    private EnemyFlocking enemyFlocking;

    //public EnemyFlocking GetEnemyFlocking => enemyFlocking;

    //�Q�̐����ƊǗ�������R���|�[�l���g
    public EnemySpawner enemySpawner;

    //�o���l�I�u�W�F�N�g
    [SerializeField]
    private GameObject experienceValuePrefab;

    [SerializeField]
    private Rigidbody rigidb;

    //�A�j���[�V�����p
    [SerializeField]
    private EnemyAnimation enemyAnimation;

    public Transform playerTransform;

    //���e�ɓ����������ǂ���
    public bool isHitting;

    //�U���\��
    private bool possibleAttack;

    //�G�̏��
    public EnemySetting.EnemyData enemyData;

    //�m�b�N�o�b�N���̓G�̒�~����(�b)
    private float knockbackPauseTime;

    //�̗�
    private float health;

    //���ς�臒l
    private float innerProductThred;

    
    private void Start()
    {
        //�G�̏�����
        isHitting = false;
        possibleAttack = true;
        health = enemyData.maxHealth;
        knockbackPauseTime = 1f;
        enemyFlocking.Init(enemyData.minSpeed, enemyData.maxSpeed);

        //����p����ς�臒l�Ɏg��
        innerProductThred = Mathf.Cos(enemyData.fieldOfView * Mathf.Deg2Rad);
    }

    private void FixedUpdate()
    {
        //���C���Q�[���V�[���̏ꍇ
        if (GameManager.Instance.CurrentSceneType == SceneType.MainGame)
        {
            //�ړ�����
            if (!isHitting)
            {
                //�ߗׂ̌̂��擾����
                enemyFlocking.AddNeighbors(
                    enemySpawner, enemyData.generationOrder, 
                    enemyData.ditectingNeiborDistance, innerProductThred);

                //�ړ�����
                Vector3 moveForce = enemyFlocking.Move(playerTransform, enemyData);
                rigidb.velocity = new Vector3(moveForce.x, rigidb.velocity.y, moveForce.z);
                enemyAnimation.SetRunAnimation();
            }
        }
        //�I�v�V�����V�[���̏ꍇ
        else if (GameManager.Instance.CurrentSceneType == SceneType.Option)
        {
            enemyAnimation.SetIdleAnimation();
        }
    }

    /// <summary>
    /// �m�b�N�o�b�N����(�C���^�[�t�F�[�X�Ŏ���)
    /// </summary>
    /// <param name="knockbackForce">�m�b�N�o�b�N��</param>
    /// <param name="bombMovingDirection">�m�b�N�o�b�N���e�̈ړ�����</param>
    public async void Knockback(float knockbackForce, Vector3 bombMovingDirection)
    {
        //GameObject���j�����ꂽ���ɃL�����Z�����΂��g�[�N�����쐬
        var token = this.GetCancellationTokenOnDestroy();

        //���g�̓������~�߂�
        isHitting = true;

        possibleAttack = false;

        enemyAnimation.SetReceiveDamageAnimation();

        //�m�b�N�o�b�N����
        Vector3 knockbackDistance = bombMovingDirection.normalized;

        //�m�b�N�o�b�N������knockbackForce�̈З͂œG���m�b�N�o�b�N
        rigidb.AddForce(knockbackDistance * knockbackForce, ForceMode.Impulse);

        Debug.Log($"{knockbackDistance * knockbackForce}�Ńm�b�N�o�b�N!!");

        //�ҋ@��̏���
        try
        {
            //�w�肵���b���҂�
            await UniTask.Delay(TimeSpan.FromSeconds(knockbackPauseTime), cancellationToken: token);

            isHitting = false;

            possibleAttack = true;
        }
        //�ҋ@���ɓG���j�����ꂽ�ꍇ
        catch (OperationCanceledException exception)
        {
            Debug.Log("�m�b�N�o�b�N�������L�����Z������܂����B");
        }
    }

    /// <summary>
    /// �_���[�W���󂯂�(�C���^�[�t�F�[�X�Ŏ���)
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    public void ReceiveDamage(float damage)
    {
        health -= damage;
        Debug.Log($"�G��{damage} ���󂯂�");

        //�̗͂�0�ɂȂ����ꍇ�A���S
        if (health <= 0)
        {
            possibleAttack = false;
            enemyAnimation.SetDeadAnimation();
        }
    }

    /// <summary>
    /// Animation Event�ŎQ�Ƃ��Ă���
    /// ����A�j���[�V�����I����ɍs�����S�������̏���
    /// </summary>
    private void Dead()
    {
        //�Q���玩�g���폜
        if (enemySpawner != null)
        {
            enemySpawner.boids[enemyData.generationOrder].Remove(this.gameObject);
        }

        //�|�����G�̐��𑝂₷
        GameManager.Instance.deadEnemyMun++;

        //���g��j��
        Destroy(this.gameObject);

        //�A�C�e���𐶐�
        GameObject experienceValue = Instantiate(experienceValuePrefab, this.transform.position, experienceValuePrefab.transform.rotation);
        GameManager.Instance.items.Add(experienceValue.GetComponent<ItemExperienceValue>());
    }

    /// <summary>
    /// �v���C���[��isTrigger�łȂ��R���C�_�[�Ɠ����蔻����s��
    /// </summary>
    /// <param name="collision">�G</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (possibleAttack)
        {
            //�_���[�W���󂯂邱�Ƃ��ł���I�u�W�F�N�g���擾
            var applicableDamageObject = collision.gameObject.GetComponent<IApplicableDamage>();

            if (applicableDamageObject != null)
            {
                //�_���[�W���󂯂�����
                applicableDamageObject.ReceiveDamage(enemyData.attack);
            }
        }
    }
}
