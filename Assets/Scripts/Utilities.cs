using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�萔���`����
public static class Utilities
{
    //�X�e�[�W�̑傫��
    public static readonly float STAGESIZE = 50f;

    //�h��͂̏��
    public static readonly int DEFENSEUPPERLIMIT = 3;

    //���e�̎퐔
    public static readonly int BOMBTYPENUM = 4;

    //�ǉ�����锚�e�̎��
    public enum AddedBombType
    {
        Planted,    //�ݒu
        Knockback,  //�m�b�N�o�b�N
        Homing,     //�U��
    }
}
