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

    /// <summary>
    /// �ő�̗͂���������
    /// </summary>
    /// <param name="player">PlayerManager</param>
    public void PowerUpMaxLife(PlayerManager player)
    {
        //�ő�HP�̑����������₷
        player.maxLife += player.maxLife * maxLifeIncreaseRate;

        //LifeController�R���|�[�l���g�̍ő�̗͂��X�V����
        player.GetLifeController.SetMaxLife = player.maxLife;

        Debug.Log($"���݂̍ő�̗́F{player.maxLife}");
    }

    /// <summary>
    /// �ړ����x����������
    /// </summary>
    /// <param name="player">PlayerManager</param>
    public void PowerUpSpeed(PlayerManager player)
    {
        //�ړ����x�̑����������₷
        player.speed += player.speed * speedIncreaseRate;
        Debug.Log($"���݂̈ړ����x�F{player.speed}");
    }

    /// <summary>
    /// ����͈͂���������
    /// </summary>
    /// <param name="player">PlayerManager</param>
    public void PowerUpCollectionRangeRate(PlayerManager player)
    {
        //����͈�(�A�C�e���p�̃R���C�_�[�̔��a)�𑝉������g�傷��
        player.capsuleColliderForItem.radius += 
            player.capsuleColliderForItem.radius * collectionRangeIncreaseRate;
        Debug.Log($"�A�C�e���p�̓����蔻��̔��a�F{player.capsuleColliderForItem.radius}");
    }

    /// <summary>
    /// �h��͂���������
    /// </summary>
    /// <param name="player">PlayerManager</param>
    public void PowerUpDifence(PlayerManager player)
    {
        //�h��͂𑝉��������₷
        player.difense += difenceIncreaseValue;
        Debug.Log($"���݂̖h��́F{player.difense}");
    }

    /// <summary>
    /// �񕜗͂���������
    /// </summary>
    /// <param name="player">PlayerManager</param>
    public void PowerUpResilience(PlayerManager player)
    {
        //�񕜗͂𑝉��������₷
        player.resilience += resilienceIncreaseValue;
        Debug.Log($"���݂̉񕜗́F{player.resilience}");
    }

    /// <summary>
    /// ���e�̍U���͈͂���������
    /// </summary>
    /// <param name="player">PlayerManager</param>
    public void PowerUpBombRange(PlayerManager player)
    {
        float explosionRadius = player.GetBombManager.GetBombPlanted.ExplosionRadius;
        explosionRadius += explosionRadius * bombRangeIncreaseRate;

        player.GetBombManager.GetBombPlanted.ExplosionRadius = explosionRadius;
        Debug.Log($"���݂̍U���͈�{explosionRadius}");
    }

    /// <summary>
    /// ���e�̎g�p���x����������
    /// </summary>
    /// <param name="bomb">���e</param>
    /// <param name="coolTime">�N�[���^�C��</param>
    public void PowerUpCoolDown(GameObject bomb, float coolTime)
    {

    }
}
