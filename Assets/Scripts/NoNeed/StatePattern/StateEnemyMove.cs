using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//移動状態
public class StateEnemyMove : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("移動状態に入りました");
        //移動アニメーション開始
        //GetOwner.GetAnimator.SetBool("Move", true);
    }

    public override void OnUpdate()
    {
        //爆弾に当たった場合、被ダメージ状態にする
        if (GetOwner.isHitting)
        {
            //stateMachine.ChangeState((int)StateType.ReceiveDamage);
        }
    }

    public override void OnFixedupdate()
    {
        //移動処理
        //Vector3 moveForce = GetOwner.GetEnemyFlocking.Move();
        //GetOwner.GetRigidb.velocity = new Vector3(moveForce.x, GetOwner.GetRigidb.velocity.y, moveForce.z);
    }

    public override void OnExit()
    {
        GetOwner.isHitting = false;
        //移動アニメーションの終了
        //GetOwner.GetAnimator.SetBool("Move", false);
    }
}