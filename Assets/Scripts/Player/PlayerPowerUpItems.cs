using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//強化する項目をまとめて記載する

public class PlayerPowerUpItems : MonoBehaviour
{
    [SerializeField, Header("最大HPの増加率"), Range(0, 1)]
    private float maxLifeIncreaseRate;

    [SerializeField, Header("移動速度の増加率"), Range(0, 1)]
    private float speedIncreaseRate;

    [SerializeField, Header("回収範囲の増加率"), Range(0, 1)]
    private float collectionRangeIncreaseRate;

    [SerializeField, Header("防御力の増加量")]
    private float difenceIncreaseValue;

    [SerializeField, Header("回復力の増加量")]
    private float resilienceIncreaseValue;

    [SerializeField, Header("爆弾の爆発範囲の増加率"), Range(0, 1)]
    private float bombRangeIncreaseRate;

    //最大体力を強化する
    public void PowerUpMaxLife(Player player)
    {
        //最大HPの増加率分増やす
        player.maxLife += player.maxLife * maxLifeIncreaseRate;

        //LifeControllerコンポーネントの最大体力を更新する
        player.GetLifeController.SetMaxLife = player.maxLife;

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
        //回収範囲(アイテム用のコライダーの半径)を増加率分拡大する
        player.capsuleColliderForItem.radius += 
            player.capsuleColliderForItem.radius * collectionRangeIncreaseRate;
        Debug.Log($"アイテム用の当たり判定の半径：{player.capsuleColliderForItem.radius}");
    }

    //防御力を強化する
    public void PowerUpDifence(Player player)
    {
        //防御力を増加率分増やす
        player.difense += difenceIncreaseValue;
        Debug.Log($"現在の防御力：{player.difense}");
    }

    //回復力を強化する
    public void PowerUpResilience(Player player)
    {
        //回復力を増加率分増やす
        player.resilience += resilienceIncreaseValue;
        Debug.Log($"現在の回復力：{player.resilience}");
    }

    //爆弾の攻撃範囲を強化する
    public void PowerUpBombRange(Player player)
    {
        float explosionRadius = player.GetBombManager.GetBombPlanted.ExplosionRadius;
        explosionRadius += explosionRadius * bombRangeIncreaseRate;

        player.GetBombManager.GetBombPlanted.ExplosionRadius = explosionRadius;
        Debug.Log($"現在の攻撃範囲{explosionRadius}");
    }

    //爆弾の使用速度を強化する
    public void PowerUpCoolDown(GameObject bomb, float coolTime)
    {

    }
}
