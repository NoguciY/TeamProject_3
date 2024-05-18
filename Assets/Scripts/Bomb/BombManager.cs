using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���e���Ǘ�����N���X
public class BombManager : MonoBehaviour
{
    //���e�̃^�C�v
    private enum BombType
    {
        Throwing,   //�����^
        Planted,    //�ݒu�^
        Knockback,  //�m�b�N�o�b�N�^
        Homing,     //�U���^
    }


    //�������e--------------------------------------------------
    //�v���n�u
    [SerializeField]
    private GameObject throwingBombPrefab;

    //�������e�̃^�C�}�[
    //private float throwingBombTimer;

    //�ݒu�^���e------------------------------------------------
    //�v���n�u
    [SerializeField]
    private GameObject plantedBombPrefab;

    //�ݒu�^���e�̍����̔���
    private float plantedBombHalfHeight;

    //�ݒu�^���e�̉�]�l
    private Quaternion plantedBombRotation;

    //�m�b�N�o�b�N���e------------------------------------------
    //�v���n�u
    [SerializeField]
    private GameObject knockbackBombPrefab;

    //�m�b�N�o�b�N���e�̍����̔���
    private float knockbackHalfHeight;

    //�m�b�N�o�b�N���e�ƃv���C���[�̊Ԃ̋���
    private float toPlayerDistance;

    //���������m�b�N�o�b�N���e�̐�
    //private int generatedKnockbackBombNum;

    //�U�����e--------------------------------------------------
    //�v���n�u
    [SerializeField]
    private GameObject missileSpawnPrefab;

    //�U�����e�̍����̔���
    private float homingBombHelfHeight;

    //���̑�----------------------------------------------------
    //�����G�t�F�N�g
    [SerializeField, Header("�������e�̔����G�t�F�N�g")]
    private GameObject throwingBombExplosionParticle;

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

    //���e�̎퐔
    //const�́A���N���X����Q�Ƃ����ꍇ�Ƀo�[�W�����Ǘ���肪�N����\��������
    public static readonly int BOMBTYPENUM = 4;

    //���e�֌W�̂��̂�����������
    public void Initialize()
    {
        //�v���C���[�̃R���|�[�l���g��l���擾
        playerTransform = transform;
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerHalfHeight = playerTransform.localScale.y * playerCapsuleCollider.height * 0.5f;
        
        deltaTime = new float[BOMBTYPENUM];
        coolTime = new float[BOMBTYPENUM];
        isUsingBomb = new bool[BOMBTYPENUM];

        //�������e�֌W�̒l�̐ݒ�
        var throwingBombComponent = throwingBombPrefab.GetComponent<ThrowingBomb>();
        throwingBombComponent.explosionParticle = throwingBombExplosionParticle;
        coolTime[(int)BombType.Throwing] = throwingBombComponent.GetCoolTime;

        //�ݒu�^���e�֌W�̒l���擾
        var plantedBombComponent = plantedBombPrefab.GetComponent<PlantedBomb>();
        plantedBombHalfHeight = plantedBombComponent.GetHalfHeight();
        plantedBombComponent.explosionParticle = knockbackBombExplosionParticle;
        plantedBombRotation = plantedBombPrefab.transform.rotation;
        coolTime[(int)BombType.Planted] = plantedBombComponent.GetCoolTime;

        //�m�b�N�o�b�N���e�֌W�̒l���擾�A�ݒ�
        var knockbackBombComponent = knockbackBombPrefab.GetComponent<KnockbackBomb>();
        knockbackHalfHeight = knockbackBombComponent.GetHalfHeight();
        toPlayerDistance = knockbackBombComponent.GetToPlayerDistance;
        knockbackBombComponent.explosionParticle = knockbackBombExplosionParticle;
        coolTime[(int)BombType.Knockback] = knockbackBombComponent.GetCoolTime;

        //�U�����e�̊֌W�̒l���擾
        //�~�T�C���X�|�i�[�I�u�W�F�N�g��MissileSpawner���擾
        var homingBombComponent = missileSpawnPrefab.GetComponent<MissileSpawner>();
        //MissileSpawner�̃v���n�u��Bullet���擾
        var bulletComponent = homingBombComponent.GetPrefab.GetComponent<Bullet>();
        //bullet�R���|�[�l���g�̃p�[�e�B�N���ɎQ�Ƃ��Ă���U�����e�p�̔����p�[�e�B�N�����Z�b�g����
        bulletComponent.explosionParticle = homingBombExplosionParticle;
        homingBombHelfHeight = homingBombComponent.GetBombHalfHeight;
        coolTime[(int)BombType.Homing] = homingBombComponent.GetCoolTime;
    }

    //�������e�𐶐�����
    public void GenerateThrowingBomb()
    {
        if (!isUsingBomb[(int)BombType.Throwing])
        {
            //�����ʒu���v�Z���Đ���
            Vector3 spawnPos = playerTransform.position + Vector3.up * playerHalfHeight;
            GameObject bombPrefab = Instantiate(throwingBombPrefab, spawnPos, playerTransform.rotation);

            //�v���C���[�̈ʒu���Z�b�g
            var throwingBombComponent = bombPrefab.GetComponent<ThrowingBomb>();
            throwingBombComponent.playerTransform = playerTransform;
            throwingBombComponent.Init();

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
            GameObject bombPrefab = Instantiate(plantedBombPrefab, spawnPos, plantedBombRotation);

            isUsingBomb[(int)BombType.Planted] = true;
        }
    }

    //�m�b�N�o�b�N���e�𐶐�����
    public void GenerateKnockbackBombs()
    {
        if (!isUsingBomb[(int)BombType.Knockback])
        {
            //��������锚�e���͕ω����邽�߂����Ŏ擾����
            int generatedKnockbackBombNum =
            knockbackBombPrefab.GetComponent<KnockbackBomb>().GetGeneratedBombNum;

            //���e���~��ɓ��Ԋu�ɒu�����߂̊p�x
            float degree = 360 / generatedKnockbackBombNum;

            Debug.Log($"���e������:{generatedKnockbackBombNum}");

            //y���W���������e�̊�ʒu
            Vector3 standardPos;

            for (int i = 0; i < generatedKnockbackBombNum; i++)
            {
                //���e�𓙊Ԋu�ɔz�u����
                standardPos = new Vector3(Mathf.Sin(degree * i * Mathf.Deg2Rad),
                    0, Mathf.Cos(degree * i * Mathf.Deg2Rad));

                //�����ʒu(�v���C���[�𒆐S�ɉ~��ɓ��Ԋu�ɔz�u)
                Vector3 spawnPos = playerTransform.position +
                    standardPos * toPlayerDistance + Vector3.up * knockbackHalfHeight;

                GameObject bombPrefab = Instantiate(knockbackBombPrefab, spawnPos, Quaternion.identity);

                //�v���C���[�̈ʒu���Z�b�g
                bombPrefab.GetComponent<KnockbackBomb>().playerTransform = playerTransform;
            }

            isUsingBomb[(int)BombType.Knockback] = true;
        }
    }

    public void GenerateHomingBomb()
    {
        if (!isUsingBomb[(int)BombType.Homing])
        {
            Vector3 spawnPos = playerTransform.position + Vector3.up * homingBombHelfHeight * 10;
            GameObject bombPrefab = Instantiate(missileSpawnPrefab, spawnPos, Quaternion.identity);

            //�v���C���[�̈ʒu���Z�b�g
            bombPrefab.GetComponent<MissileSpawner>().playerTransform = playerTransform;

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
