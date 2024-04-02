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

    //���݂̃��x��
    private int currentLevel = 1;

    //���݂̌o���l
    private int currentExp = 0;

    //�O��̌o���l
    private int beforeExp;

    //�K�v�o���l
    private int needExp;

    //�ő僌�x��
    private int maxLevel = 300;

    //���x���A�b�v�ɕK�v�Ȍo���l�̊�l
    private int offsetNeedExp = 1;

    private Experience experience;

    //�Q�b�^�[
    //public int CurrentExp { get { return currentExp; } }
    //public int NeedExp { get { return needExp; } }

    private void Start()
    {
        health = maxHealth;

        bombHelfHeight = bombPrefab.transform.localScale.y / 2;
        
        experience = new Experience(maxLevel, offsetNeedExp);
        //���x���A�b�v�ɕK�v�Ȍo���l���v�Z����
        experience.CalNeedExperience();
        //���̃��x���ɕK�v�Ȍo���l���擾
        needExp = experience.GetNeedExp(currentLevel + 1) ;

        beforeExp = 0;
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

        //���x���A�b�v
        LevelUp();

        //�o���l���ω������ꍇ�A�o���l�Q�[�W���X�V
        if (currentExp != beforeExp)
        {
            experienceEvent.Invoke(currentExp, needExp);
            beforeExp = currentExp;
        }
    }

    public void RecieveDamage(float damage)
    {
        health -= damage;
        Debug.Log($"�v���C���[��{damage}�_���[�W�H�����\n" +
            $"�c��̗̑́F{health}");
    }

    //�o���l�𓾂�
    public void GetExp(int exp)
    {
        currentExp += exp;
    }

    //���x���A�b�v
    void LevelUp()
    {
        //�o���l�����x���A�b�v�ɕK�v�Ȍo���l�ȏ�ɂȂ����ꍇ
        if (currentExp >= needExp)
        {
            //���x���A�b�v
            currentLevel++;
            currentExp = 0;
            needExp = experience.GetNeedExp(currentLevel + 1);
            Debug.Log($"���x���A�b�v�I\n ���݂̃��x���F{currentLevel}");
        }
    }

}
