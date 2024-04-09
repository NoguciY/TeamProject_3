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
    [SerializeField, Header("�ړ����x")]
    private float speed = 3.0f;

    //���e
    [SerializeField]
    private GameObject bombPrefab;

    //�̗̓R���|�[�l���g
    [SerializeField]
    private LifeController lifeController;

    //���x���A�b�v�R���|�[�l���g
    [SerializeField]
    private PlayerLevelUp playerLevelUp;

    //�ő�HP
    private int maxLife = 100;

    //���e�̏c���̔���
    private float bombHelfHeight;

    //�v���C���[�̃C�x���g�R���|�[�l���g
    public PlayerEvent playerEvent;

    private void Start()
    {
        //�̗͂̏�����
        lifeController.InitializeLife(maxLife);
        
        //�̗̓Q�[�W��̗͂̍ő�l�ɂ���
        playerEvent.getMaxLifeEvent.Invoke(maxLife);

        //���e�̍����̔���
        bombHelfHeight = bombPrefab.transform.localScale.y / 2;

        //���x���̏�����
        playerLevelUp.InitLevel();
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
        if (lifeController.IsDead())
        {
            playerEvent.gameOverEvent.Invoke();
        }

        //���x���A�b�v
        playerLevelUp.LevelUp(playerEvent.levelUpEvent);
    }

    //�_���[�W���󂯂�֐�(�C���^�[�t�F�[�X�Ŏ���)
    public void RecieveDamage(int damage)
    {
        lifeController.AddValueToLife(-damage);
        playerEvent.damageEvent.Invoke(damage);
        Debug.Log($"�v���C���[��{damage}�_���[�W�H�����\n" +
            $"�c��̗̑́F{lifeController.GetLife}");
    }


    //�o���l���擾����֐�(�C���^�[�t�F�[�X�Ŏ���)
    public void GetExp(int exp)
    {
        //�o���l��������
        playerLevelUp.AddExp(exp);

        //�o���l�Q�[�W���X�V����C�x���g�����s
        playerEvent.expEvent.Invoke(playerLevelUp.GetExp, playerLevelUp.GetNeedExp);
    }
}
