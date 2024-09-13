using UnityEngine;
using static PlayerEvent;

//爆弾を管理するクラス
public class BombManager : MonoBehaviour
{
    //爆弾のタイプ
    public enum BombType
    {
        Throwing,   //投擲型
        Planted,    //設置型
        Knockback,  //ノックバック型
        Homing,     //誘導型
    }


    //投擲爆弾--------------------------------------------------
    //プレハブ
    [SerializeField]
    private BombThrowing throwingBomb;

    //public BombThrowing GetBombThrowing => throwingBomb; 

    //設置型爆弾------------------------------------------------
    //プレハブ
    [SerializeField]
    private BombPlanted plantedBomb;

    //設置型爆弾の高さの半分
    private float plantedBombHalfHeight;

    //設置型爆弾の回転値
    private Quaternion plantedBombRotation;

    public BombPlanted GetBombPlanted => plantedBomb;

    //ノックバック爆弾------------------------------------------
    //プレハブ
    [SerializeField]
    private BombKnockback knockbackBomb;

    //public BombKnockback GetBombKnockback => knockbackBomb;

    //ノックバック爆弾の高さの半分
    private float knockbackHalfHeight;

    //ノックバック爆弾とプレイヤーの間の距離
    private float toPlayerDistance;

    //生成されるノックバック爆弾の数
    //private int generatedKnockbackBombNum;

    //誘導爆弾--------------------------------------------------
    //プレハブ
    [SerializeField]
    private BombHomingSpawner homingBombSpawner;

    //誘導爆弾の高さの半分
    private float homingBombHelfHeight;

    //その他----------------------------------------------------
    //爆発エフェクト
    [SerializeField, Header("投擲爆弾の爆発エフェクト")]
    private GameObject throwingBombExplosionParticle;

    [SerializeField, Header("設置型爆弾の爆発エフェクト")]
    private GameObject plantedBombExplosionParticle;

    [SerializeField, Header("ノックバック爆弾の爆発エフェクト")]
    private GameObject knockbackBombExplosionParticle;

    [SerializeField, Header("誘導爆弾の爆発エフェクト")]
    private GameObject homingBombExplosionParticle;

    //プレイヤーのTransformコンポーネント
    private Transform playerTransform;

    //プレイヤーのコライダー
    private CapsuleCollider playerCapsuleCollider;

    //プレイヤーの高さの半分
    private float playerHalfHeight;

    //経過時間
    private float[] deltaTime;

    //クールタイム
    private float[] coolTime;

    //爆弾を使ったかどうか
    private bool[] isUsingBomb;

    //爆弾が使用可能か
    private bool[] enableUseBomb;

    //爆弾追加カウンター
    private int addBombCounter;

    public int GetAddBombCounter => addBombCounter;


    /// <summary>
    /// 投擲爆弾を生成する
    /// </summary>
    /// <param name="coolDownEvent">クールダウンイベント</param>
    /// <param name="playerAnimation">アニメーション用クラス</param>
    private void GenerateThrowingBomb(PlayerEvent.CoolDownEvent coolDownEvent, PlayerAnimation playerAnimation)
    {
        if (isUsingBomb[(int)BombType.Throwing]) return;

        //攻撃アニメーション再生
        playerAnimation.SetAttackAnimation();

        //生成位置を計算して生成
        Vector3 spawnPos = playerTransform.position + Vector3.up * playerHalfHeight;
        GameObject bombPrefab = Instantiate(throwingBomb.gameObject, spawnPos, playerTransform.rotation);

        //プレイヤーの位置をセット
        var throwingBombComponent = bombPrefab.GetComponent<BombThrowing>();
        throwingBombComponent.playerTransform = playerTransform;
        throwingBombComponent.Initialize();

        //クールタイムのカウントを開始するフラグ
        isUsingBomb[(int)BombType.Throwing] = true;

        //クールタイムゲージのイベントを実行
        coolDownEvent.Invoke((int)BombType.Throwing, throwingBomb.GetCoolTime);
    }

    /// <summary>
    /// 設置型爆弾を生成する
    /// </summary>
    /// <param name="coolDownEvent">クールダウンイベント</param>
    private void GeneratePlantedBomb(PlayerEvent.CoolDownEvent coolDownEvent)
    {
        if (isUsingBomb[(int)BombType.Planted]) return;

        //生成位置を計算して生成
        Vector3 spawnPos = playerTransform.position + Vector3.up * plantedBombHalfHeight;
        GameObject bombPrefab = Instantiate(plantedBomb.gameObject, spawnPos, plantedBombRotation);

        //クールタイムのカウントを開始するフラグ
        isUsingBomb[(int)BombType.Planted] = true;

        //クールタイムゲージのイベントを実行
        coolDownEvent.Invoke((int)BombType.Planted, plantedBomb.GetCoolTime);
    }

    /// <summary>
    /// ノックバック爆弾を生成する
    /// </summary>
    /// <param name="coolDownEvent">クールダウンイベント</param>
    private void GenerateKnockbackBombs(PlayerEvent.CoolDownEvent coolDownEvent)
    {
        if (isUsingBomb[(int)BombType.Knockback]) return;

        //生成される爆弾数は変化するためここで取得する
        int generatedKnockbackBombNum = knockbackBomb.GetGeneratedBombNum;

        //爆弾を円状に等間隔に置くための角度
        float degree = 360 / generatedKnockbackBombNum;

        //y座標を除く爆弾の基準位置
        Vector3 offsetPos;

        for (int i = 0; i < generatedKnockbackBombNum; i++)
        {
            //爆弾を等間隔に配置する
            offsetPos = new Vector3(Mathf.Sin(degree * i * Mathf.Deg2Rad),
                0, Mathf.Cos(degree * i * Mathf.Deg2Rad));

            //生成位置(プレイヤーを中心に円状に等間隔に配置)
            Vector3 spawnPos = playerTransform.position +
                offsetPos * toPlayerDistance + Vector3.up * knockbackHalfHeight;

            GameObject bombPrefab = Instantiate(knockbackBomb.gameObject, spawnPos, Quaternion.identity);

            BombKnockback bombKnockbackComponet = bombPrefab.GetComponent<BombKnockback>();

            //プレイヤーの位置をセット
            bombKnockbackComponet.playerTransform = playerTransform;

            bombKnockbackComponet.myID = i;
        }

        //クールタイムのカウントを開始するフラグ
        isUsingBomb[(int)BombType.Knockback] = true;

        //クールタイムゲージのイベントを実行
        coolDownEvent.Invoke((int)BombType.Knockback, knockbackBomb.GetCoolTime);
    }

    /// <summary>
    /// ホーミング爆弾を生成する
    /// </summary>
    /// <param name="coolDownEvent">クールダウンイベント</param>
    private void GenerateHomingBomb(PlayerEvent.CoolDownEvent coolDownEvent)
    {
        if (isUsingBomb[(int)BombType.Homing]) return;

        Vector3 spawnPos = playerTransform.position + Vector3.up * homingBombHelfHeight * 10;
        GameObject bombPrefab = Instantiate(homingBombSpawner.gameObject, spawnPos, Quaternion.identity);

        //プレイヤーの位置をセット
        bombPrefab.GetComponent<BombHomingSpawner>().playerTransform = playerTransform;

        //クールタイムのカウントを開始するフラグ
        isUsingBomb[(int)BombType.Homing] = true;

        //クールタイムゲージのイベントを実行
        coolDownEvent.Invoke((int)BombType.Homing, homingBombSpawner.GetCoolTime);
    }

    /// <summary>
    /// 爆弾のクールタイムを計測し、再使用できるようにする
    /// </summary>
    /// <param name="bombID">爆弾の固有番号</param>
    private void CountBombCoolTime(int bombID)
    {
        if (!isUsingBomb[bombID]) return;

        //経過時間をカウント
        deltaTime[bombID] += Time.deltaTime;

        //経過時間がクールタイム以上の場合、使用可能にする
        if (deltaTime[bombID] >= coolTime[bombID])
        {
            isUsingBomb[bombID] = false;
            deltaTime[bombID] = 0;
        }
    }

    /// <summary>
    /// 爆弾関係のものを初期化する
    /// </summary>
    public void Initialize()
    {
        //プレイヤーのコンポーネントやパラメータを取得
        playerTransform = transform;
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerHalfHeight = playerTransform.localScale.y * playerCapsuleCollider.height * 0.5f;
        
        deltaTime = new float[Utilities.BOMBTYPENUM];
        coolTime = new float[Utilities.BOMBTYPENUM];
        isUsingBomb = new bool[Utilities.BOMBTYPENUM];
        isUsingBomb[(int)BombType.Throwing] = true;
        enableUseBomb = new bool[Utilities.BOMBTYPENUM];
        enableUseBomb[(int)BombType.Throwing] = true;
        addBombCounter = (int)BombType.Planted;

        //投擲爆弾関係の値の設定
        throwingBomb.explosionParticle = throwingBombExplosionParticle;
        coolTime[(int)BombType.Throwing] = throwingBomb.GetCoolTime;

        //設置型爆弾関係の値を取得
        plantedBombHalfHeight = plantedBomb.GetHalfHeight();
        plantedBomb.explosionParticle = plantedBombExplosionParticle;
        plantedBombRotation = plantedBomb.transform.rotation;
        coolTime[(int)BombType.Planted] = plantedBomb.GetCoolTime;

        //ノックバック爆弾関係の値を取得、設定
        knockbackHalfHeight = knockbackBomb.GetHalfHeight();
        toPlayerDistance = knockbackBomb.GetToPlayerDistance;
        knockbackBomb.explosionParticle = knockbackBombExplosionParticle;
        coolTime[(int)BombType.Knockback] = knockbackBomb.GetCoolTime;

        //誘導爆弾の関係の値を取得
        var homingBombComponent = homingBombSpawner.GetPrefab.GetComponent<BombHoming>();
        //homingBombのパーティクルに参照している誘導爆弾用の爆発パーティクルをセットする
        homingBombComponent.explosionParticle = homingBombExplosionParticle;
        homingBombHelfHeight = homingBombSpawner.GetBombHalfHeight();
        coolTime[(int)BombType.Homing] = homingBombSpawner.GetCoolTime;
    }

    /// <summary>
    /// 爆弾を生成する
    /// </summary>
    /// <param name="coolDownEvent">クールダウンイベント</param>
    /// <param name="playerAnimation">アニメーション用クラス</param>
    public void GenerateBomb(PlayerEvent.CoolDownEvent coolDownEvent, PlayerAnimation playerAnimation)
    {
        //爆弾のクールタイムを計測する
        for (int i = 0; i < 4; i++)
        {
            CountBombCoolTime(i);
        }

        //投擲爆弾を生成
        GenerateThrowingBomb(coolDownEvent, playerAnimation);

        //1つ目の爆弾使用可能フラグがtrueの場合
        if (enableUseBomb[(int)BombManager.BombType.Planted])
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //設置型爆弾を生成
                GeneratePlantedBomb(coolDownEvent);
            }
        }

        //2つ目の爆弾使用可能フラグがtrueの場合
        if (enableUseBomb[(int)BombManager.BombType.Knockback])
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //ノックバック爆弾を生成
                GenerateKnockbackBombs(coolDownEvent);
            }
        }
        //3つ目の爆弾使用可能フラグがtrueの場合
        if (enableUseBomb[(int)BombManager.BombType.Homing])
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //誘導爆弾を生成
                GenerateHomingBomb(coolDownEvent);
            }
        }
    }

    /// <summary>
    /// レベルアップで爆弾を追加する際に使う
    /// 爆弾を使用可能にする
    /// </summary>
    public void EnableNewBomb()
    {
        if (addBombCounter > enableUseBomb.Length) return;

        //爆弾を使用可能にするフラグをたてる
        enableUseBomb[addBombCounter] = true;

        //次にこの関数が呼ばれた時に、
        //別の爆弾を使用可能にするフラグをたてるためにカウンターを加算する
        if (addBombCounter < (int)BombType.Homing)
        {
            addBombCounter++;
        }
    }

    /// <summary>
    /// 爆弾が使用可能かを返す
    /// </summary>
    /// <param name="bombID">爆弾の固有番号</param>
    /// <returns>true:使用可能 / false:使用不可</returns>
    public bool CanUseBumb(int bombID)
    {
        if (enableUseBomb[bombID])
        {
            return true;
        }

        return false;
    }
}
