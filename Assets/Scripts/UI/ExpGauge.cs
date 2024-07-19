using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpGauge : MonoBehaviour
{
    //経験値ゲージ
    [SerializeField]
    private Slider expGauge;

    /// <summary>
    /// 経験値ゲージを更新する
    /// </summary>
    /// <param name="currentExp">現在の経験値</param>
    /// <param name="needExp">レベルアップに必要な経験値</param>
    public void UpdateExpGauge(float currentExp, float needExp)
    {
        float value = 0;
        if (currentExp != 0)
            value = currentExp / needExp;

        //値が１以上の場合、0にする
        if (value >= 1)
            value = 0;

        expGauge.value = value;
    }
}
