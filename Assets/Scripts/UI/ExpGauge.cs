using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpGauge : MonoBehaviour
{
    //�o���l�Q�[�W
    [SerializeField]
    private Slider expGauge;

    /// <summary>
    /// �o���l�Q�[�W���X�V����
    /// </summary>
    /// <param name="currentExp">���݂̌o���l</param>
    /// <param name="needExp">���x���A�b�v�ɕK�v�Ȍo���l</param>
    public void UpdateExpGauge(float currentExp, float needExp)
    {
        float value = 0;
        if (currentExp != 0)
            value = currentExp / needExp;

        //�l���P�ȏ�̏ꍇ�A0�ɂ���
        if (value >= 1)
            value = 0;

        expGauge.value = value;
    }
}
