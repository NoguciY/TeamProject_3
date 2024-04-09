using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    //�̗�
    [SerializeField]
    private int life;

    //�ő�̗�
    private int maxLife;

    //�Q�b�^�[
    public int GetLife { get { return life; } }

    //�̗͂̏�����(�̗͂��ő�ɂ���)
    public void InitializeLife(int maxLife)
    {
        SetMaxLife(maxLife);
        life = maxLife;
    }


    //�̗͂ɒl��������(�_���[�W�A��)
    public void AddValueToLife(int value)
    {
        life += value;
        
        //�̗͂��ő�l�𒴂��Ȃ��悤�ɂ���
        if (life > maxLife)
            life = maxLife;
    }


    //�̗͂̍ő�l��ݒ肷��
    public void SetMaxLife(int maxLife)
    {
        this.maxLife = maxLife;
    }

    //���S(�̗͂�0)�𔻒肷��
    public bool IsDead()
    {
        bool dead = false;

        if (life <= 0)
            dead = true;

        return dead;
    }
}
