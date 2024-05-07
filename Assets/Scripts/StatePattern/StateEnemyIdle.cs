using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//待機状態
public class StateEnemyIdle : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("待機状態に入りました");

        //待機アニメーション開始
        //GetOwner.GetAnimator.SetBool("Idle", true);
    }

    public override void OnUpdate()
    {
        //待機状態でやることがないのですぐに移動状態に移行する
        stateMachine.ChangeState((int)StateType.Move);
    }

    public override void OnExit()
    {
        //待機アニメーションの終了
        //GetOwner.GetAnimator.SetBool("Idle", false);
    }
}