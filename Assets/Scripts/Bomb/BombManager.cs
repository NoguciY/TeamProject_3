using System.Collections;
using System.Collections.Generic;
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

    public BombThrowing GetBombThrowing => throwingBomb; 

    //投擲爆弾のタイマー
    //private float throwingBombTimer;

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

    //ノックバック爆弾の高さの半分
    private float knockbackHalfHeight;

    //ノックバック爆弾とプレイヤーの間の距離
    private float toPlayerDistance;

    public BombKnockback GetBombKnockback => knockbackBomb;

    //生成されるノックバック爆弾の数
    //private int generatedKnockbackBombNum;

    //誘導爆弾--------------------------------------------------
    //プレハブ
    [SerializeField]
    private HomingBombSpawner homingBombSpawner;

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
    //[SerializeField]
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

    //爆弾関係のものを初期化する
    public void Initialize()
    {
        //プレイヤーのコンポーネントや値を取得
        playerTransform = transform;
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerHalfHeight = playerTransform.localScale.y * playerCapsuleCollider.height * 0.5f;
        
        deltaTime = new float[Utilities.BOMBTYPENUM];
        coolTime = new float[Utilities.BOMBTYPENUM];
        isUsingBomb = new bool[Utilities.BOMBTYPENUM];
        isUsingBomb[(int)BombType.Throwing] = true;

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
        homingBombHelfHeight = homingBombSpawner.GetBombHalfHeight;
        coolTime[(int)BombType.Homing] = homingBombSpawner.GetCoolTime;
    }

    //投擲爆弾を生成する
    public void GenerateThrowingBomb()
    {
        if (!isUsingBomb[(int)BombType.Throwing])
        {
            //生成位置を計算して生成
            Vector3 spawnPos = playerTransform.position + Vector3.up * playerHalfHeight;
            GameObject bombPrefab = Instantiate(throwingBomb.gameObject, spawnPos, playerTransform.rotation);

            //プレイヤーの位置をセット
            var throwingBombComponent = bombPrefab.GetComponent<BombThrowing>();
            throwingBombComponent.playerTransform = playerTransform;
            throwingBombComponent.Init();

            //クールタイムのカウントを開始するフラグ
            isUsingBomb[(int)BombType.Throwing] = true;
        }
    }

    //設置型爆弾を生成する
    public void GeneratePlantedBomb(CoolDownEvent coolDownEvent)
    {
        if (!isUsingBomb[(int)BombType.Planted])
        {
            //生成位置を計算して生成
            Vector3 spawnPos = playerTransform.position + Vector3.up * plantedBombHalfHeight;
            GameObject bombPrefab = Instantiate(plantedBomb.gameObject, spawnPos, plantedBombRotation);

            //クールタイムのカウントを開始するフラグ
            isUsingBomb[(int)BombType.Planted] = true;

            //クールタイムゲージのイベントを実行
            coolDownEvent.Invoke((int)Utilities.AddedBombType.Planted, plantedBomb.GetCoolTime);
        }
    }

    //ノックバック爆弾を生成する
    public void GenerateKnockbackBombs(CoolDownEvent coolDownEvent)
    {
        if (!isUsingBomb[(int)BombType.Knockback])
        {
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

                //プレイヤーの位置をセット
                bombPrefab.GetComponent<BombKnockback>().playerTransform = playerTransform;
            }

            //クールタイムのカウントを開始するフラグ
            isUsingBomb[(int)BombType.Knockback] = true;

            //クールタイムゲージのイベントを実行
            coolDownEvent.Invoke((int)Utilities.AddedBombType.Knockback, knockbackBomb.GetCoolTime);
        }
    }

    public void GenerateHomingBomb(CoolDownEvent coolDownEvent)
    {
        if (!isUsingBomb[(int)BombType.Homing])
        {
            Vector3 spawnPos = playerTransform.position + Vector3.up * homingBombHelfHeight * 10;
            GameObject bombPrefab = Instantiate(homingBombSpawner.gameObject, spawnPos, Quaternion.identity);

            //プレイヤーの位置をセット
            bombPrefab.GetComponent<HomingBombSpawner>().playerTransform = playerTransform;

            //クールタイムのカウントを開始するフラグ
            isUsingBomb[(int)BombType.Homing] = true;

            //クールタイムゲージのイベントを実行
            coolDownEvent.Invoke((int)Utilities.AddedBombType.Homing, homingBombSpawner.GetCoolTime);
        }
    }

    //爆弾のクールタイムを計測し、再使用できるようにする
    public void CountBombCoolTime(int num)
    {
        if (isUsingBomb[num])
        {
            //経過時間をカウント
            deltaTime[num] += Time.deltaTime;
            
            //経過時間がクールタイム以上の場合、使用可能にする
            if (deltaTime[num] >= coolTime[num])
            {
                isUsingBomb[num] = false;
                deltaTime[num] = 0;
            }
        }
    }
}
