using UnityEngine;
using UnityEngine.UI;

//�̗̓Q�[�W�ɂ���
//�v���C���[�A�G�̗̑̓Q�[�W�Ɏg����悤�ɂ���

public class LifeGauge : MonoBehaviour
{
    [SerializeField, Header("�̗̓Q�[�W")]
    private Slider lifeGauge;


    /// <summary>
    /// �Q�[�W�̏�����(�̗͂��ő�l�ɂ���)
    /// </summary>
    /// <param name="maxLife">�ő�̗�</param>
    public void InitializeGauge(float maxLife)
    {
        lifeGauge.maxValue = maxLife;
        lifeGauge.value = maxLife;
    }

    /// <summary>
    /// �Q�[�W���X�V����
    /// </summary>
    /// <param name="life">�̗�</param>
    public void UpdateGauge(float life)
    {
        lifeGauge.value = life;

        //�̗̓Q�[�W�̒l���ő�l�𒴂��Ȃ��悤�ɂ���
        if (lifeGauge.value > lifeGauge.maxValue)
        {
            lifeGauge.value = lifeGauge.maxValue;
        }
        //�̗̓Q�[�W�̒l��0��菬�������Ȃ�
        else if (lifeGauge.value < 0)
        {
            lifeGauge.value = 0;
        }

        Debug.Log("�̗̓Q�[�W�F"+ "<color=red>" + lifeGauge.value + "</color>");
    }

    /// <summary>
    /// �Q�[�W�̍ő�l�̐ݒ�
    /// </summary>
    public float SetMaxValue
    {
        set{ 
            lifeGauge.maxValue = value;
            Debug.Log($"�̗̓Q�[�W�̍ő�l:{lifeGauge.maxValue}");
        }
    }
}
