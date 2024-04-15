using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IApplicableDamage, IGettableItem
{
    //最大HP
    //値はレベルアップによって増加されるため、public
    //privateにするいい方法は何かないか
    public float maxLife;

    //移動速度
    public float speed;

    //回収範囲率
    public float collectionRangeRate;

    //防御力
    public float difence;

    //回復力
    public float resilience;

    //爆弾プレハブ
    public GameObject bombPrefab;

    //体力コンポーネント
    [SerializeField]
    private LifeController lifeController;

    //レベルアップコンポーネント
    [SerializeField]
    private PlayerLevelUp playerLevelUp;

    //プレイヤーのイベントコンポーネント
    public PlayerEvent playerEvent;

    //プレイヤーステータスコンポーネント
    [SerializeField]
    private PlayerStatus playerStatus;

    //強化項目コンポーネント
    [SerializeField]
    private PowerUpItems powerUpItems;

    //アイテム用のコライダーコンポーネント
    //[SerializeField]
    public CapsuleCollider capsuleColliderForItem;

    //爆弾の縦幅の半分
    private float bombHelfHeight;

    //設置型爆弾の回転値
    private Quaternion bombRotation;

    //ゲッター
    public PowerUpItems GetPowerUpItems { get { return powerUpItems; } }
    //public float GetMaxLife { get { return maxLife; } }

    private void Start()
    {
        //ステータスの初期化
        maxLife = playerStatus.maxLife;
        speed = playerStatus.speed;
        collectionRangeRate = playerStatus.collectionRangeRate;
        difence = playerStatus.difence;
        resilience = playerStatus.resilience;

        //体力の初期化
        lifeController.InitializeLife(maxLife);
        
        //体力ゲージを体力の最大値にする
        playerEvent.getMaxLifeEvent.Invoke(maxLife);

        //爆弾関係の値を取得
        bombHelfHeight = bombPrefab.GetComponent<PlantedBomb>().GetHalfHeight();
        bombRotation = bombPrefab.transform.rotation;

        //レベルの初期化
        playerLevelUp.InitLevel();
    }

    void Update()
    {
        //例えば、WDを一緒に押すとWを押しているときより、速く進んでいるので修正する
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= speed * transform.right * Time.deltaTime;
        }

        //爆弾を生成する
        InstantiatePlantedBomb();

        //自動回復する
        AutomaticRecovery();

        //体力が0になった場合、ゲームオーバー
        if (lifeController.IsDead())
        {
            playerEvent.gameOverEvent.Invoke();
        }

        //レベルアップ
        playerLevelUp.LevelUp(playerEvent.levelUpEvent);
    }

    //ダメージを受ける関数(インターフェースで実装)
    public void RecieveDamage(float damage)
    {
        float totalDamage = -damage + difence;
        lifeController.AddValueToLife(totalDamage);
        playerEvent.damageEvent.Invoke(totalDamage);
        Debug.Log($"プレイヤーは{totalDamage}ダメージ食らった\n" +
            $"残りの体力：{lifeController.GetLife}");
    }


    //経験値を取得する関数(インターフェースで実装)
    public void GetExp(int exp)
    {
        //経験値を加える
        playerLevelUp.AddExp(exp);

        //経験値ゲージを更新するイベントを実行
        playerEvent.expEvent.Invoke(playerLevelUp.GetExp, playerLevelUp.GetNeedExp);
    }


    //設置型爆弾を生成する
    private void InstantiatePlantedBomb()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //生成位置を計算して生成
            Vector3 spawnPos = transform.position + Vector3.up * bombHelfHeight;
            Instantiate(bombPrefab, spawnPos, bombRotation);
        }
    }

    //体力を自動回復する
    private void AutomaticRecovery()
    {
        //回復力が０より大きい場合
        if (resilience > 0)
        {
            lifeController.AddValueToLife(resilience);
        }
    }
}
