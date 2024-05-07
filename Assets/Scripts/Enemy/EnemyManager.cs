using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ステート
public enum StateType
{
    Idle,           //待機
    Move,           //移動
    Attack,         //攻撃
    ReceiveDamage,  //被ダメージ
    Dead,           //死亡
}

public class EnemyManager : MonoBehaviour, IApplicableKnockback, IApplicableDamageEnemy
{
    //敵の群れでの移動用コンポーネント
    [SerializeField]
    private EnemyFlocking enemyFlocking;

    [SerializeField]
    private Rigidbody rigidb;

    //アニメーション用コンポーネント
    [SerializeField]
    private Animator animator;

    //爆弾に当たったかどうか
    public bool isHitting = false;

    //ノックバック時の敵の停止時間(秒)
    private float knockbackPauseTime = 1f;

    //ゲッター
    public EnemyFlocking GetEnemyFlocking => enemyFlocking;
    public Rigidbody GetRigidb => rigidb;
    public Animator GetAnimator => animator;

    //ステートマシン
    //private StateMachine<EnemyManager> stateMachine;

    private void Start()
    {
        //ステートマシン定義
        //stateMachine = new StateMachine<EnemyManager>(this);
        //stateMachine.Add<StateEnemyIdle>((int)StateType.Idle);
        //stateMachine.Add<StateEnemyMove>((int)StateType.Move);
        //stateMachine.Add<StateEnemyAttack>((int)StateType.Attack);
        //stateMachine.Add<StateEnemyReceiveDamage>((int)StateType.ReceiveDamage);
        //stateMachine.Add<StateEnemyDead>((int)StateType.Dead);

        ////ステート開始
        //stateMachine.OnStart((int)StateType.Idle);
    }

    private void Update()
    {
        //stateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        //stateMachine.OnFixedUpdate();

        //移動処理
        if (!isHitting)
        {
            Vector3 moveForce = enemyFlocking.Move();
            rigidb.velocity = new Vector3(moveForce.x, rigidb.velocity.y, moveForce.z);
        }
    }

    //ノックバックする(インターフェースで実装)
    public void Knockback(float knockbackForce, Vector3 bombMovingDirection)
    {
        //自身の動きを止める
        isHitting = true;

        //仮で2秒後に再始動
        Invoke("RestartMoving", knockbackPauseTime);

        //ノックバック方向
        Vector3 knockbackDistance = (bombMovingDirection - rigidb.velocity).normalized;

        //ノックバック方向にknockbackForceの威力で敵をノックバック
        rigidb.AddForce(knockbackDistance * knockbackForce, ForceMode.Impulse);

        Debug.Log($"{knockbackDistance * knockbackForce}でノックバック!!");
    }

    //移動を再開させる
    private void RestartMoving()
    {
        isHitting = false;
    }

    //ダメージを受ける(インターフェースで実装)
    public void ReceiveDamage(float damage)
    {
        Debug.Log($"敵は{damage} を受けた");
    }

    private void OnTriggerEnter(Collider other)
    {
        //ダメージを受けることができるオブジェクトを取得
        var applicableDamageObject = other.gameObject.GetComponent<IApplicableDamage>();

        if (applicableDamageObject != null)
        {
            //ダメージを受けさせる
            applicableDamageObject.ReceiveDamage(10);
        }
    }
}
