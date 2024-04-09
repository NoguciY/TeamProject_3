//using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Player : MonoBehaviour, IApplicableDamage, IGettableItem
{
    [SerializeField, Header("移動速度")]
    private float speed = 3.0f;

    //爆弾
    [SerializeField]
    private GameObject bombPrefab;

    //体力コンポーネント
    [SerializeField]
    private LifeController lifeController;

    //レベルアップコンポーネント
    [SerializeField]
    private PlayerLevelUp playerLevelUp;

    //最大HP
    private int maxLife = 100;

    //爆弾の縦幅の半分
    private float bombHelfHeight;

    //プレイヤーのイベントコンポーネント
    public PlayerEvent playerEvent;

    private void Start()
    {
        //体力の初期化
        lifeController.InitializeLife(maxLife);
        
        //体力ゲージを体力の最大値にする
        playerEvent.getMaxLifeEvent.Invoke(maxLife);

        //爆弾の高さの半分
        bombHelfHeight = bombPrefab.transform.localScale.y / 2;

        //レベルの初期化
        playerLevelUp.InitLevel();
    }

    void Update()
    {
        //例えば、WDを一緒に押すとWを押しているときより、速く進んでいるので修正する
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= speed * transform.right * Time.deltaTime;
        }

        //爆弾を生成する
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = transform.position + Vector3.up * bombHelfHeight;

            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        }

        //体力が0になった場合、ゲームオーバー
        if (lifeController.IsDead())
        {
            playerEvent.gameOverEvent.Invoke();
        }

        //レベルアップ
        playerLevelUp.LevelUp(playerEvent.levelUpEvent);
    }

    //ダメージを受ける関数(インターフェースで実装)
    public void RecieveDamage(int damage)
    {
        lifeController.AddValueToLife(-damage);
        playerEvent.damageEvent.Invoke(damage);
        Debug.Log($"プレイヤーは{damage}ダメージ食らった\n" +
            $"残りの体力：{lifeController.GetLife}");
    }


    //経験値を取得する関数(インターフェースで実装)
    public void GetExp(int exp)
    {
        //経験値を加える
        playerLevelUp.AddExp(exp);

        //経験値ゲージを更新するイベントを実行
        playerEvent.expEvent.Invoke(playerLevelUp.GetExp, playerLevelUp.GetNeedExp);
    }
}
