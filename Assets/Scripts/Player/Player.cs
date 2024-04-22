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

    //�v���C���[�̃C�x���g�R���|�[�l���g
    [SerializeField]
    private PlayerEvent playerEvent;

    //�̗̓R���|�[�l���g
    [SerializeField]
    private LifeController lifeController;

    //���x���A�b�v�R���|�[�l���g
    [SerializeField]
    private PlayerLevelUp playerLevelUp;

    //�v���C���[�X�e�[�^�X�R���|�[�l���g
    [SerializeField]
    private PlayerStatus playerStatus;

    //�������ڃR���|�[�l���g
    [SerializeField]
    private PowerUpItems powerUpItems;

    //�Q�[���}�l�[�W���[
    [SerializeField]
    private GameManager gameManager;

    //�A�C�e���p�̃R���C�_�[�R���|�[�l���g
    public CapsuleCollider capsuleColliderForItem;

    //���e�̏c���̔���
    private float bombHelfHeight;

    //�ݒu�^���e�̉�]�l
    private Quaternion bombRotation;

    //�񕜗͂̃N�[���^�C��
    float resilienceCoolTime;

    //�Q�b�^�[
    public PowerUpItems GetPowerUpItems { get { return powerUpItems; } }
    public PlayerEvent GetPlayerEvent => playerEvent;

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

        //�񕜗͂̃N�[���^�C��������
        resilienceCoolTime = 1f;
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

        //���x���A�b�v
        playerLevelUp.LevelUp(playerEvent.levelUpEvent);

        //�̗͂�0�ɂȂ����ꍇ�A�Q�[���I�[�o�[
        if (lifeController.IsDead())
        {
            //�̗̓Q�[�W���O�o�Ȃ��ꍇ�A�̗̓Q�[�W��0�ɂ���

            playerEvent.gameOverEvent.Invoke();
        }
    }

    //�_���[�W���󂯂�֐�(�C���^�[�t�F�[�X�Ŏ���)
    public void RecieveDamage(float damage)
    {
        float totalDamage = -damage + difence;
        lifeController.AddValueToLife(totalDamage);
        playerEvent.addLifeEvent.Invoke(totalDamage);
        Debug.Log($"�v���C���[��{-totalDamage}�_���[�W�H�����\n" +
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
        //�񕜗͂��O���傫���ꍇ����
        //�Q�[���I�[�o�[�łȂ��ꍇ
        if (resilience > 0�@&& !lifeController.IsDead())
        {
            //�o�ߎ���
            float deltaTime = gameManager.GetDeltaTimeInMain;
            
            //�񕜂ɂ̓N�[���^�C����݂���
            if (deltaTime >= resilienceCoolTime)
            {
                //�̗͂��񕜗͕�����
                lifeController.AddValueToLife(resilience);
                //�̗̓Q�[�W���񕜗͕�����
                playerEvent.addLifeEvent.Invoke(resilience);

                resilienceCoolTime++;
            }
        }
    }
}
