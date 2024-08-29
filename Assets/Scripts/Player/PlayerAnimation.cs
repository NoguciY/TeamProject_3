using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //アニメーターコンポーネント
    [SerializeField]
    private Animator playerAnimator;

    //待機アニメーションをする
    public void SetIdleAnimation()
    {
        playerAnimator.SetBool("isMoving", false);
    }

    //走りアニメーションをする
    public void SetRunAnimation()
    {
        playerAnimator.SetBool("isMoving", true);

        //Sキー押下時のみ走りアニメーションを逆再生する
        if (Input.GetKey(KeyCode.S))
            playerAnimator.SetFloat("speed", -1);
        else
            playerAnimator.SetFloat("speed", 1);
    }

    //攻撃アニメーションをする
    public void SetAttackAnimation()
    {
        playerAnimator.SetTrigger("attackTrigger");
    }
}
