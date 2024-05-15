using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargetBullet : MonoBehaviour
{
    public float maxUpwardHeight = 5f; // �㏸����ő卂��
    public float moveSpeed = 10f; // �ړ����x
    public Transform[] targets; // �^�[�Q�b�g�̔z��

    private bool isAscending = true; // �㏸�����ǂ����̃t���O
    private Vector3 initialPosition; // �����ʒu

    private Transform currentTarget; // ���݂̃^�[�Q�b�g

    void Start()
    {
        initialPosition = transform.position;
        currentTarget = GetRandomTarget();
    }

    void Update()
    {
        if (isAscending)
        {
            // �㏸
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // ���̍����܂ŏ㏸������
            if (transform.position.y >= initialPosition.y + maxUpwardHeight)
            {
                isAscending = false;
                currentTarget = GetRandomTarget(); // �V���������_���ȃ^�[�Q�b�g��I��
            }
        }
        else
        {
            // �^�[�Q�b�g�Ɍ������Ĉړ�
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

            // �^�[�Q�b�g�ɓ��B������
            if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                transform.position = initialPosition; // �����ʒu�ɖ߂�
                isAscending = true; // �㏸�t���O���Ă�true�ɐݒ�
            }
        }
    }

    // �����_���ȃ^�[�Q�b�g��I�����郁�\�b�h
    Transform GetRandomTarget()
    {
        return targets[Random.Range(0, targets.Length)];
    }
}
