using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IApplicableDamage
{
    [NonSerialized]
    public UnityEvent gameOverEvent = new UnityEvent();


    public class ExperienceEvent : UnityEvent<int, int> { }

    public ExperienceEvent experienceEvent = new ExperienceEvent();

    [SerializeField, Header("移動速度")]
    private float speed = 3.0f;

    //爆弾
    [SerializeField]
    private GameObject bombPrefab;

    //最大HP
    private float maxHealth = 100f;

    //HP
    private float health;

    //爆弾の縦幅の半分
    private float bombHelfHeight;

    //現在のレベル
    private int currentLevel = 1;

    //現在の経験値
    private int currentExp = 0;

    //前回の経験値
    private int beforeExp;

    //必要経験値
    private int needExp;

    //最大レベル
    private int maxLevel = 300;

    //レベルアップに必要な経験値の基準値
    private int offsetNeedExp = 1;

    private Experience experience;

    //ゲッター
    //public int CurrentExp { get { return currentExp; } }
    //public int NeedExp { get { return needExp; } }

    private void Start()
    {
        health = maxHealth;

        bombHelfHeight = bombPrefab.transform.localScale.y / 2;
        
        experience = new Experience(maxLevel, offsetNeedExp);
        //レベルアップに必要な経験値を計算する
        experience.CalNeedExperience();
        //次のレベルに必要な経験値を取得
        needExp = experience.GetNeedExp(currentLevel + 1) ;

        beforeExp = 0;
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
        if (health <= 0)
        {
            gameOverEvent.Invoke();
        }

        //レベルアップ
        LevelUp();

        //経験値が変化した場合、経験値ゲージを更新
        if (currentExp != beforeExp)
        {
            experienceEvent.Invoke(currentExp, needExp);
            beforeExp = currentExp;
        }
    }

    public void RecieveDamage(float damage)
    {
        health -= damage;
        Debug.Log($"プレイヤーは{damage}ダメージ食らった\n" +
            $"残りの体力：{health}");
    }

    //経験値を得る
    public void GetExp(int exp)
    {
        currentExp += exp;
    }

    //レベルアップ
    void LevelUp()
    {
        //経験値がレベルアップに必要な経験値以上になった場合
        if (currentExp >= needExp)
        {
            //レベルアップ
            currentLevel++;
            currentExp = 0;
            needExp = experience.GetNeedExp(currentLevel + 1);
            Debug.Log($"レベルアップ！\n 現在のレベル：{currentLevel}");
        }
    }

}
