using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoolTimeGauge : MonoBehaviour
{
    [SerializeField, Header("�N�[���^�C���Q�[�W")]
    private Image[] coolTimeGauges;

    //�N�[���^�C��
    private float[] coolTimes;
    
    //�Q�[�W�������Ă��邩
    private bool[] isFilling;

    void Start()
    {
        coolTimes = new float[coolTimeGauges.Length];
        isFilling = new bool[coolTimeGauges.Length];
    }

    //�N�[���^�C���Q�[�W�𖞂���
    public void FillGauge(int index)
    {
        //�Q�[�W�𖞂���
        if (isFilling[index] && coolTimes[index] > 0)
        {
            coolTimeGauges[index].fillAmount += 1f / coolTimes[index] * Time.deltaTime;

            //�������������ꍇ�A�������̂��~�߂�
            if (coolTimeGauges[index].fillAmount >= 1f)
                isFilling[index] = false;
        }
    }
    
    //�N�[���^�C���Q�[�W�𖞂����n�߂�
    public void StartFillingCoolTimeGauge(int index, float coolTime)
    {
        //�N�[���^�C�����擾���Ă��Ȃ��ꍇ�A�N�[���^�C�����擾����
        if(coolTimes[index] <= 0)
            coolTimes[index] = coolTime;

        coolTimeGauges[index].fillAmount = 0;
        isFilling[index] = true;
    }
}
