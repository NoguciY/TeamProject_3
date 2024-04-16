using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������鍀�ڂ��܂Ƃ߂ċL�ڂ���

public class PowerUpItems : MonoBehaviour
{
    [SerializeField, Header("�ő�HP�̑�����"), Range(0, 1)]
    private float maxLifeIncreaseRate;

    [SerializeField, Header("�ړ����x�̑�����"), Range(0, 1)]
    private float speedIncreaseRate;

    [SerializeField, Header("����͈͂̑�����"), Range(0, 1)]
    private float collectionRangeIncreaseRate;

    [SerializeField, Header("�h��͂̑�����"), Range(0, 1)]
    private float difenceIncreaseRate;

    [SerializeField, Header("�񕜗͂̑�����"), Range(0, 1)]
    private float resilienceIncreaseRate;

    [SerializeField, Header("���e�̔����͈͂̑�����"), Range(0, 1)]
    private float bombRangeIncreaseRate;

    //�ő�̗͂���������
    public void PowerUpMaxLife(Player player)
    {
        //�ő�HP�̑����������₷
        player.maxLife += player.maxLife * maxLifeIncreaseRate;
        Debug.Log($"���݂̍ő�̗́F{player.maxLife}");
    }

    //�ړ����x����������
    public void PowerUpSpeed(Player player)
    {
        //�ړ����x�̑����������₷
        player.speed += player.speed * speedIncreaseRate;
        Debug.Log($"���݂̈ړ����x�F{player.speed}");
    }

    //����͈͂���������
    public void PowerUpCollectionRangeRate(Player player)
    {
        //����͈͗��𑝉��������₷
        //���̏��������̏����Ƃ܂Ƃ߂�ꂽ������ƒZ���R�[�h�ōςނƎv��
        //player.collectionRangeRate +=
        //    player.collectionRangeRate * collectionRangeIncreaseRate;

        //�A�C�e���p�̃R���C�_�[�̔��a���g�傷��
        player.capsuleColliderForItem.radius += 
            player.capsuleColliderForItem.radius * collectionRangeIncreaseRate;
        Debug.Log($"�A�C�e���p�̓����蔻��̔��a�F{player.capsuleColliderForItem.radius}");
    }

    //�h��͂���������
    public void PowerUpDifence(Player player)
    {
        //�h��͂𑝉��������₷
        player.difence += player.difence * difenceIncreaseRate;
        Debug.Log($"���݂̖h��́F{player.difence}");
    }

    //�񕜗͂���������
    public void PowerUpResilience(Player player)
    {
        //�񕜗͂𑝉��������₷
        player.resilience += player.resilience * resilienceIncreaseRate;
        Debug.Log($"���݂̉񕜗́F{player.resilience}");
    }

    //���e�̍U���͈͂���������
    public void PowerUpBombRange(Player player)
    {
        //���e�͈̔͂𑝉��������₷
        float radius = player.bombPrefab.GetComponent<PlantedBomb>().explosionRadius;
        radius += radius * bombRangeIncreaseRate;
        //2��GetComponent���Ă��邩��ŏ����ɗ}�����Ȃ����H
        player.bombPrefab.GetComponent<PlantedBomb>().explosionRadius = radius;
        Debug.Log($"���e�̍U���͈͂�����");
    }

    //���e�̎g�p���x����������
    public void PowerUpCoolDown(GameObject bomb, float coolTime)
    {

    }
}
