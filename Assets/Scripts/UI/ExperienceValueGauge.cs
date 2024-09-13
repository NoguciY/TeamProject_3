using UnityEngine;
using UnityEngine.UI;

public class ExperienceValueGauge : MonoBehaviour
{
    //�o���l�Q�[�W
    [SerializeField]
    private Slider experienceValueSlider;

    /// <summary>
    /// �o���l�Q�[�W���X�V����
    /// </summary>
    /// <param name="currentExperienceValue">���݂̌o���l</param>
    /// <param name="needExperienceValue">���x���A�b�v�ɕK�v�Ȍo���l</param>
    public void UpdateGauge(float currentExperienceValue, float needExperienceValue)
    {
        //�ő�l���X�V
        experienceValueSlider.maxValue = needExperienceValue;

        //�Q�[�W�̒l���X�V
        experienceValueSlider.value = currentExperienceValue;
    }
}
