using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�������ڂ̏���
//�������ڂ̉摜���܂Ƃ߂�

public abstract class PowerUpItem : MonoBehaviour
{
    //�������ډ摜
    [SerializeField]
    private Image powerUpItemImage;

    //�����֐�
    public virtual void PowerUpFunc(Player player)
    {
    }
}

//�ő�̗͂���������
public class MaxLife : PowerUpItem
{
    //�摜

    //�ő�̗͂���������
    public override void PowerUpFunc(Player player)
    {
        //�����Ɍ��݂̍ő�̗͂�����
    }
}

//�ړ����x��
