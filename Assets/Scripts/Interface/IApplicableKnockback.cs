using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�m�b�N�o�b�N�ł���I�u�W�F�N�g�Ɏ�������
public interface IApplicableKnockback
{
    /// <summary>
    /// �m�b�N�o�b�N����
    /// </summary>
    /// <param name="knockbackForce">�m�b�N�o�b�N��</param>
    /// <param name="bombMovingDirection">�m�b�N�o�b�N���e�̈ړ�����</param>
    void Knockback(float knockbackForce, Vector3 bombMovingDirection);
}
