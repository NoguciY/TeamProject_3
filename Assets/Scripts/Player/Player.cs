using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

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

    //プレイヤーのイベントコンポーネント
    [SerializeField]
    private PlayerEvent playerEvent;

    //体力コンポーネント
    [SerializeField]
    private LifeController lifeController;

    //レベルアップコンポーネント
    [SerializeField]
    private PlayerLevelUp playerLevelUp;

    //プレイヤーステータスコンポーネント
    [SerializeField]
    private PlayerDefaultStatus playerStatus;

    //強化項目コンポーネント
    [SerializeField]
    private PowerUpItems powerUpItems;

    //ゲームマネージャー
    [SerializeField]
    private GameManager gameManager;

    //爆弾を管理するコンポーネント
    [SerializeField]
    private BombManager bombManager;

    //アイテム用のコライダーコンポーネント
    public CapsuleCollider capsuleColliderForItem;

    //爆弾の縦幅の半分
    //private float bombHelfHeight;

    //設置型爆弾の回転値
    //private Quaternion bombRotation;

    //回復力のクールタイム
    private float resilienceCoolTime;

    //爆弾使用可能フラグ
    private bool[] canUseBombFlags = new bool[3];

    //ゲッター
    public PowerUpItems GetPowerUpItems => powerUpItems;
    public PlayerEvent GetPlayerEvent => playerEvent;

    //カメラ(仮)
    private Camera mainCamera;

    //新しいボムを追加するカウンター
    private int newBombCounter = 0;

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

        //レベルの初期化
        playerLevelUp.InitLevel();

        //回復力のクールタイム初期化
        resilienceCoolTime = 1f;

        mainCamera = Camera.main;

        //爆弾の初期化
        bombManager.Initialize();
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


        var screenPos = mainCamera.WorldToScreenPoint(transform.position);

        var direction = Input.mousePosition - screenPos;

        var angle = Utilities.GetAngle(Vector3.zero, direction);

        var angles = transform.localEulerAngles;
        angles.y = angle - 0;
        transform.localEulerAngles = angles;


        //爆弾を生成する
        //GenerateBomb();

        if (Input.GetMouseButtonDown(0))
        {
            bombManager.GeneratePlantedBomb();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bombManager.GenerateKnockbackBomb();
        }

        //自動回復する
        AutomaticRecovery();

        //レベルアップ
        if (playerLevelUp.GetLevel == 10)
        {
            playerLevelUp.LevelUp(playerEvent.AddNewBombEvent);
        }
        else if (playerLevelUp.GetLevel == 20)
        {

        }
        else if (playerLevelUp.GetLevel == 30)
        {

        }
        else
            playerLevelUp.LevelUp(playerEvent.levelUpEvent);


        //体力が0になった場合、ゲームオーバー
        if (lifeController.IsDead())
        {
            //体力ゲージが０出ない場合、体力ゲージを0にする
            playerEvent.gameOverEvent.Invoke();
        }
    }

    //ダメージを受ける関数(インターフェースで実装)
    public void ReceiveDamage(float damage)
    {
        float totalDamage = -damage + difence;
        lifeController.AddValueToLife(totalDamage);
        playerEvent.addLifeEvent.Invoke(totalDamage);
        Debug.Log($"プレイヤーは{-totalDamage}ダメージ食らった\n" +
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


    //爆弾を生成する
    private void GenerateBomb()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //設置型爆弾を生成
            bombManager.GeneratePlantedBomb();
        }

        //10レベルの場合
        if (canUseBombFlags[0])
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //クールダウンが終わっている場合
                //ノックバック爆弾を生成
                bombManager.GenerateKnockbackBomb();
            }
        }
    }

    //体力を自動回復する
    private void AutomaticRecovery()
    {
        //回復力が０より大きい場合かつ
        //ゲームオーバーでない場合
        if (resilience > 0　&& !lifeController.IsDead())
        {
            //経過時間
            float deltaTime = gameManager.GetDeltaTimeInMain;
            
            //回復にはクールタイムを設ける
            if (deltaTime >= resilienceCoolTime)
            {
                //体力を回復力分増加
                lifeController.AddValueToLife(resilience);
                //体力ゲージを回復力分増加
                playerEvent.addLifeEvent.Invoke(resilience);

                resilienceCoolTime++;
            }
        }
    }

    //新しい爆弾を使用可能にする
    public void NewBombEnabled()
    {
        if (newBombCounter < canUseBombFlags.Length)
        {
            //爆弾を使用可能にするフラグをたてる
            canUseBombFlags[newBombCounter] = true;
            
            //次にこの関数が呼ばれた時に、
            //別の爆弾を使用可能にするフラグをたてるためにカウンターを加算する
            newBombCounter++;
        }
    }
}
