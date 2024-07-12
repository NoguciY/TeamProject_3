using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    //�̗�
    [SerializeField]
    private float life;

    //�ő�̗�
    private float maxLife;

    //���S������
    private bool dead;

    //�Q�b�^�[
    public float GetLife => life;

    //�̗͂̏�����(�̗͂��ő�ɂ���)
    public void InitializeLife(float maxLife)
    {
        this.maxLife = maxLife;
        life = maxLife;
        dead = false;
    }


    //�̗͂ɒl��������(�_���[�W�A��)
    public void AddValueToLife(float value)
    {
        life += value;
        
        //�̗͂��ő�l�𒴂��Ȃ��悤�ɂ���
        if (life > maxLife)
            life = maxLife;
        Debug.Log("�̗͍X�V");
    }


    //�̗͂̍ő�l��ݒ肷��
    public float SetMaxLife
    {
        set { 
            this.maxLife = value;
            Debug.Log($"�ő�̗�:{maxLife}");
        }
    }

    //���S(�̗͂�0)�𔻒肷��
    public bool IsDead()
    {
        if (!dead && life <= 0)
            return dead = true;

        return false;
    }
}
