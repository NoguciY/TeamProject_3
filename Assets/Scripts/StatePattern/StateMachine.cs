using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステート基底クラスと関連処理をまとめたクラス
//各ステートをまとめてメソッドを呼び出す

public class StateMachine<TOwner>
{
    //ステート基底クラス
    public abstract class StateBase
    {
        //所属するステートマシン
        public StateMachine<TOwner> stateMachine;
        
        //ゲッター
        protected TOwner GetOwner => stateMachine.owner;

        //ステート開始時に実行する
        public virtual void OnStart() { }

        //Updateメソッド内で呼び出す
        //毎フレーム毎に行いたい処理
        public virtual void OnUpdate() { }

        //FixedUpdateメソッド内で呼び出す
        public virtual void OnFixedupdate() { }

        //ステート終了時に実行する
        public virtual void OnExit() { }
    }

    //ステートマシーンの使用者
    private TOwner owner;

    //ゲッター
    //public TOwner GetOwner => owner;

    //現在のステート
    private StateBase currentState;

    //全てのステートを定義するためのディクショナリー
    //int:enumで列挙したステート
    //StateBase:ステートクラスに継承させる基底クラス型
    private readonly Dictionary<int, StateBase> states = new Dictionary<int, StateBase>();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="owner">ステートマシンの使用者</param>
    public StateMachine(TOwner owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// ステート定義を登録
    /// ステートマシン初期化後にこのメソッドを呼ぶ
    /// </summary>
    /// <typeparam name="TState">ステート型</typeparam>
    /// <param name="stateId">ステートID</param>
    public void Add<TState>(int stateId) where TState : StateBase, new()
    {
        //同じステートID(Key)をstatesに格納するのを防ぐ
        if (states.ContainsKey(stateId))
        {
            Debug.LogError($"既にステートIDが登録されています:{stateId}");
            return;
        }
        //ステート定義を登録(ステートをディクショナリーに格納)
        var newState = new TState
        {
            //所属するステートマシンにアクセスするため
            //新しいステートにステートマシンの参照を設定
            stateMachine = this 
        };
        states.Add(stateId, newState);
    }

    /// <summary>
    /// ステート開始処理
    /// </summary>
    /// <param name="stateId">現在のステートID</param>
    public void OnStart(int stateId)
    {
        //StateIDがステートと関連付けられていない場合、エラー
        if (!states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError($"ステートIDが設定されていません:{stateId}");
            return;
        }

        //現在のステートに設定して処理を開始
        currentState = nextState;
        currentState.OnStart();
    }

    /// <summary>
    /// ステート更新処理
    /// </summary>
    public void OnUpdate()
    {
        currentState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        currentState.OnFixedupdate();
    }

    /// <summary>
    /// 次のステートに切り替える
    /// </summary>
    /// <param name="stateId">切り替えるステートID</param>
    public void ChangeState(int stateId)
    {
        if (!states.TryGetValue(stateId, out var nextState))
        {
            Debug.LogError($"ステートIDが設定されていません{stateId}");
            return;
        }

        //ステートを切り替える
        currentState.OnExit();
        currentState = nextState;
        currentState.OnStart();
    }
}