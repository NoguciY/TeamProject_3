using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

//ゲームを通してのプレイヤーのステータスを保持する

public class PlayerStatus : MonoBehaviour
{
    //最大HP
    public float maxLife = 100;

    //移動速度
    public float speed = 3f;

    //回収範囲率(1ではプレイヤーと同じ大きさ)
    public float collectionRangeRate = 1f;

    //防御力
    public float difence = 0;
    
    //回復力
    public float resilience = 0f;


    //ステータスをリセットする
    public void ResetStatus()
    {
        maxLife = 100;
        speed = 3f;
        collectionRangeRate = 1f;
        difence = 0;
        resilience = 0f;
    }
}
