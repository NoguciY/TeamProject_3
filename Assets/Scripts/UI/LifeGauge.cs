using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�̗̓Q�[�W�ɂ���
//�v���C���[�A�G�̗̑̓Q�[�W�Ɏg����悤�ɂ���

public class LifeGauge : MonoBehaviour
{
    //�̗̓Q�[�W
    [SerializeField]
    private Slider lifeGauge;

    //�Q�[�W�̏�����(�̗͂��ő�l�ɂ���)
    public void InitializeGauge(int maxLife)
    {
        lifeGauge.maxValue = maxLife;
        lifeGauge.value = maxLife;
    }

    //�Q�[�W���X�V����
    public void UpdateGauge(int value)
    {
        lifeGauge.value += value;
    }
}
