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

    //�Q�b�^�[
    public float GetLife => life;

    //�̗͂̏�����(�̗͂��ő�ɂ���)
    public void InitializeLife(float maxLife)
    {
        //SetMaxLife(maxLife);
        this.maxLife = maxLife;
        life = maxLife;
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
    private void SetMaxLife(float maxLife)
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
