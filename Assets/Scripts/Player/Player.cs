using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using static BombManager;

public class Player : MonoBehaviour, IApplicableDamage, IGettableItem
{
    //�l�̓��x���A�b�v�ɂ���đ�������邽�߁Apublic
    //private�ɂ��邢�����@�͉����Ȃ���
    [Header("�ő�HP")]
    public float maxLife;

    [Header("�ړ����x")] 
    public float speed;

    [Header("����͈͗�")]
    public float collectionRangeRate;

    [Header("�h���")]
    public float difense;

    [Header("�񕜗�")]
    public float resilience;

    [SerializeField, Header("�񕜗͂̃N�[���^�C��(�b)")]
    private float resilienceCoolTime;

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

    //�������ڃR���|�[�l���g
    [SerializeField]
    private PlayerPowerUpItems powerUpItems;

    //�ړ��Ɋւ���R���|�[�l���g
    [SerializeField]
    private PlayerMove playerMove;

    //���e���Ǘ�����R���|�[�l���g
    [SerializeField]
    private BombManager bombManager;

    //�A�j���[�V�����Ɋւ���R���|�[�l���g
    [SerializeField]
    private PlayerAnimation playerAnimation;

    //�񕜗͂̌o�ߎ���
    private float deltaTimeResilience;

    //���e�g�p�\�t���O
    private bool[] enableBombs;

    //�J����
    private Camera mainCamera;

    //�V�����{����ǉ�����J�E���^�[
    //[SerializeField]
    private int newBombCounter;

    //�V�����{����ǉ����钼�O�̃��x��(10,20,30�Œǉ�)
    [SerializeField]
    private int[] addBombLevels;

    //�g�����X�t�H�[��
    private Transform myTransform;

    //�Q�b�^�[
    public PlayerPowerUpItems GetPowerUpItems => powerUpItems;
    public PlayerEvent GetPlayerEvent => playerEvent;
    public LifeController GetLifeController => lifeController;
    public int GetNewBombCounter => newBombCounter;
    public PlayerLevelUp GetPlayerLevelUp => playerLevelUp;
    public BombManager GetBombManager => bombManager;


    private void Start()
    {
        myTransform = transform;

        mainCamera = Camera.main;

        //�̗͂̏�����
        lifeController.InitializeLife(maxLife);
        
        //�̗̓Q�[�W��̗͂̍ő�l�ɂ���
        playerEvent.getMaxLifeEvent.Invoke(maxLife);

        //���x���̏�����
        playerLevelUp.InitLevel();

        //���e�̏�����
        bombManager.Initialize();

        enableBombs = new bool[Enum.GetNames(typeof(Utilities.AddedBombType)).Length];

        //0���甚�e�̎�ސ�-1�܂ŃJ�E���g����
        newBombCounter = 0;
    }

    void Update()
    {
        //�Q�[���I�[�o�[���ǂ���
        if (!lifeController.IsDead())
        {
            if (playerMove.InputMove(speed) != Vector3.zero)
            {
                //�ړ�����
                myTransform.position += playerMove.InputMove(speed);

                //����A�j���[�V�����Đ�
                playerAnimation.SetRunAnimation();
            }
            else
                //�ҋ@�A�j���[�V�����Đ�
                playerAnimation.SetIdleAnimation();

            //�}�E�X�J�[�\���̕�������������
            UpdateRotationForMouse();

            //���e�𐶐�����
            GenerateBomb();

            //���e�e�X�g
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    bombManager.GeneratePlantedBomb();
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    bombManager.GenerateKnockbackBombs();
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    bombManager.GenerateHomingBomb();
            //}



            //���x���A�b�v
            if (playerLevelUp.GetLevel == addBombLevels[newBombCounter])
                playerLevelUp.LevelUp(playerEvent.addNewBombEvent);
            else
                playerLevelUp.LevelUp(playerEvent.levelUpEvent);

            //�����񕜂���
            HealAutomatically();
        }
        else
        {
            //�Q�[���I�[�o�[�C�x���g����
            playerEvent.gameOverEvent.Invoke();   
        }
    }

    //�_���[�W���󂯂�֐�(�C���^�[�t�F�[�X�Ŏ���)
    public void ReceiveDamage(float damage)
    {
        float totalDamage = -damage + difense;
        
        //�_���[�W�����̒l�ɂȂ�Ȃ��悤�ɂ���
        if(totalDamage > 0)
            totalDamage = 0;

        lifeController.AddValueToLife(totalDamage);
        
        playerEvent.addLifeEvent.Invoke(totalDamage);

        //���ʉ��𗬂�
        SoundManager.uniqueInstance.Play("�_���[�W");
        
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
        //���e�̃N�[���^�C�����v������
        for(int i = 0; i < Utilities.BOMBTYPENUM; i++)
            bombManager.CountBombCoolTime(i);

        //�������e�𐶐�
        bombManager.GenerateThrowingBomb();

        //1�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (enableBombs[0])
            if (Input.GetKeyDown(KeyCode.Alpha1))
                //�ݒu�^���e�𐶐�
                bombManager.GeneratePlantedBomb(playerEvent.coolDownEvent);

        //2�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (enableBombs[1])
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
                //�m�b�N�o�b�N���e�𐶐�
                bombManager.GenerateKnockbackBombs(playerEvent.coolDownEvent);
        }
        //3�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (enableBombs[2])
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
                //�U�����e�𐶐�
                bombManager.GenerateHomingBomb(playerEvent.coolDownEvent);
        }
    }

    //�̗͂������񕜂���
    private void HealAutomatically()
    {
        //�񕜗͂��O���傫���ꍇ����
        //�Q�[���I�[�o�[�łȂ��ꍇ
        if (resilience > 0)
        {
            //�o�ߎ���
            deltaTimeResilience += Time.deltaTime;
            
            //�񕜂ɂ̓N�[���^�C����݂���
            if (deltaTimeResilience >= resilienceCoolTime)
            {
                //�̗͂��񕜗͕�����
                lifeController.AddValueToLife(resilience);

                //�̗̓Q�[�W���񕜗͕�����
                playerEvent.addLifeEvent.Invoke(resilience);

                deltaTimeResilience = 0;
            }
        }
    }

    //�V�������e���g�p�\�ɂ���
    public void EnableNewBomb()
    {
        if (newBombCounter < enableBombs.Length)
        {
            //���e���g�p�\�ɂ���t���O�����Ă�
            enableBombs[newBombCounter] = true;
            
            //���ɂ��̊֐����Ă΂ꂽ���ɁA
            //�ʂ̔��e���g�p�\�ɂ���t���O�����Ă邽�߂ɃJ�E���^�[�����Z����
            if(newBombCounter <= 1)
            newBombCounter++;
        }
    }

    public void Heal(float value)
    {
        lifeController.AddValueToLife(value);

        //�̗̓Q�[�W���񕜗͕�����
        playerEvent.addLifeEvent.Invoke(value);
    }
}
