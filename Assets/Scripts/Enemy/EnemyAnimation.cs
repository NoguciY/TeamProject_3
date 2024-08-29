using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    //アニメーターコンポーネント
    [SerializeField]
    private Animator enemyAnimator;

    //待機アニメーション
    public void SetIdleAnimation()
    {
        enemyAnimator.SetBool("isMoving", false);
    }

    //走りアニメーションをする
    public void SetRunAnimation()
    {
        enemyAnimator.SetBool("isMoving", true);
    }

    //攻撃アニメーションをする
    public void SetAttackAnimation()
    {
        enemyAnimator.SetTrigger("attackTrigger");
    }

    //やられアニメーション
    public void SetDeadAnimation()
    {
        enemyAnimator.SetTrigger("deathTrigger");
    }
}
