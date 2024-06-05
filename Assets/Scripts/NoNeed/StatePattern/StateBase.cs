using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
//ステートの基底クラス
public abstract class StateBase<T> where T : StateMachine<T>
{
    //所属するステートマシン
    protected T stateMachine;

    //コンストラクタ
    public StateBase(T stateMachine){ this.stateMachine = stateMachine; }

    //ステート開始時に実行する
    public virtual void OnStart() { }

    //Updateメソッド内で呼び出す
    //毎フレーム毎に行いたい処理を実行する
    public virtual void OnUpdate() { }

    //ステート終了時に実行する
    public virtual void OnExit() { }
}
*/