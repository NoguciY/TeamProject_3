using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//爆弾を管理するクラス
public class BombManager : MonoBehaviour
{
    //投擲爆弾
    //[SerializeField]
    //private GameObject throwingBombPrefab;

    //設置型爆弾
    [SerializeField]
    private GameObject plantedBombPrefab;

    //ノックバック爆弾
    [SerializeField]
    private GameObject knockbackBombPrefab;

    //誘導爆弾
    [SerializeField]
    private GameObject missileSpawnPrefab;

    //爆発エフェクト
    [SerializeField, Header("ノックバック爆弾の爆発エフェクト")]
    private ParticleSystem knockbackBombExplosionParticle;

    //プレイヤーのTransformコンポーネント
    [SerializeField]
    private Transform playerTransform;

    //設置型爆弾の高さの半分
    private float plantedBombHelfHeight;

    //設置型爆弾の回転値
    private Quaternion plantedBombRotation;

    //ノックバック爆弾の高さの半分
    private float knockbackHelfHeight;

    //ノックバック爆弾とプレイヤーの間の距離
    private float toPlayerDistance;

    //生成されるノックバック爆弾の数
    //private int generatedKnockbackBombNum;

    //誘導爆弾の高さの半分
    private float homingBombHelfHeight;

    //定数を取得する
    public void Initialize()
    {
        //設置型爆弾関係の値を取得
        plantedBombHelfHeight = plantedBombPrefab.GetComponent<PlantedBomb>().GetHalfHeight();
        plantedBombRotation = plantedBombPrefab.transform.rotation;

        //ノックバック爆弾関係の値を取得、設定
        var knockbackBombComponent = knockbackBombPrefab.GetComponent<KnockbackBomb>();
        knockbackHelfHeight = knockbackBombComponent.GetHalfHeight();
        toPlayerDistance = knockbackBombComponent.GetToPlayerDistance;
        knockbackBombComponent.explosionParticle = knockbackBombExplosionParticle;

        //誘導爆弾の関係の値を取得
        homingBombHelfHeight = missileSpawnPrefab.GetComponent<MissileSpawner>().GetBombHalfHeight;
    }

    //設置型爆弾を生成する
    public void GeneratePlantedBomb()
    {
        //生成位置を計算して生成
        Vector3 spawnPos = playerTransform.position + Vector3.up * plantedBombHelfHeight;
        GameObject bombPrefab = Instantiate(plantedBombPrefab, spawnPos, plantedBombRotation);
    }

    //ノックバック爆弾を生成する
    public void GenerateKnockbackBomb()
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
                standardPos * toPlayerDistance + Vector3.up * knockbackHelfHeight;

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
}
