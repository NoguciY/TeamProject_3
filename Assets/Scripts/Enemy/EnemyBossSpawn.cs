using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossSpawn : MonoBehaviour
{
    //�G�v���n�u
    public GameObject[] enemyPrefab = new GameObject[3];
    //�G�������ԊԊu
    private float interval;
    //�o�ߎ���
    private float time = 0f;

    void Start()
    {
        //���ԊԊu�����肷��
        interval = 5f;
    }

    void Update()
    {
        //���Ԍv��
        time += Time.deltaTime;

        //�o�ߎ��Ԃ��������ԂɂȂ����Ƃ�(�������Ԃ��傫���Ȃ����Ƃ�)
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            if (time > interval)
            {
                //enemy���C���X�^���X������(��������)
                GameObject enemy = Instantiate(enemyPrefab[i]);
                this.enemyPrefab[i].SetActive(true);
                //���������G�̍��W�����肷��(����X=0,Y=10,Z=20�̈ʒu�ɏo��)
                enemy.transform.position = new Vector3(0, 2, 20);
                //�o�ߎ��Ԃ����������čēx���Ԍv�����n�߂�
                time = 0f;
            }
        }
    }
}
