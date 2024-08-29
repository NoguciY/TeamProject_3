using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //�A�j���[�^�[�R���|�[�l���g
    [SerializeField]
    private Animator playerAnimator;

    //�ҋ@�A�j���[�V����������
    public void SetIdleAnimation()
    {
        playerAnimator.SetBool("isMoving", false);
    }

    //����A�j���[�V����������
    public void SetRunAnimation()
    {
        playerAnimator.SetBool("isMoving", true);

        //S�L�[�������̂ݑ���A�j���[�V�������t�Đ�����
        if (Input.GetKey(KeyCode.S))
            playerAnimator.SetFloat("speed", -1);
        else
            playerAnimator.SetFloat("speed", 1);
    }

    //�U���A�j���[�V����������
    public void SetAttackAnimation()
    {
        playerAnimator.SetTrigger("attackTrigger");
    }
}
