using UnityEngine;
using UnityEngine.UI;

public class ExperienceValueGauge : MonoBehaviour
{
    //経験値ゲージ
    [SerializeField]
    private Slider experienceValueGauge;

    /// <summary>
    /// 経験値ゲージを更新する
    /// </summary>
    /// <param name="currentExperienceValue">現在の経験値</param>
    /// <param name="needExperienceValue">レベルアップに必要な経験値</param>
    public void UpdateGauge(float currentExperienceValue, float needExperienceValue)
    {
        //最大値を更新
        experienceValueGauge.maxValue = needExperienceValue;

        //ゲージの値を更新
        experienceValueGauge.value = currentExperienceValue;
    }
}
