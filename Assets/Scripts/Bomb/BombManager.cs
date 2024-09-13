using UnityEngine;
using static PlayerEvent;

//���e���Ǘ�����N���X
public class BombManager : MonoBehaviour
{
    //���e�̃^�C�v
    public enum BombType
    {
        Throwing,   //�����^
        Planted,    //�ݒu�^
        Knockback,  //�m�b�N�o�b�N�^
        Homing,     //�U���^
    }


    //�������e--------------------------------------------------
    //�v���n�u
    [SerializeField]
    private BombThrowing throwingBomb;

    //public BombThrowing GetBombThrowing => throwingBomb; 

    //�ݒu�^���e------------------------------------------------
    //�v���n�u
    [SerializeField]
    private BombPlanted plantedBomb;

    //�ݒu�^���e�̍����̔���
    private float plantedBombHalfHeight;

    //�ݒu�^���e�̉�]�l
    private Quaternion plantedBombRotation;

    public BombPlanted GetBombPlanted => plantedBomb;

    //�m�b�N�o�b�N���e------------------------------------------
    //�v���n�u
    [SerializeField]
    private BombKnockback knockbackBomb;

    //public BombKnockback GetBombKnockback => knockbackBomb;

    //�m�b�N�o�b�N���e�̍����̔���
    private float knockbackHalfHeight;

    //�m�b�N�o�b�N���e�ƃv���C���[�̊Ԃ̋���
    private float toPlayerDistance;

    //���������m�b�N�o�b�N���e�̐�
    //private int generatedKnockbackBombNum;

    //�U�����e--------------------------------------------------
    //�v���n�u
    [SerializeField]
    private BombHomingSpawner homingBombSpawner;

    //�U�����e�̍����̔���
    private float homingBombHelfHeight;

    //���̑�----------------------------------------------------
    //�����G�t�F�N�g
    [SerializeField, Header("�������e�̔����G�t�F�N�g")]
    private GameObject throwingBombExplosionParticle;

    [SerializeField, Header("�ݒu�^���e�̔����G�t�F�N�g")]
    private GameObject plantedBombExplosionParticle;

    [SerializeField, Header("�m�b�N�o�b�N���e�̔����G�t�F�N�g")]
    private GameObject knockbackBombExplosionParticle;

    [SerializeField, Header("�U�����e�̔����G�t�F�N�g")]
    private GameObject homingBombExplosionParticle;

    //�v���C���[��Transform�R���|�[�l���g
    private Transform playerTransform;

    //�v���C���[�̃R���C�_�[
    private CapsuleCollider playerCapsuleCollider;

    //�v���C���[�̍����̔���
    private float playerHalfHeight;

    //�o�ߎ���
    private float[] deltaTime;

    //�N�[���^�C��
    private float[] coolTime;

    //���e���g�������ǂ���
    private bool[] isUsingBomb;

    //���e���g�p�\��
    private bool[] enableUseBomb;

    //���e�ǉ��J�E���^�[
    private int addBombCounter;

    public int GetAddBombCounter => addBombCounter;


    /// <summary>
    /// �������e�𐶐�����
    /// </summary>
    /// <param name="coolDownEvent">�N�[���_�E���C�x���g</param>
    /// <param name="playerAnimation">�A�j���[�V�����p�N���X</param>
    private void GenerateThrowingBomb(PlayerEvent.CoolDownEvent coolDownEvent, PlayerAnimation playerAnimation)
    {
        if (isUsingBomb[(int)BombType.Throwing]) return;

        //�U���A�j���[�V�����Đ�
        playerAnimation.SetAttackAnimation();

        //�����ʒu���v�Z���Đ���
        Vector3 spawnPos = playerTransform.position + Vector3.up * playerHalfHeight;
        GameObject bombPrefab = Instantiate(throwingBomb.gameObject, spawnPos, playerTransform.rotation);

        //�v���C���[�̈ʒu���Z�b�g
        var throwingBombComponent = bombPrefab.GetComponent<BombThrowing>();
        throwingBombComponent.playerTransform = playerTransform;
        throwingBombComponent.Initialize();

        //�N�[���^�C���̃J�E���g���J�n����t���O
        isUsingBomb[(int)BombType.Throwing] = true;

        //�N�[���^�C���Q�[�W�̃C�x���g�����s
        coolDownEvent.Invoke((int)BombType.Throwing, throwingBomb.GetCoolTime);
    }

    /// <summary>
    /// �ݒu�^���e�𐶐�����
    /// </summary>
    /// <param name="coolDownEvent">�N�[���_�E���C�x���g</param>
    private void GeneratePlantedBomb(PlayerEvent.CoolDownEvent coolDownEvent)
    {
        if (isUsingBomb[(int)BombType.Planted]) return;

        //�����ʒu���v�Z���Đ���
        Vector3 spawnPos = playerTransform.position + Vector3.up * plantedBombHalfHeight;
        GameObject bombPrefab = Instantiate(plantedBomb.gameObject, spawnPos, plantedBombRotation);

        //�N�[���^�C���̃J�E���g���J�n����t���O
        isUsingBomb[(int)BombType.Planted] = true;

        //�N�[���^�C���Q�[�W�̃C�x���g�����s
        coolDownEvent.Invoke((int)BombType.Planted, plantedBomb.GetCoolTime);
    }

    /// <summary>
    /// �m�b�N�o�b�N���e�𐶐�����
    /// </summary>
    /// <param name="coolDownEvent">�N�[���_�E���C�x���g</param>
    private void GenerateKnockbackBombs(PlayerEvent.CoolDownEvent coolDownEvent)
    {
        if (isUsingBomb[(int)BombType.Knockback]) return;

        //��������锚�e���͕ω����邽�߂����Ŏ擾����
        int generatedKnockbackBombNum = knockbackBomb.GetGeneratedBombNum;

        //���e���~��ɓ��Ԋu�ɒu�����߂̊p�x
        float degree = 360 / generatedKnockbackBombNum;

        //y���W���������e�̊�ʒu
        Vector3 offsetPos;

        for (int i = 0; i < generatedKnockbackBombNum; i++)
        {
            //���e�𓙊Ԋu�ɔz�u����
            offsetPos = new Vector3(Mathf.Sin(degree * i * Mathf.Deg2Rad),
                0, Mathf.Cos(degree * i * Mathf.Deg2Rad));

            //�����ʒu(�v���C���[�𒆐S�ɉ~��ɓ��Ԋu�ɔz�u)
            Vector3 spawnPos = playerTransform.position +
                offsetPos * toPlayerDistance + Vector3.up * knockbackHalfHeight;

            GameObject bombPrefab = Instantiate(knockbackBomb.gameObject, spawnPos, Quaternion.identity);

            BombKnockback bombKnockbackComponet = bombPrefab.GetComponent<BombKnockback>();

            //�v���C���[�̈ʒu���Z�b�g
            bombKnockbackComponet.playerTransform = playerTransform;

            bombKnockbackComponet.myID = i;
        }

        //�N�[���^�C���̃J�E���g���J�n����t���O
        isUsingBomb[(int)BombType.Knockback] = true;

        //�N�[���^�C���Q�[�W�̃C�x���g�����s
        coolDownEvent.Invoke((int)BombType.Knockback, knockbackBomb.GetCoolTime);
    }

    /// <summary>
    /// �z�[�~���O���e�𐶐�����
    /// </summary>
    /// <param name="coolDownEvent">�N�[���_�E���C�x���g</param>
    private void GenerateHomingBomb(PlayerEvent.CoolDownEvent coolDownEvent)
    {
        if (isUsingBomb[(int)BombType.Homing]) return;

        Vector3 spawnPos = playerTransform.position + Vector3.up * homingBombHelfHeight * 10;
        GameObject bombPrefab = Instantiate(homingBombSpawner.gameObject, spawnPos, Quaternion.identity);

        //�v���C���[�̈ʒu���Z�b�g
        bombPrefab.GetComponent<BombHomingSpawner>().playerTransform = playerTransform;

        //�N�[���^�C���̃J�E���g���J�n����t���O
        isUsingBomb[(int)BombType.Homing] = true;

        //�N�[���^�C���Q�[�W�̃C�x���g�����s
        coolDownEvent.Invoke((int)BombType.Homing, homingBombSpawner.GetCoolTime);
    }

    /// <summary>
    /// ���e�̃N�[���^�C�����v�����A�Ďg�p�ł���悤�ɂ���
    /// </summary>
    /// <param name="bombID">���e�̌ŗL�ԍ�</param>
    private void CountBombCoolTime(int bombID)
    {
        if (!isUsingBomb[bombID]) return;

        //�o�ߎ��Ԃ��J�E���g
        deltaTime[bombID] += Time.deltaTime;

        //�o�ߎ��Ԃ��N�[���^�C���ȏ�̏ꍇ�A�g�p�\�ɂ���
        if (deltaTime[bombID] >= coolTime[bombID])
        {
            isUsingBomb[bombID] = false;
            deltaTime[bombID] = 0;
        }
    }

    /// <summary>
    /// ���e�֌W�̂��̂�����������
    /// </summary>
    public void Initialize()
    {
        //�v���C���[�̃R���|�[�l���g��p�����[�^���擾
        playerTransform = transform;
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerHalfHeight = playerTransform.localScale.y * playerCapsuleCollider.height * 0.5f;
        
        deltaTime = new float[Utilities.BOMBTYPENUM];
        coolTime = new float[Utilities.BOMBTYPENUM];
        isUsingBomb = new bool[Utilities.BOMBTYPENUM];
        isUsingBomb[(int)BombType.Throwing] = true;
        enableUseBomb = new bool[Utilities.BOMBTYPENUM];
        enableUseBomb[(int)BombType.Throwing] = true;
        addBombCounter = (int)BombType.Planted;

        //�������e�֌W�̒l�̐ݒ�
        throwingBomb.explosionParticle = throwingBombExplosionParticle;
        coolTime[(int)BombType.Throwing] = throwingBomb.GetCoolTime;

        //�ݒu�^���e�֌W�̒l���擾
        plantedBombHalfHeight = plantedBomb.GetHalfHeight();
        plantedBomb.explosionParticle = plantedBombExplosionParticle;
        plantedBombRotation = plantedBomb.transform.rotation;
        coolTime[(int)BombType.Planted] = plantedBomb.GetCoolTime;

        //�m�b�N�o�b�N���e�֌W�̒l���擾�A�ݒ�
        knockbackHalfHeight = knockbackBomb.GetHalfHeight();
        toPlayerDistance = knockbackBomb.GetToPlayerDistance;
        knockbackBomb.explosionParticle = knockbackBombExplosionParticle;
        coolTime[(int)BombType.Knockback] = knockbackBomb.GetCoolTime;

        //�U�����e�̊֌W�̒l���擾
        var homingBombComponent = homingBombSpawner.GetPrefab.GetComponent<BombHoming>();
        //homingBomb�̃p�[�e�B�N���ɎQ�Ƃ��Ă���U�����e�p�̔����p�[�e�B�N�����Z�b�g����
        homingBombComponent.explosionParticle = homingBombExplosionParticle;
        homingBombHelfHeight = homingBombSpawner.GetBombHalfHeight();
        coolTime[(int)BombType.Homing] = homingBombSpawner.GetCoolTime;
    }

    /// <summary>
    /// ���e�𐶐�����
    /// </summary>
    /// <param name="coolDownEvent">�N�[���_�E���C�x���g</param>
    /// <param name="playerAnimation">�A�j���[�V�����p�N���X</param>
    public void GenerateBomb(PlayerEvent.CoolDownEvent coolDownEvent, PlayerAnimation playerAnimation)
    {
        //���e�̃N�[���^�C�����v������
        for (int i = 0; i < 4; i++)
        {
            CountBombCoolTime(i);
        }

        //�������e�𐶐�
        GenerateThrowingBomb(coolDownEvent, playerAnimation);

        //1�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (enableUseBomb[(int)BombManager.BombType.Planted])
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //�ݒu�^���e�𐶐�
                GeneratePlantedBomb(coolDownEvent);
            }
        }

        //2�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (enableUseBomb[(int)BombManager.BombType.Knockback])
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //�m�b�N�o�b�N���e�𐶐�
                GenerateKnockbackBombs(coolDownEvent);
            }
        }
        //3�ڂ̔��e�g�p�\�t���O��true�̏ꍇ
        if (enableUseBomb[(int)BombManager.BombType.Homing])
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //�U�����e�𐶐�
                GenerateHomingBomb(coolDownEvent);
            }
        }
    }

    /// <summary>
    /// ���x���A�b�v�Ŕ��e��ǉ�����ۂɎg��
    /// ���e���g�p�\�ɂ���
    /// </summary>
    public void EnableNewBomb()
    {
        if (addBombCounter > enableUseBomb.Length) return;

        //���e���g�p�\�ɂ���t���O�����Ă�
        enableUseBomb[addBombCounter] = true;

        //���ɂ��̊֐����Ă΂ꂽ���ɁA
        //�ʂ̔��e���g�p�\�ɂ���t���O�����Ă邽�߂ɃJ�E���^�[�����Z����
        if (addBombCounter < (int)BombType.Homing)
        {
            addBombCounter++;
        }
    }

    /// <summary>
    /// ���e���g�p�\����Ԃ�
    /// </summary>
    /// <param name="bombID">���e�̌ŗL�ԍ�</param>
    /// <returns>true:�g�p�\ / false:�g�p�s��</returns>
    public bool CanUseBumb(int bombID)
    {
        if (enableUseBomb[bombID])
        {
            return true;
        }

        return false;
    }
}
