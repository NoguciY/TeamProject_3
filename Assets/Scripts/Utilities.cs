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


    /// <summary>
    /// �p�x���擾����
    /// �X�N���[�����W�n�ł̂Q�̃x�N�g���̂Ȃ��p�����߂�ꍇ�Ɏg���Ă���
    /// </summary>
    /// <param name="from">�x�N�g��</param>
    /// <param name="to">�x�N�g��</param>
    /// <returns>2�̃x�N�g���̂Ȃ��p�x</returns>
    public static float GetAngle(Vector3 from, Vector3 to)
    {
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var rad = Mathf.Atan2(dx, dy);
        return rad * Mathf.Rad2Deg;
    }
}
