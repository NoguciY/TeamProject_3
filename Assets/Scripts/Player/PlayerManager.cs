using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IApplicableDamage, IGettableItem
{
    //�l�̓��x���A�b�v�ɂ���đ�������邽�߁Apublic
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

    public PlayerEvent GetPlayerEvent => playerEvent;

    //�̗̓R���|�[�l���g
    [SerializeField]
    private PlayerLifeController lifeController;

    public PlayerLifeController GetLifeController => lifeController;

    //���x���A�b�v�R���|�[�l���g
    [SerializeField]
    private PlayerLevelUp playerLevelUp;

    public PlayerLevelUp GetPlayerLevelUp => playerLevelUp;

    //�������ڃR���|�[�l���g
    [SerializeField]
    private PlayerPowerUpItems powerUpItems;

    public PlayerPowerUpItems GetPowerUpItems => powerUpItems;

    //�ړ��Ɋւ���R���|�[�l���g
    [SerializeField]
    private PlayerMove playerMove;

    //���e���Ǘ�����R���|�[�l���g
    [SerializeField]
    private BombManager bombManager;

    public BombManager GetBombManager => bombManager;

    //�A�j���[�V�����Ɋւ���R���|�[�l���g
    [SerializeField]
    private PlayerAnimation playerAnimation;

    //�V�����{����ǉ����钼�O�̃��x��
    [SerializeField]
    private int[] addBombLevels;

    //�񕜗͂̌o�ߎ���
    private float deltaTimeResilience;

    //���e�g�p�\�t���O
    private bool[] enableBombs;

    //�J����
    private Camera mainCamera;

    //�V�����{����ǉ�����J�E���^�[
    private int newBombCounter;

    public int GetNewBombCounter => newBombCounter;

    private Transform myTransform;


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

        //�o���l�Q�[�W�̍ő�l�̍X�V
        playerEvent.getMaxExperienceValueEvent.Invoke(
            playerLevelUp.GetExperienceValue, playerLevelUp.GetNeedExperienceValue);

        //���e�̏�����
        bombManager.Initialize();

        enableBombs = new bool[Enum.GetNames(typeof(Utilities.AddedBombType)).Length];

        //0���甚�e�̎�ސ�-1�܂ŃJ�E���g����
        newBombCounter = 0;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentSceneType != SceneType.MainGame) return;

        if (!lifeController.GetIsDead)
        {
            if (playerMove.InputMove(speed) != Vector3.zero)
            {
                //�ړ�����
                myTransform.position += playerMove.InputMove(speed);

                //����A�j���[�V�����Đ�
                playerAnimation.SetRunAnimation();
            }
            else
            {
                //�ҋ@�A�j���[�V�����Đ�
                playerAnimation.SetIdleAnimation();
            }

            //�}�E�X�J�[�\���̕�������������
            playerMove.UpdateRotationForMouse(mainCamera, myTransform);

            //���e�𐶐�����
            GenerateBomb();

            //�����񕜂���
            HealAutomatically();
        }

        //�Q�[���I�[�o�[�̏ꍇ
        if (lifeController.IsDead())
        {
            GameOver();
        }
    }

    /// <summary>
    /// �_���[�W���󂯂�֐�(�C���^�[�t�F�[�X�Ŏ���)
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    public void ReceiveDamage(float damage)
    {
        if (lifeController.GetIsDead) return;

        //���ʉ�(��_���[�W�� or ���ꉹ)�𗬂�
        if (lifeController.GetLife > damage)
        {
            SoundManager.uniqueInstance.Play("�_���[�W");
        }

        //�ŏI�I�ȃ_���[�W�̒l
        float totalDamage = difense - damage;

        //�_���[�W�����̒l�ɂȂ�Ȃ��悤�ɂ���
        if (totalDamage > 0)
        {
            totalDamage = 0;
        }

        lifeController.AddValueToLife(totalDamage);

        playerEvent.addLifeEvent.Invoke(lifeController.GetLife);

        Debug.Log($"�v���C���[��{-totalDamage}�_���[�W�H�����\n" +
            $"�c��̗̑́F{lifeController.GetLife}");

    }

    /// <summary>
    /// �o���l���擾����֐�(�C���^�[�t�F�[�X�Ŏ���)
    /// </summary>
    /// <param name="experienceValue">�o���l</param>
    public void GetExperienceValue(float experienceValue)
    {
        if (lifeController.GetIsDead) return;

        //�o���l��������
        playerLevelUp.AddExperienceValue(experienceValue);

        //���x���A�b�v������
        playerLevelUp.LevelUp(playerEvent.levelUpEvent, playerEvent.addNewBombEvent,
            addBombLevels[newBombCounter]);

        //�o���l�Q�[�W���X�V����C�x���g�����s
        playerEvent.experienceValueEvent.Invoke(playerLevelUp.GetExperienceValue, playerLevelUp.GetNeedExperienceValue);

    }

    /// <summary>
    /// ���e�𐶐�����
    /// </summary>
    private void GenerateBomb()
    {
        //���e�̃N�[���^�C�����v������
        for (int i = 0; i < Utilities.BOMBTYPENUM; i++)
        {
            bombManager.CountBombCoolTime(i);
        }

        //�������e�𐶐�
        bombManager.GenerateThrowingBomb(playerAnimation);

        //1�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (enableBombs[0])
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //�ݒu�^���e�𐶐�
                bombManager.GeneratePlantedBomb(playerEvent.coolDownEvent);
            }
        }

        //2�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (enableBombs[1])
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //�m�b�N�o�b�N���e�𐶐�
                bombManager.GenerateKnockbackBombs(playerEvent.coolDownEvent);
            }
        }
        //3�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (enableBombs[2])
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //�U�����e�𐶐�
                bombManager.GenerateHomingBomb(playerEvent.coolDownEvent);
            }
        }
    }

    /// <summary>
    /// �̗͂������񕜂���
    /// </summary>
    private void HealAutomatically()
    {
        //�񕜗͂��O���傫���ꍇ
        if (resilience <= 0) return;

        //�o�ߎ���
        deltaTimeResilience += Time.deltaTime;

        //�񕜂ɂ̓N�[���^�C����݂���
        if (deltaTimeResilience >= resilienceCoolTime)
        {
            //�̗͂��񕜗͕�����
            lifeController.AddValueToLife(resilience);

            //�̗̓Q�[�W���񕜗͕�����
            playerEvent.addLifeEvent.Invoke(lifeController.GetLife);

            deltaTimeResilience = 0;
        }
    }

    /// <summary>
    /// �V�������e���g�p�\�ɂ���
    /// </summary>
    public void EnableNewBomb()
    {
        if (newBombCounter >= enableBombs.Length) return;

        //���e���g�p�\�ɂ���t���O�����Ă�
        enableBombs[newBombCounter] = true;

        //���ɂ��̊֐����Ă΂ꂽ���ɁA
        //�ʂ̔��e���g�p�\�ɂ���t���O�����Ă邽�߂ɃJ�E���^�[�����Z����
        if (newBombCounter <= 1)
        {
            newBombCounter++;
        }
    }

    /// <summary>
    /// �񕜂���
    /// </summary>
    /// <param name="recoveryValue"></param>
    public void Heal(float recoveryValue)
    {
        lifeController.AddValueToLife(recoveryValue);

        //�̗̓Q�[�W���񕜗͕�����
        playerEvent.addLifeEvent.Invoke(lifeController.GetLife);
    }

    /// <summary>
    /// ���e���g�p�\����Ԃ�
    /// </summary>
    /// <param name="bombID">���e�̌ŗL�ԍ�</param>
    /// <returns>true:�g�p�\ / false:�g�p�s��</returns>
    public bool CanUseBumb(int bombID)
    {
        if (enableBombs[bombID])
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ����A�j���[�V�����I����A�Q�[���I�[�o�[�C�x���g�̏���������
    /// </summary>
    private async void GameOver()
    {
        //GameObject���j�����ꂽ���ɃL�����Z�����΂��g�[�N�����쐬
        var token = this.GetCancellationTokenOnDestroy();

        SoundManager.uniqueInstance.Play("����");

        playerAnimation.SetDeadAnimation();

        //UniTask���\�b�h�̈�����CancellationToken������
        await UniTask.WaitUntil(() => 
            playerAnimation.isFinishedDeadAnimation, cancellationToken : token);

        playerAnimation.isFinishedDeadAnimation = false;

        //�Q�[���I�[�o�[�C�x���g����
        playerEvent.gameOverEvent.Invoke();
    }
}
