using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//爆弾を管理するクラス
public class BombManager : MonoBehaviour
{
    //投擲爆弾--------------------------------------------------
    //プレハブ
    [SerializeField]
    private GameObject throwingBombPrefab;

    //投擲爆弾のタイマー
    private float throwingBombTimer;

    //設置型爆弾------------------------------------------------
    //プレハブ
    [SerializeField]
    private GameObject plantedBombPrefab;

    //設置型爆弾の高さの半分
    private float plantedBombHalfHeight;

    //設置型爆弾の回転値
    private Quaternion plantedBombRotation;

    //ノックバック爆弾------------------------------------------
    //プレハブ
    [SerializeField]
    private GameObject knockbackBombPrefab;

    //ノックバック爆弾の高さの半分
    private float knockbackHalfHeight;

    //ノックバック爆弾とプレイヤーの間の距離
    private float toPlayerDistance;

    //生成されるノックバック爆弾の数
    //private int generatedKnockbackBombNum;

    //誘導爆弾--------------------------------------------------
    //プレハブ
    [SerializeField]
    private GameObject missileSpawnPrefab;

    //誘導爆弾の高さの半分
    private float homingBombHelfHeight;

    //その他----------------------------------------------------
    //爆発エフェクト
    [SerializeField, Header("ノックバック爆弾の爆発エフェクト")]
    private GameObject knockbackBombExplosionParticle;

    //プレイヤーのTransformコンポーネント
    //[SerializeField]
    private Transform playerTransform;

    //プレイヤーのコライダー
    private CapsuleCollider playerCapsuleCollider;

    //プレイヤーの高さの半分
    private float playerHalfHeight;


    //爆弾関係のものを初期化する
    public void Initialize()
    {
        //プレイヤーのコンポーネントや値を取得
        playerTransform = transform;
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerHalfHeight = playerTransform.localScale.y * playerCapsuleCollider.height * 0.5f;

        //投擲爆弾関係の値の設定
        var throwingBombComponent = throwingBombPrefab.GetComponent<ThrowingBomb>();
        throwingBombComponent.explosionParticle = knockbackBombExplosionParticle;
        throwingBombTimer = 0;

        //設置型爆弾関係の値を取得
        var plantedBombComponent = plantedBombPrefab.GetComponent<PlantedBomb>();
        plantedBombHalfHeight = plantedBombComponent.GetHalfHeight();
        plantedBombComponent.explosionParticle = knockbackBombExplosionParticle;
        plantedBombRotation = plantedBombPrefab.transform.rotation;

        //ノックバック爆弾関係の値を取得、設定
        var knockbackBombComponent = knockbackBombPrefab.GetComponent<KnockbackBomb>();
        knockbackHalfHeight = knockbackBombComponent.GetHalfHeight();
        toPlayerDistance = knockbackBombComponent.GetToPlayerDistance;
        knockbackBombComponent.explosionParticle = knockbackBombExplosionParticle;

        //誘導爆弾の関係の値を取得
        homingBombHelfHeight = missileSpawnPrefab.GetComponent<MissileSpawner>().GetBombHalfHeight;
    }

    //投擲爆弾を生成する
    public void GenerateThrowingBomb()
    {
        //生成位置を計算して生成
        Vector3 spawnPos = playerTransform.position + Vector3.up * playerHalfHeight;
        GameObject bombPrefab = Instantiate(throwingBombPrefab, spawnPos, playerTransform.rotation);

        //プレイヤーの位置をセット
        var throwingBombComponent = bombPrefab.GetComponent<ThrowingBomb>();
        throwingBombComponent.playerTransform = playerTransform;
        throwingBombComponent.Init();
    }

    //設置型爆弾を生成する
    public void GeneratePlantedBomb()
    {
        //生成位置を計算して生成
        Vector3 spawnPos = playerTransform.position + Vector3.up * plantedBombHalfHeight;
        GameObject bombPrefab = Instantiate(plantedBombPrefab, spawnPos, plantedBombRotation);
    }

    //ノックバック爆弾を生成する
    public void GenerateKnockbackBombs()
    {
        //生成される爆弾数は変化するためここで取得する
        int generatedKnockbackBombNum = 
            knockbackBombPrefab.GetComponent<KnockbackBomb>().GetGeneratedBombNum;

        //爆弾を円状に等間隔に置くための角度
        float degree = 360 / generatedKnockbackBombNum;

        Debug.Log($"爆弾生成数:{generatedKnockbackBombNum}");

        //y座標を除く爆弾の基準位置
        Vector3 standardPos;
        
        for (int i = 0; i < generatedKnockbackBombNum; i++)
        {
            //爆弾を等間隔に配置する
            standardPos = new Vector3(Mathf.Sin(degree * i * Mathf.Deg2Rad), 
                0, Mathf.Cos(degree * i * Mathf.Deg2Rad));

            //生成位置(プレイヤーを中心に円状に等間隔に配置)
            Vector3 spawnPos = playerTransform.position +
                standardPos * toPlayerDistance + Vector3.up * knockbackHalfHeight;

            GameObject bombPrefab = Instantiate(knockbackBombPrefab, spawnPos, Quaternion.identity);

            //プレイヤーの位置をセット
            bombPrefab.GetComponent<KnockbackBomb>().playerTransform = playerTransform;
        }
    }

    public void GenerateHomingBomb()
    {
        Vector3 spawnPos = playerTransform.position + Vector3.up * homingBombHelfHeight * 10;
        GameObject bombPrefab = Instantiate(missileSpawnPrefab, spawnPos, Quaternion.identity);

        //プレイヤーの位置をセット
        bombPrefab.GetComponent<MissileSpawner>().playerTransform = playerTransform;
    }

    //爆弾のクールタイムを計る
    public void CountBombCoolTime()
    {

    }
}
