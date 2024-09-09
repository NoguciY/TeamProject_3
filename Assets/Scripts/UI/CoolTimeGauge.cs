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

    /// <summary>
    /// クールタイムゲージを満たす
    /// </summary>
    /// <param name="bombID">爆弾の固有番号</param>
    public void FillGauge(int bombID)
    {
        //ゲージを満たす
        if (isFilling[bombID] && coolTimes[bombID] > 0)
        {
            coolTimeGauges[bombID].fillAmount += 1f / coolTimes[bombID] * Time.deltaTime;

            //満たしきった場合、満たすのを止める
            if (coolTimeGauges[bombID].fillAmount >= 1f)
                isFilling[bombID] = false;
        }
    }
    
    /// <summary>
    /// クールタイムゲージを満たし始める
    /// </summary>
    /// <param name="bombID">爆弾の固有番号</param>
    /// <param name="coolTime">クールダウンの時間(秒)</param>
    public void StartFillingCoolTimeGauge(int bombID, float coolTime)
    {
        //クールタイムを取得していない場合、クールタイムを取得する
        if (coolTimes[bombID] <= 0)
        {
            coolTimes[bombID] = coolTime;
        }

        coolTimeGauges[bombID].fillAmount = 0;
        isFilling[bombID] = true;
    }
}
