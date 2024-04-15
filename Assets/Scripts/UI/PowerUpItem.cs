using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//強化項目の処理
//強化項目の画像をまとめる

public abstract class PowerUpItem : MonoBehaviour
{
    //強化項目画像
    [SerializeField]
    private Image powerUpItemImage;

    //強化関数
    public virtual void PowerUpFunc(Player player)
    {
    }
}

//最大体力を強化する
public class MaxLife : PowerUpItem
{
    //画像

    //最大体力を強化する
    public override void PowerUpFunc(Player player)
    {
        //引数に現在の最大体力がいる
    }
}

//移動速度を
