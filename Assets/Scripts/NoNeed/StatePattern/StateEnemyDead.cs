using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//死亡状態
public class StateEnemyDead : StateMachine<EnemyManager>.StateBase
{
    public override void OnStart()
    {
        Debug.Log("死亡状態に入りました");
        //死亡アニメーション開始
    }

    public override void OnUpdate()
    {
        //何かしたら次のStateに変更する
    }

    public override void OnExit()
    {
        //死亡アニメーションの終了

    }
}