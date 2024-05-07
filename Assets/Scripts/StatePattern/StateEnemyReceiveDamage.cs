using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ダメージを受けた場合の状態
public class StateEnemyReceiveDamage : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("被ダメージ状態に入りました");
        //被ダメージアニメーション開始
    }

    public override void OnUpdate()
    {
        //ノックバックしている場合、何秒か経った後に待機状態にする


        //体力が0の場合、死亡状態にする
        //if()
        //{
        //    stateMachine.ChangeState((int)StateType.Dead);
        //}
    }

    public override void OnExit()
    {
        //被ダメージアニメーションの終了
    }
}
