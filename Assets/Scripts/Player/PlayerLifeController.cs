using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour
{
    //体力
    [SerializeField]
    private float life;

    public float GetLife => life;

    //最大体力
    private float maxLife;

    //死亡したか
    private bool isDead;

    public bool GetIsDead => isDead;


    /// <summary>
    /// 体力の初期化(体力を最大にする)
    /// </summary>
    /// <param name="maxLife">最大体力</param>
    public void InitializeLife(float maxLife)
    {
        this.maxLife = maxLife;
        life = maxLife;
        isDead = false;
    }


    /// <summary>
    /// 体力に値を加える(ダメージ、回復)
    /// </summary>
    /// <param name="value">ダメージ量、回復量</param>
    public void AddValueToLife(float value)
    {
        life += value;

        //体力が最大値を超えないようにする
        if (life > maxLife)
        {
            life = maxLife;
        }
        //体力は0より小さくしない
        else if (life < 0)
        {
            life = 0;
        }

        Debug.Log("体力更新");
    }


    /// <summary>
    /// 体力の最大値を設定する
    /// </summary>
    public float SetMaxLife
    {
        set { 
            this.maxLife = value;
            Debug.Log($"最大体力:{maxLife}");
        }
    }

    /// <summary>
    /// 死亡(体力が0)直後を判定する
    /// </summary>
    /// <returns>true:死亡直後 / false:死んでいない、死亡後</returns>
    public bool IsDead()
    {
        if (!isDead && life <= 0)
        {
            return isDead = true;
        }

        return false;
    }
}
