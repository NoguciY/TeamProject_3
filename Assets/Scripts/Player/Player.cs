using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using static BombManager;

public class Player : MonoBehaviour, IApplicableDamage, IGettableItem
{
    //値はレベルアップによって増加されるため、public
    //privateにするいい方法は何かないか
    [Header("最大HP")]
    public float maxLife;

    [Header("移動速度")] 
    public float speed;

    [Header("回収範囲率")]
    public float collectionRangeRate;

    [Header("防御力")]
    public float difense;

    [Header("回復力")]
    public float resilience;

    [SerializeField, Header("回復力のクールタイム(秒)")]
    private float resilienceCoolTime;

    //アイテム用のコライダーコンポーネント
    public CapsuleCollider capsuleColliderForItem;

    //プレイヤーのイベントコンポーネント
    [SerializeField]
    private PlayerEvent playerEvent;

    //体力コンポーネント
    [SerializeField]
    private LifeController lifeController;

    //レベルアップコンポーネント
    [SerializeField]
    private PlayerLevelUp playerLevelUp;

    //強化項目コンポーネント
    [SerializeField]
    private PlayerPowerUpItems powerUpItems;

    //移動に関するコンポーネント
    [SerializeField]
    private PlayerMove playerMove;

    //爆弾を管理するコンポーネント
    [SerializeField]
    private BombManager bombManager;

    //アニメーションに関するコンポーネント
    [SerializeField]
    private PlayerAnimation playerAnimation;

    //回復力の経過時間
    private float deltaTimeResilience;

    //爆弾使用可能フラグ
    private bool[] enableBombs;

    //カメラ
    private Camera mainCamera;

    //新しいボムを追加するカウンター
    //[SerializeField]
    private int newBombCounter;

    //新しいボムを追加する直前のレベル(10,20,30で追加)
    [SerializeField]
    private int[] addBombLevels;

    //トランスフォーム
    private Transform myTransform;

    //ゲッター
    public PlayerPowerUpItems GetPowerUpItems => powerUpItems;
    public PlayerEvent GetPlayerEvent => playerEvent;
    public LifeController GetLifeController => lifeController;
    public int GetNewBombCounter => newBombCounter;
    public PlayerLevelUp GetPlayerLevelUp => playerLevelUp;
    public BombManager GetBombManager => bombManager;


    private void Start()
    {
        myTransform = transform;

        mainCamera = Camera.main;

        //体力の初期化
        lifeController.InitializeLife(maxLife);
        
        //体力ゲージを体力の最大値にする
        playerEvent.getMaxLifeEvent.Invoke(maxLife);

        //レベルの初期化
        playerLevelUp.InitLevel();

        //爆弾の初期化
        bombManager.Initialize();

        enableBombs = new bool[Enum.GetNames(typeof(Utilities.AddedBombType)).Length];

        //0から爆弾の種類数-1までカウントする
        newBombCounter = 0;
    }

    void Update()
    {
        //ゲームオーバーかどうか
        if (!lifeController.IsDead())
        {
            if (playerMove.InputMove(speed) != Vector3.zero)
            {
                //移動する
                myTransform.position += playerMove.InputMove(speed);

                //走りアニメーション再生
                playerAnimation.SetRunAnimation();
            }
            else
                //待機アニメーション再生
                playerAnimation.SetIdleAnimation();

            //マウスカーソルの方向を向かせる
            UpdateRotationForMouse();

            //爆弾を生成する
            GenerateBomb();

            //爆弾テスト
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    bombManager.GeneratePlantedBomb();
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    bombManager.GenerateKnockbackBombs();
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    bombManager.GenerateHomingBomb();
            //}



            //レベルアップ
            if (playerLevelUp.GetLevel == addBombLevels[newBombCounter])
                playerLevelUp.LevelUp(playerEvent.addNewBombEvent);
            else
                playerLevelUp.LevelUp(playerEvent.levelUpEvent);

            //自動回復する
            HealAutomatically();
        }
        else
        {
            //ゲームオーバーイベント発火
            playerEvent.gameOverEvent.Invoke();   
        }
    }

    //ダメージを受ける関数(インターフェースで実装)
    public void ReceiveDamage(float damage)
    {
        float totalDamage = -damage + difense;
        
        //ダメージが正の値にならないようにする
        if(totalDamage > 0)
            totalDamage = 0;

        lifeController.AddValueToLife(totalDamage);
        
        playerEvent.addLifeEvent.Invoke(totalDamage);

        //効果音を流す
        SoundManager.uniqueInstance.Play("ダメージ");
        
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

    //マウスカーソル方向に回転座標を更新する
    private void UpdateRotationForMouse()
    {
        //プレイヤーのワールド空間座標をスクリーン座標に変換
        Vector3 screenPos = mainCamera.WorldToScreenPoint(myTransform.position);

        //プレイヤーからマウスカーソルの方向
        Vector3 direction = Input.mousePosition - screenPos;

        //2つのベクトルのなす角を取得
        float angle = Utilities.GetAngle(Vector3.zero, direction);

        //プレイヤーの回転座標の更新
        Vector3 angles = myTransform.localEulerAngles;
        angles.y = angle;
        myTransform.localEulerAngles = angles;
    }

    //爆弾を生成する
    private void GenerateBomb()
    {
        //爆弾のクールタイムを計測する
        for(int i = 0; i < Utilities.BOMBTYPENUM; i++)
            bombManager.CountBombCoolTime(i);

        //投擲爆弾を生成
        bombManager.GenerateThrowingBomb();

        //1つ目の爆弾使用可能フラグがtrueの場合
        if (enableBombs[0])
            if (Input.GetKeyDown(KeyCode.Alpha1))
                //設置型爆弾を生成
                bombManager.GeneratePlantedBomb(playerEvent.coolDownEvent);

        //2つ目の爆弾使用可能フラグがtrueの場合
        if (enableBombs[1])
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
                //ノックバック爆弾を生成
                bombManager.GenerateKnockbackBombs(playerEvent.coolDownEvent);
        }
        //3つ目の爆弾使用可能フラグがtrueの場合
        if (enableBombs[2])
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
                //誘導爆弾を生成
                bombManager.GenerateHomingBomb(playerEvent.coolDownEvent);
        }
    }

    //体力を自動回復する
    private void HealAutomatically()
    {
        //回復力が０より大きい場合かつ
        //ゲームオーバーでない場合
        if (resilience > 0)
        {
            //経過時間
            deltaTimeResilience += Time.deltaTime;
            
            //回復にはクールタイムを設ける
            if (deltaTimeResilience >= resilienceCoolTime)
            {
                //体力を回復力分増加
                lifeController.AddValueToLife(resilience);

                //体力ゲージを回復力分増加
                playerEvent.addLifeEvent.Invoke(resilience);

                deltaTimeResilience = 0;
            }
        }
    }

    //新しい爆弾を使用可能にする
    public void EnableNewBomb()
    {
        if (newBombCounter < enableBombs.Length)
        {
            //爆弾を使用可能にするフラグをたてる
            enableBombs[newBombCounter] = true;
            
            //次にこの関数が呼ばれた時に、
            //別の爆弾を使用可能にするフラグをたてるためにカウンターを加算する
            if(newBombCounter <= 1)
            newBombCounter++;
        }
    }

    public void Heal(float value)
    {
        lifeController.AddValueToLife(value);

        //体力ゲージを回復力分増加
        playerEvent.addLifeEvent.Invoke(value);
    }
}
