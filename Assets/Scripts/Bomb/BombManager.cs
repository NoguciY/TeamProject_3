using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public BombThrowing GetBombThrowing => throwingBomb; 

    //�������e�̃^�C�}�[
    //private float throwingBombTimer;

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

    //�m�b�N�o�b�N���e�̍����̔���
    private float knockbackHalfHeight;

    //�m�b�N�o�b�N���e�ƃv���C���[�̊Ԃ̋���
    private float toPlayerDistance;

    //���������m�b�N�o�b�N���e�̐�
    //private int generatedKnockbackBombNum;

    //�U�����e--------------------------------------------------
    //�v���n�u
    [SerializeField]
    private HomingBombSpawner homingBombSpawn;

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
    //[SerializeField]
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

    //���e�֌W�̂��̂�����������
    public void Initialize()
    {
        //�v���C���[�̃R���|�[�l���g��l���擾
        playerTransform = transform;
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerHalfHeight = playerTransform.localScale.y * playerCapsuleCollider.height * 0.5f;
        
        deltaTime = new float[Utilities.BOMBTYPENUM];
        coolTime = new float[Utilities.BOMBTYPENUM];
        isUsingBomb = new bool[Utilities.BOMBTYPENUM];

        //�������e�֌W�̒l�̐ݒ�
        throwingBomb.explosionParticle = throwingBombExplosionParticle;
        coolTime[(int)BombType.Throwing] = throwingBomb.GetCoolTime;

        //�ݒu�^���e�֌W�̒l���擾
        //var plantedBombComponent = plantedBomb.GetComponent<BombPlanted>();
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
        //�~�T�C���X�|�i�[�I�u�W�F�N�g��MissileSpawner���擾
        //var homingBombSpawnerComponent = homingBombSpawn.GetComponent<HomingBombSpawner>();
        //MissileSpawner�̃v���n�u��Bullet���擾
        var homingBombComponent = homingBombSpawn.GetPrefab.GetComponent<BombHoming>();
        //bullet�R���|�[�l���g�̃p�[�e�B�N���ɎQ�Ƃ��Ă���U�����e�p�̔����p�[�e�B�N�����Z�b�g����
        homingBombComponent.explosionParticle = homingBombExplosionParticle;
        homingBombHelfHeight = homingBombSpawn.GetBombHalfHeight;
        coolTime[(int)BombType.Homing] = homingBombSpawn.GetCoolTime;
    }

    //�������e�𐶐�����
    public void GenerateThrowingBomb()
    {
        if (!isUsingBomb[(int)BombType.Throwing])
        {
            //�����ʒu���v�Z���Đ���
            Vector3 spawnPos = playerTransform.position + Vector3.up * playerHalfHeight;
            GameObject bombPrefab = Instantiate(throwingBomb.gameObject, spawnPos, playerTransform.rotation);

            //�v���C���[�̈ʒu���Z�b�g
            var throwingBombComponent = bombPrefab.GetComponent<BombThrowing>();
            throwingBombComponent.playerTransform = playerTransform;
            throwingBombComponent.Init();

            //�N�[���^�C���̃J�E���g���J�n����t���O
            isUsingBomb[(int)BombType.Throwing] = true;
        }
    }

    //�ݒu�^���e�𐶐�����
    public void GeneratePlantedBomb()
    {
        if (!isUsingBomb[(int)BombType.Planted])
        {
            //�����ʒu���v�Z���Đ���
            Vector3 spawnPos = playerTransform.position + Vector3.up * plantedBombHalfHeight;
            GameObject bombPrefab = Instantiate(plantedBomb.gameObject, spawnPos, plantedBombRotation);

            //�N�[���^�C���̃J�E���g���J�n����t���O
            isUsingBomb[(int)BombType.Planted] = true;
        }
    }

    //�m�b�N�o�b�N���e�𐶐�����
    public void GenerateKnockbackBombs()
    {
        if (!isUsingBomb[(int)BombType.Knockback])
        {
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

                //�v���C���[�̈ʒu���Z�b�g
                bombPrefab.GetComponent<BombKnockback>().playerTransform = playerTransform;
            }

            //�N�[���^�C���̃J�E���g���J�n����t���O
            isUsingBomb[(int)BombType.Knockback] = true;
        }
    }

    public void GenerateHomingBomb()
    {
        if (!isUsingBomb[(int)BombType.Homing])
        {
            Vector3 spawnPos = playerTransform.position + Vector3.up * homingBombHelfHeight * 10;
            GameObject bombPrefab = Instantiate(homingBombSpawn.gameObject, spawnPos, Quaternion.identity);

            //�v���C���[�̈ʒu���Z�b�g
            bombPrefab.GetComponent<HomingBombSpawner>().playerTransform = playerTransform;

            //�N�[���^�C���̃J�E���g���J�n����t���O
            isUsingBomb[(int)BombType.Homing] = true;
        }
    }

    //���e�̃N�[���^�C�����v�����A�Ďg�p�ł���悤�ɂ���
    public void CountBombCoolTime(int num)
    {
        if (isUsingBomb[num])
        {
            //�o�ߎ��Ԃ��J�E���g
            deltaTime[num] += Time.deltaTime;
            
            //�o�ߎ��Ԃ��N�[���^�C���ȏ�̏ꍇ�A�g�p�\�ɂ���
            if (deltaTime[num] >= coolTime[num])
            {
                isUsingBomb[num] = false;
                deltaTime[num] = 0;
            }
        }
    }
}
