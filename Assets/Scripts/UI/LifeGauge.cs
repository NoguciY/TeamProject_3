using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//体力ゲージにつける
//プレイヤー、敵の体力ゲージに使えるようにする

public class LifeGauge : MonoBehaviour
{
    //体力ゲージ
    [SerializeField]
    private Slider lifeGauge;

    //ゲージの初期化(体力を最大値にする)
    public void InitializeGauge(int maxLife)
    {
        lifeGauge.maxValue = maxLife;
        lifeGauge.value = maxLife;
    }

    //ゲージを更新する
    public void UpdateGauge(int value)
    {
        lifeGauge.value += value;
    }
}
