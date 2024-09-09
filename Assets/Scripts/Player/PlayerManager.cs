using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IApplicableDamage, IGettableItem
{
    //値はレベルアップによって増加されるため、public
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

    public PlayerEvent GetPlayerEvent => playerEvent;

    //体力コンポーネント
    [SerializeField]
    private PlayerLifeController lifeController;

    public PlayerLifeController GetLifeController => lifeController;

    //レベルアップコンポーネント
    [SerializeField]
    private PlayerLevelUp playerLevelUp;

    public PlayerLevelUp GetPlayerLevelUp => playerLevelUp;

    //強化項目コンポーネント
    [SerializeField]
    private PlayerPowerUpItems powerUpItems;

    public PlayerPowerUpItems GetPowerUpItems => powerUpItems;

    //移動に関するコンポーネント
    [SerializeField]
    private PlayerMove playerMove;

    //爆弾を管理するコンポーネント
    [SerializeField]
    private BombManager bombManager;

    public BombManager GetBombManager => bombManager;

    //アニメーションに関するコンポーネント
    [SerializeField]
    private PlayerAnimation playerAnimation;

    //新しいボムを追加する直前のレベル
    [SerializeField]
    private int[] addBombLevels;

    //回復力の経過時間
    private float deltaTimeResilience;

    //爆弾使用可能フラグ
    private bool[] enableBombs;

    //カメラ
    private Camera mainCamera;

    //新しいボムを追加するカウンター
    private int newBombCounter;

    public int GetNewBombCounter => newBombCounter;

    private Transform myTransform;


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

        //経験値ゲージの最大値の更新
        playerEvent.getMaxExperienceValueEvent.Invoke(
            playerLevelUp.GetExperienceValue, playerLevelUp.GetNeedExperienceValue);

        //爆弾の初期化
        bombManager.Initialize();

        enableBombs = new bool[Enum.GetNames(typeof(Utilities.AddedBombType)).Length];

        //0から爆弾の種類数-1までカウントする
        newBombCounter = 0;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentSceneType != SceneType.MainGame) return;

        if (!lifeController.GetIsDead)
        {
            if (playerMove.InputMove(speed) != Vector3.zero)
            {
                //移動する
                myTransform.position += playerMove.InputMove(speed);

                //走りアニメーション再生
                playerAnimation.SetRunAnimation();
            }
            else
            {
                //待機アニメーション再生
                playerAnimation.SetIdleAnimation();
            }

            //マウスカーソルの方向を向かせる
            playerMove.UpdateRotationForMouse(mainCamera, myTransform);

            //爆弾を生成する
            GenerateBomb();

            //自動回復する
            HealAutomatically();
        }

        //ゲームオーバーの場合
        if (lifeController.IsDead())
        {
            GameOver();
        }
    }

    /// <summary>
    /// ダメージを受ける関数(インターフェースで実装)
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void ReceiveDamage(float damage)
    {
        if (lifeController.GetIsDead) return;

        //効果音(被ダメージ音 or やられ音)を流す
        if (lifeController.GetLife > damage)
        {
            SoundManager.uniqueInstance.Play("ダメージ");
        }

        //最終的なダメージの値
        float totalDamage = difense - damage;

        //ダメージが正の値にならないようにする
        if (totalDamage > 0)
        {
            totalDamage = 0;
        }

        lifeController.AddValueToLife(totalDamage);

        playerEvent.addLifeEvent.Invoke(lifeController.GetLife);

        Debug.Log($"プレイヤーは{-totalDamage}ダメージ食らった\n" +
            $"残りの体力：{lifeController.GetLife}");

    }

    /// <summary>
    /// 経験値を取得する関数(インターフェースで実装)
    /// </summary>
    /// <param name="experienceValue">経験値</param>
    public void GetExperienceValue(float experienceValue)
    {
        if (lifeController.GetIsDead) return;

        //経験値を加える
        playerLevelUp.AddExperienceValue(experienceValue);

        //レベルアップしたか
        playerLevelUp.LevelUp(playerEvent.levelUpEvent, playerEvent.addNewBombEvent,
            addBombLevels[newBombCounter]);

        //経験値ゲージを更新するイベントを実行
        playerEvent.experienceValueEvent.Invoke(playerLevelUp.GetExperienceValue, playerLevelUp.GetNeedExperienceValue);

    }

    /// <summary>
    /// 爆弾を生成する
    /// </summary>
    private void GenerateBomb()
    {
        //爆弾のクールタイムを計測する
        for (int i = 0; i < Utilities.BOMBTYPENUM; i++)
        {
            bombManager.CountBombCoolTime(i);
        }

        //投擲爆弾を生成
        bombManager.GenerateThrowingBomb(playerAnimation);

        //1つ目の爆弾使用可能フラグがtrueの場合
        if (enableBombs[0])
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //設置型爆弾を生成
                bombManager.GeneratePlantedBomb(playerEvent.coolDownEvent);
            }
        }

        //2つ目の爆弾使用可能フラグがtrueの場合
        if (enableBombs[1])
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //ノックバック爆弾を生成
                bombManager.GenerateKnockbackBombs(playerEvent.coolDownEvent);
            }
        }
        //3つ目の爆弾使用可能フラグがtrueの場合
        if (enableBombs[2])
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //誘導爆弾を生成
                bombManager.GenerateHomingBomb(playerEvent.coolDownEvent);
            }
        }
    }

    /// <summary>
    /// 体力を自動回復する
    /// </summary>
    private void HealAutomatically()
    {
        //回復力が０より大きい場合
        if (resilience <= 0) return;

        //経過時間
        deltaTimeResilience += Time.deltaTime;

        //回復にはクールタイムを設ける
        if (deltaTimeResilience >= resilienceCoolTime)
        {
            //体力を回復力分増加
            lifeController.AddValueToLife(resilience);

            //体力ゲージを回復力分増加
            playerEvent.addLifeEvent.Invoke(lifeController.GetLife);

            deltaTimeResilience = 0;
        }
    }

    /// <summary>
    /// 新しい爆弾を使用可能にする
    /// </summary>
    public void EnableNewBomb()
    {
        if (newBombCounter >= enableBombs.Length) return;

        //爆弾を使用可能にするフラグをたてる
        enableBombs[newBombCounter] = true;

        //次にこの関数が呼ばれた時に、
        //別の爆弾を使用可能にするフラグをたてるためにカウンターを加算する
        if (newBombCounter <= 1)
        {
            newBombCounter++;
        }
    }

    /// <summary>
    /// 回復する
    /// </summary>
    /// <param name="recoveryValue"></param>
    public void Heal(float recoveryValue)
    {
        lifeController.AddValueToLife(recoveryValue);

        //体力ゲージを回復力分増加
        playerEvent.addLifeEvent.Invoke(lifeController.GetLife);
    }

    /// <summary>
    /// 爆弾が使用可能かを返す
    /// </summary>
    /// <param name="bombID">爆弾の固有番号</param>
    /// <returns>true:使用可能 / false:使用不可</returns>
    public bool CanUseBumb(int bombID)
    {
        if (enableBombs[bombID])
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// やられアニメーション終了後、ゲームオーバーイベントの処理をする
    /// </summary>
    private async void GameOver()
    {
        //GameObjectが破棄された時にキャンセルを飛ばすトークンを作成
        var token = this.GetCancellationTokenOnDestroy();

        SoundManager.uniqueInstance.Play("やられ");

        playerAnimation.SetDeadAnimation();

        //UniTaskメソッドの引数にCancellationTokenを入れる
        await UniTask.WaitUntil(() => 
            playerAnimation.isFinishedDeadAnimation, cancellationToken : token);

        playerAnimation.isFinishedDeadAnimation = false;

        //ゲームオーバーイベント発火
        playerEvent.gameOverEvent.Invoke();
    }
}
