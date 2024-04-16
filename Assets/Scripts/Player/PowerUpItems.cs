using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//強化する項目をまとめて記載する

public class PowerUpItems : MonoBehaviour
{
    [SerializeField, Header("最大HPの増加率"), Range(0, 1)]
    private float maxLifeIncreaseRate;

    [SerializeField, Header("移動速度の増加率"), Range(0, 1)]
    private float speedIncreaseRate;

    [SerializeField, Header("回収範囲の増加率"), Range(0, 1)]
    private float collectionRangeIncreaseRate;

    [SerializeField, Header("防御力の増加率"), Range(0, 1)]
    private float difenceIncreaseRate;

    [SerializeField, Header("回復力の増加率"), Range(0, 1)]
    private float resilienceIncreaseRate;

    [SerializeField, Header("爆弾の爆発範囲の増加率"), Range(0, 1)]
    private float bombRangeIncreaseRate;

    //最大体力を強化する
    public void PowerUpMaxLife(Player player)
    {
        //最大HPの増加率分増やす
        player.maxLife += player.maxLife * maxLifeIncreaseRate;
        Debug.Log($"現在の最大体力：{player.maxLife}");
    }

    //移動速度を強化する
    public void PowerUpSpeed(Player player)
    {
        //移動速度の増加率分増やす
        player.speed += player.speed * speedIncreaseRate;
        Debug.Log($"現在の移動速度：{player.speed}");
    }

    //回収範囲を強化する
    public void PowerUpCollectionRangeRate(Player player)
    {
        //回収範囲率を増加率分増やす
        //この処理を下の処理とまとめられたらもっと短いコードで済むと思う
        //player.collectionRangeRate +=
        //    player.collectionRangeRate * collectionRangeIncreaseRate;

        //アイテム用のコライダーの半径を拡大する
        player.capsuleColliderForItem.radius += 
            player.capsuleColliderForItem.radius * collectionRangeIncreaseRate;
        Debug.Log($"アイテム用の当たり判定の半径：{player.capsuleColliderForItem.radius}");
    }

    //防御力を強化する
    public void PowerUpDifence(Player player)
    {
        //防御力を増加率分増やす
        player.difence += player.difence * difenceIncreaseRate;
        Debug.Log($"現在の防御力：{player.difence}");
    }

    //回復力を強化する
    public void PowerUpResilience(Player player)
    {
        //回復力を増加率分増やす
        player.resilience += player.resilience * resilienceIncreaseRate;
        Debug.Log($"現在の回復力：{player.resilience}");
    }

    //爆弾の攻撃範囲を強化する
    public void PowerUpBombRange(Player player)
    {
        //爆弾の範囲を増加率分増やす
        float radius = player.bombPrefab.GetComponent<PlantedBomb>().explosionRadius;
        radius += radius * bombRangeIncreaseRate;
        //2回GetComponentしているから最小限に抑えられないか？
        player.bombPrefab.GetComponent<PlantedBomb>().explosionRadius = radius;
        Debug.Log($"爆弾の攻撃範囲を強化");
    }

    //爆弾の使用速度を強化する
    public void PowerUpCoolDown(GameObject bomb, float coolTime)
    {

    }
}
