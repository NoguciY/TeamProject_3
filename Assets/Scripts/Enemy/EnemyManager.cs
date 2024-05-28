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

    //群の生成と管理をするコンポーネント
    public EnemyFlockManager flockManager;

    //経験値オブジェクト
    [SerializeField]
    private GameObject expPrefab;

    [SerializeField]
    private Rigidbody rigidb;

    //アニメーション用コンポーネント
    [SerializeField]
    private Animator animator;

    //爆弾に当たったかどうか
    public bool isHitting;

    //敵の情報
    public EnemySetting.EnemyData enemyData;

    //ノックバック時の敵の停止時間(秒)
    private float knockbackPauseTime;

    //体力
    private float health;

    //内積の閾値
    private float innerProductThred;

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

        //敵の初期化
        isHitting = false;
        health = enemyData.maxHealth;
        knockbackPauseTime = 1f;
        //視野角を内積の閾値に使う
        innerProductThred = Mathf.Cos(enemyData.fieldOfView * Mathf.Deg2Rad);
    }

    private void Update()
    {
        //近隣の個体を取得する
        enemyFlocking.AddNeighbors(flockManager, innerProductThred);
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

    //ノックバックされた場合に移動を再開させる
    private void RestartMoving()
    {
        isHitting = false;
    }

    //ダメージを受ける(インターフェースで実装)
    public void ReceiveDamage(float damage)
    {
        health -= damage;
        Debug.Log($"敵は{damage} を受けた");

        //体力が0になった場合、死亡
        if (health <= 0)
            Dead();
    }

    //死亡した時の処理
    private void Dead()
    {
        //群から自身を削除
        if (flockManager != null)
            flockManager.boids.Remove(this.gameObject);

        //倒した敵の数を増やす
        GameManager.Instance.deadEnemyMun++;

        //自身を破棄
        Destroy(this.gameObject);
        //アイテムを生成
        Instantiate(expPrefab, this.transform.position, expPrefab.transform.rotation);
    }

    //プレイヤーのisTriggerでないコライダーと当たり判定を行う
    private void OnCollisionEnter(Collision collision)
    {
        //ダメージを受けることができるオブジェクトを取得
        var applicableDamageObject = collision.gameObject.GetComponent<IApplicableDamage>();

        if (applicableDamageObject != null)
            //ダメージを受けさせる
            applicableDamageObject.ReceiveDamage(enemyData.attack);
    }
}
