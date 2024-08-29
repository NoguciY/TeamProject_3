using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    //�A�j���[�^�[�R���|�[�l���g
    [SerializeField]
    private Animator enemyAnimator;

    //�ҋ@�A�j���[�V����
    public void SetIdleAnimation()
    {
        enemyAnimator.SetBool("isMoving", false);
    }

    //����A�j���[�V����������
    public void SetRunAnimation()
    {
        enemyAnimator.SetBool("isMoving", true);
    }

    //�U���A�j���[�V����������
    public void SetAttackAnimation()
    {
        enemyAnimator.SetTrigger("attackTrigger");
    }

    //����A�j���[�V����
    public void SetDeadAnimation()
    {
        enemyAnimator.SetTrigger("deathTrigger");
    }
}
