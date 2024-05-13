using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���e���Ǘ�����N���X
public class BombManager : MonoBehaviour
{
    //�������e--------------------------------------------------
    //�v���n�u
    [SerializeField]
    private GameObject throwingBombPrefab;

    //�������e�̃^�C�}�[
    private float throwingBombTimer;

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
    [SerializeField, Header("�m�b�N�o�b�N���e�̔����G�t�F�N�g")]
    private GameObject knockbackBombExplosionParticle;

    //�v���C���[��Transform�R���|�[�l���g
    //[SerializeField]
    private Transform playerTransform;

    //�v���C���[�̃R���C�_�[
    private CapsuleCollider playerCapsuleCollider;

    //�v���C���[�̍����̔���
    private float playerHalfHeight;


    //���e�֌W�̂��̂�����������
    public void Initialize()
    {
        //�v���C���[�̃R���|�[�l���g��l���擾
        playerTransform = transform;
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerHalfHeight = playerTransform.localScale.y * playerCapsuleCollider.height * 0.5f;

        //�������e�֌W�̒l�̐ݒ�
        var throwingBombComponent = throwingBombPrefab.GetComponent<ThrowingBomb>();
        throwingBombComponent.explosionParticle = knockbackBombExplosionParticle;
        throwingBombTimer = 0;

        //�ݒu�^���e�֌W�̒l���擾
        var plantedBombComponent = plantedBombPrefab.GetComponent<PlantedBomb>();
        plantedBombHalfHeight = plantedBombComponent.GetHalfHeight();
        plantedBombComponent.explosionParticle = knockbackBombExplosionParticle;
        plantedBombRotation = plantedBombPrefab.transform.rotation;

        //�m�b�N�o�b�N���e�֌W�̒l���擾�A�ݒ�
        var knockbackBombComponent = knockbackBombPrefab.GetComponent<KnockbackBomb>();
        knockbackHalfHeight = knockbackBombComponent.GetHalfHeight();
        toPlayerDistance = knockbackBombComponent.GetToPlayerDistance;
        knockbackBombComponent.explosionParticle = knockbackBombExplosionParticle;

        //�U�����e�̊֌W�̒l���擾
        homingBombHelfHeight = missileSpawnPrefab.GetComponent<MissileSpawner>().GetBombHalfHeight;
    }

    //�������e�𐶐�����
    public void GenerateThrowingBomb()
    {
        //�����ʒu���v�Z���Đ���
        Vector3 spawnPos = playerTransform.position + Vector3.up * playerHalfHeight;
        GameObject bombPrefab = Instantiate(throwingBombPrefab, spawnPos, playerTransform.rotation);

        //�v���C���[�̈ʒu���Z�b�g
        var throwingBombComponent = bombPrefab.GetComponent<ThrowingBomb>();
        throwingBombComponent.playerTransform = playerTransform;
        throwingBombComponent.Init();
    }

    //�ݒu�^���e�𐶐�����
    public void GeneratePlantedBomb()
    {
        //�����ʒu���v�Z���Đ���
        Vector3 spawnPos = playerTransform.position + Vector3.up * plantedBombHalfHeight;
        GameObject bombPrefab = Instantiate(plantedBombPrefab, spawnPos, plantedBombRotation);
    }

    //�m�b�N�o�b�N���e�𐶐�����
    public void GenerateKnockbackBombs()
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
    }

    public void GenerateHomingBomb()
    {
        Vector3 spawnPos = playerTransform.position + Vector3.up * homingBombHelfHeight * 10;
        GameObject bombPrefab = Instantiate(missileSpawnPrefab, spawnPos, Quaternion.identity);

        //�v���C���[�̈ʒu���Z�b�g
        bombPrefab.GetComponent<MissileSpawner>().playerTransform = playerTransform;
    }

    //���e�̃N�[���^�C�����v��
    public void CountBombCoolTime()
    {

    }
}
