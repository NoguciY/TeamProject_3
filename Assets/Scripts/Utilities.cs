using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    //�X�e�[�W�̑傫��
    public static float stageSize = 50f;


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

    //�p�x����������擾����
    //public static Vector3 GetDirection(float angle)
    //{
    //    return new Vector3
    //    (
    //        Mathf.Cos(angle * Mathf.Deg2Rad),
    //        Mathf.Sin(angle * Mathf.Deg2Rad),
    //        0
    //    );
    //}
}
