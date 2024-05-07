using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    //�X�e�[�W�̑傫��
    public static float stageSize = 50f;


    // �p�x���擾����
    public static float GetAngle(Vector2 from, Vector2 to)
    {
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var rad = Mathf.Atan2(dx, dy);
        return rad * Mathf.Rad2Deg;
    }

    //�p�x����������擾����
    public static Vector3 GetDirection(float angle)
    {
        return new Vector3
        (
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0
        );
    }
}
