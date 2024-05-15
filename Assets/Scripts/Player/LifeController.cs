using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    //体力
    [SerializeField]
    private float life;

    //最大体力
    private float maxLife;

    //ゲッター
    public float GetLife => life;

    //体力の初期化(体力を最大にする)
    public void InitializeLife(float maxLife)
    {
        //SetMaxLife(maxLife);
        this.maxLife = maxLife;
        life = maxLife;
    }


    //体力に値を加える(ダメージ、回復)
    public void AddValueToLife(float value)
    {
        life += value;
        
        //体力が最大値を超えないようにする
        if (life > maxLife)
            life = maxLife;
        Debug.Log("体力更新");
    }


    //体力の最大値を設定する
    private void SetMaxLife(float maxLife)
    {
        this.maxLife = maxLife;
    }

    //死亡(体力が0)を判定する
    public bool IsDead()
    {
        bool dead = false;

        if (life <= 0)
            dead = true;

        return dead;
    }
}
