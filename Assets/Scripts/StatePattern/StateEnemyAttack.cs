using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻撃状態
public class StateEnemyAttack : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("攻撃状態に入りました");
        //攻撃アニメーション開始
    }

    public override void OnUpdate()
    {
        //何かしたら次のStateに変更する

    }

    public override void OnExit()
    {
        //攻撃アニメーションの終了
    }
}