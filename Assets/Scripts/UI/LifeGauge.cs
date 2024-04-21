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
    public void InitializeGauge(float maxLife)
    {
        lifeGauge.maxValue = maxLife;
        lifeGauge.value = maxLife;
    }

    //�Q�[�W���X�V����
    public void UpdateGauge(float value)
    {
        lifeGauge.value += value;
    }

    //�Q�[���I�[�o�[���ɃQ�[�W��0�ɂȂ��Ă��Ȃ��ꍇ�Q�[�W��0�ɂ���
    public void GameOverLifeGauge()
    {
        if(lifeGauge.value >= 0)
            lifeGauge.value = 0;
    }
}
