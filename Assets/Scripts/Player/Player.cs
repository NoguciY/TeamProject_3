using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IApplicableDamage
{
    [NonSerialized]
    public UnityEvent gameOverEvent = new UnityEvent();

    [SerializeField, Header("�ړ����x")]
    private float speed = 3.0f;

    //���e
    [SerializeField]
    private GameObject bombPrefab;

    //�ő�HP
    private float maxHealth = 100f;

    //HP
    private float health;

    //���e�̏c���̔���
    private float bombHelfHeight;

    private void Start()
    {
        health = maxHealth;

        bombHelfHeight = bombPrefab.transform.localScale.y / 2;
    }

    void Update()
    {
        //�Ⴆ�΁AWD���ꏏ�ɉ�����W�������Ă���Ƃ����A�����i��ł���̂ŏC������
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

        //���e�𐶐�����
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = transform.position + Vector3.up * bombHelfHeight;

            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        }

        //�̗͂�0�ɂȂ����ꍇ�A�Q�[���I�[�o�[
        if (health <= 0)
        {
            gameOverEvent.Invoke();
        }
    }

    public void RecieveDamage(float damage)
    {
        health -= damage;
        Debug.Log($"�v���C���[��{damage}�_���[�W�H�����\n" +
            $"�c��̗̑́F{health}");
    }
}
