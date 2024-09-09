using UnityEngine;
using UnityEngine.UI;

public class ExperienceValueGauge : MonoBehaviour
{
    //�o���l�Q�[�W
    [SerializeField]
    private Slider experienceValueGauge;

    /// <summary>
    /// �o���l�Q�[�W���X�V����
    /// </summary>
    /// <param name="currentExperienceValue">���݂̌o���l</param>
    /// <param name="needExperienceValue">���x���A�b�v�ɕK�v�Ȍo���l</param>
    public void UpdateGauge(float currentExperienceValue, float needExperienceValue)
    {
        //�ő�l���X�V
        experienceValueGauge.maxValue = needExperienceValue;

        //�Q�[�W�̒l���X�V
        experienceValueGauge.value = currentExperienceValue;
    }
}
