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
    public void InitializeGauge(float maxLife)
    {
        lifeGauge.maxValue = maxLife;
        lifeGauge.value = maxLife;
    }

    //ゲージを更新する
    public void UpdateGauge(float value)
    {
        lifeGauge.value += value;
    }

    //ゲームオーバー時にゲージが0になっていない場合ゲージを0にする
    public void GameOverLifeGauge()
    {
        if(lifeGauge.value >= 0)
            lifeGauge.value = 0;
    }
}
