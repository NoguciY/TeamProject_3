using UnityEngine;
using UnityEngine.UI;

//体力ゲージにつける
//プレイヤー、敵の体力ゲージに使えるようにする

public class LifeGauge : MonoBehaviour
{
    [SerializeField, Header("体力ゲージ")]
    private Slider lifeGauge;


    /// <summary>
    /// ゲージの初期化(体力を最大値にする)
    /// </summary>
    /// <param name="maxLife">最大体力</param>
    public void InitializeGauge(float maxLife)
    {
        lifeGauge.maxValue = maxLife;
        lifeGauge.value = maxLife;
    }

    /// <summary>
    /// ゲージを更新する
    /// </summary>
    /// <param name="life">体力</param>
    public void UpdateGauge(float life)
    {
        lifeGauge.value = life;

        //体力ゲージの値が最大値を超えないようにする
        if (lifeGauge.value > lifeGauge.maxValue)
        {
            lifeGauge.value = lifeGauge.maxValue;
        }
        //体力ゲージの値は0より小さくしない
        else if (lifeGauge.value < 0)
        {
            lifeGauge.value = 0;
        }

        Debug.Log("体力ゲージ："+ "<color=red>" + lifeGauge.value + "</color>");
    }

    /// <summary>
    /// ゲージの最大値の設定
    /// </summary>
    public float SetMaxValue
    {
        set{ 
            lifeGauge.maxValue = value;
            Debug.Log($"体力ゲージの最大値:{lifeGauge.maxValue}");
        }
    }
}
