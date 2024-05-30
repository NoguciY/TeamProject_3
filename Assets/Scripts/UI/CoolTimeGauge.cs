using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoolTimeGauge : MonoBehaviour
{
    [SerializeField, Header("クールタイムゲージ")]
    private Image[] coolTimeGauges;

    //クールタイム
    private float[] coolTimes;
    
    //ゲージが満ちているか
    private bool[] isFilling;

    void Start()
    {
        coolTimes = new float[coolTimeGauges.Length];
        isFilling = new bool[coolTimeGauges.Length];
    }

    //クールタイムゲージを満たす
    public void FillGauge(int index)
    {
        //ゲージを満たす
        if (isFilling[index] && coolTimes[index] > 0)
        {
            coolTimeGauges[index].fillAmount += 1f / coolTimes[index] * Time.deltaTime;

            //満たしきった場合、満たすのを止める
            if (coolTimeGauges[index].fillAmount >= 1f)
                isFilling[index] = false;
        }
    }
    
    //クールタイムゲージを満たし始める
    public void StartFillingCoolTimeGauge(int index, float coolTime)
    {
        //クールタイムを取得していない場合、クールタイムを取得する
        if(coolTimes[index] <= 0)
            coolTimes[index] = coolTime;

        coolTimeGauges[index].fillAmount = 0;
        isFilling[index] = true;
    }
}
