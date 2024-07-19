using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������鍀�ڂ��܂Ƃ߂ċL�ڂ���

public class PlayerPowerUpItems : MonoBehaviour
{
    [SerializeField, Header("�ő�HP�̑�����"), Range(0, 1)]
    private float maxLifeIncreaseRate;

    [SerializeField, Header("�ړ����x�̑�����"), Range(0, 1)]
    private float speedIncreaseRate;

    [SerializeField, Header("����͈͂̑�����"), Range(0, 1)]
    private float collectionRangeIncreaseRate;

    [SerializeField, Header("�h��͂̑�����")]
    private float difenceIncreaseValue;

    [SerializeField, Header("�񕜗͂̑�����")]
    private float resilienceIncreaseValue;

    [SerializeField, Header("���e�̔����͈͂̑�����"), Range(0, 1)]
    private float bombRangeIncreaseRate;

    //�ő�̗͂���������
    public void PowerUpMaxLife(Player player)
    {
        //�ő�HP�̑����������₷
        player.maxLife += player.maxLife * maxLifeIncreaseRate;

        //LifeController�R���|�[�l���g�̍ő�̗͂��X�V����
        player.GetLifeController.SetMaxLife = player.maxLife;

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
        //����͈�(�A�C�e���p�̃R���C�_�[�̔��a)�𑝉������g�傷��
        player.capsuleColliderForItem.radius += 
            player.capsuleColliderForItem.radius * collectionRangeIncreaseRate;
        Debug.Log($"�A�C�e���p�̓����蔻��̔��a�F{player.capsuleColliderForItem.radius}");
    }

    //�h��͂���������
    public void PowerUpDifence(Player player)
    {
        //�h��͂𑝉��������₷
        player.difense += difenceIncreaseValue;
        Debug.Log($"���݂̖h��́F{player.difense}");
    }

    //�񕜗͂���������
    public void PowerUpResilience(Player player)
    {
        //�񕜗͂𑝉��������₷
        player.resilience += resilienceIncreaseValue;
        Debug.Log($"���݂̉񕜗́F{player.resilience}");
    }

    //���e�̍U���͈͂���������
    public void PowerUpBombRange(Player player)
    {
        float explosionRadius = player.GetBombManager.GetBombPlanted.ExplosionRadius;
        explosionRadius += explosionRadius * bombRangeIncreaseRate;

        player.GetBombManager.GetBombPlanted.ExplosionRadius = explosionRadius;
        Debug.Log($"���݂̍U���͈�{explosionRadius}");
    }

    //���e�̎g�p���x����������
    public void PowerUpCoolDown(GameObject bomb, float coolTime)
    {

    }
}
