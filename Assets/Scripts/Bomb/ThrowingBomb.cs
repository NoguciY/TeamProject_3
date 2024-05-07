using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class ThrowingBomb : MonoBehaviour
{
    //���˃^�C�~���O���Ǘ�
    private float timer;
    //���ː�
    private int count;
    //���ˊԊu
    private float interval;
    //�e��
    private float speed;
    //
    private float angleRange;

    private Vector3 m_velocity; // ���x

    // ���t���[���Ăяo�����֐�
    private void Update()
    {
        // �ړ�����
        transform.localPosition += m_velocity;

        //                                            �v���C���[�̃|�W�V����
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        var direction = Input.mousePosition - screenPos;

        var angle = Utilities.GetAngle(Vector3.zero, direction);

        if (timer < interval) return;

        // �e�̔��˃^�C�~���O���Ǘ�����^�C�}�[�����Z�b�g����
        timer = 0;

        // �e�𔭎˂���
        ShootNWay(speed, count);
    }

    // �e�𔭎˂��鎞�ɏ��������邽�߂̊֐�
    public void Init(float speed)
    {
        // �e�̔��ˊp�x���x�N�g���ɕϊ�����
        var direction = transform.forward;
        // ���ˊp�x�Ƒ������瑬�x�����߂�
        m_velocity = direction * speed;
    }

    private void ShootNWay(float speed, int count)
    {
        var pos = transform.localPosition; // �v���C���[�̈ʒu
        var rot = transform.localRotation; //�v���C���[�̌���

        if (count == 1)
        {
            // ���˂���e�𐶐�����
            var shot = Instantiate(gameObject, pos, rot);

            // �e�𔭎˂�������Ƒ�����ݒ肷��
            Init(speed);
        }
    }

}
