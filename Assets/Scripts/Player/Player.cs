using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IApplicableDamage, IGettableItem
{
    //�ő�HP
    //�l�̓��x���A�b�v�ɂ���đ�������邽�߁Apublic
    //private�ɂ��邢�����@�͉����Ȃ���
    public float maxLife;

    //�ړ����x
    public float speed;

    //����͈͗�
    public float collectionRangeRate;

    //�h���
    public float difence;

    //�񕜗�
    public float resilience;

    //���e�v���n�u
    public GameObject bombPrefab;

    //�̗̓R���|�[�l���g
    [SerializeField]
    private LifeController lifeController;

    //���x���A�b�v�R���|�[�l���g
    [SerializeField]
    private PlayerLevelUp playerLevelUp;

    //�v���C���[�̃C�x���g�R���|�[�l���g
    public PlayerEvent playerEvent;

    //�v���C���[�X�e�[�^�X�R���|�[�l���g
    [SerializeField]
    private PlayerStatus playerStatus;

    //�������ڃR���|�[�l���g
    [SerializeField]
    private PowerUpItems powerUpItems;

    //�A�C�e���p�̃R���C�_�[�R���|�[�l���g
    //[SerializeField]
    public CapsuleCollider capsuleColliderForItem;

    //���e�̏c���̔���
    private float bombHelfHeight;

    //�ݒu�^���e�̉�]�l
    private Quaternion bombRotation;

    //�Q�b�^�[
    public PowerUpItems GetPowerUpItems { get { return powerUpItems; } }
    //public float GetMaxLife { get { return maxLife; } }

    private void Start()
    {
        //�X�e�[�^�X�̏�����
        maxLife = playerStatus.maxLife;
        speed = playerStatus.speed;
        collectionRangeRate = playerStatus.collectionRangeRate;
        difence = playerStatus.difence;
        resilience = playerStatus.resilience;

        //�̗͂̏�����
        lifeController.InitializeLife(maxLife);
        
        //�̗̓Q�[�W��̗͂̍ő�l�ɂ���
        playerEvent.getMaxLifeEvent.Invoke(maxLife);

        //���e�֌W�̒l���擾
        bombHelfHeight = bombPrefab.GetComponent<PlantedBomb>().GetHalfHeight();
        bombRotation = bombPrefab.transform.rotation;

        //���x���̏�����
        playerLevelUp.InitLevel();
    }

    void Update()
    {
        //�Ⴆ�΁AWD���ꏏ�ɉ�����W�������Ă���Ƃ����A�����i��ł���̂ŏC������
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= speed * transform.right * Time.deltaTime;
        }

        //���e�𐶐�����
        InstantiatePlantedBomb();

        //�����񕜂���
        AutomaticRecovery();

        //�̗͂�0�ɂȂ����ꍇ�A�Q�[���I�[�o�[
        if (lifeController.IsDead())
        {
            playerEvent.gameOverEvent.Invoke();
        }

        //���x���A�b�v
        playerLevelUp.LevelUp(playerEvent.levelUpEvent);
    }

    //�_���[�W���󂯂�֐�(�C���^�[�t�F�[�X�Ŏ���)
    public void RecieveDamage(float damage)
    {
        float totalDamage = -damage + difence;
        lifeController.AddValueToLife(totalDamage);
        playerEvent.damageEvent.Invoke(totalDamage);
        Debug.Log($"�v���C���[��{totalDamage}�_���[�W�H�����\n" +
            $"�c��̗̑́F{lifeController.GetLife}");
    }


    //�o���l���擾����֐�(�C���^�[�t�F�[�X�Ŏ���)
    public void GetExp(int exp)
    {
        //�o���l��������
        playerLevelUp.AddExp(exp);

        //�o���l�Q�[�W���X�V����C�x���g�����s
        playerEvent.expEvent.Invoke(playerLevelUp.GetExp, playerLevelUp.GetNeedExp);
    }


    //�ݒu�^���e�𐶐�����
    private void InstantiatePlantedBomb()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //�����ʒu���v�Z���Đ���
            Vector3 spawnPos = transform.position + Vector3.up * bombHelfHeight;
            Instantiate(bombPrefab, spawnPos, bombRotation);
        }
    }

    //�̗͂������񕜂���
    private void AutomaticRecovery()
    {
        //�񕜗͂��O���傫���ꍇ
        if (resilience > 0)
        {
            lifeController.AddValueToLife(resilience);
        }
    }
}
