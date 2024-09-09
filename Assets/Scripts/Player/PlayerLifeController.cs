using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour
{
    //�̗�
    [SerializeField]
    private float life;

    public float GetLife => life;

    //�ő�̗�
    private float maxLife;

    //���S������
    private bool isDead;

    public bool GetIsDead => isDead;


    /// <summary>
    /// �̗͂̏�����(�̗͂��ő�ɂ���)
    /// </summary>
    /// <param name="maxLife">�ő�̗�</param>
    public void InitializeLife(float maxLife)
    {
        this.maxLife = maxLife;
        life = maxLife;
        isDead = false;
    }


    /// <summary>
    /// �̗͂ɒl��������(�_���[�W�A��)
    /// </summary>
    /// <param name="value">�_���[�W�ʁA�񕜗�</param>
    public void AddValueToLife(float value)
    {
        life += value;

        //�̗͂��ő�l�𒴂��Ȃ��悤�ɂ���
        if (life > maxLife)
        {
            life = maxLife;
        }
        //�̗͂�0��菬�������Ȃ�
        else if (life < 0)
        {
            life = 0;
        }

        Debug.Log("�̗͍X�V");
    }


    /// <summary>
    /// �̗͂̍ő�l��ݒ肷��
    /// </summary>
    public float SetMaxLife
    {
        set { 
            this.maxLife = value;
            Debug.Log($"�ő�̗�:{maxLife}");
        }
    }

    /// <summary>
    /// ���S(�̗͂�0)����𔻒肷��
    /// </summary>
    /// <returns>true:���S���� / false:����ł��Ȃ��A���S��</returns>
    public bool IsDead()
    {
        if (!isDead && life <= 0)
        {
            return isDead = true;
        }

        return false;
    }
}
