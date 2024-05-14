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

    //�A�C�e���p�̃R���C�_�[�R���|�[�l���g
    public CapsuleCollider capsuleColliderForItem;

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

    //�񕜗͂̃N�[���^�C��
    private float resilienceCoolTime;

    //���e�g�p�\�t���O
    private bool[] canUseBombFlags;

    //�J����(��)
    private Camera mainCamera;

    //�V�����{����ǉ�����J�E���^�[
    private int newBombCounter;

    //�V�����{����ǉ����钼�O�̃��x��(10,20,30�Œǉ�������)
    private int addBombLevel;

    //�g�����X�t�H�[��
    private Transform myTransform; 


    //�Q�b�^�[
    public PowerUpItems GetPowerUpItems => powerUpItems;
    public PlayerEvent GetPlayerEvent => playerEvent;
    public int GetNewBombCounter => newBombCounter;

    private void Start()
    {
        myTransform = transform;

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

        canUseBombFlags = new bool[3];

        //0���甚�e�̎�ސ�-1�܂ŃJ�E���g����
        newBombCounter = 0;

        //�����l�͍ŏ��Ƀ{����ǉ�������1�O�̃��x��
        addBombLevel = 9;
    }

    void Update()
    {
        //�Ⴆ�΁AWD���ꏏ�ɉ�����W�������Ă���Ƃ����A�����i��ł���̂ŏC������
        if (Input.GetKey(KeyCode.W))
        {
            myTransform.position += speed * myTransform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            myTransform.position -= speed * myTransform.forward * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            myTransform.position += speed * myTransform.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            myTransform.position -= speed * myTransform.right * Time.deltaTime;
        }

        //�}�E�X�J�[�\���̕�������������
        UpdateRotationForMouse();

        //���e�𐶐�����
        GenerateBomb();

        //���e�����e�X�g�p�A��ŏ���
        if (Input.GetKeyDown(KeyCode.Alpha1))
            bombManager.GenerateHomingBomb();

        //�����񕜂���
        AutomaticRecovery();

        //���x���A�b�v
        if (playerLevelUp.GetLevel == addBombLevel * (newBombCounter + 1))
            playerLevelUp.LevelUp(playerEvent.AddNewBombEvent);
        else
            playerLevelUp.LevelUp(playerEvent.levelUpEvent);


        //�Q�[���I�[�o�[
        if (lifeController.IsDead())
            //�̗̓Q�[�W���O�o�Ȃ��ꍇ�A�̗̓Q�[�W��0�ɂ���
            playerEvent.gameOverEvent.Invoke();
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

    //�}�E�X�J�[�\�������ɉ�]���W���X�V����
    private void UpdateRotationForMouse()
    {
        //�v���C���[�̃��[���h��ԍ��W���X�N���[�����W�ɕϊ�
        Vector3 screenPos = mainCamera.WorldToScreenPoint(myTransform.position);

        //�v���C���[����}�E�X�J�[�\���̕���
        Vector3 direction = Input.mousePosition - screenPos;

        //2�̃x�N�g���̂Ȃ��p���擾
        float angle = Utilities.GetAngle(Vector3.zero, direction);

        //�v���C���[�̉�]���W�̍X�V
        Vector3 angles = myTransform.localEulerAngles;
        angles.y = angle;
        myTransform.localEulerAngles = angles;
    }

    //���e�𐶐�����
    private void GenerateBomb()
    {


        //���ԂŎ������������悤�ɂ���
        if (Input.GetMouseButtonDown(0))
            //�������e�𐶐�
            bombManager.GenerateThrowingBomb();

        //1�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (canUseBombFlags[0])
            if (Input.GetKeyDown(KeyCode.Alpha1))
                //�N�[���_�E�����I����Ă���ꍇ
                //�ݒu�^���e�𐶐�
                bombManager.GeneratePlantedBomb();

        //2�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (canUseBombFlags[1])
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //�N�[���_�E�����I����Ă���ꍇ
                //�m�b�N�o�b�N���e�𐶐�
                bombManager.GenerateKnockbackBombs();
            }
        }
        //3�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (canUseBombFlags[2])
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("�U�����e�I");
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

                resilienceCoolTime += resilienceCoolTime;
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
