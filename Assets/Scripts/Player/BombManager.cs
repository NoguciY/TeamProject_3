using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���e���Ǘ�����N���X
public class BombManager : MonoBehaviour
{
    //�������e
    //[SerializeField]
    //private GameObject throwingBombPrefab;

    //�ݒu�^���e
    [SerializeField]
    private GameObject plantedBombPrefab;

    //�m�b�N�o�b�N���e
    [SerializeField]
    private GameObject knockbackBombPrefab;

    //�U�����e
    [SerializeField]
    private GameObject missileSpawnPrefab;

    //�����G�t�F�N�g
    [SerializeField, Header("�m�b�N�o�b�N���e�̔����G�t�F�N�g")]
    private ParticleSystem knockbackBombExplosionParticle;

    //�v���C���[��Transform�R���|�[�l���g
    [SerializeField]
    private Transform playerTransform;

    //�ݒu�^���e�̍����̔���
    private float plantedBombHelfHeight;

    //�ݒu�^���e�̉�]�l
    private Quaternion plantedBombRotation;

    //�m�b�N�o�b�N���e�̍����̔���
    private float knockbackHelfHeight;

    //�m�b�N�o�b�N���e�ƃv���C���[�̊Ԃ̋���
    private float toPlayerDistance;

    //���������m�b�N�o�b�N���e�̐�
    //private int generatedKnockbackBombNum;

    //�U�����e�̍����̔���
    private float homingBombHelfHeight;

    //�萔���擾����
    public void Initialize()
    {
        //�ݒu�^���e�֌W�̒l���擾
        plantedBombHelfHeight = plantedBombPrefab.GetComponent<PlantedBomb>().GetHalfHeight();
        plantedBombRotation = plantedBombPrefab.transform.rotation;

        //�m�b�N�o�b�N���e�֌W�̒l���擾�A�ݒ�
        var knockbackBombComponent = knockbackBombPrefab.GetComponent<KnockbackBomb>();
        knockbackHelfHeight = knockbackBombComponent.GetHalfHeight();
        toPlayerDistance = knockbackBombComponent.GetToPlayerDistance;
        knockbackBombComponent.explosionParticle = knockbackBombExplosionParticle;

        //�U�����e�̊֌W�̒l���擾
        homingBombHelfHeight = missileSpawnPrefab.GetComponent<MissileSpawner>().GetBombHalfHeight;
    }

    //�ݒu�^���e�𐶐�����
    public void GeneratePlantedBomb()
    {
        //�����ʒu���v�Z���Đ���
        Vector3 spawnPos = playerTransform.position + Vector3.up * plantedBombHelfHeight;
        GameObject bombPrefab = Instantiate(plantedBombPrefab, spawnPos, plantedBombRotation);
    }

    //�m�b�N�o�b�N���e�𐶐�����
    public void GenerateKnockbackBomb()
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
                standardPos * toPlayerDistance + Vector3.up * knockbackHelfHeight;

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
}
