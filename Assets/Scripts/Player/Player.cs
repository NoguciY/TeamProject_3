using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

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
    private PlayerDefaultStatus playerStatus;

    //�������ڃR���|�[�l���g
    [SerializeField]
    private PowerUpItems powerUpItems;

    //�Q�[���}�l�[�W���[
    [SerializeField]
    private GameManager gameManager;

    //���e���Ǘ�����R���|�[�l���g
    [SerializeField]
    private BombManager bombManager;

    //�A�C�e���p�̃R���C�_�[�R���|�[�l���g
    public CapsuleCollider capsuleColliderForItem;

    //���e�̏c���̔���
    //private float bombHelfHeight;

    //�ݒu�^���e�̉�]�l
    //private Quaternion bombRotation;

    //�񕜗͂̃N�[���^�C��
    private float resilienceCoolTime;

    //���e�g�p�\�t���O
    private bool[] canUseBombFlags = new bool[3];

    //�Q�b�^�[
    public PowerUpItems GetPowerUpItems => powerUpItems;
    public PlayerEvent GetPlayerEvent => playerEvent;

    //�J����(��)
    private Camera mainCamera;

    //�V�����{����ǉ�����J�E���^�[
    private int newBombCounter = 0;

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

        //���x���̏�����
        playerLevelUp.InitLevel();

        //�񕜗͂̃N�[���^�C��������
        resilienceCoolTime = 1f;

        mainCamera = Camera.main;

        //���e�̏�����
        bombManager.Initialize();
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


        var screenPos = mainCamera.WorldToScreenPoint(transform.position);

        var direction = Input.mousePosition - screenPos;

        var angle = Utilities.GetAngle(Vector3.zero, direction);

        var angles = transform.localEulerAngles;
        angles.y = angle - 0;
        transform.localEulerAngles = angles;


        //���e�𐶐�����
        //GenerateBomb();

        if (Input.GetMouseButtonDown(0))
        {
            bombManager.GeneratePlantedBomb();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bombManager.GenerateKnockbackBomb();
        }

        //�����񕜂���
        AutomaticRecovery();

        //���x���A�b�v
        if (playerLevelUp.GetLevel == 10)
        {
            playerLevelUp.LevelUp(playerEvent.AddNewBombEvent);
        }
        else if (playerLevelUp.GetLevel == 20)
        {

        }
        else if (playerLevelUp.GetLevel == 30)
        {

        }
        else
            playerLevelUp.LevelUp(playerEvent.levelUpEvent);


        //�̗͂�0�ɂȂ����ꍇ�A�Q�[���I�[�o�[
        if (lifeController.IsDead())
        {
            //�̗̓Q�[�W���O�o�Ȃ��ꍇ�A�̗̓Q�[�W��0�ɂ���
            playerEvent.gameOverEvent.Invoke();
        }
    }

    //�_���[�W���󂯂�֐�(�C���^�[�t�F�[�X�Ŏ���)
    public void ReceiveDamage(float damage)
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


    //���e�𐶐�����
    private void GenerateBomb()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //�ݒu�^���e�𐶐�
            bombManager.GeneratePlantedBomb();
        }

        //10���x���̏ꍇ
        if (canUseBombFlags[0])
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //�N�[���_�E�����I����Ă���ꍇ
                //�m�b�N�o�b�N���e�𐶐�
                bombManager.GenerateKnockbackBomb();
            }
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

    //�V�������e���g�p�\�ɂ���
    public void NewBombEnabled()
    {
        if (newBombCounter < canUseBombFlags.Length)
        {
            //���e���g�p�\�ɂ���t���O�����Ă�
            canUseBombFlags[newBombCounter] = true;
            
            //���ɂ��̊֐����Ă΂ꂽ���ɁA
            //�ʂ̔��e���g�p�\�ɂ���t���O�����Ă邽�߂ɃJ�E���^�[�����Z����
            newBombCounter++;
        }
    }
}
