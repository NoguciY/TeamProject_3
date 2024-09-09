using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


public class EnemyManager : MonoBehaviour, IApplicableKnockback, IApplicableDamageEnemy
{
    //敵の群れでの移動用コンポーネント
    [SerializeField]
    private EnemyFlocking enemyFlocking;

    //public EnemyFlocking GetEnemyFlocking => enemyFlocking;

    //群の生成と管理をするコンポーネント
    public EnemySpawner enemySpawner;

    //経験値オブジェクト
    [SerializeField]
    private GameObject experienceValuePrefab;

    [SerializeField]
    private Rigidbody rigidb;

    //アニメーション用
    [SerializeField]
    private EnemyAnimation enemyAnimation;

    public Transform playerTransform;

    //爆弾に当たったかどうか
    public bool isHitting;

    //攻撃可能か
    private bool possibleAttack;

    //敵の情報
    public EnemySetting.EnemyData enemyData;

    //ノックバック時の敵の停止時間(秒)
    private float knockbackPauseTime;

    //体力
    private float health;

    //内積の閾値
    private float innerProductThred;

    
    private void Start()
    {
        //敵の初期化
        isHitting = false;
        possibleAttack = true;
        health = enemyData.maxHealth;
        knockbackPauseTime = 1f;
        enemyFlocking.Init(enemyData.minSpeed, enemyData.maxSpeed);

        //視野角を内積の閾値に使う
        innerProductThred = Mathf.Cos(enemyData.fieldOfView * Mathf.Deg2Rad);
    }

    private void FixedUpdate()
    {
        //メインゲームシーンの場合
        if (GameManager.Instance.CurrentSceneType == SceneType.MainGame)
        {
            //移動処理
            if (!isHitting)
            {
                //近隣の個体を取得する
                enemyFlocking.AddNeighbors(
                    enemySpawner, enemyData.generationOrder, 
                    enemyData.ditectingNeiborDistance, innerProductThred);

                //移動する
                Vector3 moveForce = enemyFlocking.Move(playerTransform, enemyData);
                rigidb.velocity = new Vector3(moveForce.x, rigidb.velocity.y, moveForce.z);
                enemyAnimation.SetRunAnimation();
            }
        }
        //オプションシーンの場合
        else if (GameManager.Instance.CurrentSceneType == SceneType.Option)
        {
            enemyAnimation.SetIdleAnimation();
        }
    }

    /// <summary>
    /// ノックバックする(インターフェースで実装)
    /// </summary>
    /// <param name="knockbackForce">ノックバック力</param>
    /// <param name="bombMovingDirection">ノックバック爆弾の移動方向</param>
    public async void Knockback(float knockbackForce, Vector3 bombMovingDirection)
    {
        //GameObjectが破棄された時にキャンセルを飛ばすトークンを作成
        var token = this.GetCancellationTokenOnDestroy();

        //自身の動きを止める
        isHitting = true;

        possibleAttack = false;

        enemyAnimation.SetReceiveDamageAnimation();

        //ノックバック方向
        Vector3 knockbackDistance = bombMovingDirection.normalized;

        //ノックバック方向にknockbackForceの威力で敵をノックバック
        rigidb.AddForce(knockbackDistance * knockbackForce, ForceMode.Impulse);

        Debug.Log($"{knockbackDistance * knockbackForce}でノックバック!!");

        //待機後の処理
        try
        {
            //指定した秒数待つ
            await UniTask.Delay(TimeSpan.FromSeconds(knockbackPauseTime), cancellationToken: token);

            isHitting = false;

            possibleAttack = true;
        }
        //待機中に敵が破棄された場合
        catch (OperationCanceledException exception)
        {
            Debug.Log("ノックバック処理がキャンセルされました。");
        }
    }

    /// <summary>
    /// ダメージを受ける(インターフェースで実装)
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void ReceiveDamage(float damage)
    {
        health -= damage;
        Debug.Log($"敵は{damage} を受けた");

        //体力が0になった場合、死亡
        if (health <= 0)
        {
            possibleAttack = false;
            enemyAnimation.SetDeadAnimation();
        }
    }

    /// <summary>
    /// Animation Eventで参照している
    /// やられアニメーション終了後に行う死亡した時の処理
    /// </summary>
    private void Dead()
    {
        //群から自身を削除
        if (enemySpawner != null)
        {
            enemySpawner.boids[enemyData.generationOrder].Remove(this.gameObject);
        }

        //倒した敵の数を増やす
        GameManager.Instance.deadEnemyMun++;

        //自身を破棄
        Destroy(this.gameObject);

        //アイテムを生成
        GameObject experienceValue = Instantiate(experienceValuePrefab, this.transform.position, experienceValuePrefab.transform.rotation);
        GameManager.Instance.items.Add(experienceValue.GetComponent<ItemExperienceValue>());
    }

    /// <summary>
    /// プレイヤーのisTriggerでないコライダーと当たり判定を行う
    /// </summary>
    /// <param name="collision">敵</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (possibleAttack)
        {
            //ダメージを受けることができるオブジェクトを取得
            var applicableDamageObject = collision.gameObject.GetComponent<IApplicableDamage>();

            if (applicableDamageObject != null)
            {
                //ダメージを受けさせる
                applicableDamageObject.ReceiveDamage(enemyData.attack);
            }
        }
    }
}
