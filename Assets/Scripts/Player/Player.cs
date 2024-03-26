using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IApplicableDamage
{
    [NonSerialized]
    public UnityEvent gameOverEvent = new UnityEvent();

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

    private void Start()
    {
        health = maxHealth;

        bombHelfHeight = bombPrefab.transform.localScale.y / 2;
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
    }

    public void RecieveDamage(float damage)
    {
        health -= damage;
        Debug.Log($"プレイヤーは{damage}ダメージ食らった\n" +
            $"残りの体力：{health}");
    }
}
